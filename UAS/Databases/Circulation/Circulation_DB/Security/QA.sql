CREATE ROLE [QA]
    AUTHORIZATION [dbo];




GO
EXECUTE sp_addrolemember @rolename = N'QA', @membername = N'jaime.mohs';


GO

EXECUTE sp_addrolemember @rolename = N'QA', @membername = N'kay.molencamp';


GO
EXECUTE sp_addrolemember @rolename = N'QA', @membername = N'meghan.salim';

