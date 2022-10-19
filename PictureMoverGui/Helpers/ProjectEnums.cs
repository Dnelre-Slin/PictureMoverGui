using System;
using System.Collections.Generic;
using System.Text;

namespace PictureMoverGui
{
    public enum RunStates
    {
        DirectoryValidation,
        DirectoryGathering,
        RunningSorter,
        Idle
    }

    public enum NameCollisionActionEnum
    {
        SkipFile,
        NrAppend,
        CompareFiles
    }

    public enum CompareFilesActionEnum
    {
        NameAndDateOnly,
        NameAndHashOnly,
        NameDateAndHash
    }

    public enum HashTypeEnum
    {
        MD5,
        SHA256
    }
}
