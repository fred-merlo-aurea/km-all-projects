CREATE PROCEDURE [dbo].[e_MasterCodeSheet_Select_CodeSheetID]
@CodeSheetID int
AS
BEGIN

	set nocount on

	Select *
	from Mastercodesheet mcs  with (nolock)
	inner join MasterGroups mg with (nolock) on mcs.MasterGroupID = mg.MasterGroupID 
	inner join CodeSheet_Mastercodesheet_Bridge cmb with (nolock) on mcs.MasterID=cmb.MasterID 
	where cmb.CodeSheetID = @CodeSheetID 

END