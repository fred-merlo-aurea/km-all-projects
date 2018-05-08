EXECUTE sp_addrolemember @rolename = N'db_datareader', @membername = N'webuser';


GO



GO
EXECUTE sp_addrolemember @rolename = N'db_datawriter', @membername = N'webuser';


GO


