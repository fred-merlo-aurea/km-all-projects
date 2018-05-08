CREATE PROCEDURE e_SubscriberDemographicOriginal_Select_SORecordIdentifier
@SORecordIdentifier uniqueidentifier
AS
BEGIN

	SET NOCOUNT ON

	SELECT *
	FROM SubscriberDemographicOriginal With(NoLock)
	WHERE SORecordIdentifier = @SORecordIdentifier

END