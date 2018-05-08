CREATE PROCEDURE [e_HistoryToHistoryMarketingMap_Save]
@HistoryID int,
@HistoryMarketingMapID int
AS
	INSERT INTO HistoryToHistoryMarketingMap (HistoryID,HistoryMarketingMapID)
	VALUES(@HistoryID,@HistoryMarketingMapID)
