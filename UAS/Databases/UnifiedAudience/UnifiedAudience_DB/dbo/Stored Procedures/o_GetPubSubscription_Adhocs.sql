CREATE PROCEDURE [dbo].[o_GetPubSubscription_Adhocs] 
@PubID int
AS
BEGIN

	SET NOCOUNT ON

	SELECT DISTINCT CustomField
	FROM PubSubscriptionsExtensionMapper
	WHERE PubID = @PubID

END