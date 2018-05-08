EXECUTE sp_addrolemember @rolename = N'db_datareader', @membername = N'ecn5';


GO
EXECUTE sp_addrolemember @rolename = N'db_datareader', @membername = N'ecn5writer';


GO
EXECUTE sp_addrolemember @rolename = N'db_datareader', @membername = N'reader';


GO
EXECUTE sp_addrolemember @rolename = N'db_datareader', @membername = N'webuser';


GO
EXECUTE sp_addrolemember @rolename = N'db_datawriter', @membername = N'ecn5';


GO
EXECUTE sp_addrolemember @rolename = N'db_datawriter', @membername = N'ecn5writer';


GO
EXECUTE sp_addrolemember @rolename = N'db_datawriter', @membername = N'webuser';

