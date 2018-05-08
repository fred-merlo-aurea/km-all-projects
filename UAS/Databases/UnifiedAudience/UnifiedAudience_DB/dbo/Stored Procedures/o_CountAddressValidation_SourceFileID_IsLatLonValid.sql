CREATE PROCEDURE o_CountAddressValidation_SourceFileID_IsLatLonValid
@SourceFileID int,
@IsLatLonValid bit
AS
BEGIN

	SET NOCOUNT ON

	SELECT Count(STRecordIdentifier)
	FROM SubscriberTransformed With(NoLock)
	WHERE SourceFileID = @SourceFileID
	AND IsLatLonValid = @IsLatLonValid

END
GO