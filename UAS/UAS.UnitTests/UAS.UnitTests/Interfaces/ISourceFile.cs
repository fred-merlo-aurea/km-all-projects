using System.Collections.Generic;
using UasEntity = FrameworkUAS.Entity;

namespace UAS.UnitTests.Interfaces
{
    public interface ISourceFile
    {
        int Delete(int SourceFileID, int ClientID);
        int Delete(int SourceFileID, bool IsDeleted, int UserID);
        bool IsFileNameUnique(int clientId, string fileName);
        int Save(UasEntity.SourceFile x, bool defaultRules = true);
        List<UasEntity.SourceFile> Select(bool includeCustomProperties = false);
        List<UasEntity.SourceFile> Select(int clientID, bool includeCustomProperties = false);
        List<UasEntity.SourceFile> Select(List<int> sourceFileIds, bool includeCustomProperties = false);
        List<UasEntity.SourceFile> Select(bool includeCustomProperties = false, bool isDeleted = false);
        UasEntity.SourceFile Select(int clientId, string fileName, bool includeCustomProperties = false);
        List<UasEntity.SourceFile> Select(int clientID, bool includeCustomProperties = false, bool isDeleted = false);
        List<UasEntity.SourceFile> SelectByFileType(int clientId, FrameworkUAD_Lookup.Enums.FileTypes fileType, bool includeCustomProperties = false);
        List<UasEntity.SourceFile> SelectPaging(int clientID, int currentPage, int pageSize, int serviceID, string type, int pubID, int fileType, string filename, string sortField, string sortDirection);
        int SelectPagingCount(int clientID, int serviceID, string type, int pubID, int fileType, string filename);
        UasEntity.SourceFile SelectSourceFileID(int sourceFileID, bool includeCustomProperties = false);
        List<UasEntity.SourceFile> SelectSpecialFiles(bool includeCustomProperties = false);
        List<UasEntity.SourceFile> SelectSpecialFiles(int clientID, bool includeCustomProperties = false);
    }
}