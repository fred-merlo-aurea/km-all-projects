
CREATE PROCEDURE [e_HistoryToHistoryMarketingMap_HistoryID]
@HistoryID int
AS
	SELECT HistoryMarketingMapID FROM HistoryToHistoryMarketingMap With(NoLock) WHERE HistoryID = @HistoryID

