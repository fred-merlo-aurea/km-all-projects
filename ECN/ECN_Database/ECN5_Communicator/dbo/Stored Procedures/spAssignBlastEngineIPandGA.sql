CREATE proc [dbo].[spAssignBlastEngineIPandGA]
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
	(b.BlastType='html' OR b.BlastType='text' OR b.BlastType='sample' OR b.BlastType='champion') 
	and c.BlastConfigID is not null 
	and c.BlastConfigID in (1)--,2) just GA
	and c.BaseChannelID != 42
	and c.BaseChannelID != 70
ORDER BY b.sendtime ASC, b.blastID ASC

OPEN c_BlastCursor  
FETCH NEXT FROM c_BlastCursor INTO @CustomerID, @CurrentBlast, @BlastType, @LayoutID, @TestBlast, @BaseChannelID

WHILE @@FETCH_STATUS = 0  
BEGIN 
	SET @BlastEngineID = NULL	
	
	--Assign canon TEXT blast to a dedicated Engine (TEXT)
	IF @CustomerID in (1054,2840,1794) and @BlastType='text'
	BEGIN
		UPDATE Blasts
		SET BlastEngineID = (SELECT BlastEngineID FROM BlastEngines WHERE Name='Canon TEXT Engine')
		WHERE BlastID = @CurrentBlast
	END
	ELSE
	BEGIN
		--EXEC spCreateNewBlastSchedule @CurrentBlast
	
		IF @TestBlast = 'y'
		BEGIN
			SELECT TOP 1 @BlastEngineID = BlastEngineID 
			FROM BlastEngines 
			WHERE IsActive = 1 and IsPort25 = 0 and ISNULL(IsDedicatedEngine,0) = 1 and Name like 'ECNBlastEngine_Test%'
				AND BlastEngineID NOT IN (SELECT DISTINCT BlastEngineID FROM [BLAST] WHERE StatusCode = 'active' OR (BlastEngineID IS NOT NULL AND StatusCode = 'pending'))
			ORDER BY BlastEngineID ASC
			
			IF @BlastEngineID IS NULL
			BEGIN
				SELECT TOP 1 @BlastEngineID = BlastEngineID 
				FROM BlastEngines 
				WHERE IsActive = 1 and IsPort25 = 0 and ISNULL(IsDedicatedEngine,0) = 1 and Name like 'ECNBlastEngine_Test%'
				ORDER BY BlastEngineID ASC
			END
		END
		ELSE
		BEGIN
			IF @BlastType = 'sample'
			BEGIN
				SELECT @BlastEngineID = BlastEngineID
				FROM [Blast]
				WHERE BlastID = (SELECT TOP 1 BlastID
								 FROM SampleBlasts 
								 WHERE
									SampleID = (SELECT SampleID 
												FROM SampleBlasts 
												WHERE BlastID = @CurrentBlast) AND 
									BlastID <> @CurrentBlast)
			END
			
			IF @BlastEngineID IS NULL
			BEGIN
				SELECT TOP 1 @BlastEngineID = BlastEngineID
				FROM [Blast]
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
				WHERE IsActive = 1 and IsPort25 = 0 and ISNULL(IsDedicatedEngine,0) = 0 
					AND BlastEngineID NOT IN (SELECT DISTINCT BlastEngineID FROM [BLAST] WHERE StatusCode = 'active' OR (BlastEngineID IS NOT NULL AND StatusCode = 'pending'))
				ORDER BY BlastEngineID ASC	
			END
		END

		UPDATE [Blast]
		SET BlastEngineID = @BlastEngineID
		WHERE BlastID = @CurrentBlast
	END
	
	FETCH NEXT FROM c_BlastCursor INTO @CustomerID, @CurrentBlast, @BlastType, @LayoutID, @TestBlast, @BaseChannelID
END

CLOSE c_BlastCursor  
DEALLOCATE c_BlastCursor

Exec spAssignBlastEnginePort25

END