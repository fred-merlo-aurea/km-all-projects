﻿EXECUTE sp_addrolemember @rolename = N'db_owner', @membername = N'ecn5';


GO
EXECUTE sp_addrolemember @rolename = N'db_ddladmin', @membername = N'webuser';


GO
EXECUTE sp_addrolemember @rolename = N'db_datawriter', @membername = N'webuser';


GO
EXECUTE sp_addrolemember @rolename = N'db_denydatareader', @membername = N'jwelter';


GO
EXECUTE sp_addrolemember @rolename = N'db_denydatawriter', @membername = N'jwelter';

