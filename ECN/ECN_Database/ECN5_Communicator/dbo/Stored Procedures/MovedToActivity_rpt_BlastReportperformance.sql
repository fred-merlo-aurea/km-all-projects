CREATE proc [dbo].[MovedToActivity_rpt_BlastReportperformance] (@blastID int)
as
Begin
	INSERT INTO ECN_ACTIVITY..OldProcsInUse (ProcName, DateUsed) VALUES ('rpt_BlastReportperformance', GETDATE())
	set nocount on

	select	@blastID as BlastID,
			Sum(Total) as total,
			ActionTypeCode
	from
	(
		SELECT  count(distinct eal.emailID) AS Total,
				case when Actiontypecode = 'testsend' then 'send' else Actiontypecode end as Actiontypecode
		FROM  EmailActivityLog eal 
		WHERE BlastID = @blastID and (ActionTypeCode in ('open','send','testsend') or (ActionTypeCode='bounce' and ActionValue<>'resend'))
		GROUP BY (case when Actiontypecode = 'testsend' then 'send' else Actiontypecode end) 
		UNION       
		SELECT  ISNULL(SUM(DistinctCount),0) , 'click'         
		FROM	(        
					SELECT  COUNT(distinct ActionValue) AS DistinctCount, COUNT(EmailID) AS total         
					FROM   EmailActivityLog         
					WHERE  ActionTypeCode = 'click' AND BlastID = @blastID        
					GROUP BY  ActionValue, EmailID        
				) AS inn      
		union 
		select 0, items from dbo.fn_split('send,open,click,bounce', ',')
	) inntable
	group by ActionTypeCode
	set nocount off
end
