CREATE PROCEDURE [dbo].[e_PubSubscriptionDetail_Delete_CodeSheetID]
	@CodeSheetID int
AS
BEGIN

	DELETE 
	FROM PubSubscriptionDetail 
	WHERE CodeSheetID = @CodeSheetID

END