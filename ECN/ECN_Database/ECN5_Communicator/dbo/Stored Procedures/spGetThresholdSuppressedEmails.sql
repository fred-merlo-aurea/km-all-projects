CREATE PROC [dbo].[spGetThresholdSuppressedEmails]
(
	@BlastID INT,
	@CheckAll CHAR(1)--Y will check all sent email, otherwise it will check only those with message types of threshold or priority
)
AS    
BEGIN  
	SET NOCOUNT ON
	CREATE TABLE #t(
       EmailId INT,
       BlastId INT)

	DECLARE		@customerID int,
				@basechannelID int,
				@thresholdlimit int,
				@customerProduct int,
				@blasttime datetime,
				@tempgroup int
			
		set @thresholdlimit = 0
		--SET @BlastID = 307575
				
	SELECT	
			@customerID = c.CustomerID, 
			@basechannelID = c.BaseChannelID, 
			@thresholdlimit = isnull(EmailThreshold,0),
			@blasttime = sendtime  
	FROM 
			blast b WITH (NOLOCK) join 
			ecn5_accounts..Customer c WITH (NOLOCK) on b.CustomerID = c.CustomerID join 
			ecn5_accounts..BaseChannel bc WITH (NOLOCK) on bc.BaseChannelID = c.BaseChannelID
	WHERE 
			blastID = @BlastID and
			b.StatusCode <> 'Deleted' and
			c.IsDeleted = 0 and
			bc.IsDeleted = 0
		
	SELECT 
			@customerProduct = count(sf.SFCode) 
	FROM 
			 KMPlatform..ClientServiceFeatureMap csfm WITH(NOLOCK)
			 JOIN KMPlatform..ServiceFeature sf WITH(NOLOCK) on csfm.ServiceFeatureID = sf.ServiceFeatureID
			 JOIN KMPlatform..Service s WITH(NOLOCK) on sf.ServiceID = s.ServiceID
			 JOIN ECN5_ACCOUNTS..Customer c with(nolock) on csfm.ClientID = c.PlatformClientID
	WHERE 
			c.CustomerID = @customerID AND 
			s.ServiceCode = 'EMAILMARKETING' AND 
			sf.SFCode = 'SetMessageThresholds' AND 
			csfm.IsEnabled = 1
			
	IF @customerProduct > 0 AND @thresholdlimit > 0
	BEGIN		
		IF @CheckAll = 'Y'
		BEGIN
			INSERT INTO #t
			 (EmailId,
			 BlastId)             
			SELECT
				EmailId,
				 b.BlastId
			FROM 
				Blast b  WITH (NOLOCK) 
				JOIN ecn5_accounts..Customer c  WITH (NOLOCK) ON b.CustomerID = c.customerID
				JOIN EmailActivityLog eal  WITH (NOLOCK) ON b.BlastID = eal.blastID 
			WHERE  
				c.BaseChannelID = @BasechannelId
				AND b.TestBlast = 'N' 
				AND b.StatusCode='sent' 
				AND eal.ActionTypeCode = 'send'
				AND b.StatusCode <> 'Deleted' 
				AND c.IsDeleted = 0   
				AND CONVERT(Date,b.SendTime) = CONVERT(Date,GETDATE())

		END
		ELSE
		BEGIN
			INSERT INTO #t
				   (EmailId,
				   BlastId)             
			SELECT
				   EmailId,
				   b.BlastId
			FROM 
				   Blast b  WITH (NOLOCK) 
				   JOIN ecn5_accounts..Customer c  WITH (NOLOCK) ON b.CustomerID = c.customerID
				   JOIN EmailActivityLog eal  WITH (NOLOCK) ON b.BlastID = eal.blastID 
				   JOIN Layout l with(nolock) on b.LayoutID = l.LayoutID
				   join MessageType mt with(nolock) on l.MessageTypeID = mt.MessageTypeID
			WHERE  
				   c.BaseChannelID = @BasechannelId
				   AND b.TestBlast = 'N' 
				   AND b.StatusCode='sent' 
				   AND eal.ActionTypeCode = 'send'
				   AND b.StatusCode <> 'Deleted' 
				   and (mt.Priority = 1 or mt.Threshold = 1)
				   AND c.IsDeleted = 0   
				   AND CONVERT(Date,b.SendTime) = CONVERT(Date,GETDATE())

		END
	END	  ;
	
	WITH cte AS(
	SELECT
		   e.emailid,
		   e.emailaddress,
		   COUNT(*) BlastCount,
		   STUFF((
				  SELECT DISTINCT ',' + CONVERT(VARCHAR(MAX),blastID)
				  FROM #t
				  WHERE  e.EmailID = #t.emailID
			 FOR XML PATH('') ), 1, 1, '') BlastsAlreadySent
	FROM  
		   Emails e WITH (NOLOCK) 
		   JOIN #t on e.EmailId = #t.EmailId
	GROUP BY 
		   e.emailid,
		   e.emailaddress
	) 
	SELECT  
		   EmailAddress,
		   Blastcount,
		   BlastsAlreadySent
	FROM 
		   cte;


	DROP TABLE #t
		
END