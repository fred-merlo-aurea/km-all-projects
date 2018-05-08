create procedure e_ResponseGroup_Save
	@ResponseGroupID int,
	@PubID int,
	@ResponseGroupName varchar(100),
	@DisplayName varchar(100) = '',
	@DateCreated datetime = null,
	@DateUpdated datetime = null,
	@CreatedByUserID int = 0,
	@UpdatedByUserID int = 0,
	@DisplayOrder int = 0,
	@IsMultipleValue bit = 'false',
	@IsRequired bit = 'false',
	@IsActive bit = 'true',
	@WQT_ResponseGroupID int = null,
	@ResponseGroupTypeId int = null
as
BEGIN

	SET NOCOUNT ON

	if(@ResponseGroupID > 0)
		begin
			if(@DateUpdated is null)
				begin
					set @DateUpdated = getdate()
				end

			update ResponseGroups
			set ResponseGroupName = @ResponseGroupName,
				DisplayName = @DisplayName,
				DateUpdated = @DateUpdated,
				UpdatedByUserID = @UpdatedByUserID,
				DisplayOrder = @DisplayOrder,
				IsMultipleValue = @IsMultipleValue,
				IsRequired = @IsRequired,
				IsActive = @IsActive,
				WQT_ResponseGroupID = @WQT_ResponseGroupID,
				ResponseGroupTypeId = @ResponseGroupTypeId
			where ResponseGroupID = @ResponseGroupID
			
			update CodeSheet 
			set ResponseGroup = @ResponseGroupName
			where ResponseGroupID = @ResponseGroupID

			select @ResponseGroupID;
		end
	else
		begin
			if(@DateCreated is null)
				begin
					set @DateCreated = getdate()
				end
			insert into ResponseGroups (PubID,ResponseGroupName,DisplayName, DateCreated, CreatedByUserID, DisplayOrder, IsMultipleValue, IsRequired, IsActive, WQT_ResponseGroupID, ResponseGroupTypeId)
			values(@PubID,@ResponseGroupName,@DisplayName, @DateCreated, @CreatedByUserID, @DisplayOrder, @IsMultipleValue, @IsRequired, @IsActive, @WQT_ResponseGroupID, @ResponseGroupTypeId);select SCOPE_IDENTITY();
		end

END