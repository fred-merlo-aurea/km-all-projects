CREATE PROCEDURE [e_HistorySubscription_Select]
AS
BEGIN

	SET NOCOUNT ON

	SELECT * 
	FROM HistorySubscription With(NoLock)

END