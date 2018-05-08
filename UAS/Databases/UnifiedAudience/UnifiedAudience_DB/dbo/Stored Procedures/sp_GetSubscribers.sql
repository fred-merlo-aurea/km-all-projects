create Proc [dbo].[sp_GetSubscribers]
(  
	@publicationID int ,
	@SubscriptionIDs  XML,
	@Profilefields varchar(2000) = '',
	@DemoFields varchar(2000) = '',
	@paidfields varchar(2000) = ''

)
as
BEGIN

	SET NOCOUNT ON

	declare @DemoColumns varchar(8000),
			@DemoFieldsNames varchar(1000),
			@demofieldsstring varchar(4000)

	set @DemoColumns  = ''    
	set @demofieldsstring = ''
	set @DemoFieldsNames = ''
	
	declare @PublisherID int = (select PublisherID from Publication with(nolock) where PublicationID = @PublicationID)
	declare @ClientID int = (select ClientID from Publisher with(nolock) where PublisherID = @PublisherID)
	declare @ClientName varchar(100) = (select ClientName from uas..Client with(nolock) where ClientID = @ClientID)
	declare @db varchar(200) = (select name from master..sysdatabases where name like @ClientName + 'MasterDB%')
	declare @Sqlstmt varchar(max)

	if LEN(@profilefields) = 0
		set @profilefields = 's.SubscriptionID, FirstName,LastName,Company,Title,Occupation,Address1,Address2,City,RegionCode,ZipCode,Country,Email,Phone,Fax,Mobile'
	      
	IF OBJECT_ID('tempdb..#tmpSubscriptionID') IS NOT NULL 
		BEGIN 
			DROP TABLE #tmpSubscriptionID;
		END 
	
	CREATE TABLE #tmpSubscriptionID (SubscriptionID int); 
	CREATE TABLE #tmpSubscriptionResponseMap (SubscriptionID int, displayname varchar(100), responsecode varchar(1000)); 

	insert into #tmpSubscriptionID
	SELECT SubscriptionID.ID.value('./@SubscriptionID','INT')FROM @SubscriptionIDs.nodes('/Subscriptions') as SubscriptionID(ID) ;	

	if LEN(@DemoFields) > 0
		Begin
		
			set @Sqlstmt = 'insert into #tmpSubscriptionResponseMap
			select srm.SubscriptionID, r.displayname,  r.ResponseCode
			from 
					#tmpSubscriptionID t join
					'+@db+'..SubscriptionResponseMap srm with(nolock) on t.SubscriptionID = srm.SubscriptionID join 
					vw_Response r with(nolock) on r.ResponseID = srm.ResponseID and r.PublicationID = '+CAST(@PublicationID as varchar(25))+'
					--join ResponseType rt on rt.ResponseTypeID = r.ResponseTypeID
			where 
					rt.PublicationID = '+cast(@publicationID as varchar(25))+'
					and r.ResponseGroupID in (select items from dbo.fn_Split('''+@DemoFields+''', '',''))   ' 
			exec(@Sqlstmt)				
					
			select @DemoColumns = @DemoColumns + coalesce('[' + RTRIM(displayname)  + '] as ''' + RTRIM(displayname)  + ''',','') ,
	 				@DemoFieldsNames = @DemoFieldsNames + coalesce('[' + RTRIM(displayname)  + '],','')       
			from   vw_Response rt with(nolock) 
			where  rt.PublicationID = @publicationID
					and rt.ResponseGroupID in (select items from dbo.fn_Split(@DemoFields, ','))      
		 
			set @DemoColumns = ',' + substring(@DemoColumns, 0, len(@DemoColumns))  
			set @DemoFieldsNames = substring(@DemoFieldsNames, 0, len(@DemoFieldsNames))  
				
			set @demofieldsstring= ' left outer join (SELECT * 
			 FROM
			 (
				SELECT 
					  [subscriptionID], displayname,
					  STUFF((
						SELECT '', '' + CAST([Responsecode] AS VARCHAR(MAX)) 
						FROM #tmpSubscriptionResponseMap 
						WHERE ([subscriptionID] = Results.[subscriptionID] and displayname= Results.displayname) 
						FOR XML PATH (''''))
					  ,1,2,'''') AS CombinedValues
					FROM #tmpSubscriptionResponseMap Results
					GROUP BY [subscriptionID], displayname
			 ) u
			 PIVOT
			 (
			 MAX (CombinedValues)
			 FOR displayname in (' + @DemoFieldsNames + ')) as pvt) demos on demos.subscriptionID = s.subscriptionID'
		End
	
	if LEN(@paidfields) > 0
		Begin
			set @DemoColumns = @DemoColumns + ',' + @paidfields
			set @demofieldsstring= @demofieldsstring + ' left outer join SubscriptionPaid spd with(nolock) on spd.SubscriptionID = s.subscriptionID'
		End
	
	exec (' select distinct ' + @profilefields +  @DemoColumns + 
			' from #tmpSubscriptionID t ' +   
			' join Subscription s with(nolock) on s.SubscriptionID = t.SubscriptionID ' +  
			' join Subscriber sb with(nolock) on sb.SubscriberID = s.SubscriberID ' + 
			@demofieldsstring
			)
		
	drop table #tmpSubscriptionResponseMap		
	DROP TABLE #tmpSubscriptionID;
	
End