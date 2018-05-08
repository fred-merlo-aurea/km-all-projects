CREATE PROCEDURE [dbo].[e_CodeSheetMasterCodeSheetBridge_Delete_MasterID]
	@MasterID int
AS	
BEGIN

	set nocount on

	DELETE FROM CodeSheet_Mastercodesheet_Bridge WHERE MasterID = @MasterID

END