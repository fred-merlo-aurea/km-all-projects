CREATE PROCEDURE [dbo].[e_Blast_Select_SampleID]
@SampleID int
AS
SELECT *
FROM Blast WITH(NOLOCK)
WHERE SampleID = @SampleID and StatusCode <> 'Deleted'
