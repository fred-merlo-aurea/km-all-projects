CREATE PROCEDURE [dbo].[v_Blast_CustomerSamples] 
(
@CustomerID int = NULL,
@UserID int = NULL
)
AS

SELECT 
	b.*, f.FilterName, g.GroupName, s.SampleName
FROM 
	Blast b WITH (NOLOCK)
	LEFT OUTER JOIN Filter f WITH (NOLOCK) ON IsNull(b.FilterID, 0) = f.FilterID and f.IsDeleted = 0
	JOIN Groups g WITH (NOLOCK) ON b.GroupID = g.GroupID
	JOIN [Sample] s WITH (NOLOCK) ON IsNull(b.SampleID, 0) = s.SampleID and s.IsDeleted = 0
WHERE 
	b.CustomerID = @CustomerID AND
	(b.CreatedUserID = @UserID or b.UpdatedUserID = @UserID) AND 
	b.BlastType = 'Sample' AND
	b.StatusCode <> 'Deleted'
ORDER BY b.SendTime DESC
