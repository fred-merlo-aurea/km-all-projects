CREATE PROCEDURE e_SubscriberDemographicFinal_Select_PubID
@PubID int
AS
BEGIN

	SET NOCOUNT ON

	SELECT *
	FROM SubscriberDemographicFinal With(NoLock)
	WHERE PubID = @PubID

END