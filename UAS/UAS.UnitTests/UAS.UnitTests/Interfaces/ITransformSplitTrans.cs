using System.Collections.Generic;
using FrameworkUAS.Entity;

namespace UAS.UnitTests.Interfaces
{
    public interface ITransformSplitTrans
    {
        int Save(TransformSplitTrans x);
        List<TransformSplitTrans> Select();
        List<TransformSplitTrans> Select(int transformationId);
        List<TransformSplitTrans> SelectSourceFileId(int sourceFileId);
    }
}