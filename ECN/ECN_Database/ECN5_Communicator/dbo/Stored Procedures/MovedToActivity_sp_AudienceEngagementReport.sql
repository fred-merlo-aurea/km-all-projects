CREATE  PROCEDURE [dbo].[MovedToActivity_sp_AudienceEngagementReport](
	@groupID int,
	@clickPercentage int,
	@Days int,
	@Download char(1),
	@DownloadType varchar(100) )
AS
BEGIN

	INSERT INTO ECN_ACTIVITY..OldProcsInUse (ProcName, DateUsed) VALUES ('sp_AudienceEngagementReport', GETDATE())

--set @groupID = 21076
--set @clickPercentage = 35
--set @Days = 120
--set @Download = 'Y'
--set @DownloadType = 'Total Subscribers'
			
declare @dt datetime
set nocount on
			
declare @customerID int 

select @customerID = customerID from Groups where GroupID = @groupID

declare @b table (blastID int, groupID int)  

set @dt = convert(varchar,dateadd(dd, -1*@Days, getdate()),101) + ' 00:00:00' 

CREATE TABLE #Emailactivitylog (EmailID int,  sendcount int, opencount int, Clickcount int, bouncecount int, HasOpenedBefore bit, HasClickedBefore bit, isNewlyCreated bit)		
CREATE UNIQUE CLUSTERED INDEX EAL_ind on  #Emailactivitylog(EmailID) with ignore_dup_key

print '0'	

insert into @b 
select distinct blastID, groupID from [BLAST] b
where b.groupID = @groupID and statuscode = 'sent' and  b.testblast='N' and  sendtime >= convert(varchar,dateadd(dd, -1*(@Days+5), getdate()),101)


insert into #Emailactivitylog
select	
		eal.emailID, 
		count(distinct eal.blastID), 
		--count(case when actiontypecode = 'send' then emailID end) as sendcount, 
		count(distinct case when actiontypecode = 'open' then eal.blastID end) as opencount, 
		count(distinct case when actiontypecode = 'click' then eal.blastID end) as Clickcount, 
		count(distinct case when actiontypecode = 'bounce' then eal.blastID end) as bouncecount,
		0, 0, 
		max(case when createdon > @dt then 1 else 0 end)
from	
		emailactivitylog eal with (NOLOCK) join 
		@b b on eal.blastID = b.blastID join 
		emailgroups eg with (NOLOCK) on b.groupID = eg.groupID and eal.emailID = eg.emailID
where 
		actiondate >= @dt and
		(actiontypecode = 'open' or actiontypecode = 'click' or actiontypecode = 'send'  or actiontypecode = 'bounce')
group by eal.emailID

print '1'

update #emailactivitylog
		set HasOpenedBefore  = 1
from  #emailactivitylog	e join 
	(select EmailID, COUNT(EAID) as counts from emailactivitylog eal  with (NOLOCK) join [BLAST] b  with (NOLOCK) on eal.BlastID = b.BlastID 
	where actiondate < @dt and groupID = @groupID and statuscode = 'sent' and  testblast='N' and actiontypecode = 'open' and
	emailID in (select emailID from #Emailactivitylog where sendcount > 0)
	group by EmailID) inn on e.EmailID = inn.EmailID
print '2'			
update #emailactivitylog
		set HasClickedBefore  = 1
from  #emailactivitylog	e join 
	(select EmailID, COUNT(EAID) as counts from emailactivitylog eal  with (NOLOCK) join [BLAST] b  with (NOLOCK) on eal.BlastID = b.BlastID 
	where actiondate < @dt and groupID = @groupID and statuscode = 'sent' and  testblast='N' and actiontypecode = 'click' and
	emailID in (select emailID from #Emailactivitylog where sendcount > 0)
	group by EmailID
	) inn on e.EmailID = inn.EmailID
print '3'		
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
	SELECT 1 as sortorder, 'Total Subscribers' as SubscriberType, COUNT(eg.emailgroupID) AS Counts, 'Total number of subscribers to the newsletter' as description FROM	Emailgroups eg WHERE groupID = @groupID and SubscribeTypeCode = 'S' 
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
	print '4'	
End
Else
Begin	
	if not exists (select top 1 GroupDatafieldsID from groupdatafields where GroupDatafields.groupID = @GroupID )
	Begin
		if @DownloadType =  'Total Subscribers'
		Begin
			SELECT e.EmailID,e.EmailAddress,e.Title,e.FirstName,e.LastName,e.FullName,e.Company,e.Occupation,e.Address,e.Address2,e.City,e.State,e.Zip,e.Country,e.Voice,e.Mobile,e.Fax,e.Website,e.Age,e.Income,e.Gender,e.Birthdate FROM Emailgroups eg join emails e on eg.emailID = e.emailID WHERE groupID = @groupID and SubscribeTypeCode = 'S' 
		End
		Else if @DownloadType =  'Total Message Receivers'
		Begin
			SELECT e.EmailID,e.EmailAddress,e.Title,e.FirstName,e.LastName,e.FullName,e.Company,e.Occupation,e.Address,e.Address2,e.City,e.State,e.Zip,e.Country,e.Voice,e.Mobile,e.Fax,e.Website,e.Age,e.Income,e.Gender,e.Birthdate FROM #Emailactivitylog eal join emails e on eal.emailID = e.emailID where sendcount > 0 and bouncecount = 0
		End 
		Else if @DownloadType =  'Best Clickers'
		Begin
			SELECT e.EmailID,e.EmailAddress,e.Title,e.FirstName,e.LastName,e.FullName,e.Company,e.Occupation,e.Address,e.Address2,e.City,e.State,e.Zip,e.Country,e.Voice,e.Mobile,e.Fax,e.Website,e.Age,e.Income,e.Gender,e.Birthdate  FROM #Emailactivitylog eal join 
				emails e on eal.emailID = e.emailID  where sendcount > 0 and bouncecount = 0 and clickcount > 0  and (clickcount*100)/sendcount >= @clickPercentage
		End 
		Else if @DownloadType =  'Active Clickers'
		Begin
			SELECT  e.EmailID,e.EmailAddress,e.Title,e.FirstName,e.LastName,e.FullName,e.Company,e.Occupation,e.Address,e.Address2,e.City,e.State,e.Zip,e.Country,e.Voice,e.Mobile,e.Fax,e.Website,e.Age,e.Income,e.Gender,e.Birthdate  FROM #Emailactivitylog eal join
				emails e on eal.emailID = e.emailID  where sendcount > 0 and bouncecount = 0 and clickcount > 0 and (clickcount*100)/sendcount < @clickPercentage
		End 
		Else if @DownloadType =  'Active Readers'
		Begin
			SELECT  e.EmailID,e.EmailAddress,e.Title,e.FirstName,e.LastName,e.FullName,e.Company,e.Occupation,e.Address,e.Address2,e.City,e.State,e.Zip,e.Country,e.Voice,e.Mobile,e.Fax,e.Website,e.Age,e.Income,e.Gender,e.Birthdate  FROM #Emailactivitylog eal join
				emails e on eal.emailID = e.emailID where sendcount > 0 and bouncecount = 0 and opencount > 0 and clickcount = 0
		End 
		Else if @DownloadType =  'Became Inactive'
		Begin
			SELECT  e.EmailID,e.EmailAddress,e.Title,e.FirstName,e.LastName,e.FullName,e.Company,e.Occupation,e.Address,e.Address2,e.City,e.State,e.Zip,e.Country,e.Voice,e.Mobile,e.Fax,e.Website,e.Age,e.Income,e.Gender,e.Birthdate  FROM #Emailactivitylog eal join
				emails e on eal.emailID = e.emailID where sendcount > 0 and bouncecount = 0 and opencount = 0 and clickcount=0 and (HasOpenedBefore =1 or HasClickedBefore=1)
		End 
		Else if @DownloadType =  'New Inactive'
		Begin
			SELECT  e.EmailID,e.EmailAddress,e.Title,e.FirstName,e.LastName,e.FullName,e.Company,e.Occupation,e.Address,e.Address2,e.City,e.State,e.Zip,e.Country,e.Voice,e.Mobile,e.Fax,e.Website,e.Age,e.Income,e.Gender,e.Birthdate  FROM #Emailactivitylog eal join
				emails e on eal.emailID = e.emailID where sendcount > 0 and bouncecount = 0 and opencount = 0 and clickcount = 0 and isNewlyCreated =1
		End 
		Else if @DownloadType =  'Old Inactive'
		Begin
			SELECT  e.EmailID,e.EmailAddress,e.Title,e.FirstName,e.LastName,e.FullName,e.Company,e.Occupation,e.Address,e.Address2,e.City,e.State,e.Zip,e.Country,e.Voice,e.Mobile,e.Fax,e.Website,e.Age,e.Income,e.Gender,e.Birthdate  FROM #Emailactivitylog eal join
				emails e on eal.emailID = e.emailID where sendcount > 0 and bouncecount = 0 and  opencount = 0 and clickcount = 0 and HasOpenedBefore =0 and HasClickedBefore=0 and isNewlyCreated = 0
		End 
	end
	else
	begin
		CREATE TABLE #tempEmail(EmailID int)
		
		DECLARE @StandAloneUDFs VARCHAR(2000)
		SELECT  @StandAloneUDFs = STUFF(( SELECT DISTINCT '],[' + ShortName FROM groupdatafields WHERE GroupID = @GroupID AND DatafieldSetID is null ORDER BY '],[' + ShortName FOR XML PATH('') ), 1, 2, '') + ']'
		DECLARE @TransactionalUDFs VARCHAR(2000)
		SELECT  @TransactionalUDFs = STUFF(( SELECT DISTINCT '],[' + ShortName FROM groupdatafields WHERE GroupID = @GroupID AND DatafieldSetID > 0 ORDER BY '],[' + ShortName FOR XML PATH('') ), 1, 2, '') + ']'       
		
		declare @sColumns varchar(200),
				@tColumns varchar(200),
				@standAloneQuery varchar(4000),
				@TransactionalQuery varchar(4000)
				
		if LEN(@standaloneUDFs) > 0
		Begin
			set @sColumns = ', SAUDFs.* '
			set @standAloneQuery= ' left outer join			
				(
					SELECT *
					 FROM
					 (
						SELECT edv.emailID as tmp_EmailID,  gdf.ShortName, edv.DataValue
						from	EmailDataValues edv  join  
								Groupdatafields gdf on edv.GroupDatafieldsID = gdf.GroupDatafieldsID
						where 
								gdf.GroupID = ' + convert(varchar(15), @GroupID) + ' and datafieldsetID is null
					 ) u
					 PIVOT
					 (
					 MAX (DataValue)
					 FOR ShortName in (' + @StandAloneUDFs + ')) as pvt 			
				) 
				SAUDFs on Emails.emailID = SAUDFs.tmp_EmailID'
		End
		if LEN(@TransactionalUDFs) > 0
		Begin

			set @tColumns = ', TUDFs.* '
			set @TransactionalQuery= '  left outer join
			(
				SELECT *
				 FROM
				 (
					SELECT edv.emailID as tmp_EmailID1, edv.entryID, gdf.ShortName, edv.DataValue
					from	EmailDataValues edv  join  
							Groupdatafields gdf on edv.GroupDatafieldsID = gdf.GroupDatafieldsID
					where 
							gdf.GroupID = ' + convert(varchar(15), @GroupID) + ' and datafieldsetID > 0
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
			exec (' select Emails.EmailID, EmailAddress, Title, FirstName, LastName, FullName, Company, Occupation, Address, Address2, ' +  
			' City, State, Zip, Country, Voice, Mobile, Fax, Website, Age, Income, Gender, ' +  
			' User1, User2, User3, User4, User5, User6, Birthdate, UserEvent1, UserEvent1Date, UserEvent2, UserEvent2Date, ' +  
			' Convert(varchar,Notes) as Notes, CreatedOn, LastChanged, GroupID, FormatTypeCode, SubscribeTypeCode ' + @sColumns + @tColumns + 
			' from Emails 
				join ' +   
			' EmailGroups on EmailGroups.EmailID = Emails.EmailID join #tempEmail Em on Em.EmailID = Emails.EmailID ' + @standAloneQuery + @TransactionalQuery + 
			' where Emails.CustomerID = ' + @CustomerID +  
			' and EmailGroups.GroupID = ' + @GroupID + ' and SubscribeTypeCode = ''S''') 			
			
		End
		Else if @DownloadType =  'Total Message Receivers'
		Begin											
			
			insert into #tempEmail(EmailID)
			SELECT e.EmailID FROM #Emailactivitylog eal join emails e on eal.emailID = e.emailID where sendcount > 0 and bouncecount = 0
			
			exec (' select Emails.EmailID, EmailAddress, Title, FirstName, LastName, FullName, Company, Occupation, Address, Address2, ' +  
			' City, State, Zip, Country, Voice, Mobile, Fax, Website, Age, Income, Gender, ' +  
			' User1, User2, User3, User4, User5, User6, Birthdate, UserEvent1, UserEvent1Date, UserEvent2, UserEvent2Date, ' +  
			' Convert(varchar,Notes) as Notes, CreatedOn, LastChanged, GroupID, FormatTypeCode, SubscribeTypeCode ' + @sColumns + @tColumns +  
			' from Emails 
				join ' +    
			' EmailGroups on EmailGroups.EmailID = Emails.EmailID join #tempEmail Em on Em.EmailID = Emails.EmailID ' + @standAloneQuery + @TransactionalQuery +  
			' where Emails.CustomerID = ' + @CustomerID +  
			' and EmailGroups.GroupID = ' + @GroupID) 			
			
		End 
		Else if @DownloadType =  'Best Clickers'
		Begin				
			
			insert into #tempEmail(EmailID)
			SELECT e.EmailID FROM #Emailactivitylog eal join 
				emails e on eal.emailID = e.emailID  where sendcount > 0 and bouncecount = 0 and clickcount > 0  and (clickcount*100)/sendcount >= @clickPercentage
				
			exec (' select Emails.EmailID, EmailAddress, Title, FirstName, LastName, FullName, Company, Occupation, Address, Address2, ' +  
			' City, State, Zip, Country, Voice, Mobile, Fax, Website, Age, Income, Gender, ' +  
			' User1, User2, User3, User4, User5, User6, Birthdate, UserEvent1, UserEvent1Date, UserEvent2, UserEvent2Date, ' +  
			' Convert(varchar,Notes) as Notes, CreatedOn, LastChanged, GroupID, FormatTypeCode, SubscribeTypeCode ' + @sColumns + @tColumns +  
			' from Emails 
				join ' +    
			' EmailGroups on EmailGroups.EmailID = Emails.EmailID join #tempEmail Em on Em.EmailID = Emails.EmailID ' + @standAloneQuery + @TransactionalQuery +  
			' where Emails.CustomerID = ' + @CustomerID +  
			' and EmailGroups.GroupID = ' + @GroupID) 			
		
		End 
		Else if @DownloadType =  'Active Clickers'
		Begin				
			
			insert into #tempEmail(EmailID)
			SELECT e.EmailID FROM #Emailactivitylog eal join
				emails e on eal.emailID = e.emailID  where sendcount > 0 and bouncecount = 0 and clickcount > 0 and (clickcount*100)/sendcount < @clickPercentage
				
			exec (' select Emails.EmailID, EmailAddress, Title, FirstName, LastName, FullName, Company, Occupation, Address, Address2, ' +  
			' City, State, Zip, Country, Voice, Mobile, Fax, Website, Age, Income, Gender, ' +  
			' User1, User2, User3, User4, User5, User6, Birthdate, UserEvent1, UserEvent1Date, UserEvent2, UserEvent2Date, ' +  
			' Convert(varchar,Notes) as Notes, CreatedOn, LastChanged, GroupID, FormatTypeCode, SubscribeTypeCode ' + @sColumns + @tColumns + 
			' from Emails 
				join ' +      
			' EmailGroups on EmailGroups.EmailID = Emails.EmailID join #tempEmail Em on Em.EmailID = Emails.EmailID ' + @standAloneQuery + @TransactionalQuery +  
			' where Emails.CustomerID = ' + @CustomerID +  
			' and EmailGroups.GroupID = ' + @GroupID) 			
		End 
		Else if @DownloadType =  'Active Readers'
		Begin				
			
			insert into #tempEmail(EmailID)
			SELECT  e.EmailID FROM #Emailactivitylog eal join
				emails e on eal.emailID = e.emailID where sendcount > 0 and bouncecount = 0 and opencount > 0 and clickcount = 0
				
			exec (' select Emails.EmailID, EmailAddress, Title, FirstName, LastName, FullName, Company, Occupation, Address, Address2, ' +  
			' City, State, Zip, Country, Voice, Mobile, Fax, Website, Age, Income, Gender, ' +  
			' User1, User2, User3, User4, User5, User6, Birthdate, UserEvent1, UserEvent1Date, UserEvent2, UserEvent2Date, ' +  
			' Convert(varchar,Notes) as Notes, CreatedOn, LastChanged, GroupID, FormatTypeCode, SubscribeTypeCode ' + @sColumns + @tColumns + 
			' from Emails 
				join ' +
			' EmailGroups on EmailGroups.EmailID = Emails.EmailID join #tempEmail Em on Em.EmailID = Emails.EmailID ' + @standAloneQuery + @TransactionalQuery +  
			' where Emails.CustomerID = ' + @CustomerID +  
			' and EmailGroups.GroupID = ' + @GroupID) 			
							
		End 
		Else if @DownloadType =  'Became Inactive'
		Begin				
			
			insert into #tempEmail(EmailID)
			SELECT  e.EmailID FROM #Emailactivitylog eal join
				emails e on eal.emailID = e.emailID where sendcount > 0 and bouncecount = 0 and opencount = 0 and clickcount=0 and (HasOpenedBefore =1 or HasClickedBefore=1)
				
			exec (' select Emails.EmailID, EmailAddress, Title, FirstName, LastName, FullName, Company, Occupation, Address, Address2, ' +  
			' City, State, Zip, Country, Voice, Mobile, Fax, Website, Age, Income, Gender, ' +  
			' User1, User2, User3, User4, User5, User6, Birthdate, UserEvent1, UserEvent1Date, UserEvent2, UserEvent2Date, ' +  
			' Convert(varchar,Notes) as Notes, CreatedOn, LastChanged, GroupID, FormatTypeCode, SubscribeTypeCode ' + @sColumns + @tColumns + 
			' from Emails 
				join ' + 
			' EmailGroups on EmailGroups.EmailID = Emails.EmailID join #tempEmail Em on Em.EmailID = Emails.EmailID ' + @standAloneQuery + @TransactionalQuery + 
			' where Emails.CustomerID = ' + @CustomerID +  
			' and EmailGroups.GroupID = ' + @GroupID) 			
							
		End 
		Else if @DownloadType =  'New Inactive'
		Begin				
			
			insert into #tempEmail(EmailID)
			SELECT  e.EmailID FROM #Emailactivitylog eal join
				emails e on eal.emailID = e.emailID where sendcount > 0 and bouncecount = 0 and opencount = 0 and clickcount = 0 and isNewlyCreated =1
				
			exec (' select Emails.EmailID, EmailAddress, Title, FirstName, LastName, FullName, Company, Occupation, Address, Address2, ' +  
			' City, State, Zip, Country, Voice, Mobile, Fax, Website, Age, Income, Gender, ' +  
			' User1, User2, User3, User4, User5, User6, Birthdate, UserEvent1, UserEvent1Date, UserEvent2, UserEvent2Date, ' +  
			' Convert(varchar,Notes) as Notes, CreatedOn, LastChanged, GroupID, FormatTypeCode, SubscribeTypeCode ' + @sColumns + @tColumns +  
			' from Emails 
				join ' + 
			' EmailGroups on EmailGroups.EmailID = Emails.EmailID join #tempEmail Em on Em.EmailID = Emails.EmailID ' + @standAloneQuery + @TransactionalQuery + 
			' where Emails.CustomerID = ' + @CustomerID +  
			' and EmailGroups.GroupID = ' + @GroupID) 			
							
		End 
		Else if @DownloadType =  'Old Inactive'
		Begin				
			
			insert into #tempEmail(EmailID)
			SELECT  e.EmailID FROM #Emailactivitylog eal join
				emails e on eal.emailID = e.emailID where sendcount > 0 and bouncecount = 0 and  opencount = 0 and clickcount = 0 and HasOpenedBefore =0 and HasClickedBefore=0 and isNewlyCreated = 0
				
			exec (' select Emails.EmailID, EmailAddress, Title, FirstName, LastName, FullName, Company, Occupation, Address, Address2, ' +  
			' City, State, Zip, Country, Voice, Mobile, Fax, Website, Age, Income, Gender, ' +  
			' User1, User2, User3, User4, User5, User6, Birthdate, UserEvent1, UserEvent1Date, UserEvent2, UserEvent2Date, ' +  
			' Convert(varchar,Notes) as Notes, CreatedOn, LastChanged, GroupID, FormatTypeCode, SubscribeTypeCode ' + @sColumns + @tColumns +   
			' from Emails 
				join ' +  
			' EmailGroups on EmailGroups.EmailID = Emails.EmailID join #tempEmail Em on Em.EmailID = Emails.EmailID ' + @standAloneQuery + @TransactionalQuery +
			' where Emails.CustomerID = ' + @CustomerID +  
			' and EmailGroups.GroupID = ' + @GroupID) 			
		End 		
						
		drop table #tempEmail
	end	
end

drop table #Emailactivitylog
END
