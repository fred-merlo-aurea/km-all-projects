using System.Collections.Generic;

namespace UAS.UnitTests.Interfaces
{
    public interface ITransformDataMap
    {
        List<global::FrameworkUAS.Entity.TransformDataMap> Delete(int TransformDataMapID);
        int Save(global::FrameworkUAS.Entity.TransformDataMap x);
        List<global::FrameworkUAS.Entity.TransformDataMap> Select();
        List<global::FrameworkUAS.Entity.TransformDataMap> Select(int TransformationID);
        List<global::FrameworkUAS.Entity.TransformDataMap> SelectSourceFileID(int sourceFileID);
    }
}