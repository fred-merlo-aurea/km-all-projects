CREATE ROLE [Developer]
    AUTHORIZATION [dbo];




GO
EXECUTE sp_addrolemember @rolename = N'Developer', @membername = N'bill.hipps';

GO
EXECUTE sp_addrolemember @rolename = N'Developer', @membername = N'justin.welter';


GO
EXECUTE sp_addrolemember @rolename = N'Developer', @membername = N'latha.sunil';


