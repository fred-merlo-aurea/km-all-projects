CREATE PROCEDURE [dbo].[e_FilterSegmentationGroup_Delete_FilterSegmentationID]
@FilterSegmentationID int
AS
Begin
 		delete
		from  
			FilterSegmentationGroup 
		where  
			FilterSegmentationID  = @FilterSegmentationID 
End
