CREATE PROCEDURE [dbo].[e_CodeSheetMasterCodeSheetBridge_Delete_CodeSheetID]
	@CodeSheetID int
AS
BEGIN

	DELETE FROM CodeSheet_Mastercodesheet_Bridge WHERE CodeSheetID = @CodeSheetID

END