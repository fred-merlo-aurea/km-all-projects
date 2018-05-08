CREATE PROCEDURE [dbo].[e_Blast_ActivePendingOrSent_LayoutID] 
	@CustomerID INT,
	@LayoutID INT
AS     
BEGIN 
	IF EXISTS (
		SELECT 
			TOP 1 BlastID
		FROM 
			Blast WITH (NOLOCK)
		WHERE 
			CustomerID = @CustomerID AND 
			LayoutID = @LayoutID AND
			(
				StatusCode = 'active' OR 
				StatusCode = 'sent' OR 
				StatusCode = 'pending' OR
				StatusCode = 'system'
			)
	) SELECT 1 ELSE SELECT 0


END
