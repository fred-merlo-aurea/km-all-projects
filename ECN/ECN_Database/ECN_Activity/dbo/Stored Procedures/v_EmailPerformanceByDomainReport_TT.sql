CREATE  proc [dbo].[v_EmailPerformanceByDomainReport_TT]  
(  
  @customerID int,  
  @startdate Datetime,  
  @enddate Datetime
)
as  
  
Begin  

set nocount on
  
SET @enddate = (@enddate + '23:59:59.998')


DECLARE 
	@MinSendId INT,
	@MinOpenId INT,
	@MinClickId INT

SELECT @MinOpenId = MIN(Minopenid) from ECN5_Warehouse.dbo.BlastOpenRangeByDate where CONVERT(Date,opendate) >= @StartDate
SELECT @MinClickId = MIN(MinClickid) from ECN5_Warehouse.dbo.BlastClickRangeByDate where CONVERT(Date,clickdate) >= @StartDate
SELECT @MinSendId =  MIN(MinSendid) from ECN5_Warehouse.dbo.BlastSEndRangeByDate where CONVERT(Date,senddate) >= @StartDate

declare @domain table (domain varchar(100))
declare @blasts table (blastID int)
declare @BlastActivity Table (BlastID int, domain varchar(100), tcounts int, ucounts int, actiontypecode varchar(50))
declare @opens table (emailID int, blastID int, UNIQUE CLUSTERED (emailID,blastID))
declare @Clicks table (emailID int, blastID int, UNIQUE CLUSTERED (emailID,blastID))
declare @Sends table (emailID int, blastID int, UNIQUE CLUSTERED (emailID,blastID))


insert into @domain values ('hotmail.com'), ('yahoo.com'), ('yahoo.ca'), ('gmail.com'), ('comcast.net'), ('live.com'), ('aol.com'), ('verizon.net'), ('msn.com'), ('earthlink.net'), ('att.net'), ('cox.net'), ('bellsouth.net'), ('juno.com'), ('netzero.net'), ('pacbell.net') , ('sbcglobal.net') 

insert into @blasts
select
	b.blastID 
from 
	ecn5_communicator..blast b with (NOLOCK) 
where
	b.customerID = @customerID 
	and b.sendtime between @startdate and @enddate 
	and statuscode = 'sent' 
	and b.testblast='N'


insert into @opens
SELECT distinct bab.emailID, bab.BlastID from ECN_Activity.dbo.BlastActivityOpens bab WITH (NOLOCK) join @blasts b  on bab.BlastID = b.blastID  
WHERE OpenID >= @MinOpenId 

insert into @Clicks
SELECT distinct bac.emailID,bac.BlastID FROM ECN_Activity.dbo.BlastActivityClicks bac WITH (NOLOCK)  join @blasts b on bac.BlastID = b.blastID  
WHERE  ClickID >= @MinClickId 

insert into @Sends
SELECT distinct bas.emailID,bas.BlastID FROM ECN_Activity.dbo.BlastActivitySends bas WITH (NOLOCK)  join @blasts b on bas.BlastID = b.blastID  
WHERE  SendID >= @MinSendId 



	insert into @BlastActivity
	
	SELECT bas.BlastID, SUBSTRING(emailaddress, charindex('@', emailaddress) + 1, len(emailaddress)), COUNT(bas.EmailId), COUNT(DISTINCT bas.EmailID), 'send'
	FROM 
		@Sends bas
		join @blasts b on bas.BlastID = b.blastID  
		join ecn5_communicator..Emails e on e.EmailID = bas.EmailID
	GROUP BY bas.BlastID  , SUBSTRING(emailaddress, charindex('@', emailaddress) + 1, len(emailaddress)) 
	
	UNION 		
	
	SELECT bao.BlastID, SUBSTRING(emailaddress, charindex('@', emailaddress) + 1, len(emailaddress)) , COUNT(bao.EmailId), COUNT(DISTINCT bao.EmailID),'open'
	FROM 
		@opens bao 
		join @blasts b on bao.BlastID = b.blastID 
		join ecn5_communicator..Emails e on e.EmailID = bao.EmailID
	GROUP BY bao.BlastID  , SUBSTRING(emailaddress, charindex('@', emailaddress) + 1, len(emailaddress)) 
	
	UNION 
	
	SELECT	bab.BlastID, SUBSTRING(emailaddress, charindex('@', emailaddress) + 1, len(emailaddress)), COUNT(bab.BounceID), COUNT(DISTINCT bab.EmailID), 'bounce'
	FROM BlastActivityBounces bab WITH (NOLOCK) join @blasts b on bab.BlastID = b.blastID join ecn5_communicator..Emails e on e.EmailID = bab.EmailID
	GROUP BY bab.BlastID, SUBSTRING(emailaddress, charindex('@', emailaddress) + 1, len(emailaddress))   
	
	UNION 
	
	SELECT bau.BlastID, SUBSTRING(emailaddress, charindex('@', emailaddress) + 1, len(emailaddress)), COUNT(bau.UnsubscribeID), COUNT(DISTINCT bau.EmailID), 'subscribe' 
	FROM BlastActivityUnSubscribes bau WITH (NOLOCK) JOIN UnsubscribeCodes uc WITH (NOLOCK) on bau.UnsubscribeCodeID = uc.UnsubscribeCodeID join @blasts b on bau.BlastID = b.blastID join ecn5_communicator..Emails e on e.EmailID = bau.EmailID
	WHERE uc.UnsubscribeCode = 'subscribe'
	GROUP BY bau.BlastID, SUBSTRING(emailaddress, charindex('@', emailaddress) + 1, len(emailaddress))
	
	UNION
	
	SELECT bac.BlastID, SUBSTRING(emailaddress, charindex('@', emailaddress) + 1, len(emailaddress)), COUNT(bac.EmailId), COUNT(DISTINCT bac.EmailID), 'click' 
	FROM 
		@Clicks bac 
		join @blasts b on bac.BlastID = b.blastID  
		join ecn5_communicator..Emails e on e.EmailID = bac.EmailID
	group by bac.BlastID, SUBSTRING(emailaddress, charindex('@', emailaddress) + 1, len(emailaddress))
  	  
    select  case when d.domain IS not null then d.domain else 'Other' end as Domain,
		sum(case when  actiontypecode='send' then ucounts else 0 end) as SendTotal,  
		sum(case when  actiontypecode='open' then ucounts else 0 end) as Opens,
		sum(case when  actiontypecode='click' then ucounts else 0 end) as Clicks,  
		sum(case when  actiontypecode='bounce' then ucounts else 0 end) as Bounce,  
		sum(case when  actiontypecode='subscribe' then ucounts else 0 end) as Unsubscribe,
		(sum(case when  actiontypecode='send' then ucounts else 0 end) - 
		sum(case when  actiontypecode='bounce' then ucounts else 0 end)) as  Delivered 
		   
	 from  
	 @BlastActivity ba join @blasts b1 on ba.blastID = b1.blastID left outer join @domain d on ba.domain = d.domain
	 group by case when d.domain IS not null then d.domain else 'Other' end
 
END