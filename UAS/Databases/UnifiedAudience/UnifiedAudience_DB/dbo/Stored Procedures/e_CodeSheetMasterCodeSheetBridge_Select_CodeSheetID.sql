CREATE PROCEDURE [dbo].[e_CodeSheetMasterCodeSheetBridge_Select_CodeSheetID]
@CodeSheetID int
AS
BEGIN

	set nocount on

	select mc.MasterValue, mc.MasterDesc, mg.DisplayName 
    from CodeSheet_Mastercodesheet_Bridge cmb With(NoLock)
    inner join Mastercodesheet mc With(NoLock) on cmb.MasterID = mc.MasterID 
    inner join MasterGroups mg With(NoLock) on mc.MasterGroupID = mg.MasterGroupID 
    where cmb.CodeSheetID = @CodeSheetID

END
