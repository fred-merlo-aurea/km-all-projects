CREATE PROCEDURE [dbo].[o_MasterData_Select]
AS
BEGIN

	SET NOCOUNT ON

	SELECT cmb.CodeSheetID, cmb.MasterID, mc.MasterValue, mc.MasterDesc, mg.DisplayName
	FROM CodeSheet_Mastercodesheet_Bridge cmb With(NoLock)
		inner join Mastercodesheet mc With(NoLock) on cmb.MasterID = mc.MasterID
		inner join MasterGroups mg With(NoLock) on mc.MasterGroupID = mg.MasterGroupID

END