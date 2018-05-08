--2014-10-24 MK Added  WITH (NOLOCK) hints
CREATE Procedure [dbo].[e_EmailActivityLog_InsertSpamFeedbackXML_Port25] (
@xmlDocument Text ,
@ActionTypeCode varchar(100)
)  
AS 
BEGIN
DECLARE   
	@CustomerID int,  
   @grpMasterSuppID int,  
   @EmailID int,  
   @blastID int  

SET NOCOUNT ON  
  
DECLARE @E  TABLE   
 (  
  emailID int,  
  blastID int,  
  customerID int,  
  mastergroupID int, 
  ActionNotes varchar(1000),
  emailAddress varchar(255),
  SubscribeTypeCode varchar(1)	,
  BounceDate datetime
 )  

DECLARE @docHandle int  

EXEC sp_xml_preparedocument @docHandle OUTPUT, @xmlDocument  

insert into @e (emailID, blastID, ActionNotes, SubscribeTypeCode, BounceDate)  
SELECT * FROM OPENXML(@docHandle, N'/ROOT/BOUNCE') 
WITH (EmailID INT '@EmailID', BlastID INT '@BlastID', DataValue varchar(255) '@Comments', SubscribeTypeCode varchar(1) '@SubscribeTypeCode', BounceDate datetime '@BounceDate')  

EXEC sp_xml_removedocument @docHandle


		/* delete if already exists */ -- added by Sunil on 07/05/2015
	Delete e
	from @e e join ECN_ACTIVITY..BlastActivityUnsubscribes b on e.EmailID = b.EmailID and e.BlastID = b.BlastID
	
	Delete e
	from @e e join EmailActivityLog eal on e.EmailID = eal.EmailID and e.BlastID = eal.BlastID and ActionTypeCode = @ActionTypeCode

--new for finding the MSCustomerID-------------------------------------------------
--set the MSCustomerID from the customer or the base channel or from the email, in that order
update @e 
set 
	customerID = case when c.MSCustomerID is not null then c.MSCustomerID when bc.MSCustomerID is not null then bc.MSCustomerID else e.CustomerID end, 
	emailAddress = e.EmailAddress
from 
	@e e1 
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
	@e e1
	left outer join Emails e WITH (NOLOCK) on e1.EmailAddress = e.EmailAddress and e1.CustomerID = e.CustomerID
where
	e.EmailID is null



-- update the emailid for the MSCustomerID
update @e
set EmailID = e.EmailID
from 
	@e e1
	join Emails e with (nolock) on e1.emailAddress = e.EmailAddress and e1.customerID = e.CustomerID	

--update the mastergroupid for the MSCustomerID
update @e 
set 
	mastergroupID = g.groupID	
from 
	@e e1 
	join groups g WITH (NOLOCK) on e1.customerID = g.customerID  and g.MasterSupression = 1
--select * from @e	

declare @dateChanged datetime = GetDate() 
  
-- update emailgroups for existing records
	-- for Mastersuppersiongroup  = 'S'
	-- for other groups that are not already unsubscribed = 'M'
	-- for the others don't change
	
update emailgroups 
set 
	SubscribeTypeCode = case when e1.mastergroupID = eg.groupID then e1.SubscribeTypeCode when (e1.mastergroupID <> eg.GroupID and eg.SubscribeTypeCode <> 'U') then 'M' else eg.SubscribeTypeCode end, 
	LastChanged = case when (e1.mastergroupID = eg.groupID) or (e1.mastergroupID <> eg.GroupID and eg.SubscribeTypeCode <> 'U') then @dateChanged else eg.LastChanged end
from 	
	emailgroups eg with (nolock)
	join @e e1 on e1.emailID = eg.emailID
	
-- insert emailgroups if master suppression not exists
insert into EmailGroups (
	EmailID, 
	GroupID, 
	FormatTypeCode, 
	SubscribeTypeCode, 
	CreatedOn, 
	LastChanged)
select 
	distinct e1.EmailID, 
	e1.mastergroupID, 
	'html', 
	e1.SubscribeTypeCode, 
	@dateChanged, 
	@dateChanged 
from 
	@e e1 
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
	e1.BounceDate, 
	'U', 
	e1.ActionNotes, 
	'n'  
from 	
	@e e1 
	--WGH: 8/5/2014 - not necessary and probably slowing down the query
	left outer join Emailactivitylog eal with (nolock) on e1.emailID = eal.emailID and e1.BlastID = eal.BlastID and ActionTypeCode = @ActionTypeCode  and ActionValue = 'U'-- and ActionDate = @dateChanged
where 
	eal.EAID is null  
  
END