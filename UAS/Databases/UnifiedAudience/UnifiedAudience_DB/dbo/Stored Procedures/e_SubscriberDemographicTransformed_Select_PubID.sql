CREATE PROCEDURE [dbo].[e_SubscriberDemographicTransformed_Select_PubID]
@PubID int
AS
BEGIN

	SET NOCOUNT ON

	SELECT *
	FROM SubscriberDemographicTransformed With(NoLock)
	WHERE PubID = @PubID

END
GO