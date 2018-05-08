CREATE PROCEDURE dbo.e_Severity_Select
AS
SELECT *
FROM Severity With(NoLock)
ORDER BY SeverityID
