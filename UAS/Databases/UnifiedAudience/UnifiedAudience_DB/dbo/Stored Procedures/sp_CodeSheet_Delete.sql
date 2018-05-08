CREATE proc [dbo].[sp_CodeSheet_Delete](
@CodeSheetID int
)
as
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

End