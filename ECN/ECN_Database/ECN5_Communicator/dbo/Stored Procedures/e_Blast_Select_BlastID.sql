CREATE PROCEDURE [dbo].[e_Blast_Select_BlastID]
@BlastID int
AS
SELECT *
FROM Blast WITH(NOLOCK)
WHERE BlastID = @BlastID and StatusCode <> 'Deleted'