CREATE PROCEDURE [dbo].[sp_Subscriber_Codesheet_Counts_With_Permissions]
@Queries XML, 
@ResponseGroupID int = 0, 
@Description varchar(50) = '', 
@PubID int
AS
Begin
	set nocount on
	
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
	case when len(@suppressedfilterno) > 0 then 
		' except ' 
		+ ' (select subscriptionID from #tblqueryresults t where filterno = ' + REPLACE(@SuppressedfilterNo , ',', '  ' + @SuppressedFilterOperation + ' select subscriptionID from #tblqueryresults t where filterno = ') + ') '
	else
	'' end
	+ ') x'

	insert into #subscriptionIDs 
	execute (@qry)
	
	drop table #tblquery
	drop table #tblqueryresults; 
	
	select psd.codesheetID as ID, c.ResponseValue as Value, c.ResponseDesc as [Desc],
	count(distinct case when MailPermission=1 then ps.SubscriptionID end) as 'Mail',
	count(distinct case when OtherProductsPermission=1 and EmailRenewPermission=1 and isnull(ps.email, '') != '' then ps.SubscriptionID end) as 'Email',
	count(distinct case when PhonePermission=1 and isnull(ps.Phone, '') != '' then ps.SubscriptionID end) as 'Phone',
	count(distinct case when MailPermission=1 and PhonePermission=1 and isnull(ps.Phone, '') != '' then ps.SubscriptionID end) as 'Mail_Phone',
	count(distinct case when PhonePermission=1 and OtherProductsPermission=1 and EmailRenewPermission=1 and isnull(ps.email, '') != '' and isnull(ps.Phone, '') != '' then ps.SubscriptionID end) as 'Email_Phone',
	count(distinct case when MailPermission=1 and OtherProductsPermission=1 and EmailRenewPermission=1 and isnull(ps.email, '') != '' then ps.SubscriptionID end) as 'Mail_Email',
	count(distinct case when MailPermission=1 and PhonePermission=1 and OtherProductsPermission=1 and EmailRenewPermission=1 and isnull(ps.email, '') != '' and isnull(ps.Phone, '') != '' then ps.SubscriptionID end) as 'All_Records',
	count(distinct ps.SubscriptionID) as 'Count'
	from PubSubscriptions ps with(nolock)
		join #subscriptionIDs ts on ps.SubscriptionID = ts.SubscriptionID
		join pubsubscriptiondetail psd  with(nolock) on psd.subscriptionid = ps.subscriptionid 
		join codesheet c  with(nolock) on c.codesheetID = psd.codesheetID 
	where ps.PubID = @PubID and c.ResponseGroupID =  @ResponseGroupID  and (LEN(ltrim(rtrim(@description))) = 0 or  c.Responsedesc like '%' + @Description + '%')
	group by psd.codesheetID, c.ResponseValue, c.ResponseDesc
	union all
	select -1 as ID, 'ZZZ' as Value, 'TOTAL UNIQUE SUBSCRIBERS' as [Desc],
	count(distinct case when MailPermission=1 then ps.SubscriptionID end) as 'Mail',
	count(distinct case when OtherProductsPermission=1 and EmailRenewPermission=1 and isnull(ps.email, '') != '' then ps.SubscriptionID end) as 'Email',
	count(distinct case when PhonePermission=1 and isnull(ps.Phone, '') != '' then ps.SubscriptionID end) as 'Phone',
	count(distinct case when MailPermission=1 and PhonePermission=1 and isnull(ps.Phone, '') != '' then ps.SubscriptionID end) as 'Mail_Phone',
	count(distinct case when PhonePermission=1 and OtherProductsPermission=1 and EmailRenewPermission=1 and isnull(ps.email, '') != '' and isnull(ps.Phone, '') != '' then ps.SubscriptionID end) as 'Email_Phone',
	count(distinct case when MailPermission=1 and OtherProductsPermission=1 and EmailRenewPermission=1 and isnull(ps.email, '') != '' then ps.SubscriptionID end) as 'Mail_Email',
	count(distinct case when MailPermission=1 and PhonePermission=1 and OtherProductsPermission=1 and EmailRenewPermission=1 and isnull(ps.email, '') != '' and isnull(ps.Phone, '') != '' then ps.SubscriptionID end) as 'All_Records',
	count(distinct ps.SubscriptionID) as 'Count'
	from PubSubscriptions ps  with(nolock)
		join #subscriptionIDs ts on ps.SubscriptionID = ts.SubscriptionID
		join pubsubscriptiondetail psd  with(nolock) on psd.subscriptionid = ps.subscriptionid 
		join codesheet c  with(nolock) on c.codesheetID = psd.codesheetID  
	where ps.PubID = @PubID and c.ResponseGroupID =  @ResponseGroupID  and (LEN(ltrim(rtrim(@description))) = 0 or  c.Responsedesc like '%' + @Description + '%')

	drop table #subscriptionIDs
	
end
