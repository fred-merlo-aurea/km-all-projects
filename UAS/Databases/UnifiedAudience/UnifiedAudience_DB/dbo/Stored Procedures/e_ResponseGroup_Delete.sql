CREATE PROCEDURE [dbo].[e_ResponseGroup_Delete]
	@ResponseGroupID int
AS
BEGIN

	SET NOCOUNT ON

	DELETE 
	from CodeSheet_Mastercodesheet_Bridge 
	where CodeSheetID in (select c.CodeSheetID from CodeSheet c with(nolock) where ResponseGroupID = @ResponseGroupID)

	DELETE 
	from PubSubscriptionDetail 
	where CodeSheetID in (select c.CodeSheetID from CodeSheet c with(nolock) where ResponseGroupID = @ResponseGroupID)

	DELETE 
	from CodeSheet 
	where ResponseGroupID = @ResponseGroupID

	DELETE 
	from ResponseGroups 
	where ResponseGroupID =  @ResponseGroupID

END