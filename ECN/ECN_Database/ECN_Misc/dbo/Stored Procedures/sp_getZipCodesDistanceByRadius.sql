CREATE PROC [dbo].[sp_getZipCodesDistanceByRadius]  (
	@SourceZipCode INT,
	@Radius INT
)

AS
	BEGIN
		DECLARE @lat NUMERIC(10,6), @lon NUMERIC(10,6)
		SELECT @lat = (SELECT DISTINCT Latitude FROM USZipCodeMaster WHERE ZipCode = @SourceZipCode )	
		SELECT @lon = (SELECT DISTINCT Longitude FROM USZipCodeMaster WHERE ZipCode = @SourceZipCode )	
		
		CREATE TABLE #SelectedZipCodes (ZipCode VARCHAR(255), Lat VARCHAR(255), Lon VARCHAR(255), Distance DECIMAL(5,2))		
		
		INSERT INTO #SelectedZipCodes
		SELECT ZipCode,Latitude,Longitude, 
			(3956.548 * ACOS(COS(PI()/2-RADIANS(90-CONVERT(NUMERIC(10,6),Latitude)))
			* COS(PI()/2-RADIANS(90-@lat)) 
			* COS(RADIANS(CONVERT(NUMERIC(10,6),Longitude))-RADIANS(@lon)) 
			+ SIN(PI()/2-RADIANS(90-CONVERT(NUMERIC(10,6),Latitude))) 
			* SIN(PI()/2-RADIANS(90-@lat)))) AS Distance
		FROM USZipCodeMaster 
		WHERE 
			(3956.548 * ACOS(COS(PI()/2-RADIANS(90-CONVERT(NUMERIC(10,6),Latitude)))
			* COS(PI()/2-RADIANS(90-@lat)) 
			* COS(RADIANS(CONVERT(NUMERIC(10,6),Longitude))-RADIANS(@lon)) 
			+ SIN(PI()/2-RADIANS(90-CONVERT(NUMERIC(10,6),Latitude))) 
			* SIN(PI()/2-RADIANS(90-@lat)))) < @Radius
		
		SELECT DISTINCT szc.ZipCode AS ZIPCODE, 
			(CASE ISNUMERIC(CONVERT(DECIMAL(5,2),CONVERT(NUMERIC(10,7),Distance)))
		 	WHEN 1 
			THEN CONVERT(DECIMAL(5,2),CONVERT(NUMERIC(10,7),Distance))
			ELSE '0' END) AS DISTANCE, Lat AS 'LATITUDE', Lon AS 'LONGITUDE'
		FROM #SelectedZipCodes szc JOIN USZipCodeMaster zcm ON szc.ZipCode = zcm.ZipCode
		WHERE CONVERT(NUMERIC(10,7),Distance) >= 0 
		ORDER BY 
			CASE ISNUMERIC(CONVERT(DECIMAL(5,2),CONVERT(NUMERIC(10,7),Distance)))
			WHEN 1 
			THEN CONVERT(DECIMAL(5,2),CONVERT(NUMERIC(10,7),Distance))
			ELSE '0' END ASC, ZipCode
		
		DROP TABLE #SelectedZipCodes

	END
