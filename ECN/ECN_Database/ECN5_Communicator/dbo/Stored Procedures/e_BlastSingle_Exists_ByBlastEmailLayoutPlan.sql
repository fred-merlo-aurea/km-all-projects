CREATE PROCEDURE [dbo].[e_BlastSingle_Exists_ByBlastEmailLayoutPlan] 
	@BlastID int,
	@EmailID int,
	@LayoutPlanID int,
	@CustomerID int
AS     
BEGIN     		
	IF EXISTS  (
				SELECT TOP 1 bs.BlastSingleID 
				FROM 
					BlastSingles bs WITH (NOLOCK)
					JOIN Blast b WITH (NOLOCK) ON bs.BlastID = b.BlastID
				WHERE 
					b.CustomerID = @CustomerID AND 
					bs.BlastID = @BlastID AND 
					bs.EmailID = @EmailID AND
					bs.LayoutPlanID = @LayoutPlanID AND
					bs.IsDeleted = 0 AND 
					b.StatusCode <> 'Deleted'
				) SELECT 1 ELSE SELECT 0
END
