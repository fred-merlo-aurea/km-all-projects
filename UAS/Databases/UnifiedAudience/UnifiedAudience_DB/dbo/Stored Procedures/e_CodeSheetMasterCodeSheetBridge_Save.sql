CREATE PROCEDURE [dbo].[e_CodeSheetMasterCodeSheetBridge_Save]
	@CodeSheetID int,
	@MasterID int
AS
BEGIN

	set nocount on

	INSERT INTO CodeSheet_Mastercodesheet_Bridge (CodeSheetID, MasterID) 
	VALUES (@CodeSheetID, @MasterID)	

END