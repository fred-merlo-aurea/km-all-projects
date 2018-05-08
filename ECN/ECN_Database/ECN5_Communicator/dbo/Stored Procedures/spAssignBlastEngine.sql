CREATE proc [dbo].[spAssignBlastEngine]
as
BEGIN

DECLARE @CurrentBlast int
DECLARE @BlastEngineID int
DECLARE @LayoutID int
DECLARE @CustomerID int
DECLARE @BlastType varchar(50)
DECLARE @TestBlast varchar(1)

DECLARE c_BlastCursor CURSOR FOR SELECT b.CustomerID, b.BlastID, b.BlastType, b.LayoutID, b.TestBlast
FROM [BLAST] b
	JOIN [ECN5_ACCOUNTS].[DBO].[CUSTOMER]  c on b.CustomerID = c.CustomerID
WHERE 
	c.ActiveFlag = 'Y' and 
	--c.BlastConfigID = 1 AND  --this is for GA only.  Comment out this line to get all MTAs
	b.SendTime <  DATEADD(mi, 1, GETDATE()) AND 
	b.StatusCode = 'pending' AND 
	b.BlastEngineID IS NULL AND 
	(b.BlastType='html' OR b.BlastType='text' OR b.BlastType='sample' OR b.BlastType='champion') 
	--AND	(c.basechannelid <> 60 or b.testblast='Y') --b.testblast='Y' or 
	and c.BlastConfigID is not null 
	--and c.customerID <> 3426
	--and c.BlastConfigID <> 1
ORDER BY b.sendtime ASC, b.blastID ASC

OPEN c_BlastCursor  
FETCH NEXT FROM c_BlastCursor INTO @CustomerID, @CurrentBlast, @BlastType, @LayoutID, @TestBlast

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
	ELSE IF exists (select customerID from [ECN5_ACCOUNTS].[DBO].[CUSTOMER]  where CustomerID = @CustomerID and BlastConfigID in (3,4))
	BEGIN
		UPDATE Blasts
		SET BlastEngineID = 98
		WHERE BlastID = @CurrentBlast	
	END
	ELSE
	BEGIN
		IF @TestBlast = 'y'
		BEGIN
			--SELECT @BlastEngineID = (SELECT BlastEngineID FROM BlastEngines WHERE Name='ECNBlastEngine_Test' AND IsActive = 1)
			SELECT TOP 1 @BlastEngineID = BlastEngineID 
			FROM BlastEngines 
			WHERE IsActive = 1 and ISNULL(IsDedicatedEngine,0) = 1 and Name like 'ECNBlastEngine_Test%'
				AND BlastEngineID NOT IN (SELECT DISTINCT BlastEngineID FROM [BLAST] WHERE StatusCode = 'active' OR (BlastEngineID IS NOT NULL AND StatusCode = 'pending'))
			ORDER BY BlastEngineID ASC
			
			IF @BlastEngineID IS NULL--all engines are assigned to with either pending or active blasts
			BEGIN
				SELECT TOP 1 @BlastEngineID = BlastEngineID 
				FROM [BLAST] 
				WHERE TestBlast = 'y' and (StatusCode = 'active' OR (BlastEngineID IS NOT NULL AND StatusCode = 'pending'))
				GROUP BY BlastEngineID
				ORDER BY COUNT(BlastEngineID) ASC
			END	
		END
	
		IF @BlastEngineID IS NULL AND @BlastType = 'sample'
		BEGIN
			SELECT @BlastEngineID = BlastEngineID
			FROM Blasts
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
			WHERE IsActive =1 and ISNULL(IsDedicatedEngine,0) = 0 
				AND BlastEngineID NOT IN (SELECT DISTINCT BlastEngineID FROM [BLAST] WHERE StatusCode = 'active' OR (BlastEngineID IS NOT NULL AND StatusCode = 'pending'))
			ORDER BY BlastEngineID ASC		
					
			--IF @BlastEngineID IS NULL--all engines are assigned to with either pending or active blasts
			--BEGIN
			--	SELECT TOP 1 @BlastEngineID = BlastEngineID 
			--	FROM [BLAST] 
			--	WHERE StatusCode = 'active' OR (BlastEngineID IS NOT NULL AND StatusCode = 'pending') 
			--	GROUP BY BlastEngineID
			--	ORDER BY COUNT(BlastEngineID) ASC
			--END
		END

		UPDATE Blasts
		SET BlastEngineID = @BlastEngineID
		WHERE BlastID = @CurrentBlast
	END
	
	FETCH NEXT FROM c_BlastCursor INTO @CustomerID, @CurrentBlast, @BlastType, @LayoutID, @TestBlast
END

CLOSE c_BlastCursor  
DEALLOCATE c_BlastCursor

END