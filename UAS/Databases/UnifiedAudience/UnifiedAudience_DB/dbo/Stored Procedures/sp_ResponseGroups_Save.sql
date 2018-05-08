CREATE PROCEDURE [dbo].[sp_ResponseGroups_Save]
@ResponseGroupID int,
@PubID int,
@ResponseGroupName varchar(100),
@DisplayName varchar(100) = '',
@DateCreated datetime = null,
@DateUpdated datetime = null,
@CreatedByUserID int = 0,
@UpdatedByUserID int = 0,
@IsMultipleValue bit = 'false',
@IsRequired bit = 'false',
@IsActive bit = 'true',
@ResponseGroupTypeID int
AS
BEGIN

	SET NOCOUNT ON

	if(@ResponseGroupID > 0)
		begin
			update ResponseGroups
			set ResponseGroupName = @ResponseGroupName,
				DisplayName = @DisplayName,
				DateUpdated = @DateUpdated,
				UpdatedByUserID = @UpdatedByUserID,
				IsMultipleValue = @IsMultipleValue,
				IsRequired = @IsRequired,
				IsActive = @IsActive,
				ResponseGroupTypeID = @ResponseGroupTypeID
			where ResponseGroupID = @ResponseGroupID
			
			select @ResponseGroupID;
		end
	else
		begin
		
			DECLARE @DisplayOrder int
		       
			SELECT @DisplayOrder = MAX(DisplayOrder)+1 from ResponseGroups where PubID = @PubID
	    
			insert into ResponseGroups (PubID,ResponseGroupName,DisplayName, DateCreated, CreatedByUserID, DisplayOrder, IsMultipleValue, IsRequired, IsActive, ResponseGroupTypeID)
			values(@PubID,@ResponseGroupName,@DisplayName, @DateCreated, @CreatedByUserID, @DisplayOrder, @IsMultipleValue, @IsRequired, @IsActive, @ResponseGroupTypeID);
			
			SELECT SCOPE_IDENTITY()
		end

END