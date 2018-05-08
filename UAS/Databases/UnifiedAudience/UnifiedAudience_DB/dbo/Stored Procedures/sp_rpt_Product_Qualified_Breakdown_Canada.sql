CREATE PROCEDURE [dbo].[sp_rpt_Product_Qualified_Breakdown_Canada]
@Queries XML,
@PubID int
AS
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

	WITH CTE (state, Mail, Email, Phone, Mail_Phone, Email_Phone, Mail_Email, All_Records, CategoryCodeTypeName, counts)
	AS
	(
		select r.RegionName as state,  
			count(distinct case when MailPermission=1 then ps.SubscriptionID end) as 'Mail',
			count(distinct case when OtherProductsPermission=1 and EmailRenewPermission=1 and isnull(ps.email, '') != '' then ps.SubscriptionID end) as 'Email',
			count(distinct case when PhonePermission=1 and isnull(ps.Phone, '') != '' then ps.SubscriptionID end) as 'Phone',
			count(distinct case when MailPermission=1 and PhonePermission=1 and isnull(ps.Phone, '') != '' then ps.SubscriptionID end) as 'Mail_Phone',
			count(distinct case when PhonePermission=1 and OtherProductsPermission=1 and EmailRenewPermission=1 and isnull(ps.email, '') != '' and isnull(ps.Phone, '') != '' then ps.SubscriptionID end) as 'Email_Phone',
			count(distinct case when MailPermission=1 and OtherProductsPermission=1 and EmailRenewPermission=1 and isnull(ps.email, '') != '' then ps.SubscriptionID end) as 'Mail_Email',
			count(distinct case when MailPermission=1 and PhonePermission=1 and OtherProductsPermission=1 and EmailRenewPermission=1 and isnull(ps.email, '') != '' and isnull(ps.Phone, '') != '' then ps.SubscriptionID end) as 'All_Records',
			case when cc.CategoryCodeValue between 60 and 65 then 'ADVERTISER AGENCY' else CategoryCodeTypeName end as CategoryCodeTypeName,
			count(ps.PubSubscriptionID) as counts
		From PubSubscriptions ps with (nolock) 
			join #subscriptionIDs ts on ts.SubscriptionID = ps.SubscriptionID  
			join UAD_Lookup..CategoryCode cc with (nolock) on ps.PubCategoryID = cc.CategoryCodeID 
			join UAD_Lookup..CategoryCodeType cct with (nolock) on cct.CategoryCodeTypeID = cc.CategoryCodeTypeID 
			join UAD_Lookup..[TransactionCode] t with (nolock) on ps.PubTransactionID = t.TransactionCodeID  
			join UAD_Lookup..Region r with (nolock) on ps.RegionCode = r.RegionCode 
			join UAD_Lookup..Country ct with (nolock) on ct.CountryID = r.CountryID 
		where ct.ShortName = 'CANADA' and
				ps.PubID = @PubID
		group by r.RegionName, cc.CategoryCodeValue, cct.CategoryCodeTypeName
	) 
	select	state, 
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
	group by state;
	
end