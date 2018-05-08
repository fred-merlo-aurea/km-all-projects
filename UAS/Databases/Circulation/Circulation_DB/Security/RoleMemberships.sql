EXECUTE sp_addrolemember @rolename = N'db_owner', @membername = N'justin.wagner';


GO
EXECUTE sp_addrolemember @rolename = N'db_owner', @membername = N'Jason.Meier';


GO
EXECUTE sp_addrolemember @rolename = N'db_datareader', @membername = N'SubscriptionManager';


GO
EXECUTE sp_addrolemember @rolename = N'db_datawriter', @membername = N'SubscriptionManager';


GO


