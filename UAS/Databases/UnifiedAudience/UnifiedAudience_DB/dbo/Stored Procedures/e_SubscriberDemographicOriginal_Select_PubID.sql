CREATE PROCEDURE e_SubscriberDemographicOriginal_Select_PubID
@PubID int
AS
BEGIN

	SET NOCOUNT ON

	SELECT *
	FROM SubscriberDemographicOriginal With(NoLock)
	WHERE PubID = @PubID

END