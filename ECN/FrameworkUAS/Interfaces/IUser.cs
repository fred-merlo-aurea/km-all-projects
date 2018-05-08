using System;
using System.Collections.Generic;
using System.Data;

namespace KMPlatform.BusinessLogic.Interfaces
{
    public interface IUser
    {
        List<Entity.User> Select(bool includeObjects = false);
        List<Entity.User> Select(int clientGroupID, bool includeObjects = false);
        List<Entity.User> Select(int clientID, int securityGroupID, bool includeObjects = false);
        List<Entity.User> Select(int clientID, string securityGroupName, bool includeObjects = false);
        List<Entity.User> SelectByClientGroupID(int clientGroupID, bool includeObjects = false);
        List<Entity.User> SelectByClientID(int clientID, bool includeObjects = false);
        void FillObjects(List<Entity.User> users, bool includeObjects);
        Entity.User SelectUser(int userID, bool includeObjects = false);
        Entity.User SearchUserName(string userName);
        Entity.User LogIn(string userName, string password, bool includeObjects = true);
        Entity.User LogInLite(string userName, string password, bool includeObjects = true);
        Entity.User LogIn(Guid accessKey, bool includeObjects = true);
        Entity.User SetAuthorizedUserObjects(Entity.User user, int clientID);
        Entity.User SetAuthorizedUserObjects(Entity.User user, int clientgroupID, int clientID);
        Entity.User SetAuthorizedUserObjectsLite(Entity.User user, int clientgroupID, int clientID);
        Entity.User SetUserObjects(Entity.User user);
        bool EmailExist(string email);
        List<Entity.User> Search(string searchValue, List<Entity.User> searchList);
        string GetEmailAddress(string userName);
        int Save(Entity.User x);
        Entity.User SearchEmail(string email);
        bool IsKmUser(Entity.User user);
        bool IsKmUserNoDbCheck(Entity.User user);
        bool Validate_UserName(string userName, int userID);
        void Delete(int userID);
        DataTable SelectUserForGrid(int clientID, int? ClientGroupID, int pageSize, int pageIndex, bool IncludePlatformAdmins, bool UserIsCAdmin, bool IncludeAllClients,bool IncludeBCAdmins, bool IsKMstaff , string searchText,bool ShowDisabledUsers, bool ShowDisabledUserRoles);
        DataTable DownloadUserGrid(int clientID, int? ClientGroupID, bool IncludePlatformAdmins, bool UserIsCAdmin, bool IncludeAllClients, bool IncludeBCAdmins, bool IsKMstaff, string searchText, bool ShowDisabledUsers, bool ShowDisabledUserRoles);
        Entity.User ECN_SetAuthorizedUserObjects(Entity.User user, int clientgroupID, int clientID);
        Entity.User ECN_SelectUser(int userID, bool includeObjects = false);
        Entity.User ECN_LogIn(Guid accessKey, bool includeObjects = true);
        string ResetPasswordHTML();
        string GenerateTempPassword();
    }
}