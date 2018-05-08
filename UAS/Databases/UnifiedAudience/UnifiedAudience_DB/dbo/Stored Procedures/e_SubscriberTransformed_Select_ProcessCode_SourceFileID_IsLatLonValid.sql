CREATE PROCEDURE e_SubscriberTransformed_Select_ProcessCode_SourceFileID_IsLatLonValid
@ProcessCode varchar(50),
@SourceFileID int,
@IsLatLonValid bit
AS
BEGIN

	SET NOCOUNT ON

	SELECT *
	FROM SubscriberTransformed With(NoLock)
	WHERE ProcessCode = @ProcessCode
	AND SourceFileID = @SourceFileID
	AND IsLatLonValid = @IsLatLonValid

END