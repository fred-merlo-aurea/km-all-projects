
CREATE Procedure [dbo].[e_EmailActivityLog_InsertSpamFeedbackXML] (
@xmlDocument	VARCHAR(MAX),
@ActionTypeCode varchar(100),
@source varchar(200) = 'e_EmailActivityLog_InsertSpamFeedbackXML' 
)  

AS 

SET NOCOUNT ON

BEGIN

DECLARE   
	@CustomerID int,  
	@grpMasterSuppID int,  
	@EmailID int,  
	@blastID int  
  
CREATE TABLE #e
 (  
  emailID int,  
  blastID int,  
  customerID int,  
  mastergroupID int, 
  ActionNotes varchar(1000),
  emailAddress varchar(255),
  SubscribeTypeCode varchar(1)	,
  ReceivedDate datetime
 )  

CREATE INDEX idx_temp_Email on #e (EmailId,mastergroupID)

DECLARE @docHandle int  

set @xmlDocument = master.dbo.fn_npclean_string(@xmlDocument)

EXEC sp_xml_preparedocument @docHandle OUTPUT, @xmlDocument  

insert into #E (emailID, blastID, ActionNotes, SubscribeTypeCode, ReceivedDate)  
SELECT * FROM OPENXML(@docHandle, N'/ROOT/BOUNCE') WITH (EmailID INT '@EmailID', BlastID INT '@BlastID', DataValue varchar(355) '@Comments', SubscribeTypeCode varchar(1) '@SubscribeTypeCode', ReceivedDate datetime '@ReceivedDate')  

EXEC sp_xml_removedocument @docHandle

	/* delete if already exists */ -- added by Sunil on 07/05/2015
if(@ActionTypeCode = 'MASTSUP_UNSUB')
	BEGIN
		Delete e
		From #E e join ECN_ACTIVITY..BlastActivityUnsubscribes b on e.EmailID = b.EmailID and e.BlastID = b.BlastID and b.UnsubscribeCodeID = 2--MASTSUP_UNSUB

		Delete e
		from #E e join EmailActivityLog eal on e.EmailID = eal.EmailID and e.BlastID = eal.BlastID and ActionTypeCode = 'MASTSUP_UNSUB'
	END

--new for finding the MSCustomerID-------------------------------------------------
--set the MSCustomerID from the customer or the base channel or from the email, in that order
update #E 
set 
	customerID = case when c.MSCustomerID is not null then c.MSCustomerID when bc.MSCustomerID is not null then bc.MSCustomerID else e.CustomerID end, 
	emailAddress = e.EmailAddress
from 
	#E e1 
	join emails e with (nolock) on e1.emailID = e.emailID
	join ecn5_accounts..Customer c with(nolock) on e.customerID = c.CustomerID
	join ecn5_accounts..Basechannel bc with(nolock) on c.BaseChannelID = bc.BaseChannelID	

--set the MSEmailID from the MSCustomerID, if it doesn't exist...add it	
--if the email address doesn't exist for the MSCustomerID add it
insert into Emails(
	EmailAddress, 
	CustomerID, 
	DateAdded, 
	BounceScore,
	SoftBounceScore)
select 
	e1.EmailAddress, 
	e1.CustomerID, 
	GETDATE(), 
	0,
	0
from
	#E e1
	left outer join Emails e WITH (NOLOCK) on e1.EmailAddress = e.EmailAddress and e1.CustomerID = e.CustomerID
where
	e.EmailID is null
	AND e1.EmailAddress iS NOT NULL

-- update the emailid for the MSCustomerID
update #E
set EmailID = e.EmailID
from 
	#E e1
	join Emails e with (nolock) on e1.emailAddress = e.EmailAddress and e1.customerID = e.CustomerID	

--update the mastergroupid for the MSCustomerID
update #E 
set 
	mastergroupID = g.groupID	
from 
	#E e1 
	join groups g WITH (NOLOCK) on e1.customerID = g.customerID  and g.MasterSupression = 1
--select * from #e	


DELETE FROM #e where EmailAddress IS NULL

declare @dateChanged datetime = GetDate() 
  
------------------------
--SUNIL UPDATE
------------------------

CREATE TABLE #EG (
	EmailGroupID int, 
	EmailID int, 
	GroupID int, 
	IsMasterSuppression bit, 
	SubscribeTypeCode varchar(10)
PRIMARY KEY (EmailGroupID))

insert into #EG (
	EmailGroupID, 
	EmailID, 
	GroupID, 
	SubscribeTypeCode)
select DISTINCT
	EmailGroupID, 
	e1.EmailID, 
	GroupID, 
	e1.SubscribeTypeCode
from 
    emailgroups eg with (nolock)
    join #E e1 on e1.emailID = eg.emailID
      
     
--- UPDATE ismastersuppresion on @eg
UPDATE
	#EG
SET 
	IsMasterSuppression = ISNULL(g.MasterSupression,0)
FROM 
	#EG eg1
	join groups g on eg1.GroupID = g.GroupID


update 
	eg
SET 
      SubscribeTypeCode = case 
								when IsMasterSuppression = 1 then e1.SubscribeTypeCode 
								when (IsMasterSuppression = 0 and eg.SubscribeTypeCode <> 'U') then 'M' END,
      LastChanged =  @dateChanged, LastChangedSource = @source 
FROM 
	emailgroups eg with (nolock)
	INNER JOIN #EG e1 on eg.EmailGroupID = e1.emailgroupID
WHERE
	IsMasterSuppression = 1 
	OR (IsMasterSuppression = 0 and eg.SubscribeTypeCode <> 'U') 

-- update emailgroups for existing records
	-- for Mastersuppersiongroup  = 'S'
	-- for other groups that are not already unsubscribed = 'M'
	-- for the others don't change
	
/*
update emailgroups 
set 
	SubscribeTypeCode = case when e1.mastergroupID = eg.groupID then e1.SubscribeTypeCode when (e1.mastergroupID <> eg.GroupID and eg.SubscribeTypeCode <> 'U') then 'M' else eg.SubscribeTypeCode end, 
	LastChanged = case when (e1.mastergroupID = eg.groupID) or (e1.mastergroupID <> eg.GroupID and eg.SubscribeTypeCode <> 'U') then @dateChanged else eg.LastChanged end
from 	
	emailgroups eg with (nolock)
	join #e e1 on e1.emailID = eg.emailID

*/	
-- insert emailgroups if master suppression not exists
insert into EmailGroups (
	EmailID, 
	GroupID, 
	FormatTypeCode, 
	SubscribeTypeCode, 
	CreatedOn, 
	LastChanged,
	CreatedSource)
select 
	distinct e1.EmailID, 
	e1.mastergroupID, 
	'html', 
	e1.SubscribeTypeCode, 
	@dateChanged, 
	@dateChanged,
	@source
from 
	#E e1 
	left outer join emailgroups eg with (nolock) on e1.emailID = eg.emailID and e1.mastergroupID = eg.groupID
where 
	eg.emailgroupID is null and 
	e1.mastergroupID is not null

-- insert unsubscribe emailActivityLog if not exists
insert into Emailactivitylog (
	EmailID, 
	BlastID, 
	ActionTypeCode, 
	ActionDate, 
	ActionValue, 
	ActionNotes, 
	Processed) 
select 	
	distinct e1.EmailID, 
	e1.BlastID, 
	@ActionTypeCode, 
	isnull(ReceivedDate,@dateChanged), 
	'U', 
	e1.ActionNotes, 
	'n'  
from 	
	#E e1 
  
END
