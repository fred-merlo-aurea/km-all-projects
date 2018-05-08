CREATE PROCEDURE [dbo].[e_Blast_GetNextBlastForEngine] 
	@BlastEngineID INT,
	@Status varchar(50)
AS     
BEGIN 
	SELECT TOP 1 BlastID
	FROM Blast
	WHERE SendTime < GetDate() AND StatusCode = @Status AND BlastEngineID = @BlastEngineID
	ORDER BY SendTime ASC, blastID ASC
END
