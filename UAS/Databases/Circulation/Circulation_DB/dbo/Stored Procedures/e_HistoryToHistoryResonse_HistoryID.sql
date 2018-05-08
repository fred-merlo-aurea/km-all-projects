CREATE PROCEDURE [e_HistoryToHistoryResonse_HistoryID]
@HistoryID int
AS
	SELECT HistoryResponseID FROM HistoryToHistoryResonse With(NoLock) WHERE HistoryID = @HistoryID

