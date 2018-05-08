﻿CREATE PROCEDURE [dbo].[e_Blast_ActivePendingOrSent_BlastID] 
	@CustomerID INT,
	@BlastID INT
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
			(
				StatusCode = 'active' OR 
				StatusCode = 'sent' OR 
				StatusCode = 'pending'
			)
	) SELECT 1 ELSE SELECT 0


END
