CREATE PROCEDURE [dbo].[e_FilterSegmentationGroup_Select_FilterSegmentationID]
@FilterSegmentationID int
AS
Begin

		SET NOCOUNT ON

 		select 
			*
		from  
			FilterSegmentationGroup with(nolock) 
		where  
			FilterSegmentationID  = @FilterSegmentationID 
End
