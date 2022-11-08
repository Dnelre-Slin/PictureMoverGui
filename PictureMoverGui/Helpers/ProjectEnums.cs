namespace PictureMoverGui.Helpers
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

    public enum MediaTypeEnum
    {
        NormalDirectory,
        MediaDevice
    }

    public enum WorkStatus
    {
        Unfinished,
        Success,
        Invalid,
        Cancelled
    }

    public enum DeviceChangeType
    {
        None,
        Added,
        Removed
    }
}
