create procedure o_DataCompare_GetCost
@userId int = 0,
@clientId int = 0,
@count int = 0,
@Match_or_Like varchar(20),-- Match or Like enum DataCompareType
@MergePurge_or_Download varchar(50) --MergePurge or Download  enum Data_Compare_Cost
as
	begin
		set nocount on
		declare @cost decimal(20,10)

		declare @matchCodeId int = (select CodeId from uad_lookup..Code where CodeName='Match' and CodeTypeId in (select CodeTypeId from uad_lookup..CodeType with(nolock) where CodeTypeName='Data Compare Type'))
		declare @likeCodeId  int = (select CodeId from uad_lookup..Code where CodeName='Like' and CodeTypeId in (select CodeTypeId from uad_lookup..CodeType with(nolock) where CodeTypeName='Data Compare Type'))
		
		declare @clientCostMod decimal(20,10) = 1
		declare @userCostMod decimal(20,10) = 1	 

		if(@Match_or_Like = 'Match')
			begin
				set @clientCostMod = (select case when CodeTypeCostModifier = 0 then 1 else isnull(CodeTypeCostModifier,1) end from DataCompareCostClient with(nolock) where ClientId = @clientId and CodeTypeId = @matchCodeId)		
				set @userCostMod   = (select case when CodeTypeCostModifier = 0 then 1 else isnull(CodeTypeCostModifier,1) end from DataCompareCostUser with(nolock) where ClientId = @clientId and UserId = @userId and CodeTypeId = @matchCodeId)		
			end
		else if(@Match_or_Like = 'Like')
			begin
				set @clientCostMod = (select case when CodeTypeCostModifier = 0 then 1 else isnull(CodeTypeCostModifier,1) end from DataCompareCostClient with(nolock) where ClientId = @clientId and CodeTypeId = @likeCodeId)		
				set @userCostMod   = (select case when CodeTypeCostModifier = 0 then 1 else isnull(CodeTypeCostModifier,1) end from DataCompareCostUser with(nolock) where ClientId = @clientId and UserId = @userId and CodeTypeId = @likeCodeId)		
			end

		declare @baseCost decimal(20,10) = 0 
		declare @clientCost decimal(20,10) = 0
		declare @userCost decimal(20,10) = 0

		if(@MergePurge_or_Download = 'MergePurge')--MergePurge
			begin
				--return Merge Purge pricing
				if(@Match_or_Like = 'Match')--Match
					begin
						set @baseCost	= (select case when IsMergePurgePerRecordPricing = 'false' then MatchMergePurgeCost else MatchMergePurgeCost * @count end from DataCompareRecordPriceRange with(nolock) where @count between MinCount and MaxCount and IsActive='true')
						set @clientCost = (select case when IsMergePurgePerRecordPricing = 'false' then MatchMergePurgeCost*@clientCostMod else (MatchMergePurgeCost * @count)*@clientCostMod end from DataCompareRecordPriceRange with(nolock) where @count between MinCount and MaxCount and IsActive='true')
						set @userCost	= (select case when IsMergePurgePerRecordPricing = 'false' then MatchMergePurgeCost*@userCostMod else (MatchMergePurgeCost * @count)*@userCostMod end from DataCompareRecordPriceRange with(nolock) where @count between MinCount and MaxCount and IsActive='true')
					end
				else if(@Match_or_Like = 'Like')--Like
					begin
						set @baseCost	= (select case when IsMergePurgePerRecordPricing = 'false' then LikeMergePurgeCost else LikeMergePurgeCost * @count end from DataCompareRecordPriceRange with(nolock) where @count between MinCount and MaxCount and IsActive='true')
						set @clientCost = (select case when IsMergePurgePerRecordPricing = 'false' then LikeMergePurgeCost*@clientCostMod else (LikeMergePurgeCost * @count)*@clientCostMod end from DataCompareRecordPriceRange with(nolock) where @count between MinCount and MaxCount and IsActive='true')
						set @userCost	= (select case when IsMergePurgePerRecordPricing = 'false' then LikeMergePurgeCost*@userCostMod else (LikeMergePurgeCost * @count)*@userCostMod end from DataCompareRecordPriceRange with(nolock) where @count between MinCount and MaxCount and IsActive='true')
					end
			end
		else if (@MergePurge_or_Download = 'Download')
			begin
				--return download pricing
				if(@Match_or_Like = 'Match')--Match
					begin
						set @baseCost	= (select  MatchPricePerRecord * @count from DataCompareRecordPriceRange with(nolock) where @count between MinCount and MaxCount and IsActive='true')
						set @clientCost = (select (MatchPricePerRecord * @count)*@clientCostMod from DataCompareRecordPriceRange with(nolock) where @count between MinCount and MaxCount and IsActive='true')
						set @userCost	= (select (MatchPricePerRecord * @count)*@userCostMod from DataCompareRecordPriceRange with(nolock) where @count between MinCount and MaxCount and IsActive='true')
					end
				else if(@Match_or_Like = 'Like')--Like
					begin
						set @baseCost	= (select  LikePricePerRecord * @count from DataCompareRecordPriceRange with(nolock) where @count between MinCount and MaxCount and IsActive='true')
						set @clientCost = (select (LikePricePerRecord * @count)*@clientCostMod from DataCompareRecordPriceRange with(nolock) where @count between MinCount and MaxCount and IsActive='true')
						set @userCost	= (select (LikePricePerRecord * @count)*@userCostMod from DataCompareRecordPriceRange with(nolock) where @count between MinCount and MaxCount and IsActive='true')
					end
			end


		if @userCost != @baseCost
			select @userCost;
		else if @clientCost != @baseCost
			select @clientCost;
		else
			select @baseCost;
	end
go
		
