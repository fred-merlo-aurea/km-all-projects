--declare @blastid int
--set @blastid = 394353
CREATE proc [dbo].[rptBlastResponseDetail] (@blastID int)
as
Begin
	SELECT 'open' as Action, substring(Convert(varchar,OpenTime,101),1,5) as Actiondate, count(distinct emailID) as total
	FROM  ecn5_communicator..[BLAST] b join BlastActivityOpens bao on b.blastID = bao.blastID 
	WHERE	b.BlastID = @blastID and 
			OpenTime <= dateadd(dd,10,convert(varchar,b.sendtime,101))
	GROUP BY substring(Convert(varchar,OpenTime,101),1,5)
				 
	UNION        
	SELECT  'click', ActionDate, ISNULL(SUM(DistinctCount),0)     
	FROM	(        
				SELECT  COUNT(distinct URL) AS DistinctCount, substring(Convert(varchar,ClickTime,101),1,5) as Actiondate        
				FROM   ecn5_communicator..[BLAST] b join BlastActivityClicks bac on b.blastID = bac.blastID          
				WHERE  b.BlastID = @blastID and ClickTime <= dateadd(dd,10,convert(varchar,b.sendtime,101))
				GROUP BY  URL, EmailID, substring(Convert(varchar,ClickTime,101),1,5)       
			) AS inn   
	group by ActionDate

end
