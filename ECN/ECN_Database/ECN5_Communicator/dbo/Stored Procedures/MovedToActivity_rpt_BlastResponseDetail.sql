CREATE proc [dbo].[MovedToActivity_rpt_BlastResponseDetail] (@blastID int)
as
Begin
	INSERT INTO ECN_ACTIVITY..OldProcsInUse (ProcName, DateUsed) VALUES ('rpt_BlastResponseDetail', GETDATE())
	
	SELECT 'open' as Action, substring(Convert(varchar,ActionDate,101),1,5) as Actiondate, count(distinct emailID) as total
	FROM  [BLAST] b join EmailActivityLog eal on b.blastID = eal.blastID 
	WHERE	b.BlastID = @blastID and 
			ActionTypeCode = 'OPEN' and 
			actiondate <= dateadd(dd,10,convert(varchar,b.sendtime,101))
	GROUP BY ActionTypeCode, substring(Convert(varchar,ActionDate,101),1,5)
				 
	UNION        
	SELECT  'click', ActionDate, ISNULL(SUM(DistinctCount),0)     
	FROM	(        
				SELECT  COUNT(distinct ActionValue) AS DistinctCount, substring(Convert(varchar,ActionDate,101),1,5) as Actiondate        
				FROM   [BLAST] b join EmailActivityLog eal on b.blastID = eal.blastID          
				WHERE  ActionTypeCode = 'click' AND b.BlastID = @blastID and actiondate <= dateadd(dd,10,convert(varchar,b.sendtime,101))
				GROUP BY  ActionValue, EmailID, substring(Convert(varchar,ActionDate,101),1,5)       
			) AS inn   
	group by ActionDate

end
