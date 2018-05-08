CREATE  PROCEDURE [dbo].[spAudienceEngagementReport](
	@groupID int,
	@clickPercentage int,
	@Days int,
	@Download char(1),
	@DownloadType varchar(100) ,
	@DataToInclude varchar(100) = 'ProfilePlusAllUDFs' )
AS
BEGIN
			
declare @dt datetime
set nocount on
			
declare @customerID int 

select @customerID = customerID from ecn5_communicator..Groups where GroupID = @groupID

declare @b table (blastID int, UNIQUE CLUSTERED (blastID))  
declare @sd table (EmailID int, sendcount int, isNewlyCreated bit, UNIQUE CLUSTERED (EmailID))
declare @bo table (EmailID int, bouncecount int, UNIQUE CLUSTERED (EmailID))
declare @cl table (EmailID int, clickcount int, UNIQUE CLUSTERED (EmailID))
declare @op table (EmailID int, opencount int, UNIQUE CLUSTERED (EmailID))

set @dt = convert(varchar,dateadd(dd, -1*@Days, getdate()),101) + ' 00:00:00' 

CREATE TABLE #Emailactivitylog (EmailID int,  sendcount int, opencount int, Clickcount int, bouncecount int, HasOpenedBefore bit, HasClickedBefore bit, isNewlyCreated bit, UNIQUE CLUSTERED (EmailID))		

-- print '0'+ ' / ' + convert(varchar(30), getdate() ,113 )

insert into @b 
select distinct blastID  from ecn5_communicator..[BLAST] b
where b.groupID = @groupID and statuscode = 'sent' and  b.testblast='N' and  sendtime >= @dt

-- print '0.1'+ ' / ' + convert(varchar(30), getdate() ,113 )


insert into @sd 
select bs.emailID, count(bs.sendID)  as sendcount , max(case when createdon > @dt then 1 else 0 end) as isNewlyCreated 
from @b b join ECN_ACTIVITY.dbo.BlastActivitySends bs with (NOLOCK) on bs.blastID = b.blastID join ECN5_COMMUNICATOR..emailgroups eg with (NOLOCK) on bs.emailID = eg.emailID 
where eg.GroupID = @groupID
group by bs.emailID

-- print '0.2 - after sends '+ ' / ' + convert(varchar(30), getdate() ,113 )


insert into @bo 
select emailID, count(distinct bb.blastID) as bouncecount from @b b join ECN_ACTIVITY.dbo.BlastActivityBounces bb with (NOLOCK) on bb.blastID = b.blastID group by emailID
-- print '0.3 - after bounce '+ ' / ' + convert(varchar(30), getdate() ,113 )

insert into @cl
select emailID, count(distinct bc.blastID) as clickcount from @b b join ECN_ACTIVITY.dbo.BlastActivityClicks bc with (NOLOCK) on bc.blastID = b.blastID group by emailID
-- print '0.4 - after clicks '+ ' / ' + convert(varchar(30), getdate() ,113 )
insert into @op
select emailID, count(distinct bo.blastID) as opencount from @b b join ECN_ACTIVITY.dbo.BlastActivityOpens bo with (NOLOCK) on bo.blastID = b.blastID group by emailID 
-- print '0.5 - after opens '+ ' / ' + convert(varchar(30), getdate() ,113 )

insert into #Emailactivitylog
select s.emailID, ISNULL(s.sendcount, 0), ISNULL(o.opencount, 0), ISNULL(c.clickcount, 0), ISNULL(bn.bouncecount, 0), 0, 0, s.isNewlyCreated
from 
	 @sd s left outer join 
	 @op o on s.emailID = o.emailID left outer join 
	 @cl c on c.emailID = o.emailID  left outer join 
	 @bo bn on bn.emailID = c.emailID 
order by 1

--select s.emailID, ISNULL(s.sendcount, 0), ISNULL(o.opencount, 0), ISNULL(c.clickcount, 0), ISNULL(bn.bouncecount, 0), 0, 0, s.isNewlyCreated
--from 
--	 (select bs.emailID, count(bs.sendID)  as sendcount , max(case when createdon > @dt then 1 else 0 end) as isNewlyCreated from @b b join ECN_ACTIVITY.dbo.BlastActivitySends bs with (NOLOCK) on bs.blastID = b.blastID join ECN5_COMMUNICATOR..emailgroups eg with (NOLOCK) on b.groupID = eg.groupID and bs.emailID = eg.emailID group by bs.emailID) s left outer join 
--	 (select emailID, count(distinct bo.blastID) as opencount from @b b join ECN_ACTIVITY.dbo.BlastActivityOpens bo with (NOLOCK) on bo.blastID = b.blastID group by emailID) o on s.emailID = o.emailID left outer join 
--	 (select emailID, count(distinct bc.blastID) as clickcount from @b b join ECN_ACTIVITY.dbo.BlastActivityClicks bc with (NOLOCK) on bc.blastID = b.blastID group by emailID) c on c.emailID = o.emailID  left outer join 
--	 (select emailID, count(distinct bb.blastID) as bouncecount from @b b join ECN_ACTIVITY.dbo.BlastActivityBounces bb with (NOLOCK) on bb.blastID = b.blastID group by emailID) bn on bn.emailID = c.emailID 
--order by 1


-- print '1'+ ' / ' + convert(varchar(30), getdate() ,113 )

update #emailactivitylog
		set HasOpenedBefore  = 1
from  #emailactivitylog	e join 
	(
			select distinct eal.EmailID --, COUNT(OpenID) as counts 
			from 
					ecn5_communicator..[BLAST] b  with (NOLOCK) join 
					BlastActivityOpens bao with (NOLOCK) on bao.BlastID = b.BlastID  join
					#Emailactivitylog eal on bao.EmailID = eal.emailID
			where 
					b.SendTime < @dt and 
					groupID = @groupID and 
					statuscode = 'sent' and  
					testblast='N'
			--group by  eal.EmailID
					) inn on e.EmailID = inn.EmailID
					
-- print '2'+ ' / ' + convert(varchar(30), getdate() ,113 )

update #emailactivitylog
		set HasClickedBefore  = 1
from  #emailactivitylog	e join 
	(
		select distinct eal.EmailID --, COUNT(ClickID) as counts 
		from 
				ecn5_communicator..[BLAST] b  with (NOLOCK) join 
				BlastActivityClicks bac with (NOLOCK) on bac.BlastID = b.BlastID join
				#Emailactivitylog eal on bac.EmailID = eal.emailID
	where	
			b.SendTime < @dt and 
			groupID = @groupID and 
			statuscode = 'sent' and  
			testblast='N'
	--group by eal.EmailID
	) inn on e.EmailID = inn.EmailID
-- print '3'		+ ' / ' + convert(varchar(30), getdate() ,113 )
--update #emailactivitylog
--set HasOpenedBefore  = (case when (select count(EAID) from emailactivitylog where emailID = #emailactivitylog.emailID and actiondate < @dt and blastID in (select blastID from [BLAST] where groupID = @groupID and statuscode = 'sent' and  testblast='N') and actiontypecode = 'open') > 0 then 1 else 0 end),
--	HasClickedBefore = (case when (select count(EAID) from emailactivitylog where emailID = #emailactivitylog.emailID and actiondate < @dt and blastID in (select blastID from [BLAST] where groupID = @groupID and statuscode = 'sent' and  testblast='N') and actiontypecode = 'click') > 0 then 1 else 0 end),
--	isNewlyCreated = (case when (select createdon from emailgroups where emailID = #emailactivitylog.emailID and groupID = @groupID) > @dt then 1 else 0 end)
--where sendcount > 0 		

if @Download <> 'Y'
Begin
	--select sum(sendcount), sum(opencount), sum(clickcount), sum(bouncecount) from  #Emailactivitylog where sendcount <> 0
	declare @totalreceivers int
	select @totalreceivers = count(emailID) FROM #Emailactivitylog where sendcount > 0 and bouncecount = 0

	select sortorder, subscribertype, counts, case when @totalreceivers<> 0 and sortorder > 2 then convert(decimal(18,2),counts*100)/(@totalreceivers) else 0 end as percents, description from
	(
	SELECT 1 as sortorder, 'Total Subscribers' as SubscriberType, COUNT(eg.emailgroupID) AS Counts, 'Total number of subscribers to the newsletter' as description FROM	ecn5_communicator..Emailgroups eg WHERE groupID = @groupID and SubscribeTypeCode = 'S' 
	union
	SELECT 2,  'Total Message Receivers', count(emailID), 'Has received messages at least once within the past ' + convert(varchar, @days) + ' days' FROM #Emailactivitylog where sendcount > 0 and bouncecount = 0
	union
	SELECT 3,  'Best Clickers', count(emailID), 'Has clicked >=' + convert(varchar, @clickPercentage) + '% of the messages at least once within the past ' + convert(varchar, @days) + ' days'  FROM #Emailactivitylog  where sendcount > 0 and bouncecount = 0 and clickcount > 0  and (clickcount*100)/sendcount >= @clickPercentage
	union
	SELECT 4,  'Active Clickers', count(emailID), 'Has clicked <' + convert(varchar, @clickPercentage) + '% of the messages at least once within the past ' + convert(varchar, @days) + ' days' FROM #Emailactivitylog where sendcount > 0 and bouncecount = 0 and clickcount > 0 and (clickcount*100)/sendcount < @clickPercentage
	union
	select 5, 'Active Readers', count(emailID), 'Has opened but not clicked in the past ' + convert(varchar, @days) + ' days' FROM #Emailactivitylog where sendcount > 0 and bouncecount = 0 and opencount > 0 and clickcount = 0
	union
	select 6, 'Became Inactive', count(emailID), 'Has opened or clicked, but not in the past ' + convert(varchar, @days) + ' days' FROM #Emailactivitylog where sendcount > 0 and bouncecount = 0 and opencount = 0 and clickcount=0 and (HasOpenedBefore =1 or HasClickedBefore=1)
	union
	select 7, 'New Inactive', count(emailID), 'Has never opened or clicked and tenure < ' + convert(varchar, @days) + ' days' FROM #Emailactivitylog where sendcount > 0 and bouncecount = 0 and opencount = 0 and clickcount = 0 and isNewlyCreated =1
	union
	select 8, 'Old Inactive', count(emailID) , 'Has never opened or clicked and tenure > ' + convert(varchar, @days) + ' days' FROM #Emailactivitylog where sendcount > 0 and bouncecount = 0 and  opencount = 0 and clickcount = 0 and HasOpenedBefore =0 and HasClickedBefore=0 and isNewlyCreated = 0
	) inn
	-- print '4'	+ ' / ' + convert(varchar(30), getdate() ,113 )
End
Else
Begin	

	if not exists (select top 1 GroupDatafieldsID from ecn5_communicator..groupdatafields where GroupDatafields.groupID = @GroupID )
	Begin
		if @DownloadType =  'Total Subscribers'
		Begin
			SELECT e.EmailID,e.EmailAddress,e.Title,e.FirstName,e.LastName,e.FullName,e.Company,e.Occupation,e.Address,e.Address2,e.City,e.State,e.Zip,e.Country,e.Voice,e.Mobile,e.Fax,e.Website,e.Age,e.Income,e.Gender,e.Birthdate 
			FROM ecn5_communicator..Emailgroups eg join ecn5_communicator..emails e on eg.emailID = e.emailID WHERE groupID = @groupID and SubscribeTypeCode = 'S' 
		End
		Else if @DownloadType =  'Total Message Receivers'
		Begin
			SELECT e.EmailID,e.EmailAddress,e.Title,e.FirstName,e.LastName,e.FullName,e.Company,e.Occupation,e.Address,e.Address2,e.City,e.State,e.Zip,e.Country,e.Voice,e.Mobile,e.Fax,e.Website,e.Age,e.Income,e.Gender,e.Birthdate
			 FROM #Emailactivitylog eal join ecn5_communicator..emails e on eal.emailID = e.emailID where sendcount > 0 and bouncecount = 0
		End 
		Else if @DownloadType =  'Best Clickers'
		Begin
			SELECT e.EmailID,e.EmailAddress,e.Title,e.FirstName,e.LastName,e.FullName,e.Company,e.Occupation,e.Address,e.Address2,e.City,e.State,e.Zip,e.Country,e.Voice,e.Mobile,e.Fax,e.Website,e.Age,e.Income,e.Gender,e.Birthdate  
			FROM #Emailactivitylog eal join 
			ecn5_communicator..emails e on eal.emailID = e.emailID  where sendcount > 0 and bouncecount = 0 and clickcount > 0  and (clickcount*100)/sendcount >= @clickPercentage
		End 
		Else if @DownloadType =  'Active Clickers'
		Begin
			SELECT  e.EmailID,e.EmailAddress,e.Title,e.FirstName,e.LastName,e.FullName,e.Company,e.Occupation,e.Address,e.Address2,e.City,e.State,e.Zip,e.Country,e.Voice,e.Mobile,e.Fax,e.Website,e.Age,e.Income,e.Gender,e.Birthdate 
			FROM #Emailactivitylog eal join
				ecn5_communicator..emails e on eal.emailID = e.emailID  where sendcount > 0 and bouncecount = 0 and clickcount > 0 and (clickcount*100)/sendcount < @clickPercentage
		End 
		Else if @DownloadType =  'Active Readers'
		Begin
			SELECT  e.EmailID,e.EmailAddress,e.Title,e.FirstName,e.LastName,e.FullName,e.Company,e.Occupation,e.Address,e.Address2,e.City,e.State,e.Zip,e.Country,e.Voice,e.Mobile,e.Fax,e.Website,e.Age,e.Income,e.Gender,e.Birthdate  
			FROM #Emailactivitylog eal join
				ecn5_communicator..emails e on eal.emailID = e.emailID where sendcount > 0 and bouncecount = 0 and opencount > 0 and clickcount = 0
		End 
		Else if @DownloadType =  'Became Inactive'
		Begin
			SELECT  e.EmailID,e.EmailAddress,e.Title,e.FirstName,e.LastName,e.FullName,e.Company,e.Occupation,e.Address,e.Address2,e.City,e.State,e.Zip,e.Country,e.Voice,e.Mobile,e.Fax,e.Website,e.Age,e.Income,e.Gender,e.Birthdate  
			FROM #Emailactivitylog eal join
				ecn5_communicator..emails e on eal.emailID = e.emailID where sendcount > 0 and bouncecount = 0 and opencount = 0 and clickcount=0 and (HasOpenedBefore =1 or HasClickedBefore=1)
		End 
		Else if @DownloadType =  'New Inactive'
		Begin
			SELECT  e.EmailID,e.EmailAddress,e.Title,e.FirstName,e.LastName,e.FullName,e.Company,e.Occupation,e.Address,e.Address2,e.City,e.State,e.Zip,e.Country,e.Voice,e.Mobile,e.Fax,e.Website,e.Age,e.Income,e.Gender,e.Birthdate  
			FROM #Emailactivitylog eal join
				ecn5_communicator..emails e on eal.emailID = e.emailID where sendcount > 0 and bouncecount = 0 and opencount = 0 and clickcount = 0 and isNewlyCreated =1
		End 
		Else if @DownloadType =  'Old Inactive'
		Begin
			SELECT  e.EmailID,e.EmailAddress,e.Title,e.FirstName,e.LastName,e.FullName,e.Company,e.Occupation,e.Address,e.Address2,e.City,e.State,e.Zip,e.Country,e.Voice,e.Mobile,e.Fax,e.Website,e.Age,e.Income,e.Gender,e.Birthdate  
			FROM #Emailactivitylog eal join
				ecn5_communicator..emails e on eal.emailID = e.emailID where sendcount > 0 and bouncecount = 0 and  opencount = 0 and clickcount = 0 and HasOpenedBefore =0 and HasClickedBefore=0 and isNewlyCreated = 0
		End 
	end
	else
	begin
		CREATE TABLE #tempEmail(EmailID int)
		
		DECLARE @StandAloneUDFs VARCHAR(max)
		SELECT  @StandAloneUDFs = STUFF(( SELECT DISTINCT '],[' + ShortName FROM ecn5_communicator..groupdatafields WHERE GroupID = @GroupID AND DatafieldSetID is null and IsDeleted = 0 ORDER BY '],[' + ShortName FOR XML PATH('') ), 1, 2, '') + ']'
		DECLARE @TransactionalUDFs VARCHAR(max)
		SELECT  @TransactionalUDFs = STUFF(( SELECT DISTINCT '],[' + ShortName FROM ecn5_communicator..groupdatafields WHERE GroupID = @GroupID AND DatafieldSetID > 0 and IsDeleted = 0 ORDER BY '],[' + ShortName FOR XML PATH('') ), 1, 2, '') + ']'       
		
		declare @sColumns varchar(max),
				@tColumns varchar(max),
				@standAloneQuery varchar(max),
				@TransactionalQuery varchar(max)
				
		if LEN(@standaloneUDFs) > 0 and (@DataToInclude = 'ProfilePlusStandalone' or @DataToInclude = 'ProfilePlusAllUDFs')
		Begin
			set @sColumns = ', SAUDFs.* '
			set @standAloneQuery= ' left outer join			
				(
					SELECT *
					 FROM
					 (
						SELECT edv.emailID as tmp_EmailID,  gdf.ShortName, edv.DataValue
						from	ecn5_communicator..EmailDataValues edv  join  
								ecn5_communicator..Groupdatafields gdf on edv.GroupDatafieldsID = gdf.GroupDatafieldsID
						where 
								gdf.GroupID = ' + convert(varchar(15), @GroupID) + ' and datafieldsetID is null and gdf.IsDeleted = 0
					 ) u
					 PIVOT
					 (
					 MAX (DataValue)
					 FOR ShortName in (' + @StandAloneUDFs + ')) as pvt 			
				) 
				SAUDFs on Emails.emailID = SAUDFs.tmp_EmailID'
		End
		if LEN(@TransactionalUDFs) > 0 and @DataToInclude = 'ProfilePlusAllUDFs'
		Begin

			set @tColumns = ', TUDFs.* '
			set @TransactionalQuery= '  left outer join
			(
				SELECT *
				 FROM
				 (
					SELECT edv.emailID as tmp_EmailID1, edv.entryID, gdf.ShortName, edv.DataValue
					from	ecn5_communicator..EmailDataValues edv  join  
							ecn5_communicator..Groupdatafields gdf on edv.GroupDatafieldsID = gdf.GroupDatafieldsID
					where 
							gdf.GroupID = ' + convert(varchar(15), @GroupID) + ' and datafieldsetID > 0 and gdf.IsDeleted = 0
				 ) u
				 PIVOT
				 (
				 MAX (DataValue)
				 FOR ShortName in (' + @TransactionalUDFs + ')) as pvt 			
			) 
			TUDFs on Emails.emailID = TUDFs.tmp_EmailID1 '
		End
		
		if @DownloadType =  'Total Subscribers'
		Begin				
			exec (' select ecn5_communicator..Emails.EmailID, EmailAddress, Title, FirstName, LastName, FullName, Company, Occupation, Address, Address2, ' +  
			' City, State, Zip, Country, Voice, Mobile, Fax, Website, Age, Income, Gender, ' +  
			' User1, User2, User3, User4, User5, User6, Birthdate, UserEvent1, UserEvent1Date, UserEvent2, UserEvent2Date, ' +  
			' Convert(varchar,Notes) as Notes, CreatedOn, LastChanged, GroupID, FormatTypeCode, SubscribeTypeCode ' + @sColumns + @tColumns + 
			' from ecn5_communicator..Emails 
				join ' +   
			' ecn5_communicator..EmailGroups on ecn5_communicator..EmailGroups.EmailID = ecn5_communicator..Emails.EmailID ' + @standAloneQuery + @TransactionalQuery + 
			' where ecn5_communicator..Emails.CustomerID = ' + @CustomerID +  
			' and EmailGroups.GroupID = ' + @GroupID + ' and SubscribeTypeCode = ''S''') 			
			
		End
		Else if @DownloadType =  'Total Message Receivers'
		Begin											
			
			insert into #tempEmail(EmailID)
			SELECT e.EmailID FROM #Emailactivitylog eal join ecn5_communicator..emails e on eal.emailID = e.emailID where sendcount > 0 and bouncecount = 0
			
			exec (' select ecn5_communicator..Emails.EmailID, EmailAddress, Title, FirstName, LastName, FullName, Company, Occupation, Address, Address2, ' +  
			' City, State, Zip, Country, Voice, Mobile, Fax, Website, Age, Income, Gender, ' +  
			' User1, User2, User3, User4, User5, User6, Birthdate, UserEvent1, UserEvent1Date, UserEvent2, UserEvent2Date, ' +  
			' Convert(varchar,Notes) as Notes, CreatedOn, LastChanged, GroupID, FormatTypeCode, SubscribeTypeCode ' + @sColumns + @tColumns +  
			' from ecn5_communicator..Emails 
				join ' +    
			' ecn5_communicator..EmailGroups on ecn5_communicator..EmailGroups.EmailID = ecn5_communicator..Emails.EmailID join #tempEmail Em on Em.EmailID = ecn5_communicator..Emails.EmailID ' + @standAloneQuery + @TransactionalQuery +  
			' where ecn5_communicator..Emails.CustomerID = ' + @CustomerID +  
			' and ecn5_communicator..EmailGroups.GroupID = ' + @GroupID) 			
			
		End 
		Else if @DownloadType =  'Best Clickers'
		Begin				
			
			insert into #tempEmail(EmailID)
			SELECT e.EmailID FROM #Emailactivitylog eal join 
				ecn5_communicator..emails e on eal.emailID = e.emailID  where sendcount > 0 and bouncecount = 0 and clickcount > 0  and (clickcount*100)/sendcount >= @clickPercentage
				
			exec (' select ecn5_communicator..Emails.EmailID, EmailAddress, Title, FirstName, LastName, FullName, Company, Occupation, Address, Address2, ' +  
			' City, State, Zip, Country, Voice, Mobile, Fax, Website, Age, Income, Gender, ' +  
			' User1, User2, User3, User4, User5, User6, Birthdate, UserEvent1, UserEvent1Date, UserEvent2, UserEvent2Date, ' +  
			' Convert(varchar,Notes) as Notes, CreatedOn, LastChanged, GroupID, FormatTypeCode, SubscribeTypeCode ' + @sColumns + @tColumns +  
			' from ecn5_communicator..Emails 
				join ' +    
			' ecn5_communicator..EmailGroups on ecn5_communicator..EmailGroups.EmailID = ecn5_communicator..Emails.EmailID join #tempEmail Em on Em.EmailID = ecn5_communicator..Emails.EmailID ' + @standAloneQuery + @TransactionalQuery +  
			' where ecn5_communicator..Emails.CustomerID = ' + @CustomerID +  
			' and ecn5_communicator..EmailGroups.GroupID = ' + @GroupID) 			
		
		End 
		Else if @DownloadType =  'Active Clickers'
		Begin				
			
			insert into #tempEmail(EmailID)
			SELECT e.EmailID FROM #Emailactivitylog eal join
				ecn5_communicator..emails e on eal.emailID = e.emailID  where sendcount > 0 and bouncecount = 0 and clickcount > 0 and (clickcount*100)/sendcount < @clickPercentage
				
			exec (' select ecn5_communicator..Emails.EmailID, EmailAddress, Title, FirstName, LastName, FullName, Company, Occupation, Address, Address2, ' +  
			' City, State, Zip, Country, Voice, Mobile, Fax, Website, Age, Income, Gender, ' +  
			' User1, User2, User3, User4, User5, User6, Birthdate, UserEvent1, UserEvent1Date, UserEvent2, UserEvent2Date, ' +  
			' Convert(varchar,Notes) as Notes, CreatedOn, LastChanged, GroupID, FormatTypeCode, SubscribeTypeCode ' + @sColumns + @tColumns + 
			' from ecn5_communicator..Emails 
				join ' +      
			' ecn5_communicator..EmailGroups on ecn5_communicator..EmailGroups.EmailID = ecn5_communicator..Emails.EmailID join #tempEmail Em on Em.EmailID = ecn5_communicator..Emails.EmailID ' + @standAloneQuery + @TransactionalQuery +  
			' where ecn5_communicator..Emails.CustomerID = ' + @CustomerID +  
			' and ecn5_communicator..EmailGroups.GroupID = ' + @GroupID) 			
		End 
		Else if @DownloadType =  'Active Readers'
		Begin				
			
			insert into #tempEmail(EmailID)
			SELECT  e.EmailID FROM #Emailactivitylog eal join
				ecn5_communicator..emails e on eal.emailID = e.emailID where sendcount > 0 and bouncecount = 0 and opencount > 0 and clickcount = 0
				
			exec (' select ecn5_communicator..Emails.EmailID, EmailAddress, Title, FirstName, LastName, FullName, Company, Occupation, Address, Address2, ' +  
			' City, State, Zip, Country, Voice, Mobile, Fax, Website, Age, Income, Gender, ' +  
			' User1, User2, User3, User4, User5, User6, Birthdate, UserEvent1, UserEvent1Date, UserEvent2, UserEvent2Date, ' +  
			' Convert(varchar,Notes) as Notes, CreatedOn, LastChanged, GroupID, FormatTypeCode, SubscribeTypeCode ' + @sColumns + @tColumns + 
			' from ecn5_communicator..Emails 
				join ' +
			' ecn5_communicator..EmailGroups on ecn5_communicator..EmailGroups.EmailID = ecn5_communicator..Emails.EmailID join #tempEmail Em on Em.EmailID = ecn5_communicator..Emails.EmailID ' + @standAloneQuery + @TransactionalQuery +  
			' where ecn5_communicator..Emails.CustomerID = ' + @CustomerID +  
			' and ecn5_communicator..EmailGroups.GroupID = ' + @GroupID) 			
							
		End 
		Else if @DownloadType =  'Became Inactive'
		Begin				
			
			insert into #tempEmail(EmailID)
			SELECT  e.EmailID FROM #Emailactivitylog eal join
				ecn5_communicator..emails e on eal.emailID = e.emailID where sendcount > 0 and bouncecount = 0 and opencount = 0 and clickcount=0 and (HasOpenedBefore =1 or HasClickedBefore=1)
				
			exec (' select ecn5_communicator..Emails.EmailID, EmailAddress, Title, FirstName, LastName, FullName, Company, Occupation, Address, Address2, ' +  
			' City, State, Zip, Country, Voice, Mobile, Fax, Website, Age, Income, Gender, ' +  
			' User1, User2, User3, User4, User5, User6, Birthdate, UserEvent1, UserEvent1Date, UserEvent2, UserEvent2Date, ' +  
			' Convert(varchar,Notes) as Notes, CreatedOn, LastChanged, GroupID, FormatTypeCode, SubscribeTypeCode ' + @sColumns + @tColumns + 
			' from ecn5_communicator..Emails 
				join ' + 
			' ecn5_communicator..EmailGroups on ecn5_communicator..EmailGroups.EmailID = ecn5_communicator..Emails.EmailID join #tempEmail Em on Em.EmailID = ecn5_communicator..Emails.EmailID ' + @standAloneQuery + @TransactionalQuery + 
			' where ecn5_communicator..Emails.CustomerID = ' + @CustomerID +  
			' and ecn5_communicator..EmailGroups.GroupID = ' + @GroupID) 			
							
		End 
		Else if @DownloadType =  'New Inactive'
		Begin				
			
			insert into #tempEmail(EmailID)
			SELECT  e.EmailID FROM #Emailactivitylog eal join
				ecn5_communicator..emails e on eal.emailID = e.emailID where sendcount > 0 and bouncecount = 0 and opencount = 0 and clickcount = 0 and isNewlyCreated =1
				
			exec (' select ecn5_communicator..Emails.EmailID, EmailAddress, Title, FirstName, LastName, FullName, Company, Occupation, Address, Address2, ' +  
			' City, State, Zip, Country, Voice, Mobile, Fax, Website, Age, Income, Gender, ' +  
			' User1, User2, User3, User4, User5, User6, Birthdate, UserEvent1, UserEvent1Date, UserEvent2, UserEvent2Date, ' +  
			' Convert(varchar,Notes) as Notes, CreatedOn, LastChanged, GroupID, FormatTypeCode, SubscribeTypeCode ' + @sColumns + @tColumns +  
			' from ecn5_communicator..Emails 
				join ' + 
			' ecn5_communicator..EmailGroups on ecn5_communicator..EmailGroups.EmailID = ecn5_communicator..Emails.EmailID join #tempEmail Em on Em.EmailID = ecn5_communicator..Emails.EmailID ' + @standAloneQuery + @TransactionalQuery + 
			' where ecn5_communicator..Emails.CustomerID = ' + @CustomerID +  
			' and ecn5_communicator..EmailGroups.GroupID = ' + @GroupID) 			
							
		End 
		Else if @DownloadType =  'Old Inactive'
		Begin				
			
			insert into #tempEmail(EmailID)
			SELECT  e.EmailID FROM #Emailactivitylog eal join
				ecn5_communicator..emails e on eal.emailID = e.emailID where sendcount > 0 and bouncecount = 0 and  opencount = 0 and clickcount = 0 and HasOpenedBefore =0 and HasClickedBefore=0 and isNewlyCreated = 0
				
			exec (' select ecn5_communicator..Emails.EmailID, EmailAddress, Title, FirstName, LastName, FullName, Company, Occupation, Address, Address2, ' +  
			' City, State, Zip, Country, Voice, Mobile, Fax, Website, Age, Income, Gender, ' +  
			' User1, User2, User3, User4, User5, User6, Birthdate, UserEvent1, UserEvent1Date, UserEvent2, UserEvent2Date, ' +  
			' Convert(varchar,Notes) as Notes, CreatedOn, LastChanged, GroupID, FormatTypeCode, SubscribeTypeCode ' + @sColumns + @tColumns +   
			' from ecn5_communicator..Emails 
				join ' +  
			' ecn5_communicator..EmailGroups on ecn5_communicator..EmailGroups.EmailID = ecn5_communicator..Emails.EmailID join #tempEmail Em on Em.EmailID = ecn5_communicator..Emails.EmailID ' + @standAloneQuery + @TransactionalQuery +
			' where ecn5_communicator..Emails.CustomerID = ' + @CustomerID +  
			' and ecn5_communicator..EmailGroups.GroupID = ' + @GroupID) 			
		End 		
						
		drop table #tempEmail
	end	
end

drop table #Emailactivitylog

END