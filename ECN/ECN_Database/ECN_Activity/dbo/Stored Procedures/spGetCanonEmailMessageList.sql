CREATE  PROCEDURE [dbo].[spGetCanonEmailMessageList]      
	@blastID varchar(100),
	@subscribergroupID int,
	@startDate varchar(10),
	@endDate varchar(10)
AS 
	
Begin
	set nocount on 

	declare @blasts table (blastID int)

	insert into @blasts
	select * from ecn5_communicator..fn_split(@blastID, ',')

	declare @GroupID int,
			@CustomerID int,
			@bID int,
			@Demo7ID int

	select top 1 @bID = blastID from @blasts

	select @GroupID = GroupID, @CustomerID = CustomerID from ecn5_communicator..[BLAST] where blastID = @bID	

	select @Demo7ID = GroupDatafieldsID from ecn5_communicator..groupdatafields where groupID = @subscribergroupID and shortname = 'demo7'

	create TABLE #demo71 (EmailID int, EmailAddress varchar(255))

	-- select subscriber who answered Demo7= 'B'
	INSERT into #demo71
	Select	Emails.EmailID, Emails.EmailAddress
	FROM	ecn5_communicator..Emails INNER JOIN
			ecn5_communicator..EmailDataValues ON Emails.EmailID = EmailDataValues.EmailID INNER JOIN
			ecn5_communicator..EmailGroups ON Emails.EmailID = EmailGroups.EmailID
	WHERE   
			(EmailGroups.GroupID = @subscribergroupID) AND (EmailDataValues.GroupDatafieldsID = @Demo7ID) AND (EmailDataValues.DataValue = 'b') and
			EmailDataValues.ModifiedDate BETWEEN @startDate AND @EndDate

	-- Emails list incling Opened, Clicked, subscribed and excluding unsubscribed and Hard bounced(count > 2)
	select distinct Emails.EmailID, EmailAddress, FirstName, LastName, Company, Address, Address2, City, State, Zip, Country, Voice, Mobile, Fax
	from  
		ecn5_communicator..Emails join 
		BlastActivitySends bas on bas.EmailID = Emails.EmailID
	where 
		bas.BlastID in (select blastID from @blasts) and 
		(Emails.EmailAddress in  (select EmailAddress from #demo71) or
			Emails.EmailID in (select emailid from BlastActivityOpens where BlastID  in (select blastID from @blasts)) or 
			Emails.EmailID in (select emailid from BlastActivityClicks where BlastID  in (select blastID from @blasts))			
		) and
		Emails.EmailID not in (select emailid from BlastActivityBounces where BlastID  in (select blastID from @blasts)) and
		Emails.EmailID not in (select emailid from BlastActivityUnSubscribes bau join UnsubscribeCodes uc on bau.UnsubscribeCodeID = uc.UnsubscribeCodeID where uc.UnsubscribeCode != 'subscribe' and BlastID  in (select blastID from @blasts))
		
	order by Emails.EmailID
	
	drop table #demo71

End
