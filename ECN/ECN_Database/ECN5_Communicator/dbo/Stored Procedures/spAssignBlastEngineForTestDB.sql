CREATE proc [dbo].[spAssignBlastEngineForTestDB]
as
BEGIN

DECLARE @CurrentBlast int
DECLARE @BlastEngineID int
DECLARE @LayoutID int
DECLARE @CustomerID int
DECLARE @BlastType varchar(50)
DECLARE @TestBlast varchar(1)

DECLARE c_BlastCursor CURSOR FOR SELECT b.BlastID
FROM [BLAST] b
	JOIN [ECN5_ACCOUNTS].[DBO].[CUSTOMER]  c on b.CustomerID = c.CustomerID
WHERE 
	b.SendTime <  DATEADD(mi, 1, GETDATE()) AND 
	b.StatusCode = 'pending' AND 
	b.BlastEngineID IS NULL AND 
	(b.BlastType='html' OR b.BlastType='text' OR b.BlastType='sample' OR b.BlastType='champion') 
	and c.customerID in (1)
ORDER BY b.sendtime ASC, b.blastID ASC

OPEN c_BlastCursor  
FETCH NEXT FROM c_BlastCursor INTO @CurrentBlast

WHILE @@FETCH_STATUS = 0  
BEGIN 
	SELECT TOP 1 @BlastEngineID = BlastEngineID 
	FROM BlastEngines 
	WHERE IsActive = 1 and IsPort25 = 0 and ISNULL(IsDedicatedEngine,0) = 0 
		AND BlastEngineID NOT IN (SELECT DISTINCT BlastEngineID FROM [BLAST] WHERE StatusCode = 'active' OR (BlastEngineID IS NOT NULL AND StatusCode = 'pending'))
	ORDER BY BlastEngineID ASC	

	UPDATE Blasts
	SET BlastEngineID = @BlastEngineID
	WHERE BlastID = @CurrentBlast
		
	FETCH NEXT FROM c_BlastCursor INTO @CurrentBlast
END

CLOSE c_BlastCursor  
DEALLOCATE c_BlastCursor

END