CREATE PROCEDURE [dbo].[MovedToActivity_sp_getBlastGroupClicksData] (
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
	INSERT INTO ECN_ACTIVITY..OldProcsInUse (ProcName, DateUsed) VALUES ('sp_getBlastGroupClicksData', GETDATE()) 
	
	declare @blastIDs varchar(4000), @groupID int, @layoutID int, @custID int, @query varchar (8000), @innquery varchar(5000)
	create table #UDFFilter (emailID int)  

	set @groupID = 0

	if @IsBlastGroup = 'Y'
	Begin
		SELECT @blastIDs = BlastIDs from BlastGrouping WHERE BlastGroupID = @ID
		select @layoutID = layoutID, @custID = customerID  from [BLAST] b JOIN 
					(SELECT top 1 ITEMS FROM DBO.fn_split(@blastIDs, ',')) ids ON ids.items = b.blastID
	End
	else
	Begin


		SELECT @blastIDs = @ID
		select @groupID = groupID, @layoutID = layoutID, @custID = customerID  from [BLAST] where blastID = @ID
	End

	declare @RecordNoStart int,  
			@RecordNoEnd int  
	  
		--set @PageNo = @PageNo - 1
		Set @RecordNoStart = (@PageNo * @PageSize) + 1  
		Set @RecordNoEnd = (@PageNo * @PageSize) + @PageSize  

	if (len(@UDFname) > 0 and len(@UDFdata) > 0 and @IsBlastGroup <> 'Y')
	Begin

		insert into #UDFFilter
		select * from dbo.[fn_Blast_Report_Filter_By_UDF](@blastIDs,@UDFname,@UDFdata )

		set @innquery = ' select  eal.EAID, e.EMailID as EmailID, E.emailaddress, eal.ActionDate as ClickTime, eal.ActionValue AS FullLink,isnull(a.Alias,'''') as smallLink, ' +
						  '	case when (CHARINDEX(''?eid'', eal.ActionValue) > 0 or CHARINDEX(''&eid'', eal.ActionValue) > 0) then SUBSTRING(eal.ActionValue,1,(case when CHARINDEX(''?eid'', eal.ActionValue) > 0 then  CHARINDEX(''?eid'', eal.ActionValue) else CHARINDEX(''&eid'', eal.ActionValue) end)+4) else eal.ActionValue end as NewActionValue, '+
						  '	CASE WHEN eal.ActionValue like ''http://www.kmpsgroup.com/subforms/mpmndigital_new.htm?promoCode=E017%'' THEN ''http://www.kmpsgroup.com/subforms/mpmndigital_new.htm?promoCode=E017'' ELSE eal.ActionValue END as SmallActionValue ' +
								' from  EmailActivityLog eal join emails e on eal.emailID = e.emailID  join #UDFFilter t on eal.emailID = t.emailID left outer join ' +
								' ( select distinct link, la.Alias
									from linkAlias la join
										Content c on c.contentiD = la.contentID join
										[LAYOUT] L on (l.ContentSlot1 = c.contentID OR l.ContentSlot2 = c.contentID OR l.ContentSlot3 = c.contentID OR l.ContentSlot4 = c.contentID OR 
										l.ContentSlot5 = c.contentID OR l.ContentSlot6 = c.contentID OR l.ContentSlot7 = c.contentID OR l.ContentSlot8 = c.contentID OR 
										l.ContentSlot9 = c.contentID)
									WHERE  l.layoutID = ' + Convert(varchar,@layoutID) + ' and isnull(la.Alias, '''') <> '''') a on eal.ActionValue = a.Link	
								WHERE blastID in (' + @blastIDs + ') and  ActionTypeCode=''click'''

		if len(rtrim(ltrim(@ISP))) > 0
			set @innquery = @innquery + ' AND e.emailaddress like ''%' + @ISP + ''' '

		if @ReportType = 'topclickslink'	--Top Clicks links download
		Begin
			exec(' SELECT eal.EMailID, e.EmailAddress, eal.ActionDate , e.FirstName, e.LastName, e.Voice AS Phone#, e.Company AS Company, '+
					' ''EmailID= ''+ CONVERT(VARCHAR,eal.EmailID) + ''&GroupID='+ @GroupID + ''' AS URL '+
					' FROM Emails e JOIN EmailActivityLog eal on e.EMailID=eal.EMailID join #UDFFilter t on eal.emailID = t.emailID '+
					' WHERE eal.ActionTypeCode=''click'' AND eal.BlastID in (' + @blastIDs + ') AND eal.ActionValue = '''+ @TopClickURL+ ''' ORDER BY ActionDate DESC');
		End
		else if @ReportType = 'topclicks'	--Top Clicks
		Begin
			if @custID = 1209
				set @query = 'select '+@HowMuch+' Count(SmallActionValue) as ClickCount, Count(distinct EMailID) as distinctClickCount, SmallActionValue as NewActionValue,  ' +
					' CASE WHEN LEN(SmallActionValue) > 6 THEN ' +
						' (CASE when len(smallLink) =0 then LEFT(RIGHT(SmallActionValue,LEN(SmallActionValue)-7),58) else smallLink end) ELSE SmallActionValue END AS SmallLink ' +
					' from (' + @innquery +') inn group by SmallActionValue, smallLink order by ClickCount desc'
			else 
				set @query = 'select '+@HowMuch+' Count(SmallActionValue) as ClickCount, Count(distinct EMailID) as distinctClickCount, NewActionValue, ' +
							 ' CASE WHEN LEN(NewActionValue) > 6 THEN ' +
							 ' (CASE when len(smallLink) =0 then LEFT(RIGHT(NewActionValue,LEN(NewActionValue)-7),58) else smallLink end) ELSE NewActionValue END AS SmallLink ' +
							 ' from (' + @innquery +') inn group by NewActionValue, smallLink  order by ClickCount desc '
		end
		Else if @ReportType = 'topvisitors' 		--Top 10 Visistors

			set @query = 'SELECT TOP 10 Count(FullLink) AS ClickCount, EmailID, EmailAddress, ''EmailID='' + CONVERT(VARCHAR,EmailID) + ''&GroupID=' + CONVERT(VARCHAR,@groupID) + ''' AS ''URL''' +
						 ' FROM (' + @innquery + ') inn GROUP BY EmailID, EmailAddress ORDER BY ClickCount DESC, EmailAddress '

		Else if @ReportType = 'allclicks' --All clicks by datetime
		Begin
			

			set @innquery = ' select eal.EAID, e.EMailID as EmailID, E.emailaddress, eal.ActionDate as ClickTime, eal.ActionValue AS FullLink,isnull(a.Alias,'''') as smallLink, ' +
							  '	case when CHARINDEX(''eid'', eal.ActionValue) = 0 then eal.ActionValue else SUBSTRING(eal.ActionValue,1,CHARINDEX(''eid'',eal.ActionValue)+2) END as NewActionValue, '+
							  '	CASE WHEN eal.ActionValue like ''http://www.kmpsgroup.com/subforms/mpmndigital_new.htm?promoCode=E017%'' THEN ''http://www.kmpsgroup.com/subforms/mpmndigital_new.htm?promoCode=E017'' ELSE eal.ActionValue END as SmallActionValue ' +
									' from  EmailActivityLog eal join emails e on eal.emailID = e.emailID join #UDFFilter t on eal.emailID = t.emailID left outer join ' +
									' ( select distinct link, la.Alias
										from linkAlias la join
											Content c on c.contentiD = la.contentID join
											[LAYOUT] L on (l.ContentSlot1 = c.contentID OR l.ContentSlot2 = c.contentID OR l.ContentSlot3 = c.contentID OR l.ContentSlot4 = c.contentID OR 
											l.ContentSlot5 = c.contentID OR l.ContentSlot6 = c.contentID OR l.ContentSlot7 = c.contentID OR l.ContentSlot8 = c.contentID OR 
											l.ContentSlot9 = c.contentID)
										WHERE  l.layoutID = ' + Convert(varchar,@layoutID) + ' and isnull(la.Alias, '''') <> '''') a on eal.ActionValue = a.Link	
									WHERE blastID in (' + @blastIDs + ') and ActionTypeCode=''click'''

			if len(rtrim(ltrim(@ISP))) > 0
			Begin
				set @innquery = @innquery + ' AND e.emailaddress like ''%' + @ISP + ''' '

				select	Count(distinct eal.emailID) 
				from	EmailActivityLog eal join emails e on eal.emailID = e.emailID  JOIN 
								(SELECT ITEMS FROM DBO.fn_split(@blastIDs, ',')) ids ON ids.items = eal.blastID join #UDFFilter t on eal.emailID = t.emailID
				WHERE ActionTypeCode='click' AND e.emailaddress like '%' + @ISP 

			End
			else
			Begin
				Select count(EAL.EAID) from EmailActivityLog eal JOIN 
								(SELECT ITEMS FROM DBO.fn_split(@blastIDs, ',')) ids ON ids.items = eal.blastID join #UDFFilter t on eal.emailID = t.emailID
				WHERE ActionTypeCode='click'
			End

			set @query =  ' select emailID as EmailID, emailaddress, ''EmailID='' + CONVERT(VARCHAR,emailID) + ''&GroupID=' + CONVERT(VARCHAR,@groupID) + ''' AS ''URL'', ClickTime, FullLink, ' +
					  ' CASE WHEN LEN(FullLink) > 6 THEN (CASE when len(smallLink) =0 then LEFT(RIGHT(FullLink,LEN(FullLink)-7),40) else smallLink end) ELSE  FullLink END AS SmallLink ' +
					  ' from (' + @innquery + ') inn order by EAID DESC '

		end
	End
	Else
	Begin	
		set @innquery = ' select  eal.EAID, e.EMailID as EmailID, E.emailaddress, eal.ActionDate as ClickTime, eal.ActionValue AS FullLink,isnull(a.Alias,'''') as smallLink, ' +
						  '	case when (CHARINDEX(''?eid'', eal.ActionValue) > 0 or CHARINDEX(''&eid'', eal.ActionValue) > 0) then SUBSTRING(eal.ActionValue,1,(case when CHARINDEX(''?eid'', eal.ActionValue) > 0 then  CHARINDEX(''?eid'', eal.ActionValue) else CHARINDEX(''&eid'', eal.ActionValue) end)+4) else eal.ActionValue end as NewActionValue, '+
						  '	CASE WHEN eal.ActionValue like ''http://www.kmpsgroup.com/subforms/mpmndigital_new.htm?promoCode=E017%'' THEN ''http://www.kmpsgroup.com/subforms/mpmndigital_new.htm?promoCode=E017'' ELSE eal.ActionValue END as SmallActionValue ' +
								' from  EmailActivityLog eal join emails e on eal.emailID = e.emailID left outer join ' +
								' ( select distinct link, la.Alias
									from linkAlias la join
										Content c on c.contentiD = la.contentID join
										[LAYOUT] L on (l.ContentSlot1 = c.contentID OR l.ContentSlot2 = c.contentID OR l.ContentSlot3 = c.contentID OR l.ContentSlot4 = c.contentID OR 
										l.ContentSlot5 = c.contentID OR l.ContentSlot6 = c.contentID OR l.ContentSlot7 = c.contentID OR l.ContentSlot8 = c.contentID OR 
										l.ContentSlot9 = c.contentID)
									WHERE  l.layoutID = ' + Convert(varchar,@layoutID) + ' and isnull(la.Alias, '''') <> '''') a on eal.ActionValue = a.Link	
								WHERE blastID in (' + @blastIDs + ') and  ActionTypeCode=''click'''

		if len(rtrim(ltrim(@ISP))) > 0
			set @innquery = @innquery + ' AND e.emailaddress like ''%' + @ISP + ''' '

		if @ReportType = 'topclickslink'	--Top Clicks links download
		Begin
			exec(' SELECT eal.EMailID, e.EmailAddress, eal.ActionDate , e.FirstName, e.LastName, e.Voice AS Phone#, e.Company AS Company, '+
					' ''EmailID= ''+ CONVERT(VARCHAR,eal.EmailID) + ''&GroupID='+ @GroupID + ''' AS URL '+
					' FROM Emails e JOIN EmailActivityLog eal on e.EMailID=eal.EMailID  '+
					' WHERE eal.ActionTypeCode=''click'' AND eal.BlastID in (' + @blastIDs + ') AND eal.ActionValue = '''+ @TopClickURL+ ''' ORDER BY ActionDate DESC');
		End
		else if @ReportType = 'topclicks'	--Top Clicks
		Begin
			if @custID = 1209
				set @query = 'select '+@HowMuch+' Count(SmallActionValue) as ClickCount, Count(distinct EMailID) as distinctClickCount, SmallActionValue as NewActionValue,  ' +
					' CASE WHEN LEN(SmallActionValue) > 6 THEN ' +
						' (CASE when len(smallLink) =0 then LEFT(RIGHT(SmallActionValue,LEN(SmallActionValue)-7),58) else smallLink end) ELSE SmallActionValue END AS SmallLink ' +
					' from (' + @innquery +') inn group by SmallActionValue, smallLink order by ClickCount desc'
			else 
				set @query = 'select '+@HowMuch+' Count(SmallActionValue) as ClickCount, Count(distinct EMailID) as distinctClickCount, NewActionValue, ' +
							 ' CASE WHEN LEN(NewActionValue) > 6 THEN ' +
							 ' (CASE when len(smallLink) =0 then LEFT(RIGHT(NewActionValue,LEN(NewActionValue)-7),58) else smallLink end) ELSE NewActionValue END AS SmallLink ' +
							 ' from (' + @innquery +') inn group by NewActionValue, smallLink  order by ClickCount desc '
		end
		Else if @ReportType = 'topvisitors' 		--Top 10 Visistors

			set @query = 'SELECT TOP 10 Count(FullLink) AS ClickCount, EmailID, EmailAddress, ''EmailID='' + CONVERT(VARCHAR,EmailID) + ''&GroupID=' + CONVERT(VARCHAR,@groupID) + ''' AS ''URL''' +
						 ' FROM (' + @innquery + ') inn GROUP BY EmailID, EmailAddress ORDER BY ClickCount DESC, EmailAddress '

		Else if @ReportType = 'allclicks' --All clicks by datetime
		Begin
			

			set @innquery = ' select  eal.EAID, e.EMailID as EmailID, E.emailaddress, eal.ActionDate as ClickTime, eal.ActionValue AS FullLink,isnull(a.Alias,'''') as smallLink, ' +
							  '	case when CHARINDEX(''eid'', eal.ActionValue) = 0 then eal.ActionValue else SUBSTRING(eal.ActionValue,1,CHARINDEX(''eid'',eal.ActionValue)+2) END as NewActionValue, '+
							  '	CASE WHEN eal.ActionValue like ''http://www.kmpsgroup.com/subforms/mpmndigital_new.htm?promoCode=E017%'' THEN ''http://www.kmpsgroup.com/subforms/mpmndigital_new.htm?promoCode=E017'' ELSE eal.ActionValue END as SmallActionValue ' +
									' from  EmailActivityLog eal join emails e on eal.emailID = e.emailID  left outer join ' +
									' ( select distinct link, la.Alias
										from linkAlias la join
											Content c on c.contentiD = la.contentID join
											[LAYOUT] L on (l.ContentSlot1 = c.contentID OR l.ContentSlot2 = c.contentID OR l.ContentSlot3 = c.contentID OR l.ContentSlot4 = c.contentID OR 
											l.ContentSlot5 = c.contentID OR l.ContentSlot6 = c.contentID OR l.ContentSlot7 = c.contentID OR l.ContentSlot8 = c.contentID OR 
											l.ContentSlot9 = c.contentID)
										WHERE  l.layoutID = ' + Convert(varchar,@layoutID) + ' and isnull(la.Alias, '''') <> '''') a on eal.ActionValue = a.Link	
									WHERE blastID in (' + @blastIDs + ') and ActionTypeCode=''click'''

			if len(rtrim(ltrim(@ISP))) > 0
			Begin
				set @innquery = @innquery + ' AND e.emailaddress like ''%' + @ISP + ''' '

				select	Count(EAL.EAID) 
				from	EmailActivityLog eal join emails e on eal.emailID = e.emailID  JOIN 
								(SELECT ITEMS FROM DBO.fn_split(@blastIDs, ',')) ids ON ids.items = eal.blastID 
				WHERE ActionTypeCode='click' AND e.emailaddress like '%' + @ISP 

			End
			else
			Begin
				Select count(EAL.EAID) from EmailActivityLog eal JOIN 
								(SELECT ITEMS FROM DBO.fn_split(@blastIDs, ',')) ids ON ids.items = eal.blastID
				WHERE ActionTypeCode='click'
			End

			set @query =  ' select emailID as EmailID, emailaddress, ''EmailID='' + CONVERT(VARCHAR,emailID) + ''&GroupID=' + CONVERT(VARCHAR,@groupID) + ''' AS ''URL'', ClickTime, FullLink, ' +
					  ' CASE WHEN LEN(FullLink) > 6 THEN (CASE when len(smallLink) =0 then LEFT(RIGHT(FullLink,LEN(FullLink)-7),40) else smallLink end) ELSE  FullLink END AS SmallLink ' +
					  ' from (' + @innquery + ') inn order by EAID DESC '

		end
	End

	exec(@query)

	drop table #UDFFilter
	SET NOCOUNT OFF
End
