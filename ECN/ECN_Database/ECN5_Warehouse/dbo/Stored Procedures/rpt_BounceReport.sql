
CREATE PROCEDURE rpt_BounceReport

@Startdate DATETIME = NULL

AS

IF @Startdate IS NULL SET @Startdate = '2014-05-25'

--DECLARE @Enddate DATETIME
declare @bounceID int

SET NOCOUNT ON

SET @bounceID  = (SELECT top 1  bounceID from ECN_ACTIVITY.dbo.BlastActivityBounces with (NOLOCK) where bounceID > -1991277077 and BounceTime >= @Startdate order by bouncetime asc)

SELECT --top 1000
	bs.CustomerID, 
	m.MTAName, 
	SUBSTRING(bs.emailfrom, charindex('@', bs.emailfrom)+1, LEN(emailfrom)) as DomainName, 
	bs.BlastID, 
	EmailSubject, 
	emailfrom, 
	bs.sendtime, 
	bouncetime, 
	BounceMessage, 
	BounceCode 
INTO 
	#tempbounces 
FROM 
	ECN_ACTIVITY..BlastActivityBounces bab with (NOLOCK) 
	join ECN_ACTIVITY..BounceCodes b  with (NOLOCK)on bab.BounceCodeID = b.BounceCodeID 
	join ECN5_COMMUNICATOR..Blast bs  with (NOLOCK)on bs.BlastID = bab.BlastID 
	join ECN5_ACCOUNTS..Customer c  with (NOLOCK)on c.CustomerID = bs.CustomerID 
	join ECN5_COMMUNICATOR..MTACustomer mc  with (NOLOCK)on mc.CustomerID = bs.customerID 
	join ECN5_COMMUNICATOR..MTA m  with (NOLOCK)on m.MTAID = mc.MTAID
WHERE 
	BounceID >= @bounceID 
	and BounceCode in ('blocks','hardbounce','softbounce','SpamNotification')
	and CONVERT(date,BounceTime) <  convert(date,GETDATE()) --and BounceMessage <> 'smtp;554 rejected due to spam content'
ORDER BY 
	bounceID 
DESC

-------------
-- Blocks --
-------------

select distinct 'Blocks' AS ReportType, MTAName, DomainName, 'comcast' as ISP, BounceMessage from #tempbounces t where BounceCode = 'blocks' and BounceMessage like '%comcast%'
union all
select distinct 'Blocks' AS ReportType,MTAName,DomainName, 'barracuda',  BounceMessage  from #tempbounces t where BounceCode = 'blocks' and  BounceMessage like '%barracuda%'
union all
select distinct 'Blocks' AS ReportType,MTAName, DomainName, 'qwest', BounceMessage  from #tempbounces t where BounceCode = 'blocks' and  BounceMessage like '%qwest%'
union all
select distinct 'Blocks' AS ReportType,MTAName, DomainName,'aol', BounceMessage from #tempbounces t where BounceCode = 'blocks' and  BounceMessage like '%aol.com%'
union all
select distinct 'Blocks' AS ReportType,MTAName, DomainName,'proofpoint', BounceMessage from #tempbounces t where BounceCode = 'blocks' and  BounceMessage like '%proofpoint.com%'
union all
select distinct 'Blocks' AS ReportType,MTAName, DomainName,'COX', BounceMessage from #tempbounces t where BounceCode = 'blocks' and  BounceMessage like '%COX%'
union all
select distinct 'Blocks' AS ReportType,MTAName, DomainName,'borderware', BounceMessage from #tempbounces t where BounceCode = 'blocks' and  BounceMessage like '%borderware%'
union all
select distinct 'Blocks' AS ReportType,MTAName, DomainName,'reputationauthority', BounceMessage from #tempbounces t where BounceCode = 'blocks' and  BounceMessage like '%reputationauthority%'
union all
select distinct 'Blocks' AS ReportType,MTAName, DomainName, 'other1',  BounceMessage from #tempbounces t where BounceCode = 'blocks' and  
( BounceMessage like '%is on one or more dns blacklists%' or BounceMessage like '%permanent failure for one or more recipients%' or BounceMessage like '%smtp;550 5.1.1 mail refused - address%' or 
BounceMessage like '%you are not allowed to connect%' or BounceMessage like '%recipient not authorized your ip has been found on a block list%')
union all
select distinct 'Blocks' AS ReportType,MTAName, DomainName,'ALLother', BounceMessage as  ALLother from #tempbounces t where BounceCode = 'blocks' and  
not (
		BounceMessage like '%aol.com%' or 
		BounceMessage like '%barracuda%' or 
		BounceMessage like '%qwest%' or 
		BounceMessage like '%aol.com%' or  
		BounceMessage like '%proofpoint.com%' or  
		BounceMessage like '%COX%' or 
		BounceMessage like '%borderware%' or 
		BounceMessage like '%reputationauthority%' or
		BounceMessage like '%is on one or more dns blacklists%' or 
		BounceMessage like '%permanent failure for one or more recipients%' or 
		BounceMessage like '%smtp;550 5.1.1 mail refused - address%' or 
		BounceMessage like '%you are not allowed to connect%' or 
		BounceMessage like '%recipient not authorized your ip has been found on a block list%'
	)
	
UNION ALL

------------------
-- Hard Bounces --
------------------

select distinct 'Hard Bounce' AS ReportType,MTAName, DomainName, 'comcast' as ISP, BounceMessage from #tempbounces t where BounceCode = 'hardbounce' and   BounceMessage like '%comcast%'
union all
select distinct 'Hard Bounce' AS ReportType,MTAName,DomainName, 'barracuda',  BounceMessage  from #tempbounces t where BounceCode = 'hardbounce' and  BounceMessage like '%barracuda%'
union all
select distinct 'Hard Bounce' AS ReportType,MTAName, DomainName, 'qwest', BounceMessage  from #tempbounces t where BounceCode = 'hardbounce' and  BounceMessage like '%qwest%'
union all
select distinct 'Hard Bounce' AS ReportType,MTAName, DomainName,'aol', BounceMessage from #tempbounces t where BounceCode = 'hardbounce' and  BounceMessage like '%aol.com%'
union all
select distinct 'Hard Bounce' AS ReportType,MTAName, DomainName,'proofpoint', BounceMessage from #tempbounces t where BounceCode = 'hardbounce' and  BounceMessage like '%proofpoint.com%'
union all
select distinct 'Hard Bounce' AS ReportType,MTAName, DomainName,'COX', BounceMessage from #tempbounces t where BounceCode = 'hardbounce' and  BounceMessage like '%COX%'
union all
select distinct 'Hard Bounce' AS ReportType,MTAName, DomainName,'borderware', BounceMessage from #tempbounces t where BounceCode = 'hardbounce' and  BounceMessage like '%borderware%'
union all
select distinct 'Hard Bounce' AS ReportType,MTAName, DomainName,'reputationauthority', BounceMessage from #tempbounces t where BounceCode = 'hardbounce' and  BounceMessage like '%reputationauthority%'
union all
select distinct 'Hard Bounce' AS ReportType,MTAName, DomainName, 'other1',  BounceMessage from #tempbounces t where BounceCode = 'hardbounce' and  
( BounceMessage like '%is on one or more dns blacklists%' or BounceMessage like '%permanent failure for one or more recipients%' or BounceMessage like '%smtp;550 5.1.1 mail refused - address%' or 
BounceMessage like '%you are not allowed to connect%' or BounceMessage like '%recipient not authorized your ip has been found on a block list%')
union all
select distinct 'Hard Bounce' AS ReportType,MTAName, DomainName,'ALLother', BounceMessage as  ALLother from #tempbounces t where BounceCode = 'hardbounce' and  
not (
		BounceMessage like '%aol.com%' or 
		BounceMessage like '%barracuda%' or 
		BounceMessage like '%qwest%' or 
		BounceMessage like '%aol.com%' or  
		BounceMessage like '%proofpoint.com%' or  
		BounceMessage like '%COX%' or 
		BounceMessage like '%borderware%' or 
		BounceMessage like '%reputationauthority%' or
		BounceMessage like '%is on one or more dns blacklists%' or 
		BounceMessage like '%permanent failure for one or more recipients%' or 
		BounceMessage like '%smtp;550 5.1.1 mail refused - address%' or 
		BounceMessage like '%you are not allowed to connect%' or 
		BounceMessage like '%recipient not authorized your ip has been found on a block list%'
	)

UNION ALL


------------------
-- Soft Bounces --
------------------

select distinct 'Soft Bounce' AS ReportType,MTAName, DomainName, 'comcast' as ISP, BounceMessage from #tempbounces t where BounceCode = 'softbounce' and  BounceMessage like '%comcast%'
union all
select distinct 'Soft Bounce' AS ReportType,MTAName,DomainName, 'barracuda',  BounceMessage  from #tempbounces t where BounceCode = 'softbounce' and  BounceMessage like '%barracuda%'
union all
select distinct 'Soft Bounce' AS ReportType,MTAName, DomainName, 'qwest', BounceMessage  from #tempbounces t where BounceCode = 'softbounce' and  BounceMessage like '%qwest%'
union all
select distinct 'Soft Bounce' AS ReportType,MTAName, DomainName,'aol', BounceMessage from #tempbounces t where BounceCode = 'softbounce' and  BounceMessage like '%aol.com%'
union all
select distinct 'Soft Bounce' AS ReportType,MTAName, DomainName,'proofpoint', BounceMessage from #tempbounces t where BounceCode = 'softbounce' and  BounceMessage like '%proofpoint.com%'
union all
select distinct 'Soft Bounce' AS ReportType,MTAName, DomainName,'COX', BounceMessage from #tempbounces t where BounceCode = 'softbounce' and  BounceMessage like '%COX%'
union all
select distinct 'Soft Bounce' AS ReportType,MTAName, DomainName,'borderware', BounceMessage from #tempbounces t where BounceCode = 'softbounce' and  BounceMessage like '%borderware%'
union all
select distinct 'Soft Bounce' AS ReportType,MTAName, DomainName,'reputationauthority', BounceMessage from #tempbounces t where BounceCode = 'softbounce' and  BounceMessage like '%reputationauthority%'
union all
select distinct 'Soft Bounce' AS ReportType,MTAName, DomainName, 'other1',  BounceMessage from #tempbounces t where BounceCode = 'softbounce' and  
( BounceMessage like '%is on one or more dns blacklists%' or BounceMessage like '%permanent failure for one or more recipients%' or BounceMessage like '%smtp;550 5.1.1 mail refused - address%' or 
BounceMessage like '%you are not allowed to connect%' or BounceMessage like '%recipient not authorized your ip has been found on a block list%')
union all
select distinct 'Soft Bounce' AS ReportType,MTAName, DomainName,'ALLother', BounceMessage as  ALLother from #tempbounces t where BounceCode = 'softbounce' and  
not (
		BounceMessage like '%aol.com%' or 
		BounceMessage like '%barracuda%' or 
		BounceMessage like '%qwest%' or 
		BounceMessage like '%aol.com%' or  
		BounceMessage like '%proofpoint.com%' or  
		BounceMessage like '%COX%' or 
		BounceMessage like '%borderware%' or 
		BounceMessage like '%reputationauthority%' or
		BounceMessage like '%is on one or more dns blacklists%' or 
		BounceMessage like '%permanent failure for one or more recipients%' or 
		BounceMessage like '%smtp;550 5.1.1 mail refused - address%' or 
		BounceMessage like '%you are not allowed to connect%' or 
		BounceMessage like '%recipient not authorized your ip has been found on a block list%'
	)

UNION ALL

-----------------------
-- Spam Notification --
-----------------------

select distinct 'Spam Nofication' AS ReportType,MTAName, DomainName, 'comcast' as ISP, BounceMessage from #tempbounces t where BounceCode = 'SpamNotification' and  BounceMessage like '%comcast%'
union all
select distinct 'Spam Nofication' AS ReportType,MTAName,DomainName, 'barracuda',  BounceMessage  from #tempbounces t where BounceCode = 'SpamNotification' and  BounceMessage like '%barracuda%'
union all
select distinct 'Spam Nofication' AS ReportType,MTAName, DomainName, 'qwest', BounceMessage  from #tempbounces t where BounceCode = 'SpamNotification' and  BounceMessage like '%qwest%'
union all
select distinct 'Spam Nofication' AS ReportType,MTAName, DomainName,'aol', BounceMessage from #tempbounces t where BounceCode = 'SpamNotification' and  BounceMessage like '%aol.com%'
union all
select distinct 'Spam Nofication' AS ReportType,MTAName, DomainName,'proofpoint', BounceMessage from #tempbounces t where BounceCode = 'SpamNotification' and  BounceMessage like '%proofpoint.com%'
union all
select distinct 'Spam Nofication' AS ReportType,MTAName, DomainName,'COX', BounceMessage from #tempbounces t where BounceCode = 'SpamNotification' and  BounceMessage like '%COX%'
union all
select distinct'Spam Nofication' AS ReportType, MTAName, DomainName,'borderware', BounceMessage from #tempbounces t where BounceCode = 'SpamNotification' and  BounceMessage like '%borderware%'
union all
select distinct 'Spam Nofication' AS ReportType,MTAName, DomainName,'reputationauthority', BounceMessage from #tempbounces t where BounceCode = 'SpamNotification' and  BounceMessage like '%reputationauthority%'
union all
select distinct 'Spam Nofication' AS ReportType, MTAName, DomainName, 'other1',  BounceMessage from #tempbounces t where BounceCode = 'SpamNotification' and  
( BounceMessage like '%is on one or more dns blacklists%' or BounceMessage like '%permanent failure for one or more recipients%' or BounceMessage like '%smtp;550 5.1.1 mail refused - address%' or 
BounceMessage like '%you are not allowed to connect%' or BounceMessage like '%recipient not authorized your ip has been found on a block list%')
union all
select distinct 'Spam Nofication' AS ReportType, MTAName, DomainName,'ALLother', BounceMessage as  ALLother from #tempbounces t where BounceCode = 'SpamNotification' and  
not (
		BounceMessage like '%aol.com%' or 
		BounceMessage like '%barracuda%' or 
		BounceMessage like '%qwest%' or 
		BounceMessage like '%aol.com%' or  
		BounceMessage like '%proofpoint.com%' or  
		BounceMessage like '%COX%' or 
		BounceMessage like '%borderware%' or 
		BounceMessage like '%reputationauthority%' or
		BounceMessage like '%is on one or more dns blacklists%' or 
		BounceMessage like '%permanent failure for one or more recipients%' or 
		BounceMessage like '%smtp;550 5.1.1 mail refused - address%' or 
		BounceMessage like '%you are not allowed to connect%' or 
		BounceMessage like '%recipient not authorized your ip has been found on a block list%'
	)
order by ReportType,ISP, BounceMessage

drop table #tempbounces