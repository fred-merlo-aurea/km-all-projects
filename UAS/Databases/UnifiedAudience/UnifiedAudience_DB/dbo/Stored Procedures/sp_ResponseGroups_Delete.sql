CREATE proc [dbo].[sp_ResponseGroups_Delete](
@ResponseGroupID int
)
as
Begin

	delete 
	from CodeSheet_Mastercodesheet_Bridge 
	where CodeSheetID in (select c.CodeSheetID from CodeSheet c where ResponseGroupID = @ResponseGroupID)

	delete 
	from PubSubscriptionDetail 
	where CodeSheetID in (select c.CodeSheetID from CodeSheet c where ResponseGroupID = @ResponseGroupID)

	delete 
	from CodeSheet 
	where ResponseGroupID = @ResponseGroupID

	delete 
	from ResponseGroups 
	where ResponseGroupID =  @ResponseGroupID

End