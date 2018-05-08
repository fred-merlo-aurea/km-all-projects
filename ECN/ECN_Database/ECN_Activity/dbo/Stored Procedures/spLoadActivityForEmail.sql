CREATE PROCEDURE [dbo].[spLoadActivityForEmail](
	@EmailID int)
AS
BEGIN 	
	SET NOCOUNT ON
	
	--sends
	SELECT 
		bas.EMailID, e.EmailAddress, bas.BlastID, b.EmailSubject, 'send', bas.SendTime AS ActionDate, 'send' AS ActionValue
	FROM 
		BlastActivitySends bas WITH (NOLOCK) 
		JOIN ECN5_COMMUNICATOR..Emails e WITH (NOLOCK) ON bas.EMailID = e.EMailID 
		JOIN ECN5_COMMUNICATOR..[BLAST] b WITH (NOLOCK) ON bas.BlastID = b.BlastID
	WHERE 
		bas.EMailID= @EmailID
	UNION
	--opens	
	SELECT 
		bao.EMailID, e.EmailAddress, bao.BlastID, b.EmailSubject, 'open', bao.OpenTime AS ActionDate, bao.BrowserInfo AS ActionValue
	FROM 
		BlastActivityOpens bao WITH (NOLOCK) 
		JOIN ECN5_COMMUNICATOR..Emails e WITH (NOLOCK) ON bao.EMailID = e.EMailID 
		JOIN ECN5_COMMUNICATOR..[BLAST] b WITH (NOLOCK) ON bao.BlastID = b.BlastID
	WHERE 
		bao.EMailID= @EmailID
	UNION	
	--clicks	
	SELECT 
		bac.EMailID, e.EmailAddress, bac.BlastID, b.EmailSubject, 'click', bac.ClickTime AS ActionDate, bac.URL AS ActionValue
	FROM 
		BlastActivityClicks bac WITH (NOLOCK) 
		JOIN ECN5_COMMUNICATOR..Emails e WITH (NOLOCK) ON bac.EMailID = e.EMailID 
		JOIN ECN5_COMMUNICATOR..[BLAST] b WITH (NOLOCK) ON bac.BlastID = b.BlastID
	WHERE 
		bac.EMailID= @EmailID
	UNION
	--bounces	
	SELECT 
		bab.EMailID, e.EmailAddress, bab.BlastID, b.EmailSubject, 'bounce', bab.BounceTime AS ActionDate, bc.BounceCode AS ActionValue
	FROM 
		BlastActivityBounces bab WITH (NOLOCK) 
		JOIN BounceCodes bc WITH (NOLOCK) ON bab.BounceCodeID = bc.BounceCodeID 
		JOIN ECN5_COMMUNICATOR..Emails e WITH (NOLOCK) ON bab.EMailID = e.EMailID 
		JOIN ECN5_COMMUNICATOR..[BLAST] b WITH (NOLOCK) ON bab.BlastID = b.BlastID
	WHERE 
		bab.EMailID= @EmailID
	UNION
	--refer	
	SELECT 
		bar.EMailID, e.EmailAddress, bar.BlastID, b.EmailSubject, 'refer', bar.ReferTime AS ActionDate, bar.EmailAddress AS ActionValue
	FROM 
		BlastActivityRefer bar WITH (NOLOCK) 
		JOIN ECN5_COMMUNICATOR..Emails e WITH (NOLOCK) ON bar.EMailID = e.EMailID 
		JOIN ECN5_COMMUNICATOR..[BLAST] b WITH (NOLOCK) ON bar.BlastID = b.BlastID
	WHERE 
		bar.EMailID= @EmailID
	UNION
	--resend	
	SELECT 
		bar.EMailID, e.EmailAddress, bar.BlastID, b.EmailSubject, 'resend', bar.ResendTime AS ActionDate, 'resend' AS ActionValue
	FROM 
		BlastActivityResends bar WITH (NOLOCK) 
		JOIN ECN5_COMMUNICATOR..Emails e WITH (NOLOCK) ON bar.EMailID = e.EMailID 
		JOIN ECN5_COMMUNICATOR..[BLAST] b WITH (NOLOCK) ON bar.BlastID = b.BlastID
	WHERE 
		bar.EMailID= @EmailID
	UNION
	--suppressed	
	SELECT 
		basupp.EMailID, e.EmailAddress, basupp.BlastID, b.EmailSubject, 'suppressed', b.SendTime AS ActionDate, sc.SupressedCode AS ActionValue
	FROM 
		BlastActivitySuppressed basupp WITH (NOLOCK) 
		JOIN SuppressedCodes sc WITH (NOLOCK) ON basupp.SuppressedCodeID = sc.SuppressedCodeID 
		JOIN ECN5_COMMUNICATOR..Emails e WITH (NOLOCK) ON basupp.EMailID = e.EMailID 
		JOIN ECN5_COMMUNICATOR..[BLAST] b WITH (NOLOCK) ON basupp.BlastID = b.BlastID
	WHERE 
		basupp.EMailID= @EmailID
	UNION
	--unsubscribe
	SELECT 
		bau.EMailID, e.EmailAddress, bau.BlastID, b.EmailSubject, 'unsubscribe', bau.UnsubscribeTime AS ActionDate, uc.UnsubscribeCode AS ActionValue
	FROM 
		BlastActivityUnSubscribes bau WITH (NOLOCK) 
		JOIN UnsubscribeCodes uc WITH (NOLOCK) ON bau.UnsubscribeCodeID = uc.UnsubscribeCodeID
		JOIN ECN5_COMMUNICATOR..Emails e WITH (NOLOCK) ON bau.EMailID = e.EMailID 
		JOIN ECN5_COMMUNICATOR..[BLAST] b WITH (NOLOCK) ON bau.BlastID = b.BlastID
	WHERE 
		bau.EMailID= @EmailID
	ORDER BY 
		ActionDate DESC
	
END
