CREATE PROCEDURE dc_MatchProfileInsertDetail
@dcResultQueId int,
@DataCompareResultId int,
@stdProfCount int,
@premProfCount int,
@totalProfCount int,
@FileName varchar(250),
@clientId int,
@parentClientId int
AS
BEGIN

	set nocount on

	declare @ctPS int = (select CodeTypeId from UAD_Lookup..CodeType with(nolock) where CodeTypeName = 'Profile Standard Attributes')
	declare @ctPP int = (select CodeTypeId from UAD_Lookup..CodeType with(nolock) where CodeTypeName = 'Profile Premium Attributes')
	declare @dcOptId int = (select DataCompareOptionId from DataCompareOption with(nolock) where OptionName = 'Matching Profiles')

	declare @psItemCount int = 0
	set @psItemCount = (select COUNT(cm.DataCompareOptionCodeMapId) * @stdProfCount
								from DataCompareUserMatrix um with(nolock)
								join DataCompareOptionCodeMap cm with(nolock) on um.DataCompareOptionCodeMapId = cm.DataCompareOptionCodeMapId
								where um.DataCompareResultQueId = @dcResultQueId
								and um.IsActive = 'true'
								and cm.CodeTypeId = @ctPS)
				
    declare @ppItemCount int = 0
	set @ppItemCount = (select COUNT(cm.DataCompareOptionCodeMapId) * @premProfCount
								from DataCompareUserMatrix um with(nolock)
								join DataCompareOptionCodeMap cm with(nolock) on um.DataCompareOptionCodeMapId = cm.DataCompareOptionCodeMapId
								where um.DataCompareResultQueId = @dcResultQueId
								and um.IsActive = 'true'
								and cm.CodeTypeId = @ctPP)

	declare @userId int = 0
	set @userId = (select q.UserId 
						   from DataCompareResultQue q with(nolock) 
						   join DataCompareResult r with(nolock) on q.DataCompareResultQueId= r.DataCompareResultQueId
						   where r.DataCompareResultId = @DataCompareResultId)

	declare @dcCost decimal(20,10) = 0;
	set @dcCost = (select CostPerItem from DataCompareCost with(nolock) where DataCompareOptionId = @dcOptId and ParentClientId = @parentClientId)

	declare @ctPSCost decimal(20,10) = 0;
	set @ctPSCost = (select CodeTypeIdCostPerItem from DataCompareCodeTypeCost with(nolock) where CodeTypeId = @ctPS)

	declare @ctPPCost decimal(20,10) = 0;
	set @ctPPCost = (select CodeTypeIdCostPerItem from DataCompareCodeTypeCost with(nolock) where CodeTypeId = @ctPP)

	declare @costMod decimal(20,10) = 1
	
	if exists (select ParentClientId from DataCompareCostToUser with(nolock) where UserId = @userId and ParentClientId = @parentClientId)
		begin
			set @costMod = (select CostPerItemModifier from DataCompareCostToUser with(nolock) where UserId = @userId and ParentClientId = @parentClientId)
		end
	else if exists (select ParentClientId from DataCompareCostToClient with(nolock) where ClientId = @clientId and ParentClientId = @parentClientId)
		begin
			 set @costMod = (select CostPerItemModifier from DataCompareCostToClient with(nolock) where ClientId = @clientId and ParentClientId = @parentClientId)
		end		

	declare @psCostPerItem decimal(20,10) = 0;
	set @psCostPerItem = ((@dcCost + @ctPSCost) * @costMod)

	if(@psCostPerItem is null)
		begin
			set @psCostPerItem = 0
		end

	declare @psCostDetail varchar(500) = ('DCCost: ' + cast(@dcCost as varchar(21)) + ' Std Prof: ' + cast(@ctPSCost as varchar(21)) + ' times CostMod: ' + cast(@costMod as varchar(21)))
	if(@psCostDetail is null)
		begin
			set @psCostDetail = ''
		end

	declare @psTotalCost decimal(20,10) = 0;
	set @psTotalCost = (@psCostPerItem * @psItemCount);

	if(@psTotalCost is null)
		begin
			set @psTotalCost = 0
		end

	--Insert into DataCompareResultCount
	--Standard Profile Item Costs	
	insert into DataCompareResultCount (DataCompareResultId,DataCompareOptionId,CodeTypeId,CostPerItem,CostPerItemDetail,ItemCount,ItemTotalCost)
	values(@DataCompareResultId,@dcOptId,@ctPS,@psCostPerItem,@psCostDetail,@psItemCount,@psTotalCost)
	
	declare @ppCostPerItem decimal(20,10) = 0;
	set @ppCostPerItem = ((@dcCost + @ctPPCost) * @costMod)

	if(@ppCostPerItem is null)
		begin
			set @ppCostPerItem = 0
		end

	declare @ppCostDetail varchar(500) = ('DCCost: ' + cast(@dcCost as varchar(21)) + ' Prem Prof: ' + cast(@ctPPCost as varchar(21)) + ' times CostMod: ' + cast(@costMod as varchar(21)))
	if(@ppCostDetail is null)
		begin
			set @ppCostDetail = ''
		end

	declare @ppTotalCost decimal(20,10) = 0;
	set @ppTotalCost = (@ppCostPerItem * @ppItemCount);

	if(@ppTotalCost is null)
		begin
			set @ppTotalCost = 0
		end

	--Premium Profile Item Costs	
	insert into DataCompareResultCount (DataCompareResultId,DataCompareOptionId,CodeTypeId,CostPerItem,CostPerItemDetail,ItemCount,ItemTotalCost)
	values(@DataCompareResultId,@dcOptId,@ctPP,@ppCostPerItem,@ppCostDetail,@ppItemCount,@ppTotalCost)
	
	declare @totalItemCount int = 0;
	set @totalItemCount = (@psItemCount + @ppItemCount)	
	if(@totalItemCount is null)
		begin
			set @totalItemCount = 0
		end

	declare @totalRecordCount int = @totalProfCount;
	if(@totalRecordCount is null)
		begin
			set @totalRecordCount = 0
		end

	declare @totalCost decimal(20,10) = 0;
	set @totalCost = (@ppTotalCost + @psTotalCost)

	if(@totalCost is null)
		begin
			set @totalCost = 0
		end

	-- Insert into DataCompareResultDetail  ReportFile, TotalItemCount, TotalCost
    insert into DataCompareResultDetail (DataCompareResultId,DataCompareOptionId,ReportFile,TotalRecordCount,TotalItemCount,TotalCost,IsPurchased,IsBilled,DateCreated,CreatedByUserId)
    values(@DataCompareResultId,@dcOptId,@FileName,@totalRecordCount,@totalItemCount,@totalCost,'false','false',GETDATE(),1)

END
go