     CREATE PROCEDURE e_Application_Select_ApplicationName
@ApplicationName varchar(255)
AS
SELECT * 
FROM Application With(NoLock)
WHERE ApplicationName = @ApplicationName
