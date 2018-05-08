CREATE PROCEDURE [dbo].[e_CodeSheet_Delete_CodeSheetID]
	@CodeSheetID int
AS
BEGIN

	SET NOCOUNT ON

	delete 
	from CodeSheet_Mastercodesheet_Bridge 
	where CodeSheetID = @CodeSheetID

	delete 
	from PubSubscriptionDetail 
	where CodeSheetID = @CodeSheetID

	delete 
	from CodeSheet 
	where CodeSheetID = @CodeSheetID

END