using System.Collections.Generic;
using FrameworkUAS.Entity;

namespace UAS.UnitTests.Interfaces
{
    public interface ITransformAssign
    {
        int Delete(int transformDataMapId);
        int Save(TransformAssign x);
        List<TransformAssign> Select();
        List<TransformAssign> Select(int transformationId);
        List<TransformAssign> SelectSourceFileId(int sourceFileId);
    }
}