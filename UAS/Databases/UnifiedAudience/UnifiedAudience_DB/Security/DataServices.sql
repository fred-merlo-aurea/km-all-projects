CREATE ROLE [DataServices]
    AUTHORIZATION [dbo];


GO
EXECUTE sp_addrolemember @rolename = N'DataServices', @membername = N'DS_Admin';

