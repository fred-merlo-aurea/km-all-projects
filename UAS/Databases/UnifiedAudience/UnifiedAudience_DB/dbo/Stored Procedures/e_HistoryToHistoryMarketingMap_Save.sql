CREATE PROCEDURE [e_HistoryToHistoryMarketingMap_Save]
@HistoryID int,
@HistoryMarketingMapID int
AS
BEGIN

	SET NOCOUNT ON

	INSERT INTO HistoryToHistoryMarketingMap (HistoryID,HistoryMarketingMapID)
	VALUES(@HistoryID,@HistoryMarketingMapID)

END