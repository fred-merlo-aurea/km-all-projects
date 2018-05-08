CREATE proc [dbo].[spAssignBlastEngineSMS_ToRemove]
as
BEGIN

DECLARE @CurrentBlast int
DECLARE @BlastEngineID int
DECLARE @LayoutID int
DECLARE @CustomerID int
DECLARE @BlastType varchar(50)
DECLARE @TestBlast varchar(1)
DECLARE @BaseChannelID int

DECLARE c_BlastCursor CURSOR FOR SELECT b.CustomerID, b.BlastID, b.BlastType, b.LayoutID, b.TestBlast, c.BaseChannelID
FROM [BLAST] b
	JOIN [ECN5_ACCOUNTS].[DBO].[CUSTOMER]  c on b.CustomerID = c.CustomerID
WHERE 
c.ActiveFlag = 'Y' and 
	b.SendTime <  DATEADD(mi, 1, GETDATE()) AND 
	b.StatusCode = 'pending' AND 
	b.BlastEngineID IS NULL AND 
	(b.BlastType='sms') 
	and c.BlastConfigID is not null 
ORDER BY b.sendtime ASC, b.blastID ASC

OPEN c_BlastCursor  
FETCH NEXT FROM c_BlastCursor INTO @CustomerID, @CurrentBlast, @BlastType, @LayoutID, @TestBlast, @BaseChannelID

WHILE @@FETCH_STATUS = 0  
BEGIN 
	SET @BlastEngineID = NULL		

	IF @TestBlast = 'y'
	BEGIN
		SELECT TOP 1 @BlastEngineID = BlastEngineID 
		FROM BlastEngines 
		WHERE IsActive = 1  and IsSMSEngine=1 and ISNULL(IsDedicatedEngine,0) = 1 and Name like 'ECNBlastEngine_TestSMS%'
			AND BlastEngineID NOT IN (SELECT DISTINCT BlastEngineID FROM [BLAST] WHERE StatusCode = 'active' OR (BlastEngineID IS NOT NULL AND StatusCode = 'pending'))
		ORDER BY BlastEngineID ASC
	END
	
	IF @BlastEngineID IS NULL
	BEGIN
		SELECT TOP 1 @BlastEngineID = BlastEngineID
		FROM Blasts
		WHERE  
			LayoutID = @LayoutID AND 
			BlastEngineID is not null AND 
			(StatusCode='active' OR StatusCode='pending') and TestBlast = 'N'
		ORDER BY sendtime ASC
	END

	IF @BlastEngineID IS NULL
	BEGIN
		SELECT TOP 1 @BlastEngineID = BlastEngineID 
		FROM BlastEngines 
		WHERE IsActive = 1 and IsSMSEngine=1 and ISNULL(IsDedicatedEngine,0) = 1 
			AND BlastEngineID NOT IN (SELECT DISTINCT BlastEngineID FROM [BLAST] WHERE StatusCode = 'active' OR (BlastEngineID IS NOT NULL AND StatusCode = 'pending'))
		ORDER BY BlastEngineID ASC	
	END

	UPDATE Blasts
	SET BlastEngineID = @BlastEngineID
	WHERE BlastID = @CurrentBlast	
	
	FETCH NEXT FROM c_BlastCursor INTO @CustomerID, @CurrentBlast, @BlastType, @LayoutID, @TestBlast, @BaseChannelID
END

CLOSE c_BlastCursor  
DEALLOCATE c_BlastCursor


END
