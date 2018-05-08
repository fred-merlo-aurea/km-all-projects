CREATE  PROC [dbo].[e_Blast_Exists_ByBlastID] 
(
	@BlastID int,
	@CustomerID int
)
AS 
BEGIN
	IF EXISTS (
		SELECT 
			TOP 1 BlastID
		FROM 
			Blast WITH (NOLOCK)
		WHERE 
			CustomerID = @CustomerID AND 
			BlastID = @BlastID AND 
			StatusCode <> 'Deleted'
	) SELECT 1 ELSE SELECT 0
END