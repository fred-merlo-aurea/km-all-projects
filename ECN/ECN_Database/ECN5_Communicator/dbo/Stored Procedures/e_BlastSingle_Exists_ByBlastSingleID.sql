CREATE PROCEDURE [dbo].[e_BlastSingle_Exists_ByBlastSingleID] 
	@blastsingleID int,
	@CustomerID int
AS     
BEGIN     		
	IF EXISTS  (
				SELECT TOP 1 bs.BlastSingleID 
				FROM 
					BlastSingles bs WITH (NOLOCK)
					JOIN Blast b WITH (NOLOCK) ON bs.BlastID = b.BlastID
				WHERE 
					b.CustomerID = @CustomerID AND bs.BlastSingleID = @blastsingleID AND bs.IsDeleted = 0 AND b.StatusCode <> 'Deleted'
				) SELECT 1 ELSE SELECT 0
END
