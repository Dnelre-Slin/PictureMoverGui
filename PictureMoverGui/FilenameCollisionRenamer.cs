using System;
using System.Collections.Generic;
using System.IO;
using System.Security.Cryptography;
using System.Text;

namespace PictureMoverGui
{
    class FilenameCollisionRenamerException : NullReferenceException
    {
        public FilenameCollisionRenamerException() { }
        public FilenameCollisionRenamerException(string? message) : base(message) { }
        public FilenameCollisionRenamerException(string? message, Exception? innerException) : base(message, innerException) { }
    }

    class FilenameCollisionRenamer
    {
        const int max_rename_tries = 100;

        private NameCollisionActionEnum nameCollisionAction;
        private CompareFilesActionEnum compareFilesAction;
        private HashTypeEnum hashType;
        private DirectoryInfo destinationDir;
        private FileInfo file;
        private string filename;

        private bool isValid;
        private string validFilename;
        private bool isValidIsCalled;
        private bool fileWasRenamed;

        public FilenameCollisionRenamer(NameCollisionActionEnum nameCollisionAction, CompareFilesActionEnum compareFilesAction, HashTypeEnum hashType, DirectoryInfo destinationDir, FileInfo file, string filename)
        {
            this.nameCollisionAction = nameCollisionAction;
            this.compareFilesAction = compareFilesAction;
            this.hashType = hashType;
            this.destinationDir = destinationDir;
            this.file = file;
            this.filename = filename;

            this.isValid = false;
            this.isValidIsCalled = false;
            this.fileWasRenamed = false;
        }

        public bool WasFileRenamed()
        {
            if (!this.isValidIsCalled)
            {
                throw new FilenameCollisionRenamerException("Called \"GetValidFilename()\" before calling \"IsValid()\". This is not allowed");
            }
            return this.fileWasRenamed;
        }

        public string GetValidFilename()
        {
            if (!this.isValidIsCalled)
            {
                throw new FilenameCollisionRenamerException("Called \"GetValidFilename()\" before calling \"IsValid()\". This is not allowed");
            }
            if (!this.isValid)
            {
                throw new FilenameCollisionRenamerException("Called \"GetValidFilename()\" even though the filename is not valid. This is not allowed");
            }
            return this.validFilename;
        }

        public bool IsValid()
        {
            switch (this.nameCollisionAction)
            {
                case NameCollisionActionEnum.SkipFile:
                    CheckFilenameSkipFile();
                    break;
                case NameCollisionActionEnum.NrAppend:
                    CheckFilenameAlwaysAppend();
                    break;
                case NameCollisionActionEnum.CompareFiles:
                    CheckFilenameCompareFiles();
                    break;
            }
            this.isValidIsCalled = true;
            return this.isValid;
        }

        private void CheckFilenameSkipFile()
        {
            if (DirSearcher.FilenameInDir(destinationDir, filename))
            {
                this.isValid = false;
            }
            else // No name collsion
            {
                this.validFilename = this.filename;
                this.isValid = true;
            }
        }

        private void CheckFilenameAlwaysAppend()
        {
            this.validFilename = filename;
            if (DirSearcher.FilenameInDir(destinationDir, filename)) // Rename to a int postfix, if name is already taken. Example: filename.png -> filename_1.png
            {
                string new_filename = GetNewFilename();
                this.validFilename = new_filename;
                this.fileWasRenamed = true;
            }
            this.isValid = true;
        }

        private void CheckFilenameCompareFiles()
        {
            FileInfo otherFile;
            if (DirSearcher.FilenameInDir(this.destinationDir, this.filename, out otherFile))
            {
                if (CompareFiles(otherFile)) // Compare file and otherFile. If true, the files are the same file, so do not transfer.
                {
                    this.isValid = false;
                }
                else // Different files, but with same name. Rename file
                {
                    string new_filename = GetNewFilename();
                    this.validFilename = new_filename;
                    this.fileWasRenamed = true;
                    this.isValid = true;
                }
            }
            else // No name collision
            {
                this.validFilename = this.filename;
                this.isValid = true;
            }
        }

        private bool CompareFiles(FileInfo otherFile)
        {
            switch(this.compareFilesAction)
            {
                case CompareFilesActionEnum.NameAndDateOnly:
                    return CompareWriteDate(otherFile);
                case CompareFilesActionEnum.NameAndHashOnly:
                    return CompareHashValue(otherFile);
                case CompareFilesActionEnum.NameDateAndHash:
                    return CompareWriteDateAndHashValue(otherFile);
            }
            throw new FilenameCollisionRenamerException("Switch case in CompareFiles, did not handle all cases.");
        }

        private bool CompareWriteDateAndHashValue(FileInfo otherFile)
        {
            return CompareWriteDate(otherFile) && CompareHashValue(otherFile);
        }

        private bool CompareWriteDate(FileInfo otherFile)
        {
            return this.file.LastWriteTime == otherFile.LastWriteTime;
        }

        private bool CompareHashValue(FileInfo otherFile)
        {
            switch (this.hashType)
            {
                case HashTypeEnum.MD5:
                    return CompareHashMD5(otherFile);
                case HashTypeEnum.SHA256:
                    return CompareHashSHA256(otherFile);
            }
            throw new FilenameCollisionRenamerException("Switch case in CompareHashValue, did not handle all cases.");
        }

        private bool CompareHashMD5(FileInfo otherFile)
        {
            using (MD5 md5Instance = MD5.Create())
            using (FileStream fileStream = File.OpenRead(this.file.FullName))
            using (FileStream otherFileStream = File.OpenRead(otherFile.FullName))
            {
                string fileMd5Value = BitConverter.ToString(md5Instance.ComputeHash(fileStream)).Replace("-", "").ToLowerInvariant();
                string otherFileMd5Value = BitConverter.ToString(md5Instance.ComputeHash(otherFileStream)).Replace("-", "").ToLowerInvariant();
                return fileMd5Value == otherFileMd5Value;
            }
        }

        private bool CompareHashSHA256(FileInfo otherFile)
        {
            using (var sha256Instance = SHA256.Create())
            using (var fileStream = File.OpenRead(this.file.FullName))
            using (var otherFileStream = File.OpenRead(otherFile.FullName))
            {
                string fileSha256Value = BitConverter.ToString(sha256Instance.ComputeHash(fileStream)).Replace("-", "").ToLowerInvariant();
                string otherFileSha256Value = BitConverter.ToString(sha256Instance.ComputeHash(otherFileStream)).Replace("-", "").ToLowerInvariant();
                return fileSha256Value == otherFileSha256Value;
            }
        }

        private string GetNewFilename()
        {
            string new_filename = this.filename;
            string[] filename_extension_split = new_filename.Split(".");
            string fname = filename_extension_split[0];
            string extname = filename_extension_split[1];
            for (int i = 0; i < max_rename_tries; i++)
            {
                string renamed_filename = $"{fname}_{i + 1}.{extname}";
                if (!DirSearcher.FilenameInDir(destinationDir, renamed_filename))
                {
                    new_filename = renamed_filename;
                    break;
                }
            }
            return new_filename;
        }
    }
}
