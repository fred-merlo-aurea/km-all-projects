CREATE PROCEDURE [dbo].[e_SubscriberDemographicInvalid_Select_SORecordIdentifier]
@SORecordIdentifier uniqueidentifier
AS
BEGIN

	SET NOCOUNT ON

	SELECT *
	FROM SubscriberDemographicInvalid With(NoLock)
	WHERE SORecordIdentifier = @SORecordIdentifier

END
GO