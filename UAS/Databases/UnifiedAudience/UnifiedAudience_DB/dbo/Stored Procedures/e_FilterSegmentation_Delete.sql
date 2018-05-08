CREATE PROCEDURE [dbo].[e_FilterSegmentation_Delete]
@FilterSegmentationID int,
@UserID int
AS
BEGIN

	SET NOCOUNT ON

	update FilterSegmentation 
	set IsDeleted = 1, UpdatedUserID = @UserID, UpdatedDate=GETDATE() 
	where FilterSegmentationID  = @FilterSegmentationID 

End
