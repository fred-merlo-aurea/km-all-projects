EXECUTE sp_addrolemember @rolename = N'db_owner', @membername = N'Jason.Meier';


GO
EXECUTE sp_addrolemember @rolename = N'db_owner', @membername = N'justin.wagner';


GO
EXECUTE sp_addrolemember @rolename = N'db_ddladmin', @membername = N'Jason.Meier';


GO
EXECUTE sp_addrolemember @rolename = N'db_datareader', @membername = N'Jason.Meier';


GO

EXECUTE sp_addrolemember @rolename = N'db_datawriter', @membername = N'Jason.Meier';


GO
EXECUTE sp_addrolemember @rolename = N'db_owner', @membername = N'webuser';


GO
EXECUTE sp_addrolemember @rolename = N'db_owner', @membername = N'ADMS_Admin';


GO
EXECUTE sp_addrolemember @rolename = N'db_datawriter', @membername = N'webuser';


GO
EXECUTE sp_addrolemember @rolename = N'db_datareader', @membername = N'webuser';

