CREATE PROCEDURE e_SubscriberDemographicFinal_Select_SORecordIdentifier
@SFRecordIdentifier uniqueidentifier
AS
BEGIN

	SET NOCOUNT ON

	SELECT *
	FROM SubscriberDemographicFinal With(NoLock)
	WHERE SFRecordIdentifier = @SFRecordIdentifier

END