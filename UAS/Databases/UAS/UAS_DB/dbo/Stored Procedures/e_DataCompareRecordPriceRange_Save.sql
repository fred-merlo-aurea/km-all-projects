create procedure e_DataCompareRecordPriceRange_Save
@DcRecordPriceRangeId int,
@MinCount int,
@MaxCount int,
@MatchMergePurgeCost decimal(20, 10),
@MatchPricePerRecord decimal(20,10),
@LikeMergePurgeCost decimal(20, 10),
@LikePricePerRecord decimal(20,10),
@IsMergePurgePerRecordPricing bit = 'false',
@IsActive bit = 'true',
@DateCreated datetime,
@CreatedByUserId int,
@DateUpdated datetime = null,
@UpdatedByUserId int = null 
as
	begin
		set nocount on
		
		if @DcRecordPriceRangeId > 0
		begin
			if @DateUpdated IS NULL
				begin
					set @DateUpdated = getdate();
				end
			update DataCompareRecordPriceRange
			set 
				MinCount = @MinCount,
				MaxCount = @MaxCount,
				MatchMergePurgeCost = @MatchMergePurgeCost,
				MatchPricePerRecord = @MatchPricePerRecord,
				LikeMergePurgeCost = @LikeMergePurgeCost,
				LikePricePerRecord = @LikePricePerRecord,
				IsMergePurgePerRecordPricing = @IsMergePurgePerRecordPricing,
				IsActive = @IsActive,
				DateUpdated = @DateUpdated,
				UpdatedByUserID = @UpdatedByUserID
			where DcRecordPriceRangeId = @DcRecordPriceRangeId;
		
			select @DcRecordPriceRangeId;
		end
	else
		begin
			if @DateCreated IS NULL
				begin
					set @DateCreated = getdate();
				end

			insert into DataCompareRecordPriceRange (MinCount,MaxCount,MatchMergePurgeCost,MatchPricePerRecord,LikeMergePurgeCost,LikePricePerRecord,IsMergePurgePerRecordPricing,IsActive,DateCreated,CreatedByUserId)
			values(@MinCount,@MaxCount,@MatchMergePurgeCost,@MatchPricePerRecord,@LikeMergePurgeCost,@LikePricePerRecord,@IsMergePurgePerRecordPricing,@IsActive,@DateCreated,@CreatedByUserId);select @@IDENTITY;
		end
	end
go
