CREATE ROLE [Developer]
    AUTHORIZATION [dbo];






GO
EXECUTE sp_addrolemember @rolename = N'Developer', @membername = N'bill.hipps';


GO
EXECUTE sp_addrolemember @rolename = N'Developer', @membername = N'justin.wagner';


GO
EXECUTE sp_addrolemember @rolename = N'Developer', @membername = N'justin.welter';


GO
EXECUTE sp_addrolemember @rolename = N'Developer', @membername = N'latha.sunil';



GO
EXECUTE sp_addrolemember @rolename = N'Developer', @membername = N'Robert.Rawleigh';


GO
EXECUTE sp_addrolemember @rolename = N'Developer', @membername = N'Narsi.Ienampudi';

