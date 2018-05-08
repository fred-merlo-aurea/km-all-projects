create procedure o_DataCompareViewCost_Select
@matchCount int,
@likeCount int,
@clientId int,
@userId int
as
	begin
		set nocount on
		declare @MatchTotal decimal(20,10), @MatchBaseCost decimal(20,10),  @MatchClientCost decimal(20,10),  @MatchUserCost decimal(20,10),  @MatchThirdPartyCost decimal(20,10), 
											@MatchBaseTotal decimal(20,10), @MatchClientTotal decimal(20,10), @MatchUserTotal decimal(20,10), @MatchThirdPartyTotal decimal(20,10), 
				@LikeTotal decimal(20,10),	@LikeBaseCost decimal(20,10),   @LikeClientCost decimal(20,10),	  @LikeUserCost decimal(20,10),	  @LikeThirdPartyCost decimal(20,10),
											@LikeBaseTotal decimal(20,10),  @LikeClientTotal decimal(20,10),  @LikeUserTotal decimal(20,10),  @LikeThirdPartyTotal decimal(20,10)
	
		declare @matchCodeId int = (select CodeId from uad_lookup..Code where CodeName='Match' and CodeTypeId in (select CodeTypeId from uad_lookup..CodeType with(nolock) where CodeTypeName='Data Compare Type'))
		declare @likeCodeId  int = (select CodeId from uad_lookup..Code where CodeName='Like' and CodeTypeId in (select CodeTypeId from uad_lookup..CodeType with(nolock) where CodeTypeName='Data Compare Type'))
		
		declare @clientCostMod decimal(20,10) = (select case when CodeTypeCostModifier = 0 then 1 else isnull(CodeTypeCostModifier,1) end from DataCompareCostClient with(nolock) where ClientId = @clientId and CodeTypeId = @matchCodeId)		
		declare @userCostMod decimal(20,10)	  = (select case when CodeTypeCostModifier = 0 then 1 else isnull(CodeTypeCostModifier,1) end from DataCompareCostUser with(nolock) where ClientId = @clientId and UserId = @userId and CodeTypeId = @matchCodeId)		
		declare @thirdCostMod decimal(20,10)  = (select case when CodeTypeCostModifier = 0 then 1 else isnull(CodeTypeCostModifier,1) end from DataCompareCostThirdParty with(nolock) where ClientId = @clientId and CodeTypeId = @matchCodeId)			

		set @MatchBaseCost		  = (select MatchPricePerRecord from DataCompareRecordPriceRange with(nolock) where @matchCount between MinCount and MaxCount and IsActive='true')
		set @MatchClientCost	  = (select MatchPricePerRecord*@clientCostMod from DataCompareRecordPriceRange with(nolock) where @matchCount between MinCount and MaxCount and IsActive='true')
		set @MatchUserCost		  = (select MatchPricePerRecord*@userCostMod from DataCompareRecordPriceRange with(nolock) where @matchCount between MinCount and MaxCount and IsActive='true')
		set @MatchThirdPartyCost  = (select MatchPricePerRecord*@thirdCostMod from DataCompareRecordPriceRange with(nolock) where @matchCount between MinCount and MaxCount and IsActive='true')

		set @LikeBaseCost        = (select LikePricePerRecord from DataCompareRecordPriceRange with(nolock) where @matchCount between MinCount and MaxCount and IsActive='true')
		set @LikeClientCost      = (select LikePricePerRecord*@clientCostMod from DataCompareRecordPriceRange with(nolock) where @matchCount between MinCount and MaxCount and IsActive='true')
		set @LikeUserCost        = (select LikePricePerRecord*@userCostMod from DataCompareRecordPriceRange with(nolock) where @matchCount between MinCount and MaxCount and IsActive='true')
		set @LikeThirdPartyCost  = (select LikePricePerRecord*@thirdCostMod from DataCompareRecordPriceRange with(nolock) where @matchCount between MinCount and MaxCount and IsActive='true')

		if(@matchCount > 500000)
			begin
				--this will be our last pricing step which is cost per MILLION records
				--take matchCount/1000000 = some decimal : MilRecords
				--take MilRecords * costing logic
				declare @milRecs decimal(16,3) = (select @matchCount/1000000)
				if @MatchUserCost != @MatchBaseCost
					set @MatchTotal = @MatchUserCost * @milRecs;
				else if @MatchClientCost != @MatchBaseCost
					set @MatchTotal = @MatchClientCost * @milRecs;
				else
					set @MatchTotal = @MatchBaseCost * @milRecs;
			end
		else
			begin

				if @MatchUserCost != @MatchBaseCost
					set @MatchTotal = @MatchUserCost * @matchCount;
				else if @MatchClientCost != @MatchBaseCost
					set @MatchTotal = @MatchClientCost * @matchCount;
				else
					set @MatchTotal = @MatchBaseCost * @matchCount;
				--Select max(val)
				--From
				--(
				--  Select @MatchBase as val 
				--  union
				--  Select @MatchClient as val 
				--  union
				--  Select @MatchUser as val 
				--) x
			end

		if(@likeCount > 500000)
			begin
				declare @milLikeRecs decimal(16,3) = (select @likeCount/1000000)
				if @LikeUserCost != @LikeBaseCost
					set @LikeTotal = @LikeUserCost * @milLikeRecs;
				else if @LikeClientCost != @LikeBaseCost
					set @LikeTotal = @LikeClientCost * @milLikeRecs;
				else
					set @LikeTotal = @LikeBaseCost * @milLikeRecs;
			end
		else
			begin
				if @LikeUserCost != @LikeBaseCost
					set @LikeTotal = @LikeUserCost * @likeCount;
				else if @LikeClientCost != @LikeBaseCost
					set @LikeTotal = @LikeClientCost * @likeCount;
				else
					set @LikeTotal = @LikeBaseCost * @likeCount;
			end

		select @MatchTotal as 'MatchCostTotal', @MatchBaseTotal as 'MatchBaseTotal', @MatchClientTotal as 'MatchClientTotal', @MatchUserTotal as 'MatchUserTotal', @MatchThirdPartyTotal as 'MatchThirdPartyTotal', 
				@LikeTotal as 'LikeCostTotal', @LikeBaseTotal  as 'LikeBaseTotal',  @LikeClientTotal  as 'LikeClientTotal',  @LikeUserTotal  as 'LikeUserTotal',  @LikeThirdPartyTotal as 'LikeThirdPartyTotal',
				0 as 'DcRunId', @matchCount as 'MatchRecordCount', @likeCount as 'LikeRecordCount'
	end
go
