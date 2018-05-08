CREATE PROCEDURE [dbo].[rpt_Blast_GetBlastGroupClicksData]  
	@CampaignItemID int = null,
	@BlastID int = null, 
	@HowMuch varchar(25), 
	@ISP varchar(100),
	@ReportType varchar(20),
	@TopClickURL varchar(2048),
	@PageNo int,  
	@PageSize int ,
	@UDFname varchar(100),
	@UDFdata    varchar(100),
	@StartDate Varchar(30) = null,
	@EndDate Varchar(30) = null,
	@Unique bit = 0
	AS
BEGIN
	if @StartDate is null
		begin
		set @StartDate = (select convert(char(10),DATEADD(Year,-1,GETDATE()),110))
		end
	else
		begin
		set @StartDate = @StartDate + ' 00:00:00'
		end
		
	
	if @EndDate is null
		begin
		set @EndDate = (select Convert(char(10),GETDATE(),110))
		end
	else
		begin
		set @EndDate = @EndDate + ' 23:59:59'
		end

	SET NOCOUNT ON

	DECLARE @BlastIDs varchar(4000)
	SET @BlastIDs = ''
	
	DECLARE @LayoutIDs varchar(4000)
	SET @LayoutIDs = ''

	DECLARE @CustomerID int
	
	DECLARE @GroupID int
	DECLARE @query varchar(8000)
	DECLARE @innquery varchar(5000)
	DECLARE @BlastType varchar(10)
	CREATE TABLE #UDFFilter (emailID int)  

	SET @GroupID = 0
	
	IF @CampaignItemID IS NOT NULL
	BEGIN
		SELECT 
			@BlastIDs = COALESCE(@BlastIDs + ',' ,'') + CONVERT(varchar(10), cib.BlastID), @LayoutIDs = COALESCE(@LayoutIDs + ',' ,'') + CONVERT(varchar(10), cib.LayoutID), @CustomerID = c.CustomerID
		FROM 
			[Campaign] c WITH (NOLOCK)
			JOIN [CampaignItem] ci WITH (NOLOCK) ON c.CampaignID = ci.CampaignID and ci.IsDeleted = 0
			JOIN [CampaignItemBlast] cib WITH (NOLOCK) ON ci.CampaignItemID = cib.CampaignItemID and cib.IsDeleted = 0 and ISNULL(cib.BlastID, 0) != 0
		WHERE
			--c.CustomerID = @CustomerID AND
			c.IsDeleted = 0 AND
			ci.CampaignItemID = @CampaignItemID
			
		SET @BlastIDs = SUBSTRING(@BlastIDs,2,4000)	
		SET @LayoutIDs = SUBSTRING(@LayoutIDs,2,4000)	
		
	END
	ELSE
	BEGIN
		SELECT @BlastIDs = CONVERT(varchar(10),BlastID), @LayoutIDs = CONVERT(varchar(10),LayoutID), @GroupID = ISNULL(GroupID, 0), @CustomerID = CustomerID FROM [Blast] WITH (NOLOCK) WHERE BlastID = @BlastID-- and CustomerID = @CustomerID
		Select @BlastType = BlastType FROM Blast with(nolock) where BlastID = @BlastID
		If @GroupID = 0 and (@BlastType = 'Layout' or @BlastType = 'NoOpen')
		BEGIN
		Select @GroupID = GroupID from Blast WITH(NOLOCK) WHERE BlastID = (SELECT TOP 1 refBlastID from BlastSingles WITH(NOLOCK) WHERE BlastID = @BlastID)
		END
	END
	
	IF LEN(@BlastIDs) > 0 AND LEN(@LayoutIDs) > 0 AND LEN(@GroupID) > 0
	BEGIN
		DECLARE @RecordNoStart int
		DECLARE @RecordNoEnd int  
	  
		--set @PageNo = @PageNo - 1
		Set @RecordNoStart = (@PageNo * @PageSize) + 1  
		Set @RecordNoEnd = (@PageNo * @PageSize) + @PageSize  
		--print ('1')
		if (len(@UDFname) > 0 and len(@UDFdata) > 0 and @CampaignItemID IS NOT NULL)
		Begin

			insert into #UDFFilter
			select * from [fn_Blast_Report_Filter_By_UDF](@blastIDs,@UDFname,@UDFdata )

			set @innquery = ' select  bac.ClickID, b.groupID as groupID, e.EMailID as EmailID, E.emailaddress, bac.ClickTime as ClickTime, bac.URL AS FullLink,isnull(a.Alias,'''') as smallLink, ' +
							  --'	case when (CHARINDEX(''?eid'', bac.URL) > 0 or CHARINDEX(''&eid'', bac.URL) > 0) then SUBSTRING(bac.URL,1,(case when CHARINDEX(''?eid'', bac.URL) > 0 then  CHARINDEX(''?eid'', bac.URL) else CHARINDEX(''&eid'', bac.URL) end)+4) else bac.URL end as NewActionValue, '+
							  '  bac.URL as NewActionValue,' +
							  '	CASE WHEN bac.URL like ''http://www.kmpsgroup.com/subforms/mpmndigital_new.htm?promoCode=E017%'' THEN ''http://www.kmpsgroup.com/subforms/mpmndigital_new.htm?promoCode=E017'' ELSE bac.URL END as SmallActionValue ' +
									' from  ecn_Activity..[BlastActivityClicks] bac with (nolock) join [emails] e with (nolock) on bac.emailID = e.emailID  join #UDFFilter t on bac.emailID = t.emailID  join ' +
									' blast b on b.blastID = bac.blastID left outer join ' +
									' ( select distinct link, la.Alias
										from [linkAlias] la with (nolock) join
											[Content] c with (nolock) on c.contentiD = la.contentID join
											[Layout] L with (nolock) on (l.ContentSlot1 = c.contentID OR l.ContentSlot2 = c.contentID OR l.ContentSlot3 = c.contentID OR l.ContentSlot4 = c.contentID OR 
											l.ContentSlot5 = c.contentID OR l.ContentSlot6 = c.contentID OR l.ContentSlot7 = c.contentID OR l.ContentSlot8 = c.contentID OR 
											l.ContentSlot9 = c.contentID)
										WHERE  l.layoutID in (' + @LayoutIDs + ') and isnull(la.Alias, '''') <> '''') a on bac.URL = a.Link	
									WHERE bac.blastID in (' + @blastIDs + ') and b.blastID in (' + @blastIDs + ') '

			if len(rtrim(ltrim(@ISP))) > 0
				set @innquery = @innquery + ' AND e.emailaddress like ''%' + @ISP + ''' '

			if @ReportType = 'topclickslink'	--Top Clicks links download
			Begin
				exec(' SELECT bac.EMailID, e.EmailAddress, bac.ClickTime as ActionDate , e.FirstName, e.LastName, e.Voice AS Phone#, e.Company AS Company, '+
						' ''EmailID= ''+ CONVERT(VARCHAR,bac.EmailID) + ''&GroupID=''+ CONVERT(VARCHAR,b.groupid) AS URL '+
						' FROM [emails] e with (nolock) JOIN ecn_Activity..[BlastActivityClicks] bac with (nolock) on e.EMailID=bac.EMailID join #UDFFilter t on bac.emailID = t.emailID join blast b on bac.blastId = b.bID'+
						' WHERE bac.BlastID in (' + @blastIDs + ') AND bac.URL = '''+ @TopClickURL+ ''' ORDER BY ClickTime DESC');
			End
			else if @ReportType = 'topclicks'	--Top Clicks
			Begin
				if @CustomerID = 1209
					set @query = 'select '+@HowMuch+' Count(SmallActionValue) as ClickCount, Count(distinct EMailID) as distinctClickCount, SmallActionValue as NewActionValue,  ' +
						' CASE WHEN LEN(SmallActionValue) > 6 THEN ' +
							' (CASE when len(smallLink) =0 then LEFT(RIGHT(SmallActionValue,LEN(SmallActionValue)-7),100) else smallLink end) ELSE SmallActionValue END AS SmallLink ' +
						' from (' + @innquery +') inn group by SmallActionValue, smallLink order by ClickCount desc'
				else 
					set @query = 'select '+@HowMuch+' Count(SmallActionValue) as ClickCount, Count(distinct EMailID) as distinctClickCount, NewActionValue, ' +
								 ' CASE WHEN LEN(NewActionValue) > 6 THEN ' +
								 ' (CASE when len(smallLink) =0 then LEFT(RIGHT(NewActionValue,LEN(NewActionValue)-7),100) else smallLink end) ELSE NewActionValue END AS SmallLink ' +
								 ' from (' + @innquery +') inn group by NewActionValue, smallLink  order by ClickCount desc '
			end
			Else if @ReportType = 'topvisitors' 		--Top 10 Visistors

				set @query = 'SELECT TOP 10 Count(FullLink) AS ClickCount, EmailID, EmailAddress, ''EmailID='' + CONVERT(VARCHAR,EmailID) + ''&GroupID='' + CONVERT(VARCHAR,inn.groupID) AS URL' +
							 ' FROM (' + @innquery + ') inn GROUP BY EmailID, EmailAddress, inn.groupID ORDER BY ClickCount DESC, EmailAddress '

			Else if @ReportType = 'allclicks' --All clicks by datetime
			Begin
				

				set @innquery = ' select bac.ClickID, b.groupID, e.EMailID as EmailID, E.emailaddress, bac.ClickTime as ClickTime, bac.URL AS FullLink,isnull(a.Alias,'''') as smallLink, ' +
								  '	case when CHARINDEX(''eid'', bac.URL) = 0 then bac.URL else SUBSTRING(bac.URL,1,CHARINDEX(''eid'',bac.URL)+2) END as NewActionValue, '+
								  '	CASE WHEN bac.URL like ''http://www.kmpsgroup.com/subforms/mpmndigital_new.htm?promoCode=E017%'' THEN ''http://www.kmpsgroup.com/subforms/mpmndigital_new.htm?promoCode=E017'' ELSE bac.URL END as SmallActionValue ' +
										' from  ecn_Activity..[BlastActivityClicks] bac with (nolock) join [emails] e with (nolock) on bac.emailID = e.emailID join #UDFFilter t on eal.emailID = t.emailID  join ' +
										' blast b on b.blastID = bac.blastID left outer join ' +
										' ( select distinct link, la.Alias
											from [linkAlias] la with (nolock) join
												[Content] c with (nolock) on c.contentiD = la.contentID join
												[Layout] L with (nolock) on (l.ContentSlot1 = c.contentID OR l.ContentSlot2 = c.contentID OR l.ContentSlot3 = c.contentID OR l.ContentSlot4 = c.contentID OR 
												l.ContentSlot5 = c.contentID OR l.ContentSlot6 = c.contentID OR l.ContentSlot7 = c.contentID OR l.ContentSlot8 = c.contentID OR 
												l.ContentSlot9 = c.contentID)
											WHERE  l.layoutID in (' + @LayoutIDs + ') and isnull(la.Alias, '''') <> '''') a on bac.URL = a.Link	
										WHERE bac.blastID in (' + @blastIDs + ') and b.blastID in (' + @blastIDs + ') '

				if len(rtrim(ltrim(@ISP))) > 0
				Begin
					set @innquery = @innquery + ' AND e.emailaddress like ''%' + @ISP + ''' '

					select	Count(distinct ClickID) 
					from	ecn_Activity..[BlastActivityClicks] bac with (nolock) join ecn5_communicator..emails e with (nolock) on bac.emailID = e.emailID  JOIN 
									(SELECT ITEMS FROM fn_split(@blastIDs, ',')) ids ON ids.items = bac.blastID join #UDFFilter t on bac.emailID = t.emailID
					WHERE e.emailaddress like '%' + @ISP 

				End
				else
				Begin
					Select count(ClickID) from ecn_Activity..[BlastActivityClicks] bac with (nolock) JOIN 
									(SELECT ITEMS FROM fn_split(@blastIDs, ',')) ids ON ids.items = bac.blastID join #UDFFilter t on bac.emailID = t.emailID
				End
				

				set @query =  ' select emailID as EmailID, emailaddress, ''EmailID='' + CONVERT(VARCHAR,emailID) + ''&GroupID='' + CONVERT(VARCHAR,inn.groupID) AS URL, ClickTime, FullLink, ' +
						  ' CASE WHEN LEN(FullLink) > 6 THEN (CASE when len(smallLink) =0 then LEFT(RIGHT(FullLink,LEN(FullLink)-7),100) else smallLink end) ELSE  FullLink END AS SmallLink ' +
						  ' from (' + @innquery + ') inn order by ClickID DESC '

			end
			else if @ReportType = 'uniqueclicks'
			BEGIN
				declare @EmailIDs table (EmailID int)

				inSert into @EmailIDs(EmailID)
				Select distinct bac.EmailID 
				FROM ecn_Activity..[BlastActivityClicks] bac with (nolock)
					join #UDFFilter t on bac.emailID = t.emailID 
					
				WHERE bac.BlastID in ( @blastIDs )
;

				WITH Results
					AS (SELECT ROW_NUMBER() OVER (ORDER BY e.emailaddress) 
						AS ROWNUM,Count(e.EmailID) over () AS TotalCount, e.EMailID, e.EmailAddress, e.FirstName, e.LastName, e.Voice AS Phone#, e.Company AS Company
							  FROM [emails] e with (nolock) 
					 			JOIN @EmailIDs e2 on e.EmailID = e2.EmailID							
							  
					)
				SELECT * from Results
				WHERE ROWNUM between ((@PageNo - 1) * @PageSize + 1) and (@PageNo * @PageSize)
			END
		End
		Else
		begin	
			--print ('2')
			set @innquery = ' select  ClickID, b.groupID, e.EMailID as EmailID, E.emailaddress, bac.ClickTime as ClickTime, bac.URL AS FullLink,isnull(a.Alias,'''') as smallLink, ' +
							  --'	case when (CHARINDEX(''?eid'', bac.URL) > 0 or CHARINDEX(''&eid'', bac.URL) > 0) then SUBSTRING(bac.URL,1,(case when CHARINDEX(''?eid'', bac.URL) > 0 then  CHARINDEX(''?eid'', bac.URL) else CHARINDEX(''&eid'', bac.URL) end)+4) else bac.URL end as NewActionValue, '+
							  ' bac.URL as NewActionValue, '+
							  '	CASE WHEN bac.URL like ''http://www.kmpsgroup.com/subforms/mpmndigital_new.htm?promoCode=E017%'' THEN ''http://www.kmpsgroup.com/subforms/mpmndigital_new.htm?promoCode=E017'' ELSE bac.URL END as SmallActionValue ' +
									' from  ecn_Activity..[BlastActivityClicks] bac with (nolock) join ecn5_communicator..emails e with (nolock) on bac.emailID = e.emailID join ' +
									' blast b on b.blastID = bac.blastID left outer join ' +
									' ( select distinct link, la.Alias
										from [linkAlias] la with (nolock) join
											[Content] c with (nolock) on c.contentiD = la.contentID join
											[Layout] L with (nolock) on (l.ContentSlot1 = c.contentID OR l.ContentSlot2 = c.contentID OR l.ContentSlot3 = c.contentID OR l.ContentSlot4 = c.contentID OR 
											l.ContentSlot5 = c.contentID OR l.ContentSlot6 = c.contentID OR l.ContentSlot7 = c.contentID OR l.ContentSlot8 = c.contentID OR 
											l.ContentSlot9 = c.contentID)
										WHERE  l.layoutID in (' + @LayoutIDs + ') and isnull(la.Alias, '''') <> '''') a on bac.URL = a.Link	
									WHERE bac.blastID in (' + @blastIDs + ') and b.blastID in (' + @blastIDs + ') '

			if len(rtrim(ltrim(@ISP))) > 0
				set @innquery = @innquery + ' AND e.emailaddress like ''%' + @ISP + ''' '

			if @ReportType = 'topclickslink'	--Top Clicks links download
			Begin
			
				exec(' SELECT bac.EMailID, e.EmailAddress, bac.ClickTime as ActionDate , e.FirstName, e.LastName, e.Voice AS Phone#, e.Company AS Company, '+
						' ''EmailID= ''+ CONVERT(VARCHAR,bac.EmailID) + ''&GroupID='' + CONVERT(VARCHAR,b.groupID) AS URL '+
						' FROM [emails] e with (nolock) JOIN ecn_Activity..[BlastActivityClicks] bac with (nolock) on e.EMailID=bac.EMailID  join blast b on b.blastID = bac.blastID '+
						' WHERE bac.BlastID in (' + @blastIDs + ') AND  b.BlastID in (' + @blastIDs + ') AND bac.URL = '''+ @TopClickURL+ ''' ORDER BY ActionDate DESC');
			End
			else if @ReportType = 'topclicks'	--Top Clicks
			begin
				if @CustomerID = 1209
					set @query = 'select '+@HowMuch+' Count(SmallActionValue) as ClickCount, Count(distinct EMailID) as distinctClickCount, SmallActionValue as NewActionValue,  ' +
						' CASE WHEN LEN(SmallActionValue) > 6 THEN ' +
							' (CASE when len(smallLink) =0 then LEFT(RIGHT(SmallActionValue,LEN(SmallActionValue)-7),100) else smallLink end) ELSE SmallActionValue END AS SmallLink ' +
						' from (' + @innquery +') inn group by SmallActionValue, smallLink order by ClickCount desc'
				else 
					set @query = 'select '+@HowMuch+' Count(SmallActionValue) as ClickCount, Count(distinct EMailID) as distinctClickCount, NewActionValue, ' +
								 ' CASE WHEN LEN(NewActionValue) > 6 THEN ' +
								 ' (CASE when len(smallLink) =0 then LEFT(RIGHT(NewActionValue,LEN(NewActionValue)-7),100) else smallLink end) ELSE NewActionValue END AS SmallLink ' +
								 ' from (' + @innquery +') inn group by NewActionValue, smallLink  order by ClickCount desc '
			end
			Else if @ReportType = 'topvisitors' 		--Top 10 Visistors

				set @query = 'SELECT TOP 10 Count(FullLink) AS ClickCount, EmailID, EmailAddress, ''EmailID='' + CONVERT(VARCHAR,EmailID) + ''&GroupID='' + CONVERT(VARCHAR,inn.groupID) AS URL' +
							 ' FROM (' + @innquery + ') inn GROUP BY EmailID, EmailAddress, inn.groupID ORDER BY ClickCount DESC, EmailAddress '

			Else if @ReportType = 'allclicks' --All clicks by datetime
			Begin
			if @Unique = 1
				begin
					set @innquery = ' select ClickID, b.groupID, e.EMailID as EmailID, E.emailaddress, bac.ClickTime as ClickTime, bac.URL as FullLink,isnull(a.Alias,'''') as smallLink, ' +
									  '	case when CHARINDEX(''eid'', bac.URL) = 0 then bac.URL else SUBSTRING(bac.URL,1,CHARINDEX(''eid'',bac.URL)+2) END as NewActionValue, '+
									  '	CASE WHEN bac.URL like ''http://www.kmpsgroup.com/subforms/mpmndigital_new.htm?promoCode=E017%'' THEN ''http://www.kmpsgroup.com/subforms/mpmndigital_new.htm?promoCode=E017'' ELSE bac.URL END as SmallActionValue ' +
											' from  ecn_Activity..[BlastActivityClicks] bac with (nolock) join ecn5_communicator..emails e with (nolock) on bac.emailID = e.emailID   join ' +
										' blast b on b.blastID = bac.blastID left outer join ' +
											' ( select link, la.Alias
												from [linkAlias] la with (nolock) join
													[Content] c with (nolock) on c.contentiD = la.contentID join
													[Layout] L with (nolock) on (l.ContentSlot1 = c.contentID OR l.ContentSlot2 = c.contentID OR l.ContentSlot3 = c.contentID OR l.ContentSlot4 = c.contentID OR 
													l.ContentSlot5 = c.contentID OR l.ContentSlot6 = c.contentID OR l.ContentSlot7 = c.contentID OR l.ContentSlot8 = c.contentID OR 
													l.ContentSlot9 = c.contentID)
												WHERE  l.layoutID in (' + @LayoutIDs + ') and isnull(la.Alias, '''') <> '''') a on bac.URL = a.Link	
											WHERE bac.blastID in (' + @blastIDs + ') and b.blastID in (' + @blastIDs + ') '

					if len(rtrim(ltrim(@ISP))) > 0
					Begin
						set @innquery = @innquery + ' AND e.emailaddress like ''%' + @ISP + ''' '

						select	Count(distinct Convert(varchar(10),bac.EmailID) + URL) 
						from	ecn_Activity..[BlastActivityClicks] bac with (nolock) join [emails] e with (nolock) on bac.emailID = e.emailID  JOIN 
										(SELECT ITEMS FROM fn_split(@blastIDs, ',')) ids ON ids.items = bac.blastID 
						WHERE e.emailaddress like '%' + @ISP and bac.ClickTime between @StartDate and @EndDate

					End
					else
					Begin
						Select count(distinct Convert(varchar(10),EmailID) + URL) from ecn_Activity..[BlastActivityClicks] bac with (nolock) JOIN 
										(SELECT ITEMS FROM fn_split(@blastIDs, ',')) ids ON ids.items = bac.blastID
										where bac.ClickTime between @StartDate and @EndDate
					End

						set @innquery = @innquery + ' AND bac.ClickTime between ' + char(39) + @StartDate + char(39) + ' and ' + char(39) + @EndDate + char(39)
						
					set @query =  ' select emailID as EmailID, emailaddress, ''EmailID='' + CONVERT(VARCHAR,emailID) + ''&GroupID='' + CONVERT(VARCHAR,inn.groupID) AS URL , min(ClickTime) as ClickTime, FullLink, ' +
							  ' CASE WHEN LEN(FullLink) > 6 THEN (CASE when len(smallLink) =0 then LEFT(RIGHT(FullLink,LEN(FullLink)-7),100) else smallLink end) ELSE  FullLink END AS SmallLink ' +
							  ' from (' + @innquery + ') inn group by emailID ,	emailaddress, ''EmailID='' + CONVERT(VARCHAR,emailID) + ''&GroupID='' + CONVERT(VARCHAR,inn.groupID),FullLink, CASE 
							  WHEN LEN(FullLink) > 6 THEN (CASE when len(smallLink) =0 then LEFT(RIGHT(FullLink,LEN(FullLink)-7),100) else smallLink end) ELSE  FullLink END order by ClickTime DESC '
				end
				else
				begin
					set @innquery = ' select ClickID, b.groupID, e.EMailID as EmailID, E.emailaddress, bac.ClickTime as ClickTime, bac.URL as FullLink,isnull(a.Alias,'''') as smallLink, ' +
									  '	case when CHARINDEX(''eid'', bac.URL) = 0 then bac.URL else SUBSTRING(bac.URL,1,CHARINDEX(''eid'',bac.URL)+2) END as NewActionValue, '+
									  '	CASE WHEN bac.URL like ''http://www.kmpsgroup.com/subforms/mpmndigital_new.htm?promoCode=E017%'' THEN ''http://www.kmpsgroup.com/subforms/mpmndigital_new.htm?promoCode=E017'' ELSE bac.URL END as SmallActionValue ' +
											' from  ecn_Activity..[BlastActivityClicks] bac with (nolock) join ecn5_communicator..emails e with (nolock) on bac.emailID = e.emailID   join ' +
										' blast b on b.blastID = bac.blastID left outer join ' +
											' ( select link, la.Alias
												from [linkAlias] la with (nolock) join
													[Content] c with (nolock) on c.contentiD = la.contentID join
													[Layout] L with (nolock) on (l.ContentSlot1 = c.contentID OR l.ContentSlot2 = c.contentID OR l.ContentSlot3 = c.contentID OR l.ContentSlot4 = c.contentID OR 
													l.ContentSlot5 = c.contentID OR l.ContentSlot6 = c.contentID OR l.ContentSlot7 = c.contentID OR l.ContentSlot8 = c.contentID OR 
													l.ContentSlot9 = c.contentID)
												WHERE  l.layoutID in (' + @LayoutIDs + ') and isnull(la.Alias, '''') <> '''') a on bac.URL = a.Link	
											WHERE bac.blastID in (' + @blastIDs + ') and b.blastID in (' + @blastIDs + ') '

					if len(rtrim(ltrim(@ISP))) > 0
					Begin
						set @innquery = @innquery + ' AND e.emailaddress like ''%' + @ISP + ''' '

						select	Count(Convert(varchar(10),bac.EmailID) + URL) 
						from	ecn_Activity..[BlastActivityClicks] bac with (nolock) join [emails] e with (nolock) on bac.emailID = e.emailID  JOIN 
										(SELECT ITEMS FROM fn_split(@blastIDs, ',')) ids ON ids.items = bac.blastID 
						WHERE e.emailaddress like '%' + @ISP AND BAC.ClickTime between @StartDate and @EndDate

					End
					else
					Begin
						Select count(Convert(varchar(10),EmailID) + URL) 
						from ecn_Activity..[BlastActivityClicks] bac with (nolock) JOIN (SELECT ITEMS FROM fn_split(@blastIDs, ',')) ids ON ids.items = bac.blastID
						where BAC.ClickTime between @StartDate and @EndDate
					End

						set @innquery = @innquery + ' AND bac.ClickTime between ' + char(39) + @StartDate + char(39) + ' and ' + char(39) + @EndDate + char(39)
						
					set @query =  ' select emailID as EmailID, emailaddress, ''EmailID='' + CONVERT(VARCHAR,emailID) + ''&GroupID='' + CONVERT(VARCHAR,inn.groupID) AS URL , ClickTime, FullLink, ' +
							  ' CASE WHEN LEN(FullLink) > 6 THEN (CASE when len(smallLink) =0 then LEFT(RIGHT(FullLink,LEN(FullLink)-7),100) else smallLink end) ELSE  FullLink END AS SmallLink ' +
							  ' from (' + @innquery + ') inn order by ClickID desc '
				end
			end
			ELSE IF @ReportType = 'uniqueclicks'
			BEGIN
				declare @EmailIDs2 table (EmailID int)

				inSert into @EmailIDs2(EmailID)
				Select distinct bac.EmailID 
				FROM ecn_Activity..[BlastActivityClicks] bac with (nolock)
				JOIN (SELECT ITEMS FROM fn_split(@blastIDs, ',')) ids ON ids.items = bac.blastID ;

				WITH Results
					AS (SELECT ROW_NUMBER() OVER (ORDER BY e.emailaddress) 
						AS ROWNUM,Count(e.EmailID) over () AS TotalCount, e.EMailID, e.EmailAddress, e.FirstName, e.LastName, e.Voice AS Phone#, e.Company AS Company
							  FROM [emails] e with (nolock) 
					 			JOIN @EmailIDs2 e2 on e.EmailID = e2.EmailID							
							  
					)
				SELECT * from Results
				WHERE ROWNUM between ((@PageNo - 1) * @PageSize + 1) and (@PageNo * @PageSize)
			END
		END
		exec(@query)
	END
	
	drop table #UDFFilter
	SET NOCOUNT OFF
end