CREATE PROCEDURE e_SubscriberTransformed_Select_SourceFileID_IsLatLonValid
@SourceFileID int,
@IsLatLonValid bit
AS
BEGIN

	SET NOCOUNT ON

	SELECT *
	FROM SubscriberTransformed With(NoLock)
	WHERE SourceFileID = @SourceFileID AND IsLatLonValid = @IsLatLonValid

END
GO