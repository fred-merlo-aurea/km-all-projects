CREATE PROCEDURE [dbo].[e_FilterSegmentation_Save]
@FilterSegmentationID int,
@FilterSegmentationName varchar(50),
@Notes varchar(250),
@FilterID int,
@IsDeleted bit,
@CreatedUserID int,
@CreatedDate datetime,
@UpdatedUserID int,
@UpdatedDate datetime
AS
BEGIN

	set nocount on

	if (@FilterSegmentationID > 0)
	begin
		update FilterSegmentation 
			set FilterSegmentationName = @FilterSegmentationName, 
				Notes = @Notes, 
				FilterID = @FilterID, 
				IsDeleted = @IsDeleted,
				UpdatedUserID = @UpdatedUserID, 
				UpdatedDate = GETDATE()  
			where FilterSegmentationID = @FilterSegmentationID
		select @FilterSegmentationID;
	end
	else
	begin
		insert into FilterSegmentation (FilterSegmentationName, Notes, FilterID, IsDeleted, CreatedUserID, CreatedDate) 
		values (@FilterSegmentationName, @Notes, @FilterID, @IsDeleted, @CreatedUserID, @CreatedDate)
		
		select @@IDENTITY;
    end
End

