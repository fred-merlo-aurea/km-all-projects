CREATE PROCEDURE e_SubscriberFinal_Select_ProcessCode_SourceFileID_IsLatLonValid
@ProcessCode varchar(50),
@SourceFileID int,
@IsLatLonValid bit
AS
BEGIN

	SET NOCOUNT ON

	SELECT *
	FROM SubscriberFinal With(NoLock)
	WHERE SourceFileID = @SourceFileID
	AND IsLatLonValid = @IsLatLonValid
	AND ProcessCode = @ProcessCode

END
GO