-- =============================================
-- Author:		Rohit Pooserla
-- =============================================
CREATE PROCEDURE [dbo].[sp_OpensByTime_fromCampaignItem]
	-- Add the parameters for the stored procedure here
	@CampaignItemID int
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from

declare @BlastSendTime datetime;
declare @BlastID varchar(2000);
	SELECT @BlastID = COALESCE(@BlastID + ', ', '') + CAST(BlastID as Varchar)
	from ecn5_communicator..CampaignItemBlast  
	where 
		CampaignItemID=@CampaignItemID and 
		IsDeleted=0 and 
		BlastID is not null
	
	select @BlastSendTime=sendtime 
	from ECN5_COMMUNICATOR..Blast 
	where 
		BlastID in (select top 1 * from ecn_Activity.dbo.fn_Split(@BlastID,','))
		
	declare @BlastUniqueOpens float;	
	select @BlastUniqueOpens=COUNT(distinct EmailID) 
	from ecn_Activity..BlastActivityOpens 
	where 
		BlastID in (select * from ecn_Activity.dbo.fn_Split(@BlastID,','))
	
	select CAST(q1.Hour as varchar) as 'Hour', q1.Opens as 'Opens', q1.OpensPerc as 'OpensPerc' 
	from 
		(
		select top 5 (DATEDIFF(hh, @BlastSendTime, OpenTime)+1) as Hour, 
			COUNT(distinct emailID) as Opens,
			CAST(ROUND((COUNT(distinct emailID)*100)/@BlastUniqueOpens,2) as varchar)+'%' as OpensPerc
		from BlastActivityOpens
		where 
			BlastID in (select * from ecn_Activity.dbo.fn_Split(@BlastID,',')) and DATEDIFF(hh, @BlastSendTime, OpenTime) < 6
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
				BlastID in (select * from ecn_Activity.dbo.fn_Split(@BlastID,',')) and
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
			where BlastID in (select * from ecn_Activity.dbo.fn_Split(@BlastID,',')) 
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
				BlastID in (select * from ecn_Activity.dbo.fn_Split(@BlastID,','))	and
				DATEDIFF(hh, @BlastSendTime, OpenTime) between 49 and 72
			group by (DATEDIFF(hh, @BlastSendTime, OpenTime))/24
			order by ((DATEDIFF(hh, @BlastSendTime, OpenTime))/24)*24
		) q4


END