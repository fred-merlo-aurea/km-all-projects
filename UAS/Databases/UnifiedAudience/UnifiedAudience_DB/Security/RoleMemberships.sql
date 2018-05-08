EXECUTE sp_addrolemember @rolename = N'db_owner', @membername = N'webuser';


GO
EXECUTE sp_addrolemember @rolename = N'db_datawriter', @membername = N'webuser';


GO
EXECUTE sp_addrolemember @rolename = N'db_datareader', @membername = N'webuser';
GO
EXECUTE sp_addrolemember @rolename = N'db_owner', @membername = N'justin.wagner';

GO
