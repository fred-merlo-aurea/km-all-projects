CREATE PROCEDURE [dbo].[e_Blast_Select_GroupID]
@GroupID int
AS
SELECT *
FROM Blast WITH(NOLOCK)
WHERE GroupID = @GroupID and StatusCode <> 'Deleted'
