CREATE PROCEDURE [dbo].[e_Blast_Select_Top1ByLayoutID]
@LayoutID int
AS
SELECT TOP 1 *
FROM Blast WITH(NOLOCK)
WHERE LayoutID = @LayoutID AND StatusCode <> 'Deleted' AND TestBlast <> 'Y'
ORDER BY SendTime DESC
