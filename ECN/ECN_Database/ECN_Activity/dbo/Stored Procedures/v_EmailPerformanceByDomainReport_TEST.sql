CREATE  proc [dbo].[v_EmailPerformanceByDomainReport_TEST]  
(  
  @customerID int,  
  @startdate varchar(20),  
  @enddate varchar(20)
)
as  
  
Begin  

set nocount on
  
set @startdate = @startdate + ' 00:00:00 '   
set @enddate = @enddate + '  23:59:59'  


declare @domain table (domain varchar(100))

insert into @domain values ('hotmail.com'), ('yahoo.com'), ('yahoo.ca'), ('gmail.com'), ('comcast.net'), ('live.com'), ('aol.com'), ('verizon.net'), ('msn.com'), ('earthlink.net'), ('att.net'), ('cox.net'), ('bellsouth.net'), ('juno.com'), ('netzero.net'), ('pacbell.net') , ('sbcglobal.net') 
declare @blasts table (blastID int)

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

--declare @BlastActivity Table (BlastID int, domain varchar(100), tcounts int, ucounts int, actiontypecode varchar(50))

/*	insert into @BlastActivity

	SELECT 
		b.BlastID, 
		SUBSTRING(emailaddress, charindex('@', emailaddress) + 1, len(emailaddress)), 
		COUNT(bas.SendID), COUNT(DISTINCT bas.EmailID), 'send'
		COUNT(bao.OpenID), COUNT(DISTINCT bao.EmailID),'open'
		COUNT(bab.BounceID), COUNT(DISTINCT bab.EmailID), 	'bounce'
			COUNT(bau.UnsubscribeID), 
			COUNT(DISTINCT bau.EmailID), 'subscribe' 
		COUNT(bac.ClickID), 
		COUNT(DISTINCT bac.EmailID), 'click' 
*/

declare @BlastActivity Table (domain varchar(100), sendtotal int, opens int ,clicks int, bounce int,unsubscribe int) -- actiontypecode varchar(50))

insert into @BlastActivity 

SELECT 
	s.DOmain,
	SendTotal,
	opens,
	Clicks,
	Bounce,
	Unsubscribe
FROM (

SELECT 
	b.blastId,
	CASE WHEN d.domain IS NULL THEN 'Other' ELSE D.domain end AS Domain
	,COUNT(*) SendTotal
--	,(SELECT COUNT(DISTINCT bao.EmailID) FROM BlastActivityOpens bao WITH (NOLOCK) WHERE bao.BlastID = b.blastID and bao.EmailID = e.EmailID) Opens
--	(SELECT COUNT(DISTINCT bac.EmailID) FROM BlastActivityClicks bac WITH (NOLOCK) WHERE bac.BlastID = b.blastID and bac.EmailID = e.EmailID) Clicks,
--	(SELECT COUNT(DISTINCT bab.EmailID) FROM BlastActivityBounces bab WITH (NOLOCK) WHERE bab.BlastID = b.blastID and bab.EmailID = e.EmailID) Bounce,
--	(SELECT COUNT(DISTINCT bau.EmailID) FROM BlastActivityUnSubscribes bau WITH (NOLOCK) WHERE bau.BlastID = b.blastID and bau.EmailID = e.EmailID) Unsubscribe
	--(SELECT COUNT(DISTINCT bao.EmailID) FROM BlastActivityOpens bao WITH (NOLOCK) WHERE bao.BlastID = b.blastID ) Delivered
FROM 
		@blasts b 
		join BlastActivitySends bas WITH (NOLOCK)   on bas.BlastID = b.blastID  
		join ecn5_communicator..Emails e on e.EmailID = bas.EmailID
		left join @domain d on d.domain= SUBSTRING(emailaddress, charindex('@', emailaddress) + 1, len(emailaddress))		
GROUP BY
	b.blastID,
	CASE WHEN d.domain IS NULL THEN 'Other' ELSE D.domain end) S

LEFT JOIN 
(
SELECT 
	b.blastId,
	CASE WHEN d.domain IS NULL THEN 'Other' ELSE D.domain end AS Domain
	,COUNT(Distinct(bao.emailid)) Opens
FROM 
		@blasts b 
		join BlastActivityOpens bao WITH (NOLOCK)   on bao.BlastID = b.blastID  
		join ecn5_communicator..Emails e on e.EmailID = bao.EmailID
		left join @domain d on d.domain= SUBSTRING(emailaddress, charindex('@', emailaddress) + 1, len(emailaddress))		
GROUP BY
	b.blastId,
	CASE WHEN d.domain IS NULL THEN 'Other' ELSE D.domain end) O 

ON s.Domain = O.DOmain and 	s.blastId = o.blastId

LEFT JOIN 
(
SELECT 
	b.blastId
	,CASE WHEN d.domain IS NULL THEN 'Other' ELSE D.domain end AS Domain
	,COUNT(Distinct(bac.emailid)) Clicks
FROM 
		@blasts b 
		join BlastActivityClicks bac WITH (NOLOCK)   on bac.BlastID = b.blastID  
		join ecn5_communicator..Emails e on e.EmailID = bac.EmailID
		left join @domain d on d.domain= SUBSTRING(emailaddress, charindex('@', emailaddress) + 1, len(emailaddress))		
GROUP BY
	b.blastId,
	CASE WHEN d.domain IS NULL THEN 'Other' ELSE D.domain end) c

ON s.Domain = c.DOmain and 	s.blastId = c.blastId


LEFT JOIN 
(
SELECT	
	bab.BlastID
	,CASE WHEN d.domain IS NULL THEN 'Other' ELSE D.domain end AS Domain
	,COUNT(Distinct(bab.emailid)) Bounce
FROM 
	@blasts b 
	JOIN BlastActivityBounces bab WITH (NOLOCK) on bab.BlastID = b.blastID
	join ecn5_communicator..Emails e on e.EmailID = bab.EmailID
	left join @domain d on d.domain= SUBSTRING(emailaddress, charindex('@', emailaddress) + 1, len(emailaddress))		
GROUP BY
	bab.BlastID, 
	CASE WHEN d.domain IS NULL THEN 'Other' ELSE D.domain end) b
ON s.Domain = b.DOmain and 	s.blastId = b.blastId

LEFT JOIN 
(
SELECT 
	bau.BlastID
	,CASE WHEN d.domain IS NULL THEN 'Other' ELSE D.domain end AS Domain
	,COUNT(Distinct(bau.emailid)) Unsubscribe
FROM 
	@blasts b 
	JOIN BlastActivityUnSubscribes bau WITH (NOLOCK) on bau.BlastID = b.blastID
	JOIN UnsubscribeCodes uc WITH (NOLOCK) on bau.UnsubscribeCodeID = uc.UnsubscribeCodeID and uc.UnsubscribeCode = 'subscribe'
	join ecn5_communicator..Emails e on e.EmailID = bau.EmailID
	left join @domain d on d.domain= SUBSTRING(emailaddress, charindex('@', emailaddress) + 1, len(emailaddress))		
GROUP BY
	bau.BlastID, 
	CASE WHEN d.domain IS NULL THEN 'Other' ELSE D.domain end) un
ON s.Domain = un.DOmain and 	s.blastId = un.blastId


/*
select  case when d.domain IS not null then d.domain else 'Other' end as Domain,
		sum(case when  actiontypecode='send' then ucounts else 0 end) as SendTotal,  
		sum(case when  actiontypecode='open' then ucounts else 0 end) as Opens,
		sum(case when  actiontypecode='click' then ucounts else 0 end) as Clicks,  
		sum(case when  actiontypecode='bounce' then ucounts else 0 end) as Bounce,  
		sum(case when  actiontypecode='subscribe' then ucounts else 0 end) as Unsubscribe,
		(sum(case when  actiontypecode='send' then ucounts else 0 end) - 
		sum(case when  actiontypecode='bounce' then ucounts else 0 end)) as  Delivered 
		   
	 from  
		@BlastActivity ba 
		join @blasts b1 on ba.blastID = b1.blastID 
		left outer join @domain d on ba.domain = d.domain
	 group by 
		case when d.domain IS not null then d.domain else 'Other' end
*/

select 
	domain, 
	sum(sendtotal) AS SendTotal, 
	sum(opens) AS Opens,
	SUM(clicks) AS Clicks,
	SUM(Bounce) AS Bounce,
	ISNULL(SUM(unsubscribe),0)  AS Unsubscribe,
	(sum(sendtotal) - sum(Bounce)) as  Delivered 
from @BlastActivity
group by domain
order by 1	

END