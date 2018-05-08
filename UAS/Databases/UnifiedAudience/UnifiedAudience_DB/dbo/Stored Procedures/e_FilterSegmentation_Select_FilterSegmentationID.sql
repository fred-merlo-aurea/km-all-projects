CREATE PROCEDURE [dbo].[e_FilterSegmentation_Select_FilterSegmentationID]
@FilterSegmentationID int
AS
Begin

	SET NOCOUNT ON
	
	select 
		*
	from  
		FilterSegmentation with(nolock) 
	where  
		FilterSegmentationID  = @FilterSegmentationID 
End
