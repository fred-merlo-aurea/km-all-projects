CREATE PROCEDURE [dbo].[e_Adhoc_Delete_CategoryID]
	@CategoryID int
AS
BEGIN

	set nocount on

	DELETE Adhoc WHERE CategoryID = @CategoryID
END