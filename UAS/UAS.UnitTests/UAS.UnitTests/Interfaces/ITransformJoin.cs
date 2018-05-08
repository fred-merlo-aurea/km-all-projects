using System.Collections.Generic;
using FrameworkUAS.Entity;

namespace UAS.UnitTests.Interfaces
{
    public interface ITransformJoin
    {
        int Save(TransformJoin x);
        List<TransformJoin> Select();
        List<TransformJoin> Select(int transformationId);
        List<TransformJoin> SelectSourceFileId(int sourceFileId);
    }
}