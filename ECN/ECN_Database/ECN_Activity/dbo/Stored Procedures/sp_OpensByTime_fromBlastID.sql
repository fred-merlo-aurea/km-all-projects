-- =============================================
-- Author:		Rohit Pooserla
-- Create date: 10/2/2012	
-- Description:	sp_OpensByTime_fromBlastID
-- =============================================
CREATE PROCEDURE [dbo].[sp_OpensByTime_fromBlastID]
	-- Add the parameters for the stored procedure here
	@BlastID int
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;
	
	declare @BlastSendTime datetime;	
	select @BlastSendTime=sendtime from ECN5_COMMUNICATOR..[BLAST]  with(nolock) where BlastID =@BlastID;
	
	declare @BlastUniqueOpens float;	
	select @BlastUniqueOpens=COUNT(distinct EmailID) from ecn_Activity..BlastActivityOpens  with(nolock) where BlastID =@BlastID	
	
	
	select CAST(q1.Hour as varchar) as 'Hour', q1.Opens as 'Opens', q1.OpensPerc as 'OpensPerc' 
	from 
		(
		select top 5 (DATEDIFF(hh, @BlastSendTime, OpenTime)+1) as Hour, 
			COUNT(distinct emailID) as Opens,
			CAST(ROUND((COUNT(distinct emailID)*100)/@BlastUniqueOpens,2) as varchar)+'%' as OpensPerc
		from BlastActivityOpens
		where 
			BlastID =@BlastID and DATEDIFF(hh, @BlastSendTime, OpenTime) < 6
		group by DATEDIFF(hh, @BlastSendTime, OpenTime)+1
		order by 1
		) q1
	UNION ALL
		select * from (
			select top 4 CAST(((((DATEDIFF(hh, @BlastSendTime, OpenTime))/5)*5)+1) as varchar) + '-' + case when CAST(((DATEDIFF(hh, @BlastSendTime, OpenTime))/5)*5+5 as varchar) = '25' then '24'
																											else CAST(((DATEDIFF(hh, @BlastSendTime, OpenTime))/5)*5+5 as varchar) end  as 'Hour', 
				count(distinct emailID)  as 'Opens',
				CAST(ROUND((COUNT(distinct emailID)*100)/@BlastUniqueOpens,2) as varchar)+'%'  as OpensPerc
			from BlastActivityOpens 
			where 
				BlastID =@BlastID and
				DATEDIFF(HH, @BlastSendTime, OpenTime) between 6 and 24
			group by (DATEDIFF(hh, @BlastSendTime, OpenTime))/5
			order by ((DATEDIFF(hh, @BlastSendTime, OpenTime))/5)*5
		) q2	
		UNION ALL
	SELECT * 
	FROM
		(
			select top 1 '25-48'  as 'Hour', count(distinct emailID)  as 'Opens',
			CAST(ROUND((COUNT(distinct emailID)*100)/@BlastUniqueOpens,2) as varchar)+'%'  as OpensPerc
			from BlastActivityOpens  with(nolock)
			where BlastID =@BlastID 
			and
			 DATEDIFF(hh, @BlastSendTime, OpenTime) between 25 and 48
			group by (DATEDIFF(hh, @BlastSendTime, OpenTime))/24
			order by ((DATEDIFF(hh, @BlastSendTime, OpenTime))/24)*24
		) q5
	UNION ALL
		select * from (
			select top 1 '49-72'  as 'Hour', 
				count(distinct emailID)  as 'Opens',
				CAST(ROUND((COUNT(distinct emailID)*100)/@BlastUniqueOpens,2) as varchar)+'%'  as OpensPerc
			from BlastActivityOpens 
			where 
				BlastID =@BlastID	and
				DATEDIFF(hh, @BlastSendTime, OpenTime) between 49 and 72
			group by (DATEDIFF(hh, @BlastSendTime, OpenTime))/24
			order by ((DATEDIFF(hh, @BlastSendTime, OpenTime))/24)*24
		) q4



END
