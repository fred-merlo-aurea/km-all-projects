CREATE proc [dbo].[sp_crosstab]       
(      
	@ReportID int,  
	@RowID varchar(1000),  
	@ColID varchar(1000),  
	@Filters TEXT,
	@PrintColumns varchar(4000),
	@Download char(1)       
)      
as     

--declare  
--	@ReportID int,  
--	@RowID varchar(1000),  
--	@ColID varchar(1000),  
--	@Filters varchar(max),
--	@PrintColumns varchar(4000),
--	@Download char(1)       

--set @ReportID = 95
--set @RowID = ''
--set @ColID = ''
--set @Filters = '<xml></xml>' --'<xml><Filters><FilterType ID="P"></FilterType><FilterType ID="D"></FilterType><FilterType ID="M"><FilterGroup Type="CATEGORY"><Value>1</Value><Value>2</Value></FilterGroup><FilterGroup Type="CATCODES"><Value>10</Value><Value>11</Value><Value>17</Value><Value>20</Value><Value>21</Value></FilterGroup><FilterGroup Type="TRANSACTION"><Value>1</Value></FilterGroup></FilterType><FilterType ID="C"></FilterType><FilterType ID="A"></FilterType></Filters></xml>'
--set @PrintColumns = ''
--set @Download = 0

Begin   

	declare @Row varchar(50),  
			@Column varchar(50),  
			@PublicationID int  

	declare @PublicationCode varchar(20)

	set ANSI_WARNINGS OFF
	set nocount on
	
/*
	select        
			'' Row_Value,
			'' Row_DESCRIPTION,
			0 ROWGROUP_SORTORDER,
			'' ROWGROUP_DisplayName,
			0 Row_response_sortorder,          
			'' Column_Value,
			'' Column_DESCRIPTION,
			0 COLGROUP_SORTORDER,
			'' COLGROUP_DisplayName, 
			0 Col_response_sortorder,         
			0 as counts   
			
	*/
	
	create table #SubscriptionID (SubscriptionID int, copies int)  
	create table #responseID (responseID int)  

	if len(ltrim(rtrim(@PrintColumns))) > 0 
	Begin
		set @PrintColumns  = ', ' + @PrintColumns 
	end

	select	@PublicationID = PublicationID,  
			@Row = Row,  
			@Column = [Column]  
	from   
			PublicationReports  
	Where  
			ReportID = @ReportID  
	
	
	select @PublicationCode  = Publicationcode from Publication where PublicationID = @PublicationID
	
	--print '1. ' + convert(varchar(45), getdate(), 21) 
  
	Insert into #SubscriptionID  (SubscriptionID , copies )   
	exec sp_getSubscribers_using_XMLFilters @PublicationID, @Filters, 1 
		    
	CREATE UNIQUE CLUSTERED INDEX IX_1 on #SubscriptionID (SubscriptionID)
   
	---- Qoua add
	
	declare @PublisherID int = (select PublisherID from Publication with(nolock) where PublicationID = @PublicationID)
	declare @ClientID int = (select ClientID from Publisher with(nolock) where PublisherID = @PublisherID)
	declare @ClientName varchar(100) = (select ClientName from uas..Client with(nolock) where ClientID = @ClientID)
	declare @db varchar(200) = (select name from master..sysdatabases where name like @ClientName + 'MasterDB%')
	declare @Sqlstmt varchar(8000)
   
   
	if Len(ltrim(rtrim(@RowID))) = 0  
	begin
		--insert into #responseID select responseID from response where PublicationID = @PublicationID and ResponseName = @Row  
		set @Sqlstmt = 'insert into #responseID 
						select CodeSheetID from '+@db+'..CodeSheet c join '+@db+'..Pubs p on c.PubId = p.PubId
																    join Circulation..Publication pp on p.PubCode = pp.PublicationCode
						where pp.PublicationID = '+cast(@PublicationID as varchar(50))+' and ResponseGroup = '''+@Row+'''  '
		exec(@Sqlstmt)						
	end
	else
	begin  
		insert into #responseID 
		select items from dbo.fn_Split(@RowID,',')   
	end
	
	if Len(ltrim(rtrim(@ColID))) = 0  
	begin
		--insert into #responseID select responseID from response  where PublicationID = @PublicationID and ResponseName = @Column  
		set @Sqlstmt = 'insert into #responseID 
						select CodeSheetID from '+@db+'..CodeSheet c join '+@db+'..Pubs p on c.PubId = p.PubId
																    join Circulation..Publication pp on p.PubCode = pp.PublicationCode
						where pp.PublicationID = '+cast(@PublicationID as varchar(50))+' and ResponseGroup = '''+@Column+'''  '
		exec(@Sqlstmt)	
	end
	else
	begin  
		insert into #responseID 
		select items from dbo.fn_Split(@ColID,',')   
	end
	
	if @Download = 0
	Begin
	
		--declare @subscriptions table (subscriptionID int, copies int, Row_ID int, Column_ID int)
		create table #subscriptions (subscriptionID int, copies int, Row_ID int, Column_ID int)
		create table #Crosstab (Row_ID int , Row_Value varchar(500),Row_DESCRIPTION varchar(500), ROWGROUP_SORTORDER int, ROWGROUP_DisplayName varchar(50), Column_ID int, Column_Value varchar(500), Column_DESCRIPTION varchar(500), COLGROUP_SORTORDER int, COLGROUP_DisplayName varchar(50), Row_response_sortorder int, Col_response_sortorder int)  

		print '3. ' + convert(varchar(45), getdate(), 21) 

		-- This is the original - q.k.
		--insert into #Crosstab
		--select	r1.responseID, r1.ResponseCode as Row_Value, r1.DisplayName as Row_DESCRIPTION, RG1.DISPLAYORDER AS ROWGROUP_SORTORDER, RG1.DisplayName AS ROWGROUP_DisplayName,          
		--		r2.responseID, r2.ResponseCode as Column_Value, r2.DisplayName as Column_DESCRIPTION, RG2.DISPLAYORDER AS COLGROUP_SORTORDER, RG2.DisplayName AS COLGROUP_DisplayName, 
		--		r1.DisplayOrder, r2.DisplayOrder 
		--from		
		--		Response r1 LEFT OUTER JOIN REPORTGROUPS RG1 ON R1.REPORTGROUPid = RG1.REPORTGROUPid cross join 
		--		Response r2 LEFT OUTER JOIN REPORTGROUPS RG2 ON R2.REPORTGROUPid = RG2.REPORTGROUPid
		--where r1.PublicationID = @PublicationID and  r1.ResponseName = @Row and r2.PublicationID = @PublicationID    and  r2.ResponseName = @Column 
		
		
		set @Sqlstmt = 'insert into #Crosstab
						select	r1.CodeSheetID, r1.Responsevalue as Row_Value, r1.Responsedesc as Row_DESCRIPTION, RG1.DISPLAYORDER AS ROWGROUP_SORTORDER, RG1.DisplayName AS ROWGROUP_DisplayName,          
								r2.CodeSheetID, r2.Responsevalue as Column_Value, r2.Responsedesc as Column_DESCRIPTION, RG2.DISPLAYORDER AS COLGROUP_SORTORDER, RG2.DisplayName AS COLGROUP_DisplayName, 
								r1.DisplayOrder, r2.DisplayOrder 
						from		
								'+@db+'..CodeSheet r1 join '+@db+'..Pubs p on r1.PubID = p.PubID 
													  join Circulation..Publication pp on p.PubCode = pp.PublicationCode
								LEFT OUTER JOIN REPORTGROUPS RG1 ON R1.REPORTGROUPid = RG1.REPORTGROUPid 
								cross join '+@db+'..CodeSheet r2 join '+@db+'..Pubs p2 on r1.PubID = p2.PubID 
																 join Circulation..Publication pp2 on p2.PubCode = pp2.PublicationCode
								LEFT OUTER JOIN REPORTGROUPS RG2 ON R2.REPORTGROUPid = RG2.REPORTGROUPid
						where pp.PublicationID = '+cast(@PublicationID as varchar(25))+' and  r1.ResponseGroup = '''+@Row+''' 
							  and pp2.PublicationID = '+cast(@PublicationID as varchar(25))+' and  r2.ResponseGroup = '''+@Column+''' '
		exec(@Sqlstmt)

		
		CREATE INDEX IDX_Crosstab_rowID_ColumnID ON #Crosstab(Row_ID,Column_ID )
		
		print '4. ' + convert(varchar(45), getdate(), 21) 

		--insert into #subscriptions 
		--select        
		--	sf.subscriptionID,      
		--	sf.copies,
		--	max(case when ResponseName=@Row THEN R.RESPONSEid END) Row_ID,      
		--	max(case when ResponseName=@Column THEN R.RESPONSEid END) Column_ID      
		--FROM        
		--	#SubscriptionID sf with (NOLOCK) join
		--	SubscriptionResponseMap sd with (NOLOCK) on sd.SubscriptionID = sf.SubscriptionID join  
		--	Response r with (NOLOCK) on sd.CodeSheetID = r.responseID JOIN 
		--	#responseID rd on rd.responseID = r.ResponseID
		--Where 
		--	 sd.IsActive = 1
		--GROUP BY sf.SUBSCRIPTIONid,sf.copies;  
		
		set @Sqlstmt = 'insert into #subscriptions 
						select        
							sf.subscriptionID,      
							sf.copies,
							max(case when ResponseGroup='''+@Row+''' THEN R.CodeSheetID END) Row_ID,      
							max(case when ResponseGroup='''+@Column+''' THEN R.CodeSheetID END) Column_ID      
						FROM        
							#SubscriptionID sf with (NOLOCK) join
							'+@db+'..SubscriptionResponseMap sd with (NOLOCK) on sd.SubscriptionID = sf.SubscriptionID join  
							'+@db+'..CodeSheet r with (NOLOCK) on sd.ResponseID = r.CodeSheetID JOIN 
							#responseID rd on rd.responseID = r.CodeSheetID
						Where 
							 sd.IsActive = 1
						GROUP BY sf.SUBSCRIPTIONid,sf.copies'
		exec(@sqlstmt)

		;WITH CTR 
		AS
		(	
		select 
			c.Row_ID,
			Row_Value,
			Row_DESCRIPTION,
			ROWGROUP_SORTORDER,
			ROWGROUP_DisplayName,
			Row_response_sortorder, 
			c.Column_ID,         
			Column_Value,
			Column_DESCRIPTION,
			COLGROUP_SORTORDER,
			COLGROUP_DisplayName, 
			Col_response_sortorder,         
			isnull(sum(copies),0) as counts   
		FROM 
				#crosstab c inner join 
				#responseID r1 on r1.responseID = c.Row_ID inner join 
				#responseID r2 on r2.responseID = c.Column_ID
				LEFT outer join #subscriptions t on t.Row_ID = c.Row_ID and t.Column_ID = c.Column_ID
		group by         
			c.Row_ID,
			Row_Value,
			Row_DESCRIPTION,  
			c.Column_ID,               
			Column_Value,
			Column_DESCRIPTION,
			ROWGROUP_SORTORDER,
			ROWGROUP_DisplayName,
			Row_response_sortorder,
			COLGROUP_SORTORDER,
			COLGROUP_DisplayName,
			Col_response_sortorder
		)
		select  Row_Value,
			Row_DESCRIPTION,
			ROWGROUP_SORTORDER,
			ROWGROUP_DisplayName,
			Row_response_sortorder,          
			Column_Value,
			Column_DESCRIPTION,
			COLGROUP_SORTORDER,
			COLGROUP_DisplayName, 
			Col_response_sortorder,         
			counts  
		from CTR
			
		drop table #Crosstab

	end
	
	drop table #SubscriptionID
	drop table #responseID 
	
	set ANSI_WARNINGS ON
	set NOCOUNT off
	
End
