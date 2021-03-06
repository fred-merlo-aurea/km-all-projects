USE [ecn5_communicator]
GO
/****** Object:  StoredProcedure [dbo].[spAssignBlastEngine]    Script Date: 05/23/2011 07:44:42 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

ALTER proc [dbo].[spAssignBlastEngine]
as
BEGIN

DECLARE @CurrentBlast int
DECLARE @BlastEngineID int
DECLARE @LayoutID int
DECLARE @CustomerID int
DECLARE @BlastType varchar(50)

DECLARE c_BlastCursor CURSOR FOR SELECT b.CustomerID, b.BlastID, b.BlastType, b.LayoutID
FROM Blasts b
	JOIN ecn5_accounts..Customer c on b.CustomerID = c.CustomerID
WHERE 
	--c.BlastConfigID = 1 AND  --this is for GA only.  Comment out this line to get all MTAs
	b.SendTime <  DATEADD(mi, 1, GETDATE()) AND 
	b.StatusCode = 'pending' AND 
	b.BlastEngineID IS NULL AND 
	(b.BlastType='html' OR b.BlastType='text' OR b.BlastType='sample' OR b.BlastType='champion')
ORDER BY b.sendtime ASC, b.blastID ASC

OPEN c_BlastCursor  
FETCH NEXT FROM c_BlastCursor INTO @CustomerID, @CurrentBlast, @BlastType, @LayoutID

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
		IF @BlastType = 'sample'
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
				(StatusCode='active' OR StatusCode='pending')
			ORDER BY sendtime ASC
		END

		IF @BlastEngineID IS NULL
		BEGIN
			SELECT TOP 1 @BlastEngineID = BlastEngineID 
			FROM BlastEngines 
			WHERE IsActive =1 and ISNULL(IsDedicatedEngine,0) = 0 
				AND BlastEngineID NOT IN (SELECT BlastEngineID FROM Blasts WHERE StatusCode = 'active' OR (BlastEngineID IS NOT NULL AND StatusCode = 'pending'))
			ORDER BY BlastEngineID ASC				
			IF @BlastEngineID IS NULL--all engines are assigned to with either pending or active blasts
			BEGIN
				SELECT TOP 1 @BlastEngineID = BlastEngineID 
				FROM Blasts 
				WHERE StatusCode = 'active' OR (BlastEngineID IS NOT NULL AND StatusCode = 'pending') 
				GROUP BY BlastEngineID
				ORDER BY COUNT(BlastEngineID) ASC
			END
		END

		UPDATE Blasts
		SET BlastEngineID = @BlastEngineID
		WHERE BlastID = @CurrentBlast
	END
	
	FETCH NEXT FROM c_BlastCursor INTO @CustomerID, @CurrentBlast, @BlastType, @LayoutID
END

CLOSE c_BlastCursor  
DEALLOCATE c_BlastCursor

END




