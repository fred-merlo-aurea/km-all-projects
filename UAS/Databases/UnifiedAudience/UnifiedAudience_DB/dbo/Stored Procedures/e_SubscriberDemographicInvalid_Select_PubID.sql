CREATE PROCEDURE [dbo].[e_SubscriberDemographicInvalid_Select_PubID]
@PubID int
AS
BEGIN

	SET NOCOUNT ON

	SELECT *
	FROM SubscriberDemographicFinal With(NoLock)
	WHERE PubID = @PubID

END
GO