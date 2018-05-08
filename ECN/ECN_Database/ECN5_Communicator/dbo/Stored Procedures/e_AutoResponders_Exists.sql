CREATE PROCEDURE [dbo].[e_AutoResponders_Exists] 
	@AutoResponderID int,
	@CustomerID int
AS     
BEGIN     		
	IF EXISTS	(
					SELECT TOP 1 ar.AutoResponderID 
					FROM 
						AutoResponders ar
							join Blast b on ar.BlastID = b.BlastID 
					WHERE 
						b.CustomerID = @CustomerID AND 
						ar.AutoResponderID = @AutoResponderID AND 
						ar.IsDeleted = 0 AND
						b.StatusCode <> 'Deleted'
				) SELECT 1 ELSE SELECT 0
END
