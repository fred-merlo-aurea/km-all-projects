-- =============================================
-- Author:		Rohit Pooserla
-- Create date: 8/8/2012
-- Description:	sp_GetDeliverability_byIP
-- =============================================
CREATE PROCEDURE [dbo].[sp_GetDeliverability_byIP]
(
@startdate date,
@enddate date,
@ip varchar(50) = ''
)
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;

	create table #tmp (CustomerID int, CustomerName varchar(100), BlastID int, EmailID int, SourceIP varchar(50))

	CREATE NONCLUSTERED INDEX T_DST_A ON #tmp(BlastID,EmailID) 

	print (' Start ' + convert(varchar(20), getdate(), 114))

	insert into #tmp
	select   q1.CustomerID, q1.CustomerName, bs.BlastID, bs.EmailID, Isnull(bs.SourceIP , 'IRONPORT')
	from ecn_Activity..BlastActivitySends bs with (NOLOCK)
	inner join
	(select c.CustomerID, C.CustomerName, b.BlastID  from ecn5_communicator..[BLAST] b join ECN5_ACCOUNTS..[CUSTOMER] c on c.CustomerID = b.CustomerID
	where  CONVERT (date,b.SendTime) >= CONVERT (date, @startdate) 
	and CONVERT (date,b.SendTime) <= CONVERT (date, @enddate) 
	and b.TestBlast='N') 
	q1 on bs.BlastID=q1.BlastID
	where (LEN(@IP) = 0 or SourceIP like '%' + @ip + '%')
	--where bs.SourceIP is not null

	print (' after Temp ' + convert(varchar(20), getdate(), 114))

	Select CustomerID, CustomerName, mta.MTAName, ip.hostname, inn2.*, inn1.sendtotal as 'TotalSent' from 
	(Select CustomerID, CustomerName, t.BlastID, SourceIP, COUNT(*) as sendtotal from #tmp t  group by CustomerID, CustomerName,t.BlastID, SourceIP) inn1
	left outer join
	(
	select      SourceiP as 'SourceIP',t.BlastID, COUNT(case when BounceCode = 'softbounce' then BounceID end) as SoftBounces,
				COUNT(case when BounceCode = 'hard' or BounceCode = 'hardbounce' then BounceID end) as HardBounces,
				COUNT(case when BounceCode = 'blocks' then BounceID end) as MailBlock,
				COUNT(case when UnsubscribeCode = 'FEEDBACK_UNSUB' or UnsubscribeCode = 'ABUSERPT_UNSUB' then UnsubscribeID end) as Complaint,
				COUNT(case when UnsubscribeCode = 'subscribe' then UnsubscribeID end) as OptOut,
				COUNT(case when UnsubscribeCode = 'MASTSUP_UNSUB' then UnsubscribeID end) as MasterSupp
	from 
		  #tmp t left outer join
		  BlastActivityBounces bb with (NOLOCK) on bb.BlastID = t.BlastID and bb.EmailID = t.EmailID 
		   left outer join
		  BounceCodes bc with (NOLOCK) on bc.BounceCodeID = bb.BounceCodeID left outer join
		  BlastActivityUnSubscribes bs with (NOLOCK) on bs.BlastID = t.BlastID and bs.emailID = t.emailID 
		  left outer join
		  UnsubscribeCodes uc with (NOLOCK) on uc.UnsubscribeCodeID = bs.UnsubscribeCodeID
	group by SourceIP,t.BlastID
	) inn2 on inn1.sourceip = inn2.SourceiP and inn1.BlastID = inn2.BlastID left outer join 
	ECN5_COMMUNICATOR..MTAIP ip on replace(ip.IPAddress, '108.160.', '10.10.' ) = inn1.SourceIP left outer join
	ECN5_COMMUNICATOR..MTA mta on mta.MTAID = ip.MTAID
	
	print (' END ' + convert(varchar(20), getdate(), 114))
	
	drop table #tmp               


END
