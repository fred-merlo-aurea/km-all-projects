CREATE proc [dbo].[rpt_ViewResponseTotals]    
(    
	@ReportID int,  
	@RowID varchar(1000),
	@ProductID int, 
	@CategoryIDs varchar(800),
	@CategoryCodes varchar(800),
	@TransactionIDs varchar(800),
	@TransactionCodes varchar(800),
	@QsourceIDs varchar(800),
	@StateIDs varchar(800),
	@Regions varchar(max),
	@CountryIDs varchar(1500),
	@Email varchar(10),
	@Phone varchar(10),
	@Mobile varchar(10),
	@Fax varchar(10),
	@ResponseIDs varchar(1000),
	@Demo7 varchar(50),		
	@Year varchar(20),
	@startDate varchar(10),		
	@endDate varchar(10),
	@AdHocXML varchar(8000),
	@WaveMail varchar(100) = ''  
)    
as 
BEGIN
	
	SET NOCOUNT ON
         			
	declare	@MagazineID int,  
			@ResponseGroup varchar(25)  ,
			@distinctcount int,    
			@Responded int  
	
	set @distinctcount = 0
	set @Responded = 0

	create table #SubscriptionID (SubscriptionID int, copies int)  
	select @MagazineID = ProductID, @ResponseGroup=Row from reports where reportID = @ReportID  
	
	declare @magazineCode varchar(20)
	select @magazineCode  = PubCode from Pubs where PubID = @MagazineID
	
	Insert into #SubscriptionID   
	exec rpt_GetSubscriptionIDs_Copies_From_Filter 
	@ProductID, 
	@CategoryIDs,
	@CategoryCodes,
	@TransactionIDs,
	@TransactionCodes,
	@QsourceIDs,
	@StateIDs,
	@CountryIDs,
	@Email,
	@Phone,
	@Mobile,
	@Fax,
	@ResponseIDs,
	@Demo7,		
	@Year,
	@startDate,		
	@endDate,
	@AdHocXML 

	CREATE UNIQUE CLUSTERED INDEX IX_1 on #SubscriptionID (SubscriptionID)

	create table #cte (ResponseID int, DESCRIPTION varchar(255), DisplayName varchar(100), SubscriptionID int,  copies int,  rgsortorder int)  

	create table #responseID (responseID int, SubscriberID int) 
  
	if Len(ltrim(rtrim(@RowID))) = 0  
		insert into #responseID select p.CodesheetID, p.SubscriptionID 
		from PubSubscriptionDetail p 
			JOIN CodeSheet c ON c.CodeSheetID = p.CodesheetID
			JOIN ResponseGroups rg ON rg.ResponseGroupID = c.ResponseGroupID 
		where c.PubID = @MagazineID and rg.ResponseGroupName = @ResponseGroup  
	else  
		insert into #responseID 
		select items 
		from dbo.fn_Split(@RowID,',')   
		
		--select * FROM #cte
		--select * FROM #responseID
		--select * FROM #SubscriptionID
		
		select	@distinctcount = sum(copies) 
		FROM  #SubscriptionID sf   

		delete 
		from #SubscriptionID 
		from #SubscriptionID s 
			left outer join 
			(
				select sd.SubscriptionID 
				FROM PubSubscriptionDetail sd 
				where CodesheetID in (select responseID from #responseID)
			) inn on s.SubscriptionID = inn.SubscriptionID
		where inn.subscriptionID is null
    
		select	@Responded = isnull(sum(copies), 0) FROM #SubscriptionID;
		
		insert into #cte
			select sd.CodesheetID,  
				c.Responsevalue + '.' + c.Responsedesc AS 'DESCRIPTION',     
				isnull(rg.DisplayName,'') as reportgroupname,
				sf.SubscriptionID,   
				sf.copies,
				c.DisplayOrder		
			From #SubscriptionID sf  
				join PubSubscriptionDetail sd on sf.SubscriptionID = sd.SubscriptionID   
				join CodeSheet c on sd.CodesheetID = c.CodeSheetID 
				join ResponseGroups rsg ON rsg.ResponseGroupID = c.ResponseGroupID 
				join #responseID rID on rID.responseID = c.CodeSheetID and sf.SubscriptionID = rid.SubscriberID 
				--LEFT OUTER JOIN #responseID rID on rID.responseID = c.CodeSheetID 
				LEFT OUTER JOIN ReportGroups rg ON c.ReportGroupID = rg.ReportGroupID 
			  
			WHERE rsg.responsegroupname = @ResponseGroup
	
		select DESCRIPTION, SUM(copies) as TOTAL,
			convert(varchar(200), (case when isnull(@Responded,0) = 0  then 0 else convert(decimal(18,2),(convert(decimal(18,2),count(subscriptionID))*100)/@distinctcount) end)) + '%' as 'Total Qualified %',
			convert(varchar(200), (case when isnull(@distinctcount,0) = 0 then 0 else convert(decimal(18,2),(convert(decimal(18,2),count(subscriptionID))*100)/@Responded) end)) + '%' as 'Unique Response %',
			cte.rgsortorder as 'Display Order', 1 as sort    
		from #cte cte  
			join (select DisplayName, count(distinct subscriptionID) as groupUniques from #cte ct2 group by DisplayName) as inn2 on cte.DisplayName  = inn2.displayname
		group by ResponseID, DESCRIPTION, cte.DisplayName, groupUniques, rgsortorder
		UNION    
		select 'ZZ. NO RESPONSE' , @distinctcount - @Responded ,
		convert(varchar(200), (case when isnull(@distinctcount,0) = 0 then 0 else convert(decimal(18,2),(convert(decimal(18,2),(@distinctcount - @Responded))*100)/@distinctcount) end)) + '%' as 'Total Qualified %', '0.00%',
		0, 2 as sort
		UNION		
		SELECT 'Total (unique): ', @Responded, NULL, NULL, 0, 3 as sort
		UNION
		SELECT 'Total: ', @distinctcount, NULL, NULL, 0, 4 as sort
		ORDER BY SORT, rgsortorder
   
	drop table #SubscriptionID 	
	drop table #responseID 	
	drop table #cte

END