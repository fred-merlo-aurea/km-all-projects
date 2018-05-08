CREATE PROCEDURE dc_LikeDemoInsertDetail
@dcResultQueId int,
@DataCompareResultId int,
@stdDemoCount int,
@premDemoCount int,
@custDemoCount int,
@FileName varchar(250),
@clientId int,
@parentClientId int
AS
BEGIN

	set nocount on

	declare @ctDS int = (select CodeTypeId from UAD_Lookup..CodeType with(nolock) where CodeTypeName = 'Demographic Standard Attributes')
	declare @ctDP int = (select CodeTypeId from UAD_Lookup..CodeType with(nolock) where CodeTypeName = 'Demographic Premium Attributes')
	declare @ctDC int = (select CodeTypeId from UAD_Lookup..CodeType with(nolock) where CodeTypeName = 'Demographic Custom Attributes')
	declare @dcOptId int = (select DataCompareOptionId from DataCompareOption with(nolock) where OptionName = 'Like Demographics')

	declare @dsItemCount int = 0
	set @dsItemCount = (select COUNT(cm.DataCompareOptionCodeMapId) * @stdDemoCount
								from DataCompareUserMatrix um with(nolock)
								join DataCompareOptionCodeMap cm with(nolock) on um.DataCompareOptionCodeMapId = cm.DataCompareOptionCodeMapId
								where um.DataCompareResultQueId = @dcResultQueId
								and um.IsActive = 'true'
								and cm.CodeTypeId = @ctDS)

	declare @dpItemCount int = 0
	set @dpItemCount = (select COUNT(cm.DataCompareOptionCodeMapId) * @premDemoCount
								from DataCompareUserMatrix um with(nolock)
								join DataCompareOptionCodeMap cm with(nolock) on um.DataCompareOptionCodeMapId = cm.DataCompareOptionCodeMapId
								where um.DataCompareResultQueId = @dcResultQueId
								and um.IsActive = 'true'
								and cm.CodeTypeId = @ctDP)

	declare @dcItemCount int = 0
	set @dcItemCount = (select COUNT(cm.DataCompareOptionCodeMapId) * @custDemoCount
								from DataCompareUserMatrix um with(nolock)
								join DataCompareOptionCodeMap cm with(nolock) on um.DataCompareOptionCodeMapId = cm.DataCompareOptionCodeMapId
								where um.DataCompareResultQueId = @dcResultQueId
								and um.IsActive = 'true'
								and cm.CodeTypeId = @ctDC)

	declare @userId int = (select q.UserId 
						   from DataCompareResultQue q with(nolock) 
						   join DataCompareResult r with(nolock) on q.DataCompareResultQueId= r.DataCompareResultQueId
						   where r.DataCompareResultId = @DataCompareResultId)

	declare @dcCost decimal(20,10) = 0
	set @dcCost = (select CostPerItem from DataCompareCost with(nolock) where DataCompareOptionId = @dcOptId and ParentClientId = @parentClientId)

	declare @ctDSCost decimal(20,10) = 0
	set @ctDSCost = (select CodeTypeIdCostPerItem from DataCompareCodeTypeCost with(nolock) where CodeTypeId = @ctDS)

	declare @ctDPCost decimal(20,10) = 0
	set @ctDPCost = (select CodeTypeIdCostPerItem from DataCompareCodeTypeCost with(nolock) where CodeTypeId = @ctDP)

	declare @ctDCCost decimal(20,10) = 0
	set @ctDCCost = (select CodeTypeIdCostPerItem from DataCompareCodeTypeCost with(nolock) where CodeTypeId = @ctDC)

	declare @costMod decimal(20,10) = 1
	if exists (select ParentClientId from DataCompareCostToUser with(nolock) where UserId = @userId and ParentClientId = @parentClientId)
		begin
			set @costMod = (select CostPerItemModifier from DataCompareCostToUser with(nolock) where UserId = @userId and ParentClientId = @parentClientId)
		end
	else if exists (select ParentClientId from DataCompareCostToClient with(nolock) where ClientId = @clientId and ParentClientId = @parentClientId)
		begin
			 set @costMod = (select CostPerItemModifier from DataCompareCostToClient with(nolock) where ClientId = @clientId and ParentClientId = @parentClientId)
		end		
		
	declare @dsCostPerItem decimal(20,10) = 0;
	set @dsCostPerItem = ((@dcCost + @ctDSCost) * @costMod)
	if(@dsCostPerItem is null)
		begin
			set @dsCostPerItem = 0
		end

	declare @dsCostDetail varchar(500) = ('DCCost: ' + cast(@dcCost as varchar(21)) + ' Std Demo: ' + cast(@ctDSCost as varchar(21)) + ' times CostMod: ' + cast(@costMod as varchar(21)))
	if(@dsCostDetail is null)
		begin
			set @dsCostDetail = ''
		end

	declare @dsTotalCost decimal(20,10) = 0;
	set @dsTotalCost = (@dsCostPerItem * @dsItemCount);
	if(@dsTotalCost is null)
		begin
			set @dsTotalCost = 0
		end
	--Insert into DataCompareResultCount
	--Standard Demo Item Costs	
	insert into DataCompareResultCount (DataCompareResultId,DataCompareOptionId,CodeTypeId,CostPerItem,CostPerItemDetail,ItemCount,ItemTotalCost)
	values(@DataCompareResultId,@dcOptId,@ctDS,@dsCostPerItem,@dsCostDetail,@dsItemCount,@dsTotalCost)
	
	declare @dpCostPerItem decimal(20,10) = 0;
	set @dpCostPerItem = ((@dcCost + @ctDPCost) * @costMod)
	if(@dpCostPerItem is null)
		begin
			set @dpCostPerItem = 0
		end

	declare @dpCostDetail varchar(500) = ('DCCost: ' + cast(@dcCost as varchar(21)) + ' Prem Demo: ' + cast(@ctDPCost as varchar(21)) + ' times CostMod: ' + cast(@costMod as varchar(21)))
	if(@dpCostDetail is null)
		begin
			set @dpCostDetail = ''
		end

	declare @dpTotalCost decimal(20,10) = 0;
	set @dpTotalCost = (@dpCostPerItem * @dpItemCount);
	if(@dpTotalCost is null)
		begin
			set @dpTotalCost = 0
		end

	--Premium Demo Item Costs	
	insert into DataCompareResultCount (DataCompareResultId,DataCompareOptionId,CodeTypeId,CostPerItem,CostPerItemDetail,ItemCount,ItemTotalCost)
	values(@DataCompareResultId,@dcOptId,@ctDP,@dpCostPerItem,@dpCostDetail,@dpItemCount,@dpTotalCost)
	
	declare @dcCostPerItem decimal(20,10) = 0;
	set @dcCostPerItem = ((@dcCost + @ctDCCost) * @costMod)
	if(@dcCostPerItem is null)
		begin
			set @dcCostPerItem = 0
		end

	declare @dcCostDetail varchar(500) = ('DCCost: ' + cast(@dcCost as varchar(21)) + ' Custom Demo: ' + cast(@ctDCCost as varchar(21)) + ' times CostMod: ' + cast(@costMod as varchar(21)))
	if(@dcCostDetail is null)
		begin
			set @dcCostDetail = ''
		end

	declare @dcTotalCost decimal(20,10) = 0;
	set @dcTotalCost = (@dcCostPerItem * @dcItemCount);
	if(@dcTotalCost is null)
		begin
			set @dcTotalCost = 0
		end

	--Custom Demo Item Costs	
	insert into DataCompareResultCount (DataCompareResultId,DataCompareOptionId,CodeTypeId,CostPerItem,CostPerItemDetail,ItemCount,ItemTotalCost)
	values(@DataCompareResultId,@dcOptId,@ctDC,@dcCostPerItem,@dcCostDetail,@dcItemCount,@dcTotalCost)
	
		
	declare @totalItemCount int = 0;
	declare @totalRecordCount int = 0;
	set @totalRecordCount = (@stdDemoCount + @premDemoCount + @custDemoCount)
	set @totalItemCount = (@dsItemCount + @dpItemCount + @dcItemCount)	

	declare @totalCost decimal(20,10) = 0;
	set @totalCost = (@dpTotalCost + @dsTotalCost + @dcTotalCost)

	if(@totalItemCount is null)
		begin
			set @totalItemCount = 0
		end
	if(@totalRecordCount is null)
		begin
			set @totalRecordCount = 0
		end
	if(@totalCost is null)
		begin
			set @totalCost = 0
		end
	-- Insert into DataCompareResultDetail  ReportFile, TotalItemCount, TotalCost
    insert into DataCompareResultDetail (DataCompareResultId,DataCompareOptionId,ReportFile,TotalRecordCount,TotalItemCount,TotalCost,IsPurchased,IsBilled,DateCreated,CreatedByUserId)
    values(@DataCompareResultId,@dcOptId,@FileName,@totalRecordCount,@totalItemCount,@totalCost,'false','false',GETDATE(),1)

END
go