CREATE  proc [dbo].[rptBlastReportDetail] (
@blastID int)
--set @blastID = 447399
as
Begin

	set nocount on
	
	SELECT @blastID AS BlastID, ISNULL(count(distinct bau.EmailID),0) AS unique_total, ISNULL(count(bau.UnsubscribeID),0) AS Total, 'FEEDBACK_UNSUB' AS ActionTypeCode
	FROM ecn_Activity..BlastActivityUnSubscribes bau
		JOIN ecn_Activity..UnsubscribeCodes uc ON bau.UnsubscribeCodeID = uc.UnsubscribeCodeID
	WHERE BlastID = @blastID AND uc.UnsubscribeCode = 'FEEDBACK_UNSUB'
	UNION
	SELECT @blastID AS BlastID, ISNULL(count(distinct bau.EmailID),0) AS unique_total, ISNULL(count(bau.UnsubscribeID),0) AS Total, 'MASTSUP_UNSUB' AS ActionTypeCode
	FROM ecn_Activity..BlastActivityUnSubscribes bau
		JOIN ecn_Activity..UnsubscribeCodes uc ON bau.UnsubscribeCodeID = uc.UnsubscribeCodeID
	WHERE BlastID = @blastID AND uc.UnsubscribeCode = 'MASTSUP_UNSUB'
	UNION
	SELECT @blastID AS BlastID, ISNULL(count(distinct bau.EmailID),0) AS unique_total, ISNULL(count(bau.UnsubscribeID),0) AS Total, 'ABUSERPT_UNSUB' AS ActionTypeCode
	FROM ecn_Activity..BlastActivityUnSubscribes bau
		JOIN ecn_Activity..UnsubscribeCodes uc ON bau.UnsubscribeCodeID = uc.UnsubscribeCodeID
	WHERE BlastID = @blastID AND uc.UnsubscribeCode = 'ABUSERPT_UNSUB'
	UNION
	SELECT @blastID AS BlastID, ISNULL(count(distinct bau.EmailID),0) AS unique_total, ISNULL(count(bau.UnsubscribeID),0) AS Total, 'subscribe' AS ActionTypeCode
	FROM ecn_Activity..BlastActivityUnSubscribes bau
		JOIN ecn_Activity..UnsubscribeCodes uc ON bau.UnsubscribeCodeID = uc.UnsubscribeCodeID
	WHERE BlastID = @blastID AND uc.UnsubscribeCode = 'subscribe'
	UNION
	SELECT @blastID AS BlastID, ISNULL(count(distinct EmailID),0) AS unique_total, ISNULL(count(ReferID),0) AS Total, 'refer' AS ActionTypeCode
	FROM ecn_Activity..BlastActivityRefer
	WHERE BlastID = @blastID
	UNION
	SELECT @blastID AS BlastID, ISNULL(count(distinct bab.EmailID),0) AS unique_total, ISNULL(count(bab.BounceID),0) AS Total, 'hardbounce' AS ActionTypeCode
	FROM ecn_Activity..BlastActivityBounces bab
		JOIN ecn_Activity..BounceCodes bc ON bab.BounceCodeID = bc.BounceCodeID
	WHERE BlastID = @blastID AND  bc.BounceCode = 'hardbounce'
	UNION
	SELECT @blastID AS BlastID, ISNULL(count(distinct bab.EmailID),0) AS unique_total, ISNULL(count(bab.BounceID),0) AS Total, 'softbounce' AS ActionTypeCode
	FROM ecn_Activity..BlastActivityBounces bab
		JOIN ecn_Activity..BounceCodes bc ON bab.BounceCodeID = bc.BounceCodeID
	WHERE BlastID = @blastID AND bc.BounceCode = 'softbounce'
	UNION
	SELECT @blastID AS BlastID, ISNULL(count(distinct bab.EmailID),0) AS unique_total, ISNULL(count(bab.BounceID),0) AS Total, 'otherbounce' AS ActionTypeCode
	FROM ecn_Activity..BlastActivityBounces bab
		JOIN ecn_Activity..BounceCodes bc ON bab.BounceCodeID = bc.BounceCodeID
	WHERE BlastID = @blastID AND bc.BounceCode != 'softbounce' and bc.BounceCode != 'hardbounce'
	UNION
	SELECT @blastID AS BlastID, ISNULL(count(distinct bab.EmailID),0) AS unique_total, ISNULL(count(bab.BounceID),0) AS Total, 'bounce' AS ActionTypeCode
	FROM ecn_Activity..BlastActivityBounces bab
		JOIN ecn_Activity..BounceCodes bc ON bab.BounceCodeID = bc.BounceCodeID
	WHERE BlastID = @blastID
	UNION
	SELECT @blastID AS BlastID, ISNULL(SUM(DistinctCount),0) AS unique_total, ISNULL(sum(total),0) AS Total, 'click' AS ActionTypeCode
	FROM	(        
				SELECT  COUNT(distinct URL) AS DistinctCount, COUNT(ClickID) AS total         
				FROM   ecn_Activity..BlastActivityClicks        
				WHERE  BlastID = @blastID        
				GROUP BY  URL, EmailID        
			) AS inn
	UNION
	SELECT @blastID AS BlastID, ISNULL(COUNT(distinct EmailID),0) AS unique_total, 0 AS Total, 'clickthrough' AS ActionTypeCode
	FROM ECN_Activity..BlastActivityClicks bac with(nolock)
	WHERE bac.BlastID = @blastID
	UNION
	SELECT @blastID AS BlastID, ISNULL(count(distinct EmailID),0) AS unique_total, ISNULL(count(OpenID),0) AS Total, 'open' AS ActionTypeCode
	FROM ecn_Activity..BlastActivityOpens
	WHERE BlastID = @blastID
	UNION
	SELECT @blastID AS BlastID, ISNULL(count(distinct EmailID),0) AS unique_total, ISNULL(count(SendID),0) AS Total, 'send' AS ActionTypeCode
	FROM ecn_Activity..BlastActivitySends
	WHERE BlastID = @blastID
    UNION
	SELECT @blastID AS BlastID, ISNULL(count(distinct EmailID),0) AS unique_total, ISNULL(count(SuppressID),0) AS Total, 'Suppressed' AS ActionTypeCode
	FROM ecn_Activity..BlastActivitySuppressed
	WHERE BlastID = @blastID

end

--Before eal split
--USE [ecn5_communicator]
--GO
--/****** Object:  StoredProcedure [dbo].[rpt_BlastReportDetail]    Script Date: 12/28/2011 10:54:53 ******/
--SET ANSI_NULLS ON
--GO
--SET QUOTED_IDENTIFIER ON
--GO
--ALTER  proc [dbo].[rpt_BlastReportDetail] (@blastID int)
--as
--Begin

--	set nocount on

--	SELECT  @blastID as BlastID, 
--			ISNULL(count(distinct 
--					case 
--						when ActionTypeCode = 'bounce' AND NOT (ActionValue='resend') then eal.EmailID
--						when ActionTypeCode = 'bounce' AND (ActionValue = 'hard' OR ActionValue = 'hardbounce'  OR ActionValue='U') then eal.EmailID 
--						when ActionTypeCode = 'bounce' AND (ActionValue = 'soft' OR ActionValue = 'softbounce') then eal.EmailID 
--						when ActionTypeCode in ('send','testsend') then eal.EmailID 
--						else eal.emailID
--					end),0) AS unique_total,
--			ISNULL(count(
--					case 
--						when ActionTypeCode = 'bounce' AND NOT (ActionValue='resend') then eal.EAID
--						when ActionTypeCode = 'bounce' AND (ActionValue = 'hard' OR ActionValue = 'hardbounce'  OR ActionValue='U') then eal.EAID 
--						when ActionTypeCode = 'bounce' AND (ActionValue = 'soft' OR ActionValue = 'softbounce') then eal.EAID 
--						when ActionTypeCode in ('send','testsend') then eal.EmailID 
--						else eal.EAID
--					end),0) AS Total,
--			case 
--				when ActionTypeCode='bounce' and ActionValue not in ('resend', 'hard', 'hardbounce', 'U', 'soft', 'softbounce') then 'otherbounce'
--				when ActionTypeCode='bounce' and ActionValue in ('hard', 'hardbounce', 'U') then 'hardbounce'
--				when ActionTypeCode='bounce' and ActionValue in ('soft','softbounce') then 'softbounce'
--				when ActionTypeCode in ('send','testsend') then 'send' else Actiontypecode 
--			end as 'Actiontypecode' 
--	FROM  EmailActivityLog eal WHERE eal.BlastID = @blastID and ActionTypeCode <> 'click'
--	GROUP BY case 
--						when ActionTypeCode='bounce' and ActionValue not in ('resend', 'hard', 'hardbounce', 'U', 'soft', 'softbounce') then 'otherbounce'
--						when ActionTypeCode='bounce' and ActionValue in ('hard', 'hardbounce', 'U') then 'hardbounce'
--						when ActionTypeCode='bounce' and ActionValue in ('soft','softbounce') then 'softbounce'
--						when ActionTypeCode in ('send','testsend') then 'send' else Actiontypecode end 
				 
--	UNION  
--	SELECT  @blastID as BlastID, 
--			ISNULL(count(distinct eal.EmailID),0) AS unique_total,
--			ISNULL(count(eal.EAID),0) AS Total,
--			'bounce'
--	FROM  EmailActivityLog eal WHERE eal.BlastID = @blastID and ActionTypeCode = 'bounce'
--	UNION              
--	SELECT  @blastID, ISNULL(SUM(DistinctCount),0) , ISNULL(sum(total),0), 'click'         
--	FROM	(        
--				SELECT  COUNT(distinct ActionValue) AS DistinctCount, COUNT(EAID) AS total         
--				FROM   EmailActivityLog         
--				WHERE  ActionTypeCode = 'click' AND BlastID = @blastID        
--				GROUP BY  ActionValue, EmailID        
--			) AS inn      

--end

--BlastID	unique_total	Total	Actiontypecode
--447399	2	2	FEEDBACK_UNSUB
--447399	4	7	refer
--447399	20	22	subscribe
--447399	27	27	hardbounce
--447399	373	442	click
--447399	1617	1617	softbounce
--447399	2667	2667	otherbounce
--447399	3490	5407	open
--447399	4310	4311	bounce
--447399	34217	34217	send
