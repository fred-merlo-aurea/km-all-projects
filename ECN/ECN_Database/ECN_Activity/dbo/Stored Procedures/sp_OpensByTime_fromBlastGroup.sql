-- =============================================
-- Author:		Rohit Pooserla
-- Create date: 10/2/2012	
-- Description:	OpensByTimefromBlastGroupID
-- =============================================
CREATE PROCEDURE [dbo].[sp_OpensByTime_fromBlastGroup]
	-- Add the parameters for the stored procedure here
	@BlastGroupID int
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from

declare @BlastSendTime datetime;
declare @BlastID varchar(2000);

	select @BlastID=ecn5_communicator.dbo.BlastGrouping.BlastIDs from ecn5_communicator.dbo.BlastGrouping where BlastGroupID=@BlastGroupID;
	select @BlastSendTime=sendtime from ECN5_COMMUNICATOR..[BLAST] where BlastID in (select top 1 * from ecn_Activity.dbo.fn_Split(@BlastID,','))
		
	declare @BlastUniqueOpens float;	
	select @BlastUniqueOpens=COUNT(distinct EmailID) from ecn_Activity..BlastActivityOpens where BlastID in (select * from ecn_Activity.dbo.fn_Split(@BlastID,','))
	
	select CAST(q1.Hour as varchar) as 'Hour', q1.Opens as 'Opens', q1.OpensPerc as 'OpensPerc' from 
	(
	select top 5 (DATEDIFF(hh, @BlastSendTime, OpenTime)+1) as Hour, COUNT(distinct emailID) as Opens,
	CAST(ROUND((COUNT(distinct emailID)*100)/@BlastUniqueOpens,2) as varchar)+'%' as OpensPerc
	from BlastActivityOpens
	where BlastID in (select * from ecn_Activity.dbo.fn_Split(@BlastID,','))
	group by DATEDIFF(hh, @BlastSendTime, OpenTime)+1
	order by 1
	) q1
	UNION ALL
	select * from (
	select top 3 CAST(((((DATEDIFF(hh, @BlastSendTime, OpenTime))/5)*5)+1) as varchar) + '-' +  CAST(((DATEDIFF(hh, @BlastSendTime, OpenTime))/5)*5+5 as varchar)  as 'Hour', count(distinct emailID)  as 'Opens',
	CAST(ROUND((COUNT(distinct emailID)*100)/@BlastUniqueOpens,2) as varchar)+'%'  as OpensPerc
	from BlastActivityOpens 
	where BlastID in (select * from ecn_Activity.dbo.fn_Split(@BlastID,','))
	and
	CAST(((((DATEDIFF(hh, @BlastSendTime, OpenTime))/5)*5)+1) as varchar) + '-' +  CAST(((DATEDIFF(hh, @BlastSendTime, OpenTime))/5)*5+5 as varchar)<>'1-5'
	group by (DATEDIFF(hh, @BlastSendTime, OpenTime))/5
	order by ((DATEDIFF(hh, @BlastSendTime, OpenTime))/5)*5
	) q2
	UNION ALL
	select * from (
	select '21-24'  as 'Hour', count(distinct emailID)  as 'Opens',
	CAST(ROUND((COUNT(distinct emailID)*100)/@BlastUniqueOpens,2) as varchar)+'%'  as OpensPerc
	from BlastActivityOpens 
	where BlastID in (select * from ecn_Activity.dbo.fn_Split(@BlastID,','))
	and
	DATEDIFF(hh, @BlastSendTime, OpenTime)>20 and DATEDIFF(hh, @BlastSendTime, OpenTime)<25
	) q3
	UNION ALL
	select * from (
	select top 2 CAST(((((DATEDIFF(hh, @BlastSendTime, OpenTime))/24)*24)+1) as varchar) + '-' +  CAST(((DATEDIFF(hh, @BlastSendTime, OpenTime))/24)*24+24 as varchar)  as 'Hour', count(distinct emailID)  as 'Opens',
	CAST(ROUND((COUNT(distinct emailID)*100)/@BlastUniqueOpens,2) as varchar)+'%'  as OpensPerc
	from BlastActivityOpens 
	where BlastID in (select * from ecn_Activity.dbo.fn_Split(@BlastID,','))
	and
	CAST(((((DATEDIFF(hh, @BlastSendTime, OpenTime))/24)*24)+1) as varchar) + '-' +  CAST(((DATEDIFF(hh, @BlastSendTime, OpenTime))/24)*24+24 as varchar)<>'1-24'
	group by (DATEDIFF(hh, @BlastSendTime, OpenTime))/24
	order by ((DATEDIFF(hh, @BlastSendTime, OpenTime))/24)*24
	) q4
END
