CREATE  proc [dbo].[rpt_BlastReportDetail] (@blastID int)
as
Begin
	INSERT INTO ECN_ACTIVITY..OldProcsInUse (ProcName, DateUsed) VALUES ('rpt_BlastReportDetail', GETDATE())
	set nocount on

	SELECT  @blastID as BlastID, 
			ISNULL(count(distinct 
					case 
						when ActionTypeCode = 'bounce' AND NOT (ActionValue='resend') then eal.EmailID
						when ActionTypeCode = 'bounce' AND (ActionValue = 'hard' OR ActionValue = 'hardbounce'  OR ActionValue='U') then eal.EmailID 
						when ActionTypeCode = 'bounce' AND (ActionValue = 'soft' OR ActionValue = 'softbounce') then eal.EmailID 
						when ActionTypeCode in ('send','testsend') then eal.EmailID 
						else eal.emailID
					end),0) AS unique_total,
			ISNULL(count(
					case 
						when ActionTypeCode = 'bounce' AND NOT (ActionValue='resend') then eal.EAID
						when ActionTypeCode = 'bounce' AND (ActionValue = 'hard' OR ActionValue = 'hardbounce'  OR ActionValue='U') then eal.EAID 
						when ActionTypeCode = 'bounce' AND (ActionValue = 'soft' OR ActionValue = 'softbounce') then eal.EAID 
						when ActionTypeCode in ('send','testsend') then eal.EmailID 
						else eal.EAID
					end),0) AS Total,
			case 
				when ActionTypeCode='bounce' and ActionValue not in ('resend', 'hard', 'hardbounce', 'U', 'soft', 'softbounce') then 'otherbounce'
				when ActionTypeCode='bounce' and ActionValue in ('hard', 'hardbounce', 'U') then 'hardbounce'
				when ActionTypeCode='bounce' and ActionValue in ('soft','softbounce') then 'softbounce'
				when ActionTypeCode in ('send','testsend') then 'send' else Actiontypecode 
			end as 'Actiontypecode' 
	FROM  EmailActivityLog eal WITH (NOLOCK) WHERE eal.BlastID = @blastID and ActionTypeCode <> 'click'
	GROUP BY case 
						when ActionTypeCode='bounce' and ActionValue not in ('resend', 'hard', 'hardbounce', 'U', 'soft', 'softbounce') then 'otherbounce'
						when ActionTypeCode='bounce' and ActionValue in ('hard', 'hardbounce', 'U') then 'hardbounce'
						when ActionTypeCode='bounce' and ActionValue in ('soft','softbounce') then 'softbounce'
						when ActionTypeCode in ('send','testsend') then 'send' else Actiontypecode end 
				 
	UNION  
	SELECT  @blastID as BlastID, 
			ISNULL(count(distinct eal.EmailID),0) AS unique_total,
			ISNULL(count(eal.EAID),0) AS Total,
			'bounce'
	FROM  EmailActivityLog eal WITH (NOLOCK) WHERE eal.BlastID = @blastID and ActionTypeCode = 'bounce'
	UNION              
	SELECT  @blastID, ISNULL(SUM(DistinctCount),0) , ISNULL(sum(total),0), 'click'         
	FROM	(        
				SELECT  COUNT(distinct ActionValue) AS DistinctCount, COUNT(EAID) AS total         
				FROM   EmailActivityLog WITH (NOLOCK)         
				WHERE  ActionTypeCode = 'click' AND BlastID = @blastID        
				GROUP BY  ActionValue, EmailID        
			) AS inn      

end