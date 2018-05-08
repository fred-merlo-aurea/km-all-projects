CREATE  PROC [dbo].[e_APILogging_UpdateLog] 
(
	@APILogID int,
	@LogID int = null
)
AS 
BEGIN
	UPDATE APILogging 
	SET LogID = @LogID, EndTime = GETDATE()
	WHERE APILogID = @APILogID
END