CREATE PROCEDURE [dbo].[spGetDataImportEngines]
AS
BEGIN	
	SET NOCOUNT ON;
	
	DECLARE @currenttime DATETIME
	SET @currenttime = GETDATE()
	
	SELECT ImporterID, ImportType, ImportFrequency, DATEDIFF(mi,CONVERT(time, importdatetime), CONVERT(time, @currenttime)) as 'DiffTime', 
    CONVERT(time, importdatetime) as 'ImportTime', CONVERT(time, @currenttime) as 'CurrentTime'  
    FROM UDFImportSchedule
	WHERE 
		Active = 'Y' 
		AND DATEDIFF(mi,CONVERT(time, importdatetime), CONVERT(time, @currenttime))  BETWEEN 0 AND 60 
		AND DATEDIFF(hour, Convert(time, importdatetime), CONvert(time, @currenttime))  = 0
		and LEN(ImportFrequency) > 0
		and ImportFrequency = 'Daily'
	UNION
	SELECT ImporterID, ImportType, ImportFrequency, DATEDIFF(mi,CONVERT(time, importdatetime), CONVERT(time, @currenttime)) as 'DiffTime', 
    CONVERT(time, importdatetime) as 'ImportTime', CONVERT(time, @currenttime) as 'CurrentTime'  
    FROM UDFImportSchedule
	WHERE 
		Active = 'Y' 
		AND DATEDIFF(mi,CONVERT(time, importdatetime), CONVERT(time, @currenttime))  BETWEEN 0 AND 60 
		and LEN(ImportFrequency) > 0
		and ImportFrequency = 'Weekly'
		and DATEPART(dw,GETDATE()) = DATEPART(dw,importDateTime)
	UNION
	SELECT ImporterID, ImportType, ImportFrequency, DATEDIFF(mi,CONVERT(time, importdatetime), CONVERT(time, @currenttime)) as 'DiffTime', 
    CONVERT(time, importdatetime) as 'ImportTime', CONVERT(time, @currenttime) as 'CurrentTime'  
    FROM UDFImportSchedule
	WHERE 
		Active = 'Y' 
		AND DATEDIFF(mi,CONVERT(time, importdatetime), CONVERT(time, @currenttime))  BETWEEN 0 AND 60 
		and LEN(ImportFrequency) > 0
		and ImportFrequency = 'Once'
		and CAST(GETDATE() as date) = CAST(ImportDateTime as date)
END