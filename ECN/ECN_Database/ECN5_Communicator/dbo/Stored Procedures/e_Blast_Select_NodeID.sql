CREATE PROCEDURE [dbo].[e_Blast_Select_NodeID]
@NodeID varchar(50)
AS
SELECT *
FROM Blast WITH(NOLOCK)
WHERE NodeID = @NodeID and StatusCode <> 'Deleted'
