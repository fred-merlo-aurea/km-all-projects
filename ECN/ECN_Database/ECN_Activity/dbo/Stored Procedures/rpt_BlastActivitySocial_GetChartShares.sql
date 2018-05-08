CREATE proc [dbo].[rpt_BlastActivitySocial_GetChartShares] 
(
	@CampaignItemID int = NULL,
	@BlastID int = NULL,
	@CustomerID int
)
as
Begin
	Set nocount on
	
	DECLARE @BlastIDs varchar(4000)
	SET @BlastIDs = ''
	
	IF @CampaignItemID IS NOT NULL
	BEGIN
		SELECT 
			@BlastIDs = COALESCE(@BlastIDs + ',' ,'') + CONVERT(varchar(10), cib.BlastID)
		FROM 
			ecn5_communicator..[Campaign] c WITH (NOLOCK)
			JOIN ecn5_communicator..[CampaignItem] ci WITH (NOLOCK) ON c.CampaignID = ci.CampaignID and ci.IsDeleted = 0
			JOIN ecn5_communicator..[CampaignItemBlast] cib WITH (NOLOCK) ON ci.CampaignItemID = cib.CampaignItemID and cib.IsDeleted = 0 and ISNULL(cib.BlastID, 0) != 0
		WHERE
			c.CustomerID = @CustomerID AND
			c.IsDeleted = 0 AND
			ci.CampaignItemID = @CampaignItemID
						SET @BlastIDs = SubString(@BlastIDs, 2,4000)
	END
	ELSE
	BEGIN
		SELECT @BlastIDs = CONVERT(varchar(10),BlastID) FROM ecn5_communicator..[Blast] WHERE BlastID = @BlastID and CustomerID = @CustomerID
	END
	
	IF LEN(@BlastIDs) > 0
	BEGIN
		SELECT sm.DisplayName, COUNT(bas.SocialID) as Share
        FROM
            [BlastActivitySocial] bas WITH (NOLOCK)
            JOIN ecn5_communicator..[SocialMedia] sm WITH (NOLOCK) ON bas.SocialMediaID = sm.SocialMediaId
			join fn_Split(@BlastIDs, ',') items on bas.BlastID = items.Items
        WHERE
            bas.SocialActivityCodeID = 1
        GROUP BY
            sm.DisplayName
	END 
End