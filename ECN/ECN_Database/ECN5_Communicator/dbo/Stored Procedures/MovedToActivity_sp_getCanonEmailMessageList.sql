CREATE  PROCEDURE [dbo].[MovedToActivity_sp_getCanonEmailMessageList]      
	@blastID varchar(100),
	@subscribergroupID int,
	@startDate varchar(10),
	@endDate varchar(10)
AS 
	
Begin
	INSERT INTO ECN_ACTIVITY..OldProcsInUse (ProcName, DateUsed) VALUES ('sp_getCanonEmailMessageList', GETDATE())
	set nocount on 

	declare @blasts table (blastID int)

	insert into @blasts
	select * from dbo.fn_split(@blastID, ',')

	declare @GroupID int,
			@CustomerID int,
			@bID int,
			@Demo7ID int

	select top 1 @bID = blastID from @blasts

	select @GroupID = GroupID, @CustomerID = CustomerID from [BLAST] where blastID = @bID	

	select @Demo7ID = GroupDatafieldsID from groupdatafields where groupID = @subscribergroupID and shortname = 'demo7'

	create TABLE #demo71 (EmailID int, EmailAddress varchar(255))

	-- select subscriber who answered Demo7= 'B'
	INSERT into #demo71
	Select	Emails.EmailID, Emails.EmailAddress
	FROM	Emails INNER JOIN
			EmailDataValues ON Emails.EmailID = EmailDataValues.EmailID INNER JOIN
			EmailGroups ON Emails.EmailID = EmailGroups.EmailID
	WHERE   
			(EmailGroups.GroupID = @subscribergroupID) AND (EmailDataValues.GroupDatafieldsID = @Demo7ID) AND (EmailDataValues.DataValue = 'b') and
			EmailDataValues.ModifiedDate BETWEEN @startDate AND @EndDate

	-- Emails list incling Opened, Clicked, subscribed and excluding unsubscribed and Hard bounced(count > 2)
	select distinct Emails.EmailID, EmailAddress, FirstName, LastName, Company, Address, Address2, City, State, Zip, Country, Voice, Mobile, Fax
	from  
		Emails join 
		EmailActivityLog on EmailActivityLog.EmailID = Emails.EmailID 
	where 
		EmailActivityLog.BlastID in (select blastID from @blasts) and 
		ActionTypeCode = 'send' and 
		Emails.EmailID not in  (select EmailID from EmailActivityLog el where BlastID in (select blastID from @blasts) and ((ActionTypeCode = 'subscribe' and ActionValue = 'U') or (ActionTypeCode='bounce' ))) and
		(
			Emails.EmailID in  (select EmailID from EmailActivityLog el where BlastID in (select blastID from @blasts) and (ActionTypeCode = 'click' or ActionTypeCode = 'open')) or
			Emails.EmailAddress in  (select EmailAddress from #demo71)
		) 
	order by Emails.EmailID
	
	drop table #demo71

End
