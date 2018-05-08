CREATE PROCEDURE [dbo].[e_SubscriberDemographicArchive_Select_PubID]
	@PubID int
AS
BEGIN

	SET NOCOUNT ON

	SELECT *
	FROM SubscriberDemographicArchive With(NoLock)
	WHERE PubID = @PubID
	ORDER BY MAFField

END
GO