CREATE proc [dbo].[spDownloaddetailsResponseCount] 
@Queries XML
AS
BEGIN

	SET NOCOUNT ON    

	Create table #tblrow  (RowDesc varchar(255), RowValue varchar(100) , subscriptionID int)
	Create table #tblcol  (colDesc varchar(255), colValue varchar(100) , subscriptionID int)
	
	Create table #tblrowvalues  (value varchar(255))
	Create table #tblcolvalues  (value varchar(255))
	
	declare @docHandle int,
		@rowtype varchar(100),
		@rowresponsegroupID varchar(10),
		@rowfield varchar(100),
		@rowfilters varchar(100),
		@coltype varchar(100),
		@colresponsegroupID varchar(10),
		@colfield varchar(100),
		@colfilters varchar(100),
		@zipFrom varchar(50),
		@zipTo varchar(50)
			
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
	
	select count(distinct s.SubscriptionID) as 'Count'
	from subscriptions s 
		join SubscriberMasterValues smv on s.SubscriptionID = smv.SubscriptionID
		join MasterGroups mg on smv.MasterGroupID = mg.MasterGroupID
		join pubsubscriptions ps on s.subscriptionID = ps.subscriptionID 
	where s.SubscriptionID in (SELECT SubscriptionID FROM #subscriptionID)

	drop table #subscriptionID
	
end
