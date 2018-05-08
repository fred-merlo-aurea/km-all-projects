--exec "sp_GroupStatisticsReport";1 48429, '01/01/2011', '12/13/2011'

CREATE proc [dbo].[MovedToActivity_sp_GroupStatisticsReport]
(
	@groupID int,
	@startdate varchar(20),
	@enddate varchar(20)
)
as
Begin
	set nocount on
	INSERT INTO ECN_ACTIVITY..OldProcsInUse (ProcName, DateUsed) VALUES ('sp_GroupStatisticsReport', GETDATE())
	set @startdate = @startdate + ' 00:00:00'
	set @enddate = @enddate + ' 23:59:59';

	declare @blasts Table (BlastID int, EmailSubject varchar(255), sendtime datetime,  BlastCategory varchar(250))
	
	insert into @blasts
	SELECT  
			b.BlastID, b.EmailSubject, b.sendtime, c.CodeDisplay
	FROM	
			[BLAST] b left outer join Code c on b.CodeID = c.CodeID and c.CodeType= 'BlastCategory' 
	WHERE	
			groupID = @groupID and 
			statuscode = 'sent' and 
			b.testblast='N' and 
			sendtime between @startdate and @enddate;

	WITH Report_CTE (BlastID, actiontype, UniqueCount , TotalCount)
	AS
	(
		SELECT  
			b.BlastID, 
			ActionTypeCode,
			COUNT(DISTINCT eal.EmailID) AS UniqueCount,
			COUNT(eal.EAID) AS TotalCount 
		FROM	
				EmailActivityLog eal join @blasts b on b.blastID = eal.blastID 
		WHERE	
				(actiontypecode = 'send' or actiontypecode = 'open' or actiontypecode = 'subscribe'  or actiontypecode = 'bounce'  or ActionTypeCode='refer' )
		GROUP BY  b.BlastID, ActionTypeCode       
		UNION        
		SELECT		
				inn.BlastID, 
				'click',    
				ISNULL(SUM(DistinctCount),0) AS UniqueCount, 
				ISNULL(SUM(total),0) AS TotalCount
		FROM (        
					SELECT  b.BlastID, 
							COUNT(distinct ActionValue) AS DistinctCount, 
							COUNT(eal.EAID) AS total         
					FROM   
						EmailActivityLog eal join @blasts b on b.blastID = eal.blastID
					WHERE  ActionTypeCode = 'click' 
					GROUP BY  b.BlastID, eal.ActionValue, eal.EmailID 
			 ) AS inn  
		group by BlastID
	) 
	select	r.blastID, EmailSubject, sendtime, BlastCategory,
			isnull(max(case when actiontype='send' then uniquecount end),0) as usend,
			isnull(max(case when actiontype='send' then TotalCount end),0) as tsend,
			isnull(max(case when actiontype='bounce' then uniquecount end),0) as ubounce,
			isnull(max(case when actiontype='bounce' then TotalCount end),0) as tbounce,
			isnull(max(case when actiontype='open' then uniquecount end),0) as uopen,
			isnull(max(case when actiontype='open' then TotalCount end),0) as topen,
			isnull(max(case when actiontype='click' then uniquecount end),0) as uClick,
			isnull(max(case when actiontype='click' then TotalCount end),0) as tClick,
			isnull(max(case when actiontype='subscribe' then uniquecount end),0) as uUnsubscribe,
			isnull(max(case when actiontype='subscribe' then TotalCount end),0) as tUnsubscribe,
			isnull(max(case when actiontype='refer' then uniquecount end),0) as uRefer,
			isnull(max(case when actiontype='refer' then TotalCount end),0) as tRefer
	FROM
			Report_CTE r join @blasts b on r.blastID = b.BlastID
	group by 
			r.blastID, 
			EmailSubject, 
			sendtime,  
			BlastCategory
	order by 
			sendtime asc;
End
