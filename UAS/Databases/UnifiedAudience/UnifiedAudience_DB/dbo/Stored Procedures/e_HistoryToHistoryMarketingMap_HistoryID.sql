CREATE PROCEDURE [e_HistoryToHistoryMarketingMap_HistoryID]
@HistoryID int
AS
BEGIN

	SET NOCOUNT ON

	SELECT HistoryMarketingMapID 
	FROM HistoryToHistoryMarketingMap With(NoLock) 
	WHERE HistoryID = @HistoryID

END