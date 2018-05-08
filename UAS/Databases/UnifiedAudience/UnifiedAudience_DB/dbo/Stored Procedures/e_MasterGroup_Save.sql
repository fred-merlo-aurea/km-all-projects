CREATE proc [dbo].[e_MasterGroup_Save]
	@MasterGroupID int,
	@DisplayName varchar(50), 
	@Name varchar(100), 
	@IsActive bit , 
	@EnableSubReporting bit, 
	@EnableSearching bit = 'true', 
	@EnableAdhocSearch bit = 'true',
	@SortOrder int = 0,
	@DateCreated datetime = null,
	@DateUpdated datetime = null,
	@CreatedByUserID int = 0,
	@UpdatedByUserID int = 0
as
Begin

	SET NOCOUNT ON

	if @MasterGroupID > 0
		begin
			if(@DateUpdated is null)
				begin
					set @DateUpdated = getdate()
				end

			UPDATE [MasterGroups] 
			SET [DisplayName] = @DisplayName, 
				[Name] = @Name, 
				[Description] = @Name,  
				[IsActive] = @IsActive, 
				[SortOrder] = @SortOrder,
				[EnableSubReporting] = @EnableSubReporting, 
				[EnableSearching] = @EnableSearching, 
				[EnableAdhocSearch] = @EnableAdhocSearch ,
				DateUpdated = @DateUpdated,
				UpdatedByUserID = @UpdatedByUserID
			WHERE [MasterGroupID] = @MasterGroupID
			select @MasterGroupID;
		end
	else
		begin
			if(@DateCreated is null)
				begin
					set @DateCreated = getdate()
				end
			INSERT INTO 
			[MasterGroups] ([DisplayName], [Name], [Description], [IsActive],[EnableSubReporting], [EnableSearching], [EnableAdhocSearch], ColumnReference, DateCreated, CreatedByUserID) 
			VALUES ( @DisplayName, @Name, @Name, @IsActive, @EnableSubReporting, @EnableSearching, @EnableAdhocSearch, 'MASTER_' + REPLACE (@Name, ' ', '_'), @DateCreated, @CreatedByUserID)
			select @@IDENTITY;
		end

END