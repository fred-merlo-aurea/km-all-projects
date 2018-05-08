CREATE ROLE [QA]
    AUTHORIZATION [dbo];


GO
--EXECUTE sp_addrolemember @rolename = N'QA', @membername = N'Robert.Boettcher';


--GO
--EXECUTE sp_addrolemember @rolename = N'QA', @membername = N'Carolyn.Ruff';


GO
EXECUTE sp_addrolemember @rolename = N'QA', @membername = N'meghan.salim';

