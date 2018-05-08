CREATE proc [dbo].[sp_rpt_Qualified_Breakdown_domestic] 
@Queries XML
as
BEGIN

	SET NOCOUNT ON

	create table #subscriptionIDs (SubscriptionID int primary key)                     

	declare @hDoc AS INT
	declare @filterno int, @qry varchar(max),
			@linenumber int, @SelectedFilterNo varchar(800), @SelectedFilterOperation varchar(20), @SuppressedFilterNo varchar(800), @SuppressedFilterOperation varchar(20) 

	create table #tblquery  (filterno int, Query varchar(max))
	create table #tblqueryresults  (filterno int, subscriptionID int, primary key (filterno, subscriptionID))

	EXEC sp_xml_preparedocument @hDoc OUTPUT, @queries

	insert into #tblquery
	SELECT filterno, Query
	FROM OPENXML(@hDoc, 'xml/Queries/Query')
	WITH 
	(
	filterno int '@filterno',
	Query [varchar](max) '.'
	)
	
	SELECT 
			@linenumber = linenumber, 
			@SelectedFilterNo = selectedfilterno, 
			@SelectedFilterOperation = selectedfilteroperation, 
			@SuppressedFilterNo = suppressedfilterno, 
			@SuppressedFilterOperation = suppressedfilteroperation
	FROM OPENXML(@hDoc, 'xml/Results/Result')
	WITH 
	(
		linenumber int							'@linenumber',
		selectedfilterno varchar(800)			'@selectedfilterno',
		selectedfilteroperation varchar(20)		'@selectedfilteroperation',
		suppressedfilterno varchar(800)			'@suppressedfilterno',
		suppressedfilteroperation varchar(20)	'@suppressedfilteroperation'
	)

	EXEC sp_xml_removedocument @hDoc
	
	DECLARE c_queries CURSOR 
	FOR select filterno from #tblquery
		
	OPEN c_queries  
	FETCH NEXT FROM c_queries INTO @filterno

	WHILE @@FETCH_STATUS = 0  
	BEGIN  
		select @qry =  query from #tblquery with (NOLOCK) where filterno = @filterno
		
		insert into #tblqueryresults  
		execute (@qry)
		
		FETCH NEXT FROM c_queries INTO @filterno
	END
	
	CLOSE c_queries  
	DEALLOCATE c_queries 

	select @qry = 'select distinct subscriptionID from ((select subscriptionID from #tblqueryresults t where filterno = ' + REPLACE(@SelectedFilterNo, ',', '  ' + @SelectedFilterOperation + ' select subscriptionID from #tblqueryresults t where filterno = ') + ') ' + 
	case when len(@SuppressedfilterNo) > 0 then 
		' except ' 
		+ ' (select subscriptionID from #tblqueryresults t where filterno = ' + REPLACE(@SuppressedfilterNo , ',', '  ' + @SuppressedFilterOperation + ' select subscriptionID from #tblqueryresults t where filterno = ') + ') '
	else
	'' end
	+ ') x'

	insert into #subscriptionIDs 
	execute (@qry)
	
	drop table #tblquery
	drop table #tblqueryresults;  	

	WITH CTE (State, Region, CategoryCodeTypeName, CategoryCodeTypeID, sort_order, country,  country_sort_order, Mail, Email, Phone, Mail_Phone, Email_Phone, Mail_Email, All_Records, counts)
	AS
	(			
		select	isnull(r.ZipCodeRange,'') + ' ' + r.RegionCode as state,   
			ct.Area,   
			case when cc.CategoryCodeValue between 60 and 65 then 'ADVERTISER AGENCY' else CategoryCodeTypeName end as CategoryCodeTypeName,  
			cc.CategoryCodeTypeID,   
			r.sort_order,   
			ct.ShortName,   
			ct.SortOrder,  
			count( distinct case when MailPermission=1 then s.igrp_no end) as 'Mail',
			count( distinct case when OtherProductsPermission=1 and EmailRenewPermission=1 and EmailExists=1 then s.igrp_no end) as 'Email',
			count( distinct case when PhonePermission=1 and PhoneExists=1 then s.igrp_no end) as 'Phone',
			count( distinct case when MailPermission=1 and PhonePermission=1 and PhoneExists=1 then s.igrp_no end) as 'Mail_Phone',
			count( distinct case when PhonePermission=1 and OtherProductsPermission=1 and EmailRenewPermission=1 and EmailExists=1 and PhoneExists=1 then s.igrp_no end) as 'Email_Phone',
			count( distinct case when MailPermission=1 and OtherProductsPermission=1 and EmailRenewPermission=1 and EmailExists=1 then s.igrp_no end) as 'Mail_Email',
			count( distinct case when MailPermission=1 and PhonePermission=1 and OtherProductsPermission=1 and EmailRenewPermission=1 and EmailExists=1 and PhoneExists=1 then s.igrp_no end) as 'All_Records',					
			count(s.subscriptionID) as counts
		From Subscriptions s with (nolock) 
			join #subscriptionIDs ts on ts.SubscriptionID = s.SubscriptionID  
			join UAD_Lookup..CategoryCode cc with (nolock) on s.CategoryID = cc.CategoryCodeID 
			join UAD_Lookup..CategoryCodeType cct with (nolock) on cct.CategoryCodeTypeID = cc.CategoryCodeTypeID 
			join UAD_Lookup..[TransactionCode] t with (nolock) on s.TransactionID = t.TransactionCodeID  
			join UAD_Lookup..Region r with (nolock) on s.state = r.RegionCode 
			join UAD_Lookup..Country ct with (nolock) on ct.CountryID = r.CountryID
		group by isnull(r.ZipCodeRange,'') + ' ' +  r.RegionCode, ct.Area, cc.CategoryCodeValue, cct.CategoryCodeTypeName, cc.CategoryCodeTypeID, r.sort_order, ct.ShortName, ct.SortOrder  
	) 
	select region,   
		state + ' ....' as state,   
		sort_order,  
		isnull(country_sort_order,10) as country_sort_order,  
		isnull(country, 'FOREIGN') AS country,  
		sum(Mail) as 'Mail', 
		sum(Email) as 'Email',
		sum(Phone) as 'Phone',
		sum(Mail_Phone) as 'Mail_Phone',
		sum(Email_Phone) as 'Email_Phone',
		sum(Mail_Email) as 'Mail_Email',
		sum(All_Records) as 'All_Records',				
		sum(case when CategoryCodeTypeName = 'ADVERTISER AGENCY' then counts else 0 end) as 'ADVERTISER_AGENCY',  
		sum(case when CategoryCodeTypeName = 'Qualified Free' then counts else 0 end) as 'QUALIFIED_NON_PAID',
		sum(case when CategoryCodeTypeName = 'Qualified Paid' then counts else 0 end) as 'QUALIFIED_PAID',
		sum(case when CategoryCodeTypeName = 'NonQualified Free' then counts else 0 end) as 'NON_QUALIFIED_NON_PAID',
		sum(case when CategoryCodeTypeName = 'NonQualified Paid' then counts else 0 end) as 'NON_QUALIFIED_PAID',
		sum(case when CategoryCodeTypeName = 'INACTIVE RECORDS' then counts else 0 end) as 'PROSPECTS',  
		sum(counts) as 'Counts'  
	from CTE
	group by Region, state, sort_order, isnull(country, 'FOREIGN'), isnull(country_sort_order,10)  
		
end