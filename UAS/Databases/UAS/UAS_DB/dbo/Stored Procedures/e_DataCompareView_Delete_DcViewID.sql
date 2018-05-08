CREATE PROCEDURE [dbo].[e_DataCompareView_Delete_DcViewID]
@DcViewID int
AS
BEGIN

	set nocount on

	DELETE DataCompareView 
	WHERE DcViewID = @DcViewID
END
