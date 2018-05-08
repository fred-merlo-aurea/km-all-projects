CREATE PROCEDURE [dbo].[v_Blast_GetSentByGroup] 
	@GroupID INT
AS     
BEGIN 
	--for testing
	--SET @GroupID = 199899
	--SET @GroupID = -1
	DECLARE @BlastIDs VARCHAR(MAX)
	SET @BlastIDs = ''

	SELECT 
		@BlastIDs = STUFF((SELECT ',' + CONVERT(VARCHAR(10),BlastID)
	FROM 
		Blast WITH (NOLOCK)
	WHERE 
		GroupID = @GroupID AND
		StatusCode = 'Sent'
	FOR XML PATH('')), 1, 1, '')

	IF @BlastIDs IS NULL OR LEN(@BlastIDs) < 7
		SET @BlastIDs = '0'

	SELECT @BlastIDs
END

