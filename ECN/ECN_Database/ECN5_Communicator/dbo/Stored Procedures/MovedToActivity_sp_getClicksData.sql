CREATE PROCEDURE [dbo].[MovedToActivity_sp_getClicksData] (
	@blastID varchar(10), 
	@HowMuch varchar(25), 
	@ISP varchar(100),
	@ReportType varchar(20),
	@PageNo int,  
	@PageSize int
)
as

Begin
	INSERT INTO ECN_ACTIVITY..OldProcsInUse (ProcName, DateUsed) VALUES ('sp_getClicksData', GETDATE())
	SET NOCOUNT ON

	declare @groupID int, @layoutID int, @custID int, @query varchar (8000), @innquery varchar(5000)

	select @groupID = groupID, @layoutID = layoutID, @custID = customerID  from [BLAST] where BlastID = @blastID

	set @innquery = ' select  eal.EAID, e.EMailID as EmailID, E.emailaddress, eal.ActionDate as ClickTime, eal.ActionValue AS FullLink,isnull(a.Alias,'''') as smallLink, ' +
					  '	case when (CHARINDEX(''?eid'', eal.ActionValue) > 0 or CHARINDEX(''&eid'', eal.ActionValue) > 0) then SUBSTRING(eal.ActionValue,1,(case when CHARINDEX(''?eid'', eal.ActionValue) > 0 then  CHARINDEX(''?eid'', eal.ActionValue) else CHARINDEX(''&eid'', eal.ActionValue) end)+4) else eal.ActionValue end as NewActionValue, '+
					  '	CASE WHEN eal.ActionValue like ''http://www.kmpsgroup.com/subforms/mpmndigital_new.htm?promoCode=E017%'' THEN ''http://www.kmpsgroup.com/subforms/mpmndigital_new.htm?promoCode=E017'' ELSE eal.ActionValue END as SmallActionValue ' +
							' from  EmailActivityLog eal join emails e on eal.emailID = e.emailID left outer join ' +
							' ( select link, la.Alias
								from linkAlias la join
									Content c on c.contentiD = la.contentID join
									[LAYOUT] L on (l.ContentSlot1 = c.contentID OR l.ContentSlot2 = c.contentID OR l.ContentSlot3 = c.contentID OR l.ContentSlot4 = c.contentID OR 
									l.ContentSlot5 = c.contentID OR l.ContentSlot6 = c.contentID OR l.ContentSlot7 = c.contentID OR l.ContentSlot8 = c.contentID OR 
									l.ContentSlot9 = c.contentID)
								WHERE  l.layoutID = ' + Convert(varchar,@layoutID) + ' and isnull(la.Alias, '''') <> '''') a on eal.ActionValue = a.Link	
							WHERE BlastID= ' + Convert(varchar,@blastID) + '  AND ActionTypeCode=''click'''

	if len(rtrim(ltrim(@ISP))) > 0
		set @innquery = @innquery + ' AND e.emailaddress like ''%' + @ISP + ''' '

	if @ReportType = 'topclicks'	--Top Clicks
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
		declare @RecordNoStart int,  
				@RecordNoEnd int  
	  
		--set @PageNo = @PageNo - 1
		Set @RecordNoStart = (@PageNo * @PageSize) + 1  
		Set @RecordNoEnd = (@PageNo * @PageSize) + @PageSize  

		set @innquery = ' select top ' + Convert(varchar,@RecordNoEnd) + ' eal.EAID, e.EMailID as EmailID, E.emailaddress, eal.ActionDate as ClickTime, eal.ActionValue AS FullLink,isnull(a.Alias,'''') as smallLink, ' +
						  '	case when CHARINDEX(''eid'', eal.ActionValue) = 0 then eal.ActionValue else SUBSTRING(eal.ActionValue,1,CHARINDEX(''eid'',eal.ActionValue)+2) END as NewActionValue, '+
						  '	CASE WHEN eal.ActionValue like ''http://www.kmpsgroup.com/subforms/mpmndigital_new.htm?promoCode=E017%'' THEN ''http://www.kmpsgroup.com/subforms/mpmndigital_new.htm?promoCode=E017'' ELSE eal.ActionValue END as SmallActionValue ' +
								' from  EmailActivityLog eal join emails e on eal.emailID = e.emailID left outer join ' +
								' ( select link, la.Alias
									from linkAlias la join
										Content c on c.contentiD = la.contentID join
										[LAYOUT] L on (l.ContentSlot1 = c.contentID OR l.ContentSlot2 = c.contentID OR l.ContentSlot3 = c.contentID OR l.ContentSlot4 = c.contentID OR 
										l.ContentSlot5 = c.contentID OR l.ContentSlot6 = c.contentID OR l.ContentSlot7 = c.contentID OR l.ContentSlot8 = c.contentID OR 
										l.ContentSlot9 = c.contentID)
									WHERE  l.layoutID = ' + Convert(varchar,@layoutID) + ' and isnull(la.Alias, '''') <> '''') a on eal.ActionValue = a.Link	
								WHERE BlastID= ' + Convert(varchar,@blastID) + '  AND ActionTypeCode=''click'''

		if len(rtrim(ltrim(@ISP))) > 0
		Begin
			set @innquery = @innquery + ' AND e.emailaddress like ''%' + @ISP + ''' '

			select	Count(e.emailID) 
			from	EmailActivityLog eal join emails e on eal.emailID = e.emailID 
			WHERE BlastID= @blastID AND ActionTypeCode='click' AND e.emailaddress like '%' + @ISP 

		End
		else
		Begin
			Select count(EAID) from EmailActivityLog WHERE BlastID= @blastID AND ActionTypeCode='click'
		End

		set @query =  ' select emailID as EmailID, emailaddress, ''EmailID='' + CONVERT(VARCHAR,emailID) + ''&GroupID=' + CONVERT(VARCHAR,@groupID) + ''' AS ''URL'', ClickTime, FullLink, ' +
				  ' CASE WHEN LEN(FullLink) > 6 THEN (CASE when len(smallLink) =0 then LEFT(RIGHT(FullLink,LEN(FullLink)-7),40) else smallLink end) ELSE  FullLink END AS SmallLink ' +
				  ' from (' + @innquery + ') inn order by EAID DESC '

	end

	exec(@query)

	SET NOCOUNT OFF
End

--exec sp_getClicksData 218366, 35, '', 'allclicks', 0, 1000
