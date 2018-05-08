CREATE  PROCEDURE [dbo].[spGetTopEvangelistsReport] 
(	  
	@CampaignItemID int,  --383451
	@bFacebook bit,
	@bTwitter bit,
	@bLinkedIn bit,
	@bForwardToFriend bit
)
as
Begin

	SELECT PivotTable.*, Emails.EmailAddress, Emails.FullName  ,  NEW.FriendShares
	INTO #TempTbl
	FROM (
		SELECT EmailID, BlastID, SocialMediaID, Count(SocialMediaID) AS SocialCount 
		FROM [ecn_activity].[dbo].[BlastActivitySocial]
		WHERE EmailID IS NOT NULL
		GROUP BY EmailID,BlastID,SocialMediaID
	) as s
	PIVOT
	(
		SUM(SocialCount)
		FOR SocialMediaID IN ([1],[2],[3],[4],[5])
	)AS PivotTable

	INNER JOIN [ECN5_COMMUNICATOR].[dbo].[Emails] Emails (nolock) on Emails.EmailID = PivotTable.EmailID

	LEFT JOIN (SELECT EmailID,COUNT(EmailID)AS FriendShares 
				FROM [ecn_activity].[dbo].[BlastActivityRefer] (nolock)			
				GROUP BY EmailID) NEW on NEW.EmailID = PivotTable.EmailID 
				
	INNER JOIN [ECN5_COMMUNICATOR].[dbo].[CampaignItemBlast] cib (nolock) on cib.BlastID = PivotTable.BlastID
	WHERE CampaignItemID =  @CampaignItemID
	
	if(UPPER(@bFacebook) <> 'TRUE')
	BEGIN
		UPDATE t
		SET t.[1] = 0
		FROM #TempTbl t	
	END

	if(UPPER(@bTwitter) <> 'TRUE')
	BEGIN
		UPDATE t
		SET t.[1] = 0
		FROM #TempTbl t	
	END

	if(UPPER(@bLinkedIn) <> 'TRUE')
	BEGIN
		UPDATE t
		SET t.[1] = 0
		FROM #TempTbl t	
	END

	if(UPPER(@bForwardToFriend) <> 'TRUE')
	BEGIN
		UPDATE t
		SET t.[1] = 0
		FROM #TempTbl t	
	END

	SELECT *, [1]+[2]+[3]+[FriendShares] as TotalShares FROM #TempTbl
	DROP table #TempTbl
	
End
	
