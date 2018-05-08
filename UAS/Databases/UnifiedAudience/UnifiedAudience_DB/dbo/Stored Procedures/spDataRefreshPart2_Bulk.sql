CREATE proc [dbo].[spDataRefreshPart2_Bulk]
as
BEGIN

	SET NOCOUNT ON

	Create table #tmpProductDemographics (subscriptionID int, pubID int, responsevalue varchar(max))

	declare @rgname varchar(50),
			@subscriptionID int, 
			@pubID int, 
			@responsevalue varchar(max)
	
	DECLARE c_ProductDemographics CURSOR FOR 
	select distinct ResponseGroupName 
	from responsegroups 
	-- where responsegroupname = 'BUSINESS'

	OPEN c_ProductDemographics  
	FETCH NEXT FROM c_ProductDemographics INTO @rgname

	WHILE @@FETCH_STATUS = 0  
		BEGIN  	
	
			truncate table #tmpProductDemographics
		
			exec ('
			insert into IncomingDataDetails	 (subscriptionID, pubid, responsegroup, responsevalue)
			select distinct s.SubscriptionID, p.PubID, ''' + @rgname + ''', i.[' + @rgname + '] from IncomingData i join Subscriptions s on i.IGRP_NO = s.IGRP_NO
			join Pubs p on p.Pubcode = i.PUBCODE join ResponseGroups rg on p.PubID = rg.PubID
			where rg.ResponseGroupName = ''' + @rgname + ''' and LTRIM(rtrim(i.[' + @rgname + '])) <> '''' and CHARINDEX('','', i.[' + @rgname + ']) = 0')
		 
			exec ('
			insert into #tmpProductDemographics 
			select s.SubscriptionID, p.PubID, i.[' + @rgname + '] from IncomingData i join Subscriptions s on i.IGRP_NO = s.IGRP_NO
			join Pubs p on p.Pubcode = i.PUBCODE join ResponseGroups rg on p.PubID = rg.PubID
			where rg.ResponseGroupName = ''' + @rgname + ''' and LTRIM(rtrim(i.[' + @rgname + '])) <> '''' and CHARINDEX('','', i.[' + @rgname + ']) > 0')
		
			DECLARE c_ProductDemographicsData CURSOR FOR 
			select subscriptionID , pubID , responsevalue  
			from  #tmpProductDemographics  
			-- where responsegroupname = 'BUSINESS'

			OPEN c_ProductDemographicsData  
			FETCH NEXT FROM c_ProductDemographicsData INTO @subscriptionID, @pubID, @responsevalue

			WHILE @@FETCH_STATUS = 0  
				BEGIN  	
					--EXEC Up_insertresponses @PUBID, @SUBSCRIPTIONID, @rgname, @responsevalue
			
					insert into IncomingDataDetails --(pubid, subscriptionID, responsegroup, responsevalue)
					select @pubId,@SUBSCRIPTIONID,@rgname, (case when PATINDEX('%[^0-9]%', items) = 0 and (Items not like '%$%' and Items not like '%.%') then CONVERT(varchar(100),CONVERT(int,items)) else items end), 1  
					from dbo.fn_split(@RESPONSEVALUE, ',')
		
					FETCH NEXT FROM c_ProductDemographicsData INTO  @subscriptionID, @pubID, @responsevalue
				END

			CLOSE c_ProductDemographicsData  
			DEALLOCATE c_ProductDemographicsData  
		
			FETCH NEXT FROM c_ProductDemographics INTO  @rgname
		END

	CLOSE c_ProductDemographics  
	DEALLOCATE c_ProductDemographics  
	
	DROP TABLE #tmpProductDemographics
end