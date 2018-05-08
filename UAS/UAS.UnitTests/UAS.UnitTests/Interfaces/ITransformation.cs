using System.Collections.Generic;

namespace UAS.UnitTests.Interfaces
{
    public interface ITransformation
    {
        int Delete(int transformationId);
        int Save(global::FrameworkUAS.Entity.Transformation x);
        List<global::FrameworkUAS.Entity.Transformation> Select();
        List<global::FrameworkUAS.Entity.Transformation> Select(int transformationId);
        List<global::FrameworkUAS.Entity.Transformation> Select(int clientId, int sourceFileId, bool includeCustomProperties = false);
        List<global::FrameworkUAS.Entity.Transformation> SelectAssigned(string fileName);
        List<global::FrameworkUAS.Entity.Transformation> SelectAssigned(int fieldMappingId);
        List<global::FrameworkUAS.Entity.Transformation> SelectClient(int clientId, bool includeCustomProperties = false);
        List<global::FrameworkUAS.Entity.Transformation> SelectPaging(int clientId, int currentPage, int pageSize, bool isTemplate, bool isActive, int transformationTypeId, bool ignoreAdminTransformationTypes, int adminTransformId, string sortField, string sortDirection);
        int SelectPagingCount(int clientId, bool isTemplate, bool isActive, int transformationTypeId, bool ignoreAdminTransformationTypes, int adminTransformId);
        global::FrameworkUAS.Entity.Transformation SelectTransformationById(int transformationId);
        List<global::FrameworkUAS.Entity.Transformation> SelectTransformationId(string transformationName, string clientName);
    }
}