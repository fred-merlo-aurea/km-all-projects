CREATE PROCEDURE [dbo].[v_Sample_GetAvailable] 
(
@CustomerID int = NULL,
@CampaignItemID int = NULL
)
AS

if @CampaignItemID is null
BEGIN
	SELECT
		s.SampleID, s.SampleName + ' - - ' + Convert(varchar,b.SendTime) AS SampleName
	FROM
		[Sample] s WITH (NOLOCK)
		JOIN Blast b WITH (NOLOCK) ON s.SampleID = b.SampleID
	WHERE
		s.CustomerID = @CustomerID AND
		b.BlastType = 'Sample' AND
		b.StatusCode <> 'Deleted' AND
		s.IsDeleted = 0 AND
		s.SampleID not in (Select SampleID from Blast where SampleID = s.SampleID and BlastType = 'Champion' and StatusCode <> 'Deleted')
	GROUP BY
		s.SampleID, b.SendTime, SampleName
		END
ELSE
BEGIN 
	SELECT
		s.SampleID, s.SampleName + ' - - ' + Convert(varchar,b.SendTime) AS SampleName
	FROM
		[Sample] s WITH (NOLOCK)
		JOIN Blast b WITH (NOLOCK) ON s.SampleID = b.SampleID
	WHERE
		s.CustomerID = @CustomerID AND
		b.BlastType = 'Sample' AND
		b.StatusCode <> 'Deleted' AND
		s.IsDeleted = 0 AND
		s.SampleID not in (Select SampleID from Blast where SampleID = s.SampleID and BlastType = 'Champion' and StatusCode <> 'Deleted')
		or
		(
		s.SampleID = (SELECT SampleID from Blast b with(nolock) 
									join CampaignItemBlast cib with(nolock) on b.BlastID = cib.BlastID
									WHERE cib.CampaignItemID = @CampaignItemID and b.StatusCode = 'pending')
		 and s.CustomerID = @CustomerID 
		 and b.BlastType = 'Sample'
		 and b.StatusCode <> 'Deleted'
		 and s.IsDeleted = 0
		)
	GROUP BY
		s.SampleID, b.SendTime, SampleName
END