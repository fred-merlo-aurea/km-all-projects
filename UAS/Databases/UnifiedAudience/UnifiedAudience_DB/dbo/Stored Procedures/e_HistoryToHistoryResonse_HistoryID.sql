CREATE PROCEDURE [e_HistoryToHistoryResonse_HistoryID]
@HistoryID int
AS
BEGIN

	SET NOCOUNT ON

	SELECT HistoryResponseID 
	FROM HistoryToHistoryResonse With(NoLock) 
	WHERE HistoryID = @HistoryID

END