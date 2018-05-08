CREATE PROCEDURE [dbo].[v_BlastActivitySend_EmailID_GroupID] 
	@BlastIDs VARCHAR(MAX),
	@EmailID INT
AS     
BEGIN 
	--for testing
	--SET @EmailID = 233958089
	--SET @BlastIDs = '1282290'
	DECLARE @SelectSQL VARCHAR(MAX)
	SET @SelectSQL =
		'IF EXISTS 
				(
					SELECT 
						TOP 1 BlastID
					FROM 
						BlastActivitySends bas WITH (NOLOCK)
					WHERE 
						EmailID =  ' + convert(VARCHAR(10),@EmailID) + ' AND
						BlastID in (' + @BlastIDs + ')
				) 
				SELECT 1 ELSE SELECT 0'
	EXEC(@SelectSQL)
END

