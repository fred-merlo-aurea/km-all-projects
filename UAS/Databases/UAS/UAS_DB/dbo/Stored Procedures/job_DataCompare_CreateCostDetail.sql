create procedure job_DataCompare_CreateCostDetail
@dcViewId int,
@dcTypeCodeID int,
@profileCount int,
@profileColumns varchar(max),
@demoColumns varchar(max), 
@userId int
as
	begin
		set nocount on
		--inserts to DataCompareDownloadCostDetail
		--2 possible types of Profile Columns
		--3 possible types of Dim columns
		--first step will be to get Profile/Dim columns into tables
		declare @demoCust int = (select CodeTypeId from Uad_Lookup..CodeType with(nolock) where CodeTypeName = 'Demographic Custom Attributes')
		declare @demoPrem int = (select CodeTypeId from Uad_Lookup..CodeType with(nolock) where CodeTypeName = 'Demographic Premium Attributes')
		declare @demoStd int = (select CodeTypeId from Uad_Lookup..CodeType with(nolock) where CodeTypeName = 'Demographic Standard Attributes')

		declare @profPrem int = (select CodeTypeId from Uad_Lookup..CodeType with(nolock) where CodeTypeName = 'Profile Premium Attributes')
		declare @profStd int = (select CodeTypeId from Uad_Lookup..CodeType with(nolock) where CodeTypeName = 'Profile Standard Attributes')

		declare @clientId int = (select r.ClientId 
								 from DataCompareRun r with(nolock)
								 join DataCompareView v with(nolock) on r.DcRunId = v.DcRunId
								 where v.DcViewId = @dcViewId)

		--since we can't store anything in the database I am just going to return what would be a DataCompareDownloadCostDetail object
		declare @Data table(DataColumn varchar(100), CodeTypeId int, CostPerItemClient decimal(19,4), CostPerItemThirdParty decimal(19,4)) 
		insert into @Data 
		select *,0,0,0
		from master.dbo.fn_Split(@profileColumns,',')
		union
		select *,0,0,0
		from master.dbo.fn_Split(@demoColumns,',')
		
		--select f.[Column], f.DcDownloadFieldCodeId, b.CostPerItem, b.CostPerItem * isnull(t.CodeTypeCostModifier, 1)
		--from DataCompareDownloadField f with(nolock)
		--join DataCompareCostBase b with(nolock) on f.DcDownloadFieldCodeId = b.CodeTypeId
		--left join DataCompareCostThirdParty t with(nolock) on f.DcDownloadFieldCodeId = t.CodeTypeId
		--where f.DcDownloadId = @DcDownloadId
		--and t.ClientId = @clientId 

		update d
		set CodeTypeId = c.CodeTypeId
		from @Data d
		join Uad_Lookup..Code c on c.CodeTypeId = @profStd and c.CodeName = d.DataColumn

		update d
		set CodeTypeId = c.CodeTypeId
		from @Data d
		join Uad_Lookup..Code c on c.CodeTypeId = @profPrem and c.CodeName = d.DataColumn

		update d
		set CodeTypeId = c.CodeTypeId
		from @Data d
		join Uad_Lookup..Code c on c.CodeTypeId = @demoStd and c.CodeName = d.DataColumn

		update d
		set CodeTypeId = c.CodeTypeId
		from @Data d
		join Uad_Lookup..Code c on c.CodeTypeId = @demoPrem and c.CodeName = d.DataColumn

		update d
		set CodeTypeId = c.CodeTypeId
		from @Data d
		join Uad_Lookup..Code c on c.CodeTypeId = @demoCust and c.CodeName = d.DataColumn

		--set base cost
		update d
		set CostPerItemClient = b.CostPerItem
		from @Data d
		join DataCompareCostBase b on d.CodeTypeId = b.CodeTypeId

		--set third party cost
		update d
		set CostPerItemThirdParty = d.CostPerItemClient * isnull(t.CodeTypeCostModifier, 1)
		from @Data d
		join DataCompareCostThirdParty t on d.CodeTypeId = t.CodeTypeId
		where t.ClientId = @clientId 

		--apply any modifiers
		--declare @userId int = (select PurchasedByUserId from DataCompareDownload with(nolock) where DcDownloadId = @DcDownloadId)
		if exists(select UserId from  DataCompareCostUser u where u.ClientId = @clientId and u.UserId = @userId)
			begin
				--set user cost
				update d
				set CostPerItemClient = d.CostPerItemClient * isnull(u.CodeTypeCostModifier, 1)
				from @Data d
				join DataCompareCostUser u on d.CodeTypeId = u.CodeTypeId
				where u.ClientId = @clientId and u.UserId = @userId
			end
		else
			begin
				--set client cost
				update d
				set CostPerItemClient = d.CostPerItemClient * isnull(c.CodeTypeCostModifier, 1)
				from @Data d
				join DataCompareCostClient c on d.CodeTypeId = c.CodeTypeId
				where c.ClientId = @clientId
			end

		declare @DcDetail table (DcDownloadId int, CodeTypeId int, 
							CostPerItemClient decimal(19,4), CostPerItemDetailClient varchar(500),
							CostPerItemThirdParty decimal(19,4), CostPerItemDetailThirdParty varchar(500), 
							ItemCount int, ItemTotalCostClient decimal(19,4), ItemTotalCostThirdParty decimal(19,4)) 

		insert into @DcDetail
		select 0,
			   d.CodeTypeId, 
			   d.CostPerItemClient, 
			   cast(@profileCount as varchar(50)) + ' * ' + cast(count(d.CodeTypeId) as varchar(50)) + ' * ' + cast(d.CostPerItemClient as varchar(50)) + ' = ' + cast((@profileCount * count(d.CodeTypeId) * d.CostPerItemClient) as varchar(50)),
			   d.CostPerItemThirdParty,
			   cast(@profileCount as varchar(50)) + ' * ' + cast(count(d.CodeTypeId) as varchar(50)) + ' * ' + cast(d.CostPerItemThirdParty as varchar(50)) + ' = ' + cast((@profileCount * count(d.CodeTypeId) * d.CostPerItemThirdParty) as varchar(50)),
			   count(d.CodeTypeId),
			   @profileCount * count(d.CodeTypeId) * d.CostPerItemClient, 
			   @profileCount * count(d.CodeTypeId) * d.CostPerItemThirdParty
		from @Data d
		group by d.CodeTypeId, d.CostPerItemClient,d.CostPerItemThirdParty

		select * from @DcDetail;
	end
GO