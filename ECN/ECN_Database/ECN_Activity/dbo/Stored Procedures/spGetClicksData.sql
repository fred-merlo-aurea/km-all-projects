CREATE PROCEDURE [dbo].[spGetClicksData] (
	@blastID varchar(10), 
	@HowMuch varchar(25), 
	@ISP varchar(100),
	@ReportType varchar(20),
	@PageNo int,  
	@PageSize int
)
as

Begin

	SET NOCOUNT ON

	declare @groupID int, @layoutID int, @custID int, @query varchar (8000), @innquery varchar(5000)

	select @groupID = groupID, @layoutID = layoutID, @custID = customerID  from ecn5_communicator..[BLAST] with (nolock) where BlastID = @blastID

	set @innquery = ' select  ClickID, e.EMailID as EmailID, E.emailaddress, bac.ClickTime as ClickTime, bac.URL AS FullLink,isnull(a.Alias,'''') as smallLink, ' +
					  '	case when (CHARINDEX(''?eid'', bac.URL) > 0 or CHARINDEX(''&eid'', bac.URL) > 0) then SUBSTRING(bac.URL,1,(case when CHARINDEX(''?eid'', bac.URL) > 0 then  CHARINDEX(''?eid'', bac.URL) else CHARINDEX(''&eid'', bac.URL) end)+4) else bac.URL end as NewActionValue, '+
					  '	CASE WHEN bac.URL like ''http://www.kmpsgroup.com/subforms/mpmndigital_new.htm?promoCode=E017%'' THEN ''http://www.kmpsgroup.com/subforms/mpmndigital_new.htm?promoCode=E017'' ELSE bac.URL END as SmallActionValue ' +
							' from  BlastActivityClicks bac join ecn5_communicator..emails e with (nolock) on bac.emailID = e.emailID left outer join ' +
							' ( select link, la.Alias
								from ecn5_communicator..linkAlias la with (nolock) join
									ecn5_communicator..Content c with (nolock) on c.contentiD = la.contentID join
									ecn5_communicator..[LAYOUT] L with (nolock) on (l.ContentSlot1 = c.contentID OR l.ContentSlot2 = c.contentID OR l.ContentSlot3 = c.contentID OR l.ContentSlot4 = c.contentID OR 
									l.ContentSlot5 = c.contentID OR l.ContentSlot6 = c.contentID OR l.ContentSlot7 = c.contentID OR l.ContentSlot8 = c.contentID OR 
									l.ContentSlot9 = c.contentID)
								WHERE  l.layoutID = ' + Convert(varchar,@layoutID) + ' and isnull(la.Alias, '''') <> '''') a on bac.URL = a.Link	
							WHERE BlastID= ' + Convert(varchar,@blastID) + ' '

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

		set @innquery = ' select top ' + Convert(varchar,@RecordNoEnd) + ' ClickID, e.EMailID as EmailID, E.emailaddress, bac.ClickTime as ClickTime, bac.URL AS FullLink,isnull(a.Alias,'''') as smallLink, ' +
						  '	case when CHARINDEX(''eid'', bac.URL) = 0 then bac.URL else SUBSTRING(bac.URL,1,CHARINDEX(''eid'',bac.URL)+2) END as NewActionValue, '+
						  '	CASE WHEN bac.URL like ''http://www.kmpsgroup.com/subforms/mpmndigital_new.htm?promoCode=E017%'' THEN ''http://www.kmpsgroup.com/subforms/mpmndigital_new.htm?promoCode=E017'' ELSE bac.URL END as SmallActionValue ' +
								' from  BlastActivityClicks bac with (nolock) join ecn5_communicator..emails e with (nolock) on bac.emailID = e.emailID left outer join ' +
								' ( select link, la.Alias
									from ecn5_communicator..linkAlias la with (nolock) join
										ecn5_communicator..Content c with (nolock) on c.contentiD = la.contentID join
										ecn5_communicator..[LAYOUT] L with (nolock) on (l.ContentSlot1 = c.contentID OR l.ContentSlot2 = c.contentID OR l.ContentSlot3 = c.contentID OR l.ContentSlot4 = c.contentID OR 
										l.ContentSlot5 = c.contentID OR l.ContentSlot6 = c.contentID OR l.ContentSlot7 = c.contentID OR l.ContentSlot8 = c.contentID OR 
										l.ContentSlot9 = c.contentID)
									WHERE  l.layoutID = ' + Convert(varchar,@layoutID) + ' and isnull(la.Alias, '''') <> '''') a on bac.URL = a.Link	
								WHERE BlastID= ' + Convert(varchar,@blastID) + ' '

		if len(rtrim(ltrim(@ISP))) > 0
		Begin
			set @innquery = @innquery + ' AND e.emailaddress like ''%' + @ISP + ''' '

			select	Count(e.emailID) 
			from	BlastActivityClicks bac join ecn5_communicator..emails e with (nolock) on bac.emailID = e.emailID 
			WHERE BlastID= @blastID AND e.emailaddress like '%' + @ISP 

		End
		else
		Begin
			Select count(ClickID) from BlastActivityClicks WHERE BlastID= @blastID
		End

		set @query =  ' select emailID as EmailID, emailaddress, ''EmailID='' + CONVERT(VARCHAR,emailID) + ''&GroupID=' + CONVERT(VARCHAR,@groupID) + ''' AS ''URL'', ClickTime, FullLink, ' +
				  ' CASE WHEN LEN(FullLink) > 6 THEN (CASE when len(smallLink) =0 then LEFT(RIGHT(FullLink,LEN(FullLink)-7),40) else smallLink end) ELSE  FullLink END AS SmallLink ' +
				  ' from (' + @innquery + ') inn order by ClickID DESC '

	end

	exec(@query)

	SET NOCOUNT OFF
End
