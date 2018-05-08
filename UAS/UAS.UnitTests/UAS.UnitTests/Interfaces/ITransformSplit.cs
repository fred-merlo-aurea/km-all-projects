using System.Collections.Generic;
using FrameworkUAS.Object;

namespace UAS.UnitTests.Interfaces
{
    public interface ITransformSplit
    {
        int Save(global::FrameworkUAS.Entity.TransformSplit x);
        List<global::FrameworkUAS.Entity.TransformSplit> Select();
        List<global::FrameworkUAS.Entity.TransformSplit> Select(int TransformationID);
        List<TransformSplitInfo> SelectObject(int sourceFileID);
        List<global::FrameworkUAS.Entity.TransformSplit> SelectSourceFileID(int sourceFileID);
    }
}