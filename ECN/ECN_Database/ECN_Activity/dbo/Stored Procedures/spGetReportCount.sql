CREATE proc [dbo].[spGetReportCount]
(
	@ActionType varchar(20),
	@BlastID varchar(20)
)
AS
BEGIN
	--DECLARE @ActionType VARCHAR(100)
	--DECLARE @BlastID VARCHAR(100)

	--SET @ActionType = 'open'
	--SET @BlastID = '512942'
	SET NOCOUNT ON
	
	IF @ActionType = 'open'
	BEGIN
		SELECT
			@ActionType as ActionTypeCode, b.BlastID,
			CONVERT(INT,(CONVERT(DECIMAL(10,2),CONVERT(DECIMAL(10,2),COUNT(EmailID)) / CONVERT(DECIMAL(10,2),
			(SendTotal - (	SELECT COUNT(DISTINCT EmailID) AS 'BouncesTotal'  
							FROM BlastActivityBounces WITH (NOLOCK)
							WHERE BlastID = b.BlastID
							GROUP BY BlastID))))) * 100) AS 'Count'
		FROM ecn5_communicator..[BLAST] b WITH (NOLOCK)
			JOIN BlastActivityOpens bao WITH (NOLOCK) ON bao.BlastID = b.BlastID
		WHERE
			bao.BlastID = b.BlastID AND
			bao.blastID = @BlastID
		GROUP BY b.blastID, b.EmailSubject, SendTotal 
	END
	ELSE IF @ActionType = 'Click'
	BEGIN
		SELECT
			@ActionType as ActionTypeCode, b.BlastID,
			CONVERT(INT,(CONVERT(DECIMAL(10,2),CONVERT(DECIMAL(10,2),COUNT(EmailID)) / CONVERT(DECIMAL(10,2),
			(SendTotal - (	SELECT COUNT(DISTINCT EmailID) AS 'BouncesTotal'  
							FROM BlastActivityBounces WITH (NOLOCK)
							WHERE BlastID = b.BlastID
							GROUP BY BlastID))))) * 100) AS 'Count'
		FROM ecn5_communicator..[BLAST] b WITH (NOLOCK)
			JOIN BlastActivityClicks bac WITH (NOLOCK) ON bac.BlastID = b.BlastID
		WHERE
			bac.BlastID = b.BlastID AND
			bac.blastID = @BlastID
		GROUP BY b.blastID, b.EmailSubject, SendTotal 
	END
	ELSE IF @ActionType = 'bounce'
	BEGIN
		SELECT
			@ActionType as ActionTypeCode, b.BlastID,
			CONVERT(INT,(CONVERT(DECIMAL(10,2),CONVERT(DECIMAL(10,2),COUNT(EmailID)) / CONVERT(DECIMAL(10,2),
			(SendTotal - (	SELECT COUNT(DISTINCT EmailID) AS 'BouncesTotal'  
							FROM BlastActivityBounces WITH (NOLOCK)
							WHERE BlastID = b.BlastID
							GROUP BY BlastID))))) * 100) AS 'Count'
		FROM ecn5_communicator..[BLAST] b WITH (NOLOCK)
			JOIN BlastActivityBounces bao WITH (NOLOCK) ON bao.BlastID = b.BlastID
		WHERE
			bao.BlastID = b.BlastID AND
			bao.blastID = @BlastID
		GROUP BY b.blastID, b.EmailSubject, SendTotal 
	END
	ELSE IF @ActionType = 'subscribe'
	BEGIN
		SELECT
			@ActionType as ActionTypeCode, b.BlastID,
			CONVERT(INT,(CONVERT(DECIMAL(10,2),CONVERT(DECIMAL(10,2),COUNT(EmailID)) / CONVERT(DECIMAL(10,2),
			(SendTotal - (	SELECT COUNT(DISTINCT EmailID) AS 'BouncesTotal'  
							FROM BlastActivityBounces WITH (NOLOCK)
							WHERE BlastID = b.BlastID
							GROUP BY BlastID))))) * 100) AS 'Count'
		FROM ecn5_communicator..[BLAST] b WITH (NOLOCK)
			JOIN BlastActivityUnSubscribes bau WITH (NOLOCK) ON bau.BlastID = b.BlastID
			JOIN UnsubscribeCodes uc WITH (NOLOCK) ON bau.UnsubscribeCodeID = uc.UnsubscribeCodeID
		WHERE
			bau.BlastID = b.BlastID AND
			bau.blastID = @BlastID AND
			uc.UnsubscribeCode = 'subscribe'
		GROUP BY b.blastID, b.EmailSubject, SendTotal 
	END
	ELSE IF @ActionType = 'refer'
	BEGIN
		SELECT
			@ActionType as ActionTypeCode, b.BlastID,
			CONVERT(INT,(CONVERT(DECIMAL(10,2),CONVERT(DECIMAL(10,2),COUNT(EmailID)) / CONVERT(DECIMAL(10,2),
			(SendTotal - (	SELECT COUNT(DISTINCT EmailID) AS 'BouncesTotal'  
							FROM BlastActivityBounces WITH (NOLOCK)
							WHERE BlastID = b.BlastID
							GROUP BY BlastID))))) * 100) AS 'Count'
		FROM ecn5_communicator..[BLAST] b WITH (NOLOCK)
			JOIN BlastActivityRefer bar WITH (NOLOCK) ON bar.BlastID = b.BlastID
		WHERE
			bar.BlastID = b.BlastID AND
			bar.blastID = @BlastID
		GROUP BY b.blastID, b.EmailSubject, SendTotal 
	END	
	
END
