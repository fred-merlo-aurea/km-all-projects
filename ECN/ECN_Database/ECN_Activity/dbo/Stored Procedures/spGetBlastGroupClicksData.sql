CREATE PROCEDURE [dbo].[spGetBlastGroupClicksData] (
	@ID int, 
	@IsBlastGroup varchar(1),   
	@HowMuch varchar(25), 
	@ISP varchar(100),
	@ReportType varchar(20),
	@TopClickURL varchar(255),
	@PageNo int,  
	@PageSize int ,
	@UDFname varchar(100),
	@UDFdata    varchar(100)
)
as

Begin

	SET NOCOUNT ON
	
	declare @blastIDs varchar(4000), @groupID int, @layoutID int, @custID int, @query varchar (8000), @innquery varchar(5000)
	create table #UDFFilter (emailID int)  

	set @groupID = 0

	if @IsBlastGroup = 'Y'
	Begin
		SELECT @blastIDs = BlastIDs from ecn5_communicator..BlastGrouping with (nolock) WHERE BlastGroupID = @ID
		select @layoutID = layoutID, @custID = customerID  from ecn5_communicator..[BLAST] b with (nolock) JOIN 
					(SELECT top 1 ITEMS FROM ecn5_communicator..fn_split(@blastIDs, ',')) ids ON ids.items = b.blastID
	End
	else
	Begin


		SELECT @blastIDs = @ID
		select @groupID = groupID, @layoutID = layoutID, @custID = customerID  from ecn5_communicator..[BLAST] with (nolock) where blastID = @ID
	End

	declare @RecordNoStart int,  
			@RecordNoEnd int  
	  
		--set @PageNo = @PageNo - 1
		Set @RecordNoStart = (@PageNo * @PageSize) + 1  
		Set @RecordNoEnd = (@PageNo * @PageSize) + @PageSize  

	if (len(@UDFname) > 0 and len(@UDFdata) > 0 and @IsBlastGroup <> 'Y')
	Begin

		insert into #UDFFilter
		select * from ecn5_communicator..[fn_Blast_Report_Filter_By_UDF](@blastIDs,@UDFname,@UDFdata )

		set @innquery = ' select  bac.ClickID, e.EMailID as EmailID, E.emailaddress, bac.ClickTime as ClickTime, bac.URL AS FullLink,isnull(a.Alias,'''') as smallLink, ' +
						  '	case when (CHARINDEX(''?eid'', bac.URL) > 0 or CHARINDEX(''&eid'', bac.URL) > 0) then SUBSTRING(bac.URL,1,(case when CHARINDEX(''?eid'', bac.URL) > 0 then  CHARINDEX(''?eid'', bac.URL) else CHARINDEX(''&eid'', bac.URL) end)+4) else bac.URL end as NewActionValue, '+
						  '	CASE WHEN bac.URL like ''http://www.kmpsgroup.com/subforms/mpmndigital_new.htm?promoCode=E017%'' THEN ''http://www.kmpsgroup.com/subforms/mpmndigital_new.htm?promoCode=E017'' ELSE bac.URL END as SmallActionValue ' +
								' from  BlastActivityClicks bac with (nolock) join ecn5_communicator..emails e with (nolock) on bac.emailID = e.emailID  join #UDFFilter t on bac.emailID = t.emailID left outer join ' +
								' ( select distinct link, la.Alias
									from ecn5_communicator..linkAlias la with (nolock) join
										ecn5_communicator..Content c with (nolock) on c.contentiD = la.contentID join
										ecn5_communicator..[LAYOUT] L with (nolock) on (l.ContentSlot1 = c.contentID OR l.ContentSlot2 = c.contentID OR l.ContentSlot3 = c.contentID OR l.ContentSlot4 = c.contentID OR 
										l.ContentSlot5 = c.contentID OR l.ContentSlot6 = c.contentID OR l.ContentSlot7 = c.contentID OR l.ContentSlot8 = c.contentID OR 
										l.ContentSlot9 = c.contentID)
									WHERE  l.layoutID = ' + Convert(varchar,@layoutID) + ' and isnull(la.Alias, '''') <> '''') a on bac.URL = a.Link	
								WHERE blastID in (' + @blastIDs + ') '

		if len(rtrim(ltrim(@ISP))) > 0
			set @innquery = @innquery + ' AND e.emailaddress like ''%' + @ISP + ''' '

		if @ReportType = 'topclickslink'	--Top Clicks links download
		Begin
			exec(' SELECT bac.EMailID, e.EmailAddress, bac.ClickTime as ActionDate , e.FirstName, e.LastName, e.Voice AS Phone#, e.Company AS Company, '+
					' ''EmailID= ''+ CONVERT(VARCHAR,bac.EmailID) + ''&GroupID='+ @GroupID + ''' AS URL '+
					' FROM ecn5_communicator..emails e with (nolock) JOIN BlastActivityClicks bac with (nolock) on e.EMailID=bac.EMailID join #UDFFilter t on bac.emailID = t.emailID '+
					' WHERE bac.BlastID in (' + @blastIDs + ') AND bac.URL = '''+ @TopClickURL+ ''' ORDER BY ClickTime DESC');
		End
		else if @ReportType = 'topclicks'	--Top Clicks
		Begin
			if @custID = 1209
				set @query = 'select '+@HowMuch+' Count(SmallActionValue) as ClickCount, Count(distinct EMailID) as distinctClickCount, SmallActionValue as NewActionValue,  ' +
					' CASE WHEN LEN(SmallActionValue) > 6 THEN ' +
						' (CASE when len(smallLink) =0 then LEFT(RIGHT(SmallActionValue,LEN(SmallActionValue)-7),88) else smallLink end) ELSE SmallActionValue END AS SmallLink ' +
					' from (' + @innquery +') inn group by SmallActionValue, smallLink order by ClickCount desc'
			else 
				set @query = 'select '+@HowMuch+' Count(SmallActionValue) as ClickCount, Count(distinct EMailID) as distinctClickCount, NewActionValue, ' +
							 ' CASE WHEN LEN(NewActionValue) > 6 THEN ' +
							 ' (CASE when len(smallLink) =0 then LEFT(RIGHT(NewActionValue,LEN(NewActionValue)-7),88) else smallLink end) ELSE NewActionValue END AS SmallLink ' +
							 ' from (' + @innquery +') inn group by NewActionValue, smallLink  order by ClickCount desc '
		end
		Else if @ReportType = 'topvisitors' 		--Top 10 Visistors

			set @query = 'SELECT TOP 10 Count(FullLink) AS ClickCount, EmailID, EmailAddress, ''EmailID='' + CONVERT(VARCHAR,EmailID) + ''&GroupID=' + CONVERT(VARCHAR,@groupID) + ''' AS ''URL''' +
						 ' FROM (' + @innquery + ') inn GROUP BY EmailID, EmailAddress ORDER BY ClickCount DESC, EmailAddress '

		Else if @ReportType = 'allclicks' --All clicks by datetime
		Begin
			

			set @innquery = ' select bac.ClickID, e.EMailID as EmailID, E.emailaddress, bac.ClickTime as ClickTime, bac.URL AS FullLink,isnull(a.Alias,'''') as smallLink, ' +
							  '	case when CHARINDEX(''eid'', bac.URL) = 0 then bac.URL else SUBSTRING(bac.URL,1,CHARINDEX(''eid'',bac.URL)+2) END as NewActionValue, '+
							  '	CASE WHEN bac.URL like ''http://www.kmpsgroup.com/subforms/mpmndigital_new.htm?promoCode=E017%'' THEN ''http://www.kmpsgroup.com/subforms/mpmndigital_new.htm?promoCode=E017'' ELSE bac.URL END as SmallActionValue ' +
									' from  BlastActivityClicks bac with (nolock) join ecn5_communicator..emails e with (nolock) on bac.emailID = e.emailID join #UDFFilter t on eal.emailID = t.emailID left outer join ' +
									' ( select distinct link, la.Alias
										from ecn5_communicator..linkAlias la with (nolock) join
											ecn5_communicator..Content c with (nolock) on c.contentiD = la.contentID join
											ecn5_communicator..[LAYOUT] L with (nolock) on (l.ContentSlot1 = c.contentID OR l.ContentSlot2 = c.contentID OR l.ContentSlot3 = c.contentID OR l.ContentSlot4 = c.contentID OR 
											l.ContentSlot5 = c.contentID OR l.ContentSlot6 = c.contentID OR l.ContentSlot7 = c.contentID OR l.ContentSlot8 = c.contentID OR 
											l.ContentSlot9 = c.contentID)
										WHERE  l.layoutID = ' + Convert(varchar,@layoutID) + ' and isnull(la.Alias, '''') <> '''') a on bac.URL = a.Link	
									WHERE blastID in (' + @blastIDs + ') '

			if len(rtrim(ltrim(@ISP))) > 0
			Begin
				set @innquery = @innquery + ' AND e.emailaddress like ''%' + @ISP + ''' '

				select	Count(distinct ClickID) 
				from	BlastActivityClicks bac with (nolock) join ecn5_communicator..emails e with (nolock) on bac.emailID = e.emailID  JOIN 
								(SELECT ITEMS FROM ecn5_communicator..fn_split(@blastIDs, ',')) ids ON ids.items = bac.blastID join #UDFFilter t on bac.emailID = t.emailID
				WHERE e.emailaddress like '%' + @ISP 

			End
			else
			Begin
				Select count(ClickID) from BlastActivityClicks bac with (nolock) JOIN 
								(SELECT ITEMS FROM ecn5_communicator..fn_split(@blastIDs, ',')) ids ON ids.items = bac.blastID join #UDFFilter t on bac.emailID = t.emailID
			End

			set @query =  ' select emailID as EmailID, emailaddress, ''EmailID='' + CONVERT(VARCHAR,emailID) + ''&GroupID=' + CONVERT(VARCHAR,@groupID) + ''' AS ''URL'', ClickTime, FullLink, ' +
					  ' CASE WHEN LEN(FullLink) > 6 THEN (CASE when len(smallLink) =0 then LEFT(RIGHT(FullLink,LEN(FullLink)-7),70) else smallLink end) ELSE  FullLink END AS SmallLink ' +
					  ' from (' + @innquery + ') inn order by ClickID DESC '

		end
	End
	Else
	Begin	
		set @innquery = ' select  ClickID, e.EMailID as EmailID, E.emailaddress, bac.ClickTime as ClickTime, bac.URL AS FullLink,isnull(a.Alias,'''') as smallLink, ' +
						  '	case when (CHARINDEX(''?eid'', bac.URL) > 0 or CHARINDEX(''&eid'', bac.URL) > 0) then SUBSTRING(bac.URL,1,(case when CHARINDEX(''?eid'', bac.URL) > 0 then  CHARINDEX(''?eid'', bac.URL) else CHARINDEX(''&eid'', bac.URL) end)+4) else bac.URL end as NewActionValue, '+
						  '	CASE WHEN bac.URL like ''http://www.kmpsgroup.com/subforms/mpmndigital_new.htm?promoCode=E017%'' THEN ''http://www.kmpsgroup.com/subforms/mpmndigital_new.htm?promoCode=E017'' ELSE bac.URL END as SmallActionValue ' +
								' from  BlastActivityClicks bac with (nolock) join ecn5_communicator..emails e with (nolock) on bac.emailID = e.emailID left outer join ' +
								' ( select distinct link, la.Alias
									from ecn5_communicator..linkAlias la with (nolock) join
										ecn5_communicator..Content c with (nolock) on c.contentiD = la.contentID join
										ecn5_communicator..[LAYOUT] L with (nolock) on (l.ContentSlot1 = c.contentID OR l.ContentSlot2 = c.contentID OR l.ContentSlot3 = c.contentID OR l.ContentSlot4 = c.contentID OR 
										l.ContentSlot5 = c.contentID OR l.ContentSlot6 = c.contentID OR l.ContentSlot7 = c.contentID OR l.ContentSlot8 = c.contentID OR 
										l.ContentSlot9 = c.contentID)
									WHERE  l.layoutID = ' + Convert(varchar,@layoutID) + ' and isnull(la.Alias, '''') <> '''') a on bac.URL = a.Link	
								WHERE blastID in (' + @blastIDs + ') '

		if len(rtrim(ltrim(@ISP))) > 0
			set @innquery = @innquery + ' AND e.emailaddress like ''%' + @ISP + ''' '

		if @ReportType = 'topclickslink'	--Top Clicks links download
		Begin
			exec(' SELECT bac.EMailID, e.EmailAddress, bac.ClickTime as ActionDate , e.FirstName, e.LastName, e.Voice AS Phone#, e.Company AS Company, '+
					' ''EmailID= ''+ CONVERT(VARCHAR,bac.EmailID) + ''&GroupID='+ @GroupID + ''' AS URL '+
					' FROM ecn5_communicator..emails e with (nolock) JOIN BlastActivityClicks bac with (nolock) on e.EMailID=bac.EMailID  '+
					' WHERE bac.BlastID in (' + @blastIDs + ') AND bac.URL = '''+ @TopClickURL+ ''' ORDER BY ActionDate DESC');
		End
		else if @ReportType = 'topclicks'	--Top Clicks
		Begin
			if @custID = 1209
				set @query = 'select '+@HowMuch+' Count(SmallActionValue) as ClickCount, Count(distinct EMailID) as distinctClickCount, SmallActionValue as NewActionValue,  ' +
					' CASE WHEN LEN(SmallActionValue) > 6 THEN ' +
						' (CASE when len(smallLink) =0 then LEFT(RIGHT(SmallActionValue,LEN(SmallActionValue)-7),88) else smallLink end) ELSE SmallActionValue END AS SmallLink ' +
					' from (' + @innquery +') inn group by SmallActionValue, smallLink order by ClickCount desc'
			else 
				set @query = 'select '+@HowMuch+' Count(SmallActionValue) as ClickCount, Count(distinct EMailID) as distinctClickCount, NewActionValue, ' +
							 ' CASE WHEN LEN(NewActionValue) > 6 THEN ' +
							 ' (CASE when len(smallLink) =0 then LEFT(RIGHT(NewActionValue,LEN(NewActionValue)-7),88) else smallLink end) ELSE NewActionValue END AS SmallLink ' +
							 ' from (' + @innquery +') inn group by NewActionValue, smallLink  order by ClickCount desc '
		end
		Else if @ReportType = 'topvisitors' 		--Top 10 Visistors

			set @query = 'SELECT TOP 10 Count(FullLink) AS ClickCount, EmailID, EmailAddress, ''EmailID='' + CONVERT(VARCHAR,EmailID) + ''&GroupID=' + CONVERT(VARCHAR,@groupID) + ''' AS ''URL''' +
						 ' FROM (' + @innquery + ') inn GROUP BY EmailID, EmailAddress ORDER BY ClickCount DESC, EmailAddress '

		Else if @ReportType = 'allclicks' --All clicks by datetime
		Begin
			

			set @innquery = ' select  ClickID, e.EMailID as EmailID, E.emailaddress, bac.ClickTime as ClickTime, bac.URL as FullLink,isnull(a.Alias,'''') as smallLink, ' +
							  '	case when CHARINDEX(''eid'', bac.URL) = 0 then bac.URL else SUBSTRING(bac.URL,1,CHARINDEX(''eid'',bac.URL)+2) END as NewActionValue, '+
							  '	CASE WHEN bac.URL like ''http://www.kmpsgroup.com/subforms/mpmndigital_new.htm?promoCode=E017%'' THEN ''http://www.kmpsgroup.com/subforms/mpmndigital_new.htm?promoCode=E017'' ELSE bac.URL END as SmallActionValue ' +
									' from  BlastActivityClicks bac with (nolock) join ecn5_communicator..emails e with (nolock) on bac.emailID = e.emailID  left outer join ' +
									' ( select distinct link, la.Alias
										from ecn5_communicator..linkAlias la with (nolock) join
											ecn5_communicator..Content c with (nolock) on c.contentiD = la.contentID join
											ecn5_communicator..[LAYOUT] L with (nolock) on (l.ContentSlot1 = c.contentID OR l.ContentSlot2 = c.contentID OR l.ContentSlot3 = c.contentID OR l.ContentSlot4 = c.contentID OR 
											l.ContentSlot5 = c.contentID OR l.ContentSlot6 = c.contentID OR l.ContentSlot7 = c.contentID OR l.ContentSlot8 = c.contentID OR 
											l.ContentSlot9 = c.contentID)
										WHERE  l.layoutID = ' + Convert(varchar,@layoutID) + ' and isnull(la.Alias, '''') <> '''') a on bac.URL = a.Link	
									WHERE blastID in (' + @blastIDs + ') '

			if len(rtrim(ltrim(@ISP))) > 0
			Begin
				set @innquery = @innquery + ' AND e.emailaddress like ''%' + @ISP + ''' '

				select	Count(ClickID) 
				from	BlastActivityClicks bac with (nolock) join ecn5_communicator..emails e with (nolock) on bac.emailID = e.emailID  JOIN 
								(SELECT ITEMS FROM ecn5_communicator..fn_split(@blastIDs, ',')) ids ON ids.items = bac.blastID 
				WHERE e.emailaddress like '%' + @ISP 

			End
			else
			Begin
				Select count(ClickID) from BlastActivityClicks bac with (nolock) JOIN 
								(SELECT ITEMS FROM ecn5_communicator..fn_split(@blastIDs, ',')) ids ON ids.items = bac.blastID
			End

			set @query =  ' select emailID as EmailID, emailaddress, ''EmailID='' + CONVERT(VARCHAR,emailID) + ''&GroupID=' + CONVERT(VARCHAR,@groupID) + ''' AS ''URL'', ClickTime, FullLink, ' +
					  ' CASE WHEN LEN(FullLink) > 6 THEN (CASE when len(smallLink) =0 then LEFT(RIGHT(FullLink,LEN(FullLink)-7),70) else smallLink end) ELSE  FullLink END AS SmallLink ' +
					  ' from (' + @innquery + ') inn order by ClickID DESC '

		end
	End

	exec(@query)

	drop table #UDFFilter
	SET NOCOUNT OFF
End
