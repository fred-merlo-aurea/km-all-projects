CREATE proc [dbo].[spAssignBlastEnginePort25Canon]
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
	JOIN [ECN5_ACCOUNTS].[DBO].[CUSTOMER] c WITH (NOLOCK) on b.CustomerID = c.CustomerID
WHERE 
c.ActiveFlag = 'Y' and 
	b.SendTime <  DATEADD(mi, 1, GETDATE()) AND 
	b.StatusCode = 'pending' AND 
	b.BlastEngineID IS NULL AND 
	(b.BlastType='html' OR b.BlastType='text' OR b.BlastType='sample' OR b.BlastType='champion') 
	and c.BlastConfigID is not null 
	and c.BlastConfigID in (3)
	and c.BaseChannelID = 42
ORDER BY b.sendtime ASC, b.blastID ASC

OPEN c_BlastCursor  
FETCH NEXT FROM c_BlastCursor INTO @CustomerID, @CurrentBlast, @BlastType, @LayoutID, @TestBlast, @BaseChannelID

WHILE @@FETCH_STATUS = 0  
BEGIN 
	SET @BlastEngineID = NULL
	
	IF 
		NOT EXISTS (SELECT TOP 1 CampaignItemBlastID FROM CampaignItemBlast WHERE BlastID = @CurrentBlast AND IsDeleted = 0) AND 
		NOT EXISTS (SELECT TOP 1 CampaignItemTestBlastID FROM CampaignItemTestBlast WHERE BlastID = @CurrentBlast AND IsDeleted = 0)
	BEGIN
	
		--put blast on hold
		UPDATE [Blast]
		SET StatusCode = 'HOLD'
		WHERE BlastID = @CurrentBlast
		
		--notify admin
		declare @b VARCHAR(8000) = ''
		set @b = 'Blast ' + convert(varchar(10), @CurrentBlast) + ' has been set to HOLD'		
		EXEC msdb..sp_send_dbmail 
		@profile_name='SQLAdmin', 
		@recipients='dev@knowledgemarketing.com', 
		--@recipients='bill@knowledgemarketing.com', 
		@importance='High',
		@body_format = 'HTML',
		@subject='Blast with no matching CampaignItemBlast or CampaignItemTestBlast record', 
		@body=@b
	END	
	ELSE
	BEGIN
	
		IF @TestBlast = 'y'
		BEGIN
			SELECT TOP 1 @BlastEngineID = BlastEngineID 
			FROM [BlastEngines] WITH (NOLOCK) 
			WHERE IsActive = 1 and IsPort25 = 1 and ISNULL(IsDedicatedEngine,0) = 1 and Name like 'ECNBlastEngine_Canon_Test%'
				AND BlastEngineID NOT IN (SELECT DISTINCT BlastEngineID FROM [BLAST] WITH (NOLOCK) WHERE StatusCode = 'active' OR (BlastEngineID IS NOT NULL AND StatusCode = 'pending'))
			ORDER BY BlastEngineID ASC
			
			IF @BlastEngineID IS NULL
			BEGIN
				SELECT TOP 1 @BlastEngineID = BlastEngineID 
				FROM [BlastEngines] WITH (NOLOCK) 
				WHERE IsActive = 1 and IsPort25 = 1 and ISNULL(IsDedicatedEngine,0) = 1 and Name like 'ECNBlastEngine_Canon_Test%'
				ORDER BY BlastEngineID ASC		
			END
		END
		ELSE
		BEGIN
			IF @BlastType = 'sample'
			BEGIN
				SELECT @BlastEngineID = BlastEngineID
				FROM [Blast] WITH (NOLOCK)
				WHERE
					SampleID = (SELECT SampleID 
								FROM [Blast] WITH (NOLOCK) 
								WHERE BlastID = @CurrentBlast) AND 
					BlastID <> @CurrentBlast
			END
			
			IF @BlastEngineID IS NULL
			BEGIN
				SELECT TOP 1 @BlastEngineID = BlastEngineID
				FROM [Blast] WITH (NOLOCK)
				WHERE  
					LayoutID = @LayoutID AND 
					BlastEngineID is not null AND 
					(StatusCode='active' OR StatusCode='pending') and TestBlast = 'N'
				ORDER BY sendtime ASC
			END
			
			--to make sure champions go thru the same engine 2/17/2014
			IF @BlastEngineID IS NULL and @BlastType = 'Champion'
			BEGIN
				SELECT TOP 1 @BlastEngineID = BlastEngineID
				FROM [Blast] WITH (NOLOCK)
				WHERE
					BlastType = 'Champion' AND
					CustomerID = @CustomerID AND
					(StatusCode='active' OR StatusCode='pending') AND 
					BlastID <> @CurrentBlast
				ORDER BY sendtime ASC
			END

			IF @BlastEngineID IS NULL
			BEGIN
				SELECT TOP 1 @BlastEngineID = BlastEngineID 
				FROM [BlastEngines] WITH (NOLOCK) 
				WHERE IsActive = 1 and IsPort25 = 1 and ISNULL(IsDedicatedEngine,0) = 1 and Name like 'ECNBlastEngine_Canon%' 
					AND BlastEngineID NOT IN (SELECT DISTINCT BlastEngineID FROM [BLAST] WITH (NOLOCK) WHERE StatusCode = 'active' OR (BlastEngineID IS NOT NULL AND StatusCode = 'pending'))
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

END
