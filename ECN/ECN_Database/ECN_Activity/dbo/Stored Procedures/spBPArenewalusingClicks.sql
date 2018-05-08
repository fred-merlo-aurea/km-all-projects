CREATE Proc [dbo].[spBPArenewalusingClicks]
(
	@CustomerID int,
	@GroupID int,
	@pubcode varchar(20),
	--@LastXmonths int,
	@fromdt date,
	@todt date,
	@HowManyClicks int
)
as
Begin

	set nocount on
	
	declare @blastID table (blastID int, URL varchar(500), unique clustered(blastID, URL))
	declare @gdf table(GID int, ShortName varchar(50))    
	
	create table #BPAReport(EmailID int, BlastID int, URL varchar(500), Clicktime datetime, unique clustered(EmailID,BlastID))
	
	create table #Emails(EmailID int, Totalclicks int, LastClicktime datetime, unique clustered(EmailID))
	create table #Edv(EmailID int, GID int, DataValue varchar(500), EntryID uniqueidentifier)    

	declare @Col1 varchar(8000),        
			@Col2 varchar(8000)
      
	set @Col1  = ''        
	set @Col2  = ''        


	insert into @blastID
	select	distinct b.blastID, c.Link 
	from	ecn5_communicator.dbo.[LAYOUT] l join 
			(
				select	c.ContentID, Link
				from	ecn5_communicator.dbo.Content c join 
						ecn5_communicator.dbo.LinkAlias la on c.ContentID = la.contentID join 
						ecn5_communicator.dbo.Code cd on cd.CodeID = la.LinkTypeID
				where 
						c.CustomerID = @CustomerID and 
						CodeValue = @pubcode+ ' DE' and CodeType = 'LinkType'
			)  c on 
			(
				l.ContentSlot1 = c.contentID or l.ContentSlot2 = c.contentID or l.ContentSlot3 = c.contentID or l.ContentSlot4 = c.contentID or l.ContentSlot5 = c.contentID or 
				l.ContentSlot6 = c.contentID or l.ContentSlot7 = c.contentID or l.ContentSlot8 = c.contentID or l.ContentSlot9 = c.contentID
			)  join 
			ecn5_communicator.dbo.[BLAST] b on b.LayoutID = l.LayoutID  join
			ecn5_communicator.dbo.BlastFields bf on b.BlastID = bf.BlastID  
	where 
			b.CustomerID = @CustomerID and
			GroupID  = @GroupID and
			bf.Field5 like '%DE' and
			TestBlast = 'n' 	and convert(date,b.SendTime) between @fromdt and @todt; -- > DATEADD(MM, -1 * @LastXmonths, GETDATE());  
		
	WITH email_CTE (emailID, blastID, URL, clicktime)
	AS
	(
		select distinct EmailID, c.blastID, MAX(c.URL) OVER(PARTITION BY EmailID, c.blastID) as clicktime, MAX(clicktime) OVER(PARTITION BY EmailID, c.blastID) as clicktime  --COUNT(c.blastID) OVER(PARTITION BY EmailID) as totalclicks, 
		from ecn_activity.dbo.BlastActivityClicks c with (NOLOCK) join @blastID b on c.BlastID = b.blastID and c.URL = b.URL 
	)
	Insert into #BPAReport
	select e.EmailID, e.blastID, e.URL, ClickTime
	from email_CTE e join 
			(
				select EmailID
				from email_CTE e1
				group by EmailID
				having COUNT(distinct blastID) >= @HowManyClicks
			) inn on e.emailID = inn.emailID 

	
	insert into #Emails
	select EmailID, count(distinct blastID), MAX(Clicktime)
	from #BPAReport
	group by emailID
	
	insert into @gdf
	select GroupDatafieldsID, ShortName from ecn5_communicator.dbo.GroupDatafields where GroupID = @GroupID and isnull(DatafieldSetID,0) <= 0
  	
	select   	@Col1 = @Col1 + coalesce('max([' + RTRIM(ShortName)  + ']) as ''' + RTRIM(ShortName)  + ''',',''),        
			@Col2 = @Col2 + coalesce('Case when E.GID=' + convert(varchar(10),g.GID) + ' then E.DataValue end [' + RTRIM(ShortName)  + '],', '')        
	from   @gdf g          	

	set @Col1 = substring(@Col1, 0, len(@Col1))         
	set @Col2 = substring(@Col2 , 0, len(@Col2))        

	insert into #Edv         
	select EmailDataValues.EmailID, EmailDataValues.GroupDataFieldsID, DataValue, EntryID
	from ecn5_communicator.dbo.EmailDataValues join @gdf g on g.GID = EmailDataValues.GroupDataFieldsID  -- and isnull(datavalue,'') <> ''

         
	exec ( '  select Emails.EmailID, Emails.EmailAddress,Totalclicks, LastClicktime, Emails.Title, Emails.FirstName, Emails.LastName, Emails.FullName, Emails.Company, Emails.Occupation, Emails.Address, Emails.Address2, Emails.City, Emails.State, Emails.Zip, Emails.Country, Emails.Voice, Emails.Mobile, Emails.Fax, innertable2.* ' +
		' into #PubRenewReport ' + 
		' from ecn5_communicator.dbo.Emails join ' +       
		' #Emails te on Emails.emailID = te.emailID join' + 	
			 '(select InnerTable1.EmailID as tmp_EmailID, InnerTable1.EntryID, ' + @Col1 + ' from  ' +         
				   '(select e.EmailID, E.EntryID, ' + @Col2 + ' from #Edv E) as InnerTable1 ' +         
			  ' Group by InnerTable1.EmailID, InnerTable1.EntryID ' +         
			 ') as InnerTable2 on Emails.EmailID = InnerTable2.tmp_EmailID join ' +        
		' ecn5_communicator.dbo.EmailGroups  on EmailGroups.EmailID = Emails.EmailID' +        
		' where Emails.CustomerID = ' + @CustomerID + ' and EmailGroups.GroupID = ' + @GroupID + ';' + 
		' select bp.emailID, r.emailaddress, r.firstname, r.lastname, bp.blastID, b.emailsubject, bp.url, bp.clicktime, r.subscriberID ' + 
		' from	#BPAReport bp join ' + 
			' ecn5_communicator.dbo.[BLAST] b on b.blastID = bp.BlastID join  ' + 
			' #PubRenewReport r on r.emailID  = bp.EmailID ' + 
		' order by bp.emailID ;' + 
		' select * from #PubRenewReport order by EmailID'
		) 
		

	drop table #BPAReport
	drop table #Emails
	drop table #Edv

End
