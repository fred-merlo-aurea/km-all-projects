CREATE PROC [dbo].[sp_Get_TopEvangelistsReport]
(
	@CampaignItemID int,
	@bForwardToFriend varchar(5),
	@bFacebook varchar(5),
	@bTwitter varchar(5),
	@bLinkedIn varchar(5),	
	@startDate DateTime = NULL,
	@endDate DateTime = NULL,
	
	--@SQLQuery AS NVARCHAR(500) = 'SELECT TOP 100 [EmailID], [BlastID], [FullName] AS [Subscriber Name], [EmailAddress] AS [Email Address], [1]+[2]+[3]+[FriendShares] as [Total Number of Shares]',
	@SQLQuery AS NVARCHAR(500) = 'SELECT TOP 100 [EmailID], [BlastID], FixSubName AS [Subscriber Name], [EmailAddress] AS [Email Address], [1]+[2]+[3]+[FriendShares] as [Total Number of Shares]',
	@SQLQueryEnd AS NVARCHAR(500) = ' FROM #TempTbl ORDER BY [Total Number of Shares] DESC'
) AS
BEGIN

	--Get social shares
	--SELECT PivotTable.*, Emails.EmailAddress, Emails.FullName  ,NEW.FriendShares
	SELECT PivotTable.*, Emails.EmailAddress, Emails.FirstName, Emails.LastName  ,NEW.FriendShares, Convert(varchar(300),'') as FixSubName
	INTO #TempTbl
	FROM (
		SELECT EmailID, BlastID, SocialMediaID, Count(SocialMediaID) AS SocialCount 
		FROM [ecn_activity].[dbo].[BlastActivitySocial]
		WHERE EmailID IS NOT NULL		
		AND ((@startDate is null) or (@startDate <= ActionDate))
		AND ((@endDate is null) or (@endDate >= ActionDate) 		)
		GROUP BY EmailID,BlastID,SocialMediaID
	) as s
	PIVOT
	(
		SUM(SocialCount)
		FOR SocialMediaID IN ([1],[2],[3])
	)AS PivotTable

	--Get extra stuff
	INNER JOIN [ECN5_COMMUNICATOR].[dbo].[Emails] Emails (nolock) on Emails.EmailID = PivotTable.EmailID
	LEFT JOIN (SELECT EmailID,COUNT(EmailID)AS FriendShares 
				FROM [ecn_activity].[dbo].[BlastActivityRefer] (nolock)			
				GROUP BY EmailID) NEW on NEW.EmailID = PivotTable.EmailID 			
	INNER JOIN [ECN5_COMMUNICATOR].[dbo].[CampaignItemBlast] cib (nolock) on cib.BlastID = PivotTable.BlastID
	WHERE CampaignItemID = @CampaignItemID
	
	--Fix FriendShares null and SubscriberName
	UPDATE t
	SET [FriendShares] = 0
	FROM  #TempTbl t
	WHERE [FriendShares] IS NULL
	
	UPDATE t  --Facebook
	SET [1] = 0
	FROM  #TempTbl t
	WHERE [1] IS NULL

	UPDATE t  --Twitter
	SET [2] = 0
	FROM  #TempTbl t
	WHERE [2] IS NULL

	UPDATE t  --LinkedIn
	SET [3] = 0
	FROM  #TempTbl t
	WHERE [3] IS NULL

	DECLARE @FullName varchar(max)
	SET @FullName = 
	(select top 1 ISNULL(FirstName,'') + ' ' + ISNULL(LastName,'') from #TempTbl) 
	UPDATE t
	SET FixSubName = @FullName
	FROM  #TempTbl t

	--Zero out unwanted columns OR add them to Query 
	if(UPPER(@bForwardToFriend) <> 'TRUE')
		BEGIN
			UPDATE t
			SET t.[FriendShares] = 0
			FROM #TempTbl t	
		END	
	ELSE
	BEGIN
		SET @SQLQuery = @SQLQuery + ',[FriendShares] AS [Forward to a Friend]'
	END
	if(UPPER(@bFacebook) <> 'TRUE')
		BEGIN
			UPDATE t
			SET t.[1] = 0
			FROM #TempTbl t	
		END	
	ELSE
		BEGIN
			SET @SQLQuery = @SQLQuery + ',[1] AS [Facebook]'
		END
	if(UPPER(@bTwitter) <> 'TRUE')
		BEGIN
			UPDATE t
			SET t.[2] = 0
			FROM #TempTbl t	
		END
	ELSE
		BEGIN
			SET @SQLQuery = @SQLQuery + ',[2] AS [Twitter]'
		END
	if(UPPER(@bLinkedIn) <> 'TRUE')
		BEGIN
			UPDATE t
			SET t.[3] = 0
			FROM #TempTbl t	
		END
	ELSE
		BEGIN
			SET @SQLQuery = @SQLQuery + ',[3] AS [LinkedIn]'
		END

	--Finish SQL Query
	SET @SQLQuery = @SQLQuery + @SQLQueryEnd
	--SELECT @SQLQuery
	EXEC sp_Executesql  @SQLQuery
	DROP table #TempTbl
	
END