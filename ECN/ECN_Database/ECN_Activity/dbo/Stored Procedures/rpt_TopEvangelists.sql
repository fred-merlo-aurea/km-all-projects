CREATE PROC [dbo].[rpt_TopEvangelists]
	@CampaignItemID VARCHAR(500)	
AS

SELECT PivotTable.EmailID as EmailID, IsNull(PivotTable.[1], 0) as Facebook, IsNull(PivotTable.[2], 0) as Twitter, IsNull(PivotTable.[3], 0) as LinkedIn, 0 as FriendShares, Emails.EmailAddress, Emails.FirstName, Emails.LastName  ,ISNULL(Emails.FirstName,'') + ' ' + ISNULL(Emails.LastName,'') as FixSubName  , 'SocialNetwork_Share' as MergeCtrl
INTO #TempTbl
FROM (
    SELECT 
          EmailID, bas.SocialMediaID, Count(bas.SocialMediaID) AS SocialCount 
    FROM 
          BlastActivitySocial bas with (nolock)
          join ECN5_COMMUNICATOR..CampaignItemBlast cib with (nolock) on bas.BlastID = cib.BlastID
    WHERE 
          cib.CampaignItemID = @CampaignItemID and
          bas.SocialActivityCodeID = 1
    GROUP BY 
          EmailID,bas.SocialMediaID
) as s
PIVOT
(
    SUM(SocialCount)
    FOR SocialMediaID IN ([1],[2],[3])
)AS PivotTable
INNER JOIN [ECN5_COMMUNICATOR].[dbo].[Emails] Emails (nolock) on Emails.EmailID = PivotTable.EmailID


INSERT INTO #TempTbl
SELECT bar.EmailID, 0, 0, 0, COUNT(bar.ReferID) as FriendShares, Emails.EmailAddress, Emails.FirstName, Emails.LastName  ,ISNULL(Emails.FirstName,'') + ' ' + ISNULL(Emails.LastName,'') as FixSubName   , 'Email_Share' as MergeCtrl
FROM 
    BlastActivityRefer bar with (nolock)
    join ECN5_COMMUNICATOR..CampaignItemBlast cib with (nolock) on bar.BlastID = cib.BlastID
    JOIN [ECN5_COMMUNICATOR].[dbo].[Emails] Emails (nolock) on Emails.EmailID = bar.EmailID
WHERE 
    cib.CampaignItemID = @CampaignItemID
GROUP BY 
    bar.EmailID,Emails.EmailAddress, Emails.FirstName, Emails.LastName  ,ISNULL(Emails.FirstName,'') + ' ' + ISNULL(Emails.LastName,'')

select * from #TempTbl
DROP table #TempTbl

