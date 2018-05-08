CREATE PROCEDURE [dbo].[e_APILogging_UpdateLog]
	@APILogID int,
	@LogID int = null
AS 
BEGIN
	
	set nocount on

	UPDATE APILogging 
	SET LogID = @LogID, EndTime = GETDATE()
	WHERE APILogID = @APILogID

END
GO
