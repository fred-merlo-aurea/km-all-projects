CREATE PROCEDURE [dbo].[e_Database_Select]	
AS
BEGIN

	SET NOCOUNT ON

	SELECT name as 'DatabaseName' 
	FROM sys.databases With(NoLock) 
	where database_id not in (1,2,3,4)

END