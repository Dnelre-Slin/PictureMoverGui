using System;
using System.Collections.Generic;
using System.Text;

namespace PictureMoverGui
{
    public enum NameCollisionActionEnum
    {
        SkipFile,
        NrAppend,
        CompareFiles
    }

    public enum CompareFilesActionEnum
    {
        NameAndDateOnly,
        MD5,
        SHA256
    }
}
