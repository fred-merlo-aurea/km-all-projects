CREATE PROCEDURE job_DeleteSubscriberImportData
@DaysToKeep INT = 15,
@CountsOnly INT = 0
AS
BEGIN   

	SET NOCOUNT ON 

	DECLARE
		@dt DATE,
		@i INT,
		@MaxCount INT

	/*IF positive value is input, switch to negative value to prevent counting days forward*/

	IF  @DaysToKeep > 0  
		SET @DaysToKeep = 0 - @DaysToKeep
		SET @dt = DATEADD(DD, @DaysToKeep, GETDATE())
	IF @dt > GETDATE() 
		RETURN 1

	CREATE TABLE #tmp1 (ID INT IDENTITY(1,1), tableName CHAR(3), PK1 INT)

	CREATE CLUSTERED INDEX IDX_tmp1_ID ON #tmp1(ID)
	CREATE INDEX IDX_tmp1_tableName ON #tmp1(tableName)

	INSERT INTO #tmp1
	SELECT 'ST', SubscriberTransformedID
	FROM SubscriberTransformed with (NOLOCK) 
	WHERE DateCreated < @dt
	UNION ALL
		SELECT 'SDT', SubscriberDemographicTransformedID
		FROM SubscriberDemographicTransformed with (NOLOCK) 
		WHERE DateCreated < @dt
	UNION ALL
		SELECT 'SO', SubscriberOriginalID
		FROM SubscriberOriginal with (NOLOCK) 
		WHERE DateCreated < @dt 
	UNION ALL
		SELECT'SDO', SDOriginalID 
		FROM SubscriberDemographicOriginal with (NOLOCK)
		WHERE DateCreated < @dt 
	UNION ALL
		SELECT 'SF', SubscriberFinalID
		FROM SubscriberFinal with (NOLOCK) 
		WHERE DateCreated < @dt
	UNION ALL
		SELECT 'SDF', SDFinalID 
		FROM SubscriberDemographicFinal with (NOLOCK)
		WHERE DateCreated < @dt
	UNION ALL
		SELECT 'SI', SubscriberInvalidID
		FROM SubscriberInvalid with (NOLOCK) 
		WHERE DateCreated < @dt 
	UNION ALL
		SELECT 'SDI', SDInvalidID 
		FROM SubscriberDemographicInvalid WITH (NOLOCK) 
		WHERE DateCreated < @dt 
	UNION ALL
		SELECT 'SA', SubscriberArchiveID
		FROM SubscriberArchive with (NOLOCK) 
		WHERE DateCreated < @dt 
	UNION ALL
		SELECT 'SDA', SDArchiveID 
		FROM SubscriberDemographicArchive with (NOLOCK)
		WHERE DateCreated < @dt 
	IF @CountsOnly = 1
		BEGIN
			SELECT 'SubscriberDemographicOriginal' AS TableName, COUNT(*) AS Records 
			FROM SubscriberDemographicOriginal p 
				INNER JOIN #tmp1 t on p.SDOriginalID = PK1 
			where tableName = 'SDO'  
			UNION ALL 
				SELECT 'SubscriberOriginal', COUNT(*) 
				FROM SubscriberOriginal p 
					INNER JOIN #tmp1 t on SubscriberOriginalID = PK1 
				where tableName = 'SO'   
			UNION ALL 
				SELECT 'SubscriberDemographicTransformed', COUNT(*) 
				FROM SubscriberDemographicTransformed p 
					INNER JOIN #tmp1 t on p.SubscriberDemographicTransformedID = PK1 
				where tableName = 'SDT'  
			UNION ALL 
				SELECT 'SubscriberTransformed', COUNT(*) 
				FROM SubscriberTransformed p 
					INNER JOIN #tmp1 t on SubscriberTransformedID = PK1 
				where tableName = 'ST'  
			UNION ALL 
				SELECT 'SubscriberDemographicFinal', COUNT(*) 
				FROM SubscriberDemographicFinal p 
					INNER JOIN #tmp1 t on p.SDFinalID = PK1 
				where tableName = 'SDF'  
			UNION ALL 
				SELECT 'SubscriberFinal', COUNT(*) 
				FROM SubscriberFinal p 
					INNER JOIN #tmp1 t on SubscriberFinalID = PK1 
				where tableName = 'SF'  
			UNION ALL 
				SELECT 'SubscriberDemographicInvalid', COUNT(*) 
				FROM SubscriberDemographicInvalid p 
					INNER JOIN #tmp1 t on p.SDInvalidID = PK1 
				where tableName = 'SDI'  
			UNION ALL 
				SELECT 'SubscriberInvalid', COUNT(*) 
				FROM SubscriberInvalid p 
					INNER JOIN #tmp1 t on SubscriberInvalidID = PK1 
				where tableName = 'SI'  
			UNION ALL 
				SELECT 'SubscriberDemographicArchive', COUNT(*) 
				FROM SubscriberDemographicArchive p 
					INNER JOIN #tmp1 t on p.SDArchiveID = PK1 
				where tableName = 'SDA'   
			UNION ALL 
				SELECT 'SubscriberArchive', COUNT(*) 
				FROM SubscriberArchive p 
					INNER JOIN #tmp1 t on SubscriberArchiveID = PK1 
				where tableName = 'SA' 
			UNION ALL
				select 'ImportError' as TableName, Count(*) 
				from ImportError with(nolock) 
				where DateCreated < @dt 
			UNION ALL
				select 'FileValidator_DemographicTransformed'  as TableName, Count(*) 
				from FileValidator_DemographicTransformed with(nolock) 
				where DateCreated < @dt 
			UNION ALL
				select 'FileValidator_ImportError' as TableName, Count(*) 
				from FileValidator_ImportError with(nolock) 
				where DateCreated < @dt 
			UNION ALL
				select 'FileValidator_Transformed' as TableName, Count(*) 
				from FileValidator_Transformed with(nolock) 
				where DateCreated < @dt
			ORDER BY 1
		END

	IF @CountsOnly = 0
		BEGIN
			DELETE p 
			FROM SubscriberDemographicOriginal p 
				INNER JOIN #tmp1 t on p.SDOriginalID = PK1 
			where tableName = 'SDO' 

			DELETE p 
			FROM SubscriberOriginal p 
				INNER JOIN #tmp1 t on SubscriberOriginalID = PK1 
			where tableName = 'SO' 

			DELETE p 
			FROM SubscriberDemographicTransformed p 
				INNER JOIN #tmp1 t on p.SubscriberDemographicTransformedID = PK1 
			where tableName = 'SDT'

			DELETE p 
			FROM SubscriberTransformed p 
				INNER JOIN #tmp1 t on SubscriberTransformedID = PK1 
			where tableName = 'ST'

			DELETE p 
			FROM SubscriberDemographicFinal p 
				INNER JOIN #tmp1 t on p.SDFinalID = PK1 
			where tableName = 'SDF'

			DELETE p 
			FROM SubscriberFinal p 
				INNER JOIN #tmp1 t on SubscriberFinalID = PK1 
			where tableName = 'SF'

			DELETE p 
			FROM subscriberDemographicInvalid p 
				INNER JOIN #tmp1 t on p.SDInvalidID = PK1 
			where tableName = 'SDI'

			DELETE p 
			FROM SubscriberInvalid p 
				INNER JOIN #tmp1 t on SubscriberInvalidID = PK1 
			where tableName = 'SI'

			DELETE p 
			FROM subscriberDemographicArchive p 
				INNER JOIN #tmp1 t on p.SDArchiveID = PK1 
			where tableName = 'SDA' 

			DELETE p 
			FROM SubscriberArchive p 
				INNER JOIN #tmp1 t on SubscriberArchiveID = PK1 
			where tableName = 'SA'

			delete ImportError 
			where DateCreated < @dt 

			delete FileValidator_DemographicTransformed 
			where DateCreated < @dt

			delete FileValidator_ImportError 
			where DateCreated < @dt

			delete FileValidator_Transformed 
			where DateCreated < @dt
		END
      
	DROP TABLE #tmp1

END
GO