CREATE PROCEDURE [e_HistoryToUserLog_HistoryID]
@HistoryID int
AS
BEGIN

	SET NOCOUNT ON

	SELECT UserLogID 
	FROM HistoryToUserLog With(NoLock) 
	WHERE HistoryID = @HistoryID

END