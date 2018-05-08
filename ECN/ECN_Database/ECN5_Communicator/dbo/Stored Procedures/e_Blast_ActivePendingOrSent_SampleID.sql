CREATE PROCEDURE [dbo].[e_Blast_ActivePendingOrSent_SampleID] 
	@CustomerID INT,
	@SampleID INT
AS     
BEGIN 
	IF EXISTS (
		SELECT 
			TOP 1 BlastID
		FROM 
			Blast WITH (NOLOCK)
		WHERE 
			CustomerID = @CustomerID AND 
			SampleID = @SampleID AND
			(
				StatusCode = 'active' OR 
				StatusCode = 'sent' OR 
				StatusCode = 'pending'
			)
	) SELECT 1 ELSE SELECT 0


END
