CREATE procedure [dbo].[sp_BillingReport]    
(     
 @channelID int, 
 @CustomerID varchar(2000),   
 @month int,    
 @year int
)    
as
Begin

	Set nocount on    
    declare @sqlstring varchar(8000),
			@fromdt varchar(25),
			@todt varchar(25)

	set @fromdt =  convert(varchar(10),@month) + '/01/' + convert(varchar(10),@year) + ' 00:00:00'
	set @todt = convert(varchar(10),dateadd(dd, -1, dateadd(mm,1, convert(datetime,convert(varchar(10),@month) + '/01/' + convert(varchar(10),@year)))),101) + ' 23:59:59'
	
	create table #tmpreport (basechannelID int, basechannelname varchar(100), CustomerID int, Customername varchar(100), source varchar(50), count1 int, count2 int)
							 
--License int, sends int, surveycount int, surveyresponse int, 

-- Surveys
	set @sqlstring = ' insert into #tmpreport
	select  b.basechannelID, b.basechannelname, c.customerID, c.customername, 
			''survey'' as source,
			count(distinct s.SurveyID) as  count1,  
			count(edv.emailID) as count2
	from 
			[ECN5_COMMUNICATOR].[DBO].groupdatafields gdf join 
			[ECN5_COMMUNICATOR].[DBO].emaildatavalues edv on gdf.groupdatafieldsID = edv.groupdatafieldsID join
			ecn5_collector..survey s on s.SurveyID = gdf.surveyID join		
			ecn5_accounts..[Customer] c on c.customerID = s.CustomerID join 
			ecn5_accounts..[BaseChannel] b on b.basechannelID = c.basechannelID
	where shortname like ''%_completionDt'' and  DataValue <> '''''

	 if @channelID > 0
		set @sqlstring = @sqlstring + ' and b.BaseChannelID = ' + convert(varchar,@channelID)

	if len(@CustomerID) > 0
		set @sqlstring = @sqlstring + ' and c.CustomerID in (' + @CustomerID + ') '

	if len(@fromdt) > 0 and  len(@todt) > 0
		set @sqlstring = @sqlstring + ' and convert(datetime,edv.datavalue) between ''' + @fromdt + ''' and ''' + @todt + ''''

	set @sqlstring = @sqlstring + ' group by b.basechannelID, b.basechannelname, c.customerID, c.customername ' 
	
-- Emails
	set @sqlstring = @sqlstring + ' union select  b.basechannelID, b.basechannelname, c.customerID, c.customername, ''email'' as source, (select sum(quantity) from [CustomerLicense] where customerID =c.customerID and isactive=1 and licensetypecode=''emailblock10k'' and ''' + @todt + ''' between AddDate and ExpirationDate), sum(sendtotal)  
								from [ECN5_COMMUNICATOR].[DBO].[BLAST] bs join		
									ecn5_accounts..[Customer] c on c.customerID = bs.customerID join 
									ecn5_accounts..[BaseChannel] b on b.basechannelID = c.basechannelID 
								where testblast = ''n'' and (statuscode = ''sent'' or statuscode = ''deleted'') '
	 if @channelID > 0
		set @sqlstring = @sqlstring + ' and b.BaseChannelID = ' + convert(varchar,@channelID)

	if len(@CustomerID) > 0
		set @sqlstring = @sqlstring + ' and c.CustomerID in (' + @CustomerID + ') '

	if len(@fromdt) > 0 and  len(@todt) > 0
		set @sqlstring = @sqlstring + ' and sendtime between ''' + @fromdt + ''' and ''' + @todt + ''''


	set @sqlstring = @sqlstring + '  group by b.basechannelID, b.basechannelname, c.customerID, c.customername'

	set @sqlstring = @sqlstring + ' union 
	select	b.basechannelID, b.basechannelName, c.customerID, c.customerName, ''DE'',  count(e.editionID), sum(e.pages)
	from	ecn5_publisher..[EDITION] e join 
			ecn5_publisher..[PUBLICATION] p on e.publicationID = p.publicationID join
			[Customer] c on p.customerID = c.customerID join
			[BaseChannel] b on b.basechannelID = c.basechannelID join
		(
			select	editionID, 
					convert(varchar(10),max(activatedDate),101) as activatedDate, 
					convert(varchar(10),max(ArchievedDate),101) as ArchievedDate, 
					convert(varchar(10),max(DeactivatedDate),101)  as DeactivatedDate  
			from	ecn5_publisher..editionhistory
			where 
					(ActivatedDate between ''' + @fromdt + ''' and ''' + @todt + ''') or
					(ArchievedDate between ''' + @fromdt + ''' and ''' + @todt + ''') or 
					(DeActivatedDate between ''' + @fromdt + ''' and ''' + @todt + ''') or 
					(ActivatedDate < ''' + @fromdt + ''' and DeActivatedDate >= ''' + @fromdt + ''') or 
					(ActivatedDate < ''' + @fromdt + ''' and isnull(DeActivatedDate,'''') = '''') or 
					(ArchievedDate < ''' + @fromdt + ''' and isnull(DeActivatedDate,'''') = '''') 
			group by editionID
		) inn on e.editionID = inn.editionID where e.editionID > 0 '

	 if @channelID > 0
		set @sqlstring = @sqlstring + ' and b.BaseChannelID = ' + convert(varchar,@channelID)

	if len(@CustomerID) > 0
		set @sqlstring = @sqlstring + ' and c.CustomerID in (' + @CustomerID + ') '

	set @sqlstring = @sqlstring + '  group by b.basechannelID, b.basechannelname, c.customerID, c.customername'

	exec(@sqlString)

	select	basechannelID, basechannelname, customerID, customername,
			isnull(sum(case when source = 'email' then count1 end),0) as licenses,
			sum(case when source = 'email' then count2 end) as sends,			
			sum(case when source = 'survey' then count1 end) as surveycount,
			sum(case when source = 'survey' then count2 end) as surveyresponse,			
			sum(case when source = 'DE' then count1 end) as DEcount,
			sum(case when source = 'DE' then count2 end) as DEpages			
	from #tmpreport
	group by basechannelID, basechannelname, customerID, customername
End
