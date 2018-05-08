CREATE proc [dbo].[rptBlastReportperformance] (@blastID int)
as
Begin
--declare @blastid int
--set @blastid = 394353
	set nocount on

	select	@blastID as BlastID,
			Sum(Total) as total,
			ActionTypeCode
	from
	(
		SELECT  count(distinct bas.emailID) AS Total, 'send' as ActionTypeCode
		FROM  BlastActivitySends bas 
		WHERE BlastID = @blastID
		UNION
		SELECT  count(distinct bao.emailID) AS Total, 'open' as ActionTypeCode
		FROM  BlastActivityOpens bao 
		WHERE BlastID = @blastID
		UNION
		SELECT  count(distinct bab.emailID) AS Total, 'bounce' as Actiontypecode
		FROM  BlastActivityBounces bab WITH (NOLOCK) JOIN BounceCodes bc WITH (NOLOCK) ON bab.BounceCodeID = bc.BounceCodeID 
		WHERE BlastID = @blastID and bc.BounceCode != 'resend'
		UNION       
		SELECT  ISNULL(SUM(DistinctCount),0) , 'click' as ActionTypeCode         
		FROM	(        
					SELECT  COUNT(distinct bac.URL) AS DistinctCount, COUNT(EmailID) AS total         
					FROM   BlastActivityClicks bac         
					WHERE  BlastID = @blastID        
					GROUP BY  bac.URL, EmailID        
				) AS inn      
		union 
		select 0, items from ecn5_communicator..fn_Split('send,open,click,bounce', ',')
	) inntable
	group by ActionTypeCode
	set nocount off
end
