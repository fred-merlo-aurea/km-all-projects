CREATE PROCEDURE e_SubscriberOriginal_Select_SourceFileID_ProcessCode
@SourceFileID int,
@ProcessCode varchar(50)
AS
BEGIN

	SET NOCOUNT ON

	SELECT *
	FROM SubscriberOriginal With(NoLock)
	WHERE SourceFileID = @SourceFileID AND ProcessCode = @ProcessCode 
	ORDER BY ImportRowNumber

END