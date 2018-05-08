CREATE PROCEDURE [dbo].[e_FilterSegmentationGroup_Save]
@FilterSegmentationGroupID int,
@FilterSegmentationID int,
@FilterGroupID_Selected varchar(500),
@FilterGroupID_Suppressed varchar(500),
@SelectedOperation varchar(500),
@SuppressedOperation varchar(500)
AS
BEGIN

	set nocount on

	if (@FilterSegmentationGroupID > 0)
	begin
		update FilterSegmentationGroup 
			set FilterGroupID_Selected = @FilterGroupID_Selected, 
				FilterGroupID_Suppressed = @FilterGroupID_Suppressed, 
				SelectedOperation = @SelectedOperation, 
				SuppressedOperation = @SuppressedOperation,
				FilterSegmentationID =  @FilterSegmentationID				
			where FilterSegmentationGroupID = @FilterSegmentationGroupID
		select @FilterSegmentationGroupID;
	end
	else
	begin
		insert into FilterSegmentationGroup (FilterGroupID_Selected, FilterGroupID_Suppressed, SelectedOperation, SuppressedOperation, FilterSegmentationID) 
		values (@FilterGroupID_Selected, @FilterGroupID_Suppressed, @SelectedOperation, @SuppressedOperation, @FilterSegmentationID)
		
		select @@IDENTITY;
    end
End