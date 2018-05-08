CREATE PROCEDURE [e_HistoryToUserLog_HistoryID]
@HistoryID int
AS
	SELECT UserLogID FROM HistoryToUserLog With(NoLock) WHERE HistoryID = @HistoryID
