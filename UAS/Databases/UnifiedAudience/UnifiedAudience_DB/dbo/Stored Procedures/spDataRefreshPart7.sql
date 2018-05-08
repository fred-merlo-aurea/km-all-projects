/****************************************
Updates: 
	2014-03-31 MK
	2014 Q1 Release added: Brand, BrandDetails, Config, UserBrand, RecordViewField
	2014-04-16 Added Truncate Brandscore and Update brandscore
	2014-07-01 Added FilterGroup to table list and updated FilterDetails Column list
	2014-07-29 Added Address criteria to Latitude/Longtitude update
	2014-09-10 Changed inserts to use dynamic column generation, exception CampaignFilterDetails.
	2014-09-26 Added Tables FilterPenetrationReportsDetails, FilterPenetrationReports,UserTracking
	2015-02-26 Added Linked server info to support link between DBSERVER1 (251) and CLIENTDBSERVER(148)
			   Also changed Filters and Markets to use views to work around XML linked server incompatibility issue.
	2015-06-17 Added Tables FilterCategory, QuestionCategory, Pricing
****************************************/
CREATE PROC [dbo].[spDataRefreshPart7]
AS
BEGIN

	SET NOCOUNT ON    
	

	declare @CurrentDBName VARCHAR(100),
			@LiveDBName VARCHAR(100),
			@CopySQL VARCHAR(MAX)

	SET @CurrentDBName = DB_NAME()

	IF CHARINDEX('refresh',@CurrentDBName) > 0
		BEGIN
			--SET @LiveDBName = REPLACE(@CurrentDBName, '_Refresh', '')
			SET @LiveDBName = 'DBSERVER1.'+ REPLACE(@CurrentDBName, '_Refresh', '')
		
			SELECT @CurrentDBName, @LiveDBName

			TRUNCATE TABLE FilterPenetrationReportsDetails 
			TRUNCATE TABLE FilterPenetrationReports 
			TRUNCATE TABLE UserTracking

			TRUNCATE TABLE HeatMapLocations
			TRUNCATE TABLE FilterExportField
			DELETE FROM FilterSchedule
				DBCC CHECKIDENT (FilterSchedule, 'reseed', 0)
			TRUNCATE TABLE FilterDetails
			TRUNCATE TABLE FilterGroup
			TRUNCATE TABLE FilterCategory
			TRUNCATE TABLE QuestionCategory
			DELETE FROM Filters
				DBCC CHECKIDENT (Filters, 'reseed', 0)
			TRUNCATE TABLE CampaignFilterDetails
			DELETE FROM CampaignFilter
				DBCC CHECKIDENT (CampaignFilter, 'reseed', 0)
			DELETE FROM Campaigns
				DBCC CHECKIDENT (Campaigns, 'reseed', 0)
			TRUNCATE TABLE PenetrationReports_Markets
			DELETE FROM PenetrationReports
				DBCC CHECKIDENT (PenetrationReports, 'reseed', 0)
			TRUNCATE TABLE MasterMarkets
			TRUNCATE TABLE PubMarkets
			DELETE FROM Markets
					DBCC CHECKIDENT (Markets, 'reseed', 0)
			TRUNCATE TABLE Users
			TRUNCATE TABLE BrandScore
			TRUNCATE TABLE UserBrand
			TRUNCATE TABLE BrandDetails
			DELETE FROM Pricing
				DBCC CHECKIDENT (Pricing, 'reseed', 0)
			DELETE FROM Brand
				DBCC CHECKIDENT (Brand, 'reseed', 0)
			TRUNCATE TABLE Config
			TRUNCATE TABLE RecordViewField
		

			SELECT @CopySQL =
			'		INSERT INTO Users (' + 
			STUFF((SELECT ', ' + COLUMN_NAME [text()] FROM (SELECT '['+COLUMN_NAME+']' AS COLUMN_NAME FROM INFORMATION_SCHEMA.COLUMNS WHERE TABLE_NAME = 'Users') x For XML PATH ('')),1,1,'') +') 
			SELECT '+ STUFF((SELECT ', ' + COLUMN_NAME [text()] FROM (SELECT CASE WHEN Data_Type = 'XML' THEN 'CAST(['+COLUMN_NAME +'] AS NVARCHAR(MAX)) ['+ COLUMN_NAME +']' ELSE '['+COLUMN_NAME+']' END AS COLUMN_NAME FROM INFORMATION_SCHEMA.COLUMNS WHERE TABLE_NAME = 'Users') z For XML PATH ('')),1,1,'') + ' FROM '+ @LiveDBName + '.dbo.Users '

			+ '
		
			SET IDENTITY_INSERT Brand ON
			' +
			'INSERT INTO Brand (' +
			STUFF((SELECT ', ' + COLUMN_NAME [text()] FROM  (SELECT '['+COLUMN_NAME+']' AS COLUMN_NAME FROM INFORMATION_SCHEMA.COLUMNS WHERE TABLE_NAME = 'Brand') x For XML PATH ('')),1,1,'') +') 
			SELECT '+ STUFF((SELECT ', ' + COLUMN_NAME [text()] FROM (SELECT CASE WHEN Data_Type = 'XML' THEN 'CAST(['+COLUMN_NAME +'] AS NVARCHAR(MAX)) ['+ COLUMN_NAME +']' ELSE '['+COLUMN_NAME+']' END AS COLUMN_NAME FROM INFORMATION_SCHEMA.COLUMNS WHERE TABLE_NAME = 'Brand') z For XML PATH ('')),1,1,'') + ' FROM '+ @LiveDBName + '.dbo.Brand '
			+ '
			SET IDENTITY_INSERT Brand OFF' +

			'
			SET IDENTITY_INSERT Pricing ON
			' +
			'INSERT INTO Pricing (' +
			STUFF((SELECT ', ' + COLUMN_NAME [text()] FROM  (SELECT '['+COLUMN_NAME+']' AS COLUMN_NAME FROM INFORMATION_SCHEMA.COLUMNS WHERE TABLE_NAME = 'Pricing') x For XML PATH ('')),1,1,'') +') 
			SELECT '+ STUFF((SELECT ', ' + COLUMN_NAME [text()] FROM (SELECT CASE WHEN Data_Type = 'XML' THEN 'CAST(['+COLUMN_NAME +'] AS NVARCHAR(MAX)) ['+ COLUMN_NAME +']' ELSE '['+COLUMN_NAME+']' END AS COLUMN_NAME FROM INFORMATION_SCHEMA.COLUMNS WHERE TABLE_NAME = 'Pricing') z For XML PATH ('')),1,1,'') + ' FROM '+ @LiveDBName + '.dbo.Pricing '
			+ '
			SET IDENTITY_INSERT Pricing OFF' 

			+ ' 
			SET IDENTITY_INSERT Markets ON 
			' +
			'INSERT INTO Markets (' +
			STUFF((SELECT ', ' + COLUMN_NAME [text()] FROM  (SELECT '['+COLUMN_NAME+']' AS COLUMN_NAME FROM INFORMATION_SCHEMA.COLUMNS WHERE TABLE_NAME = 'Markets') x For XML PATH ('')),1,1,'') +') 
			SELECT '+ STUFF((SELECT ', ' + COLUMN_NAME [text()] FROM (SELECT CASE WHEN Data_Type = 'XML' THEN 'CAST(['+COLUMN_NAME +'] AS NVARCHAR(MAX)) ['+ COLUMN_NAME +']' ELSE '['+COLUMN_NAME+']' END AS COLUMN_NAME FROM INFORMATION_SCHEMA.COLUMNS WHERE TABLE_NAME = 'Markets') z For XML PATH ('')),1,1,'') + ' FROM '+ @LiveDBName + '.dbo.vw_Markets '
			+ '
			SET IDENTITY_INSERT Markets OFF	' +
			
			+ 
			'
		
			INSERT INTO MasterMarkets (' +
			STUFF((SELECT ', ' + COLUMN_NAME [text()] FROM  (SELECT '['+COLUMN_NAME+']' AS COLUMN_NAME FROM INFORMATION_SCHEMA.COLUMNS WHERE TABLE_NAME = 'MasterMarkets') x For XML PATH ('')),1,1,'') +') 
			SELECT '+ STUFF((SELECT ', ' + COLUMN_NAME [text()] FROM (SELECT CASE WHEN Data_Type = 'XML' THEN 'CAST(['+COLUMN_NAME +'] AS NVARCHAR(MAX)) ['+ COLUMN_NAME +']' ELSE '['+COLUMN_NAME+']' END AS COLUMN_NAME FROM INFORMATION_SCHEMA.COLUMNS WHERE TABLE_NAME = 'MasterMarkets') z For XML PATH ('')),1,1,'') + ' FROM '+ @LiveDBName + '.dbo.MasterMarkets '
		
			+

			'
		
			INSERT INTO PubMarkets (' +
			STUFF((SELECT ', ' + COLUMN_NAME [text()] FROM  (SELECT '['+COLUMN_NAME+']' AS COLUMN_NAME FROM INFORMATION_SCHEMA.COLUMNS WHERE TABLE_NAME = 'PubMarkets') x For XML PATH ('')),1,1,'') +') 
			SELECT '+ STUFF((SELECT ', ' + COLUMN_NAME [text()] FROM (SELECT CASE WHEN Data_Type = 'XML' THEN 'CAST(['+COLUMN_NAME +'] AS NVARCHAR(MAX)) ['+ COLUMN_NAME +']' ELSE '['+COLUMN_NAME+']' END AS COLUMN_NAME FROM INFORMATION_SCHEMA.COLUMNS WHERE TABLE_NAME = 'PubMarkets') z For XML PATH ('')),1,1,'') + ' FROM '+ @LiveDBName + '.dbo.PubMarkets '

			+ '
		
			SET IDENTITY_INSERT PenetrationReports ON
			' +
			'INSERT INTO PenetrationReports (' +
			STUFF((SELECT ', ' + COLUMN_NAME [text()] FROM  (SELECT '['+COLUMN_NAME+']' AS COLUMN_NAME FROM INFORMATION_SCHEMA.COLUMNS WHERE TABLE_NAME = 'PenetrationReports') x For XML PATH ('')),1,1,'') +') 
			SELECT '+ STUFF((SELECT ', ' + COLUMN_NAME [text()] FROM (SELECT CASE WHEN Data_Type = 'XML' THEN 'CAST(['+COLUMN_NAME +'] AS NVARCHAR(MAX)) ['+ COLUMN_NAME +']' ELSE '['+COLUMN_NAME+']' END AS COLUMN_NAME FROM INFORMATION_SCHEMA.COLUMNS WHERE TABLE_NAME = 'PenetrationReports') z For XML PATH ('')),1,1,'') + ' FROM '+ @LiveDBName + '.dbo.PenetrationReports '
			+ '
			SET IDENTITY_INSERT PenetrationReports OFF ' +

			+ '
		
			SET IDENTITY_INSERT PenetrationReports_Markets ON
			' +
			'INSERT INTO PenetrationReports_Markets (' +
			STUFF((SELECT ', ' + COLUMN_NAME [text()] FROM  (SELECT '['+COLUMN_NAME+']' AS COLUMN_NAME FROM INFORMATION_SCHEMA.COLUMNS WHERE TABLE_NAME = 'PenetrationReports_Markets') x For XML PATH ('')),1,1,'') +') 
			SELECT '+ STUFF((SELECT ', ' + COLUMN_NAME [text()] FROM (SELECT CASE WHEN Data_Type = 'XML' THEN 'CAST(['+COLUMN_NAME +'] AS NVARCHAR(MAX)) ['+ COLUMN_NAME +']' ELSE '['+COLUMN_NAME+']' END AS COLUMN_NAME FROM INFORMATION_SCHEMA.COLUMNS WHERE TABLE_NAME = 'PenetrationReports_Markets') z For XML PATH ('')),1,1,'') + ' FROM '+ @LiveDBName + '.dbo.PenetrationReports_Markets '
			+ '
			SET IDENTITY_INSERT PenetrationReports_Markets OFF' +

			+ '
		
			SET IDENTITY_INSERT Campaigns ON
			' +
			'INSERT INTO Campaigns (' +
			STUFF((SELECT ', ' + COLUMN_NAME [text()] FROM  (SELECT '['+COLUMN_NAME+']' AS COLUMN_NAME FROM INFORMATION_SCHEMA.COLUMNS WHERE TABLE_NAME = 'Campaigns') x For XML PATH ('')),1,1,'') +') 
			SELECT '+ STUFF((SELECT ', ' + COLUMN_NAME [text()] FROM (SELECT CASE WHEN Data_Type = 'XML' THEN 'CAST(['+COLUMN_NAME +'] AS NVARCHAR(MAX)) ['+ COLUMN_NAME +']' ELSE '['+COLUMN_NAME+']' END AS COLUMN_NAME FROM INFORMATION_SCHEMA.COLUMNS WHERE TABLE_NAME = 'Campaigns') z For XML PATH ('')),1,1,'') + ' FROM '+ @LiveDBName + '.dbo.Campaigns '
			+ '
			SET IDENTITY_INSERT Campaigns OFF' +

			+ '
		
			SET IDENTITY_INSERT CampaignFilter ON
			' +
			'INSERT INTO CampaignFilter (' +
			STUFF((SELECT ', ' + COLUMN_NAME [text()] FROM  (SELECT '['+COLUMN_NAME+']' AS COLUMN_NAME FROM INFORMATION_SCHEMA.COLUMNS WHERE TABLE_NAME = 'CampaignFilter') x For XML PATH ('')),1,1,'') +') 
			SELECT '+ STUFF((SELECT ', ' + COLUMN_NAME [text()] FROM (SELECT CASE WHEN Data_Type = 'XML' THEN 'CAST(['+COLUMN_NAME +'] AS NVARCHAR(MAX)) ['+ COLUMN_NAME +']' ELSE '['+COLUMN_NAME+']' END AS COLUMN_NAME FROM INFORMATION_SCHEMA.COLUMNS WHERE TABLE_NAME = 'CampaignFilter') z For XML PATH ('')),1,1,'') + ' FROM '+ @LiveDBName + '.dbo.CampaignFilter '
			+ '
			SET IDENTITY_INSERT CampaignFilter OFF' +

			+ '
		
			SET IDENTITY_INSERT CampaignFilterDetails ON
			' +
			'INSERT INTO CampaignFilterDetails (CampaignFilterDetailsID, CampaignFilterID, SubscriptionID)
				SELECT cd.CampaignFilterDetailsID, cd.campaignfilterID, s1.SubscriptionID 
				FROM 
					' + @LiveDBName + '.dbo.campaignfilterdetails cd 
					JOIN ' + @LiveDBName + '.dbo.Subscriptions s on cd.subscriptionID = s.subscriptionID
					JOIN Subscriptions s1 on s1.IGRP_NO = s.igrp_no '

			+ '
			SET IDENTITY_INSERT CampaignFilterDetails OFF' +

			+ '
		
			SET IDENTITY_INSERT FilterCategory ON
			' +
			'INSERT INTO FilterCategory (' +
			STUFF((SELECT ', ' + COLUMN_NAME [text()] FROM  (SELECT '['+COLUMN_NAME+']' AS COLUMN_NAME FROM INFORMATION_SCHEMA.COLUMNS WHERE TABLE_NAME = 'FilterCategory') x For XML PATH ('')),1,1,'') +') 
			SELECT '+ STUFF((SELECT ', ' + COLUMN_NAME [text()] FROM (SELECT CASE WHEN Data_Type = 'XML' THEN 'CAST(['+COLUMN_NAME +'] AS NVARCHAR(MAX)) ['+ COLUMN_NAME +']' ELSE '['+COLUMN_NAME+']' END AS COLUMN_NAME FROM INFORMATION_SCHEMA.COLUMNS WHERE TABLE_NAME = 'FilterCategory') z For XML PATH ('')),1,1,'') + ' FROM '+ @LiveDBName + '.dbo.FilterCategory '
			+ '
			SET IDENTITY_INSERT FilterCategory OFF'

			+ '

			SET IDENTITY_INSERT QuestionCategory ON
			' +
			'INSERT INTO QuestionCategory (' +
			STUFF((SELECT ', ' + COLUMN_NAME [text()] FROM  (SELECT '['+COLUMN_NAME+']' AS COLUMN_NAME FROM INFORMATION_SCHEMA.COLUMNS WHERE TABLE_NAME = 'QuestionCategory') x For XML PATH ('')),1,1,'') +') 
			SELECT '+ STUFF((SELECT ', ' + COLUMN_NAME [text()] FROM (SELECT CASE WHEN Data_Type = 'XML' THEN 'CAST(['+COLUMN_NAME +'] AS NVARCHAR(MAX)) ['+ COLUMN_NAME +']' ELSE '['+COLUMN_NAME+']' END AS COLUMN_NAME FROM INFORMATION_SCHEMA.COLUMNS WHERE TABLE_NAME = 'QuestionCategory') z For XML PATH ('')),1,1,'') + ' FROM '+ @LiveDBName + '.dbo.QuestionCategory '
			+ '
			SET IDENTITY_INSERT QuestionCategory OFF' 
		
			+ '
		
			SET IDENTITY_INSERT Filters ON
			' +
			'INSERT INTO Filters (' +
			STUFF((SELECT ', ' + COLUMN_NAME [text()] FROM  (SELECT '['+COLUMN_NAME+']' AS COLUMN_NAME FROM INFORMATION_SCHEMA.COLUMNS WHERE TABLE_NAME = 'Filters') x For XML PATH ('')),1,1,'') +') 
			SELECT '+ STUFF((SELECT ', ' + COLUMN_NAME [text()] FROM (SELECT CASE WHEN Data_Type = 'XML' THEN 'CAST(['+COLUMN_NAME +'] AS NVARCHAR(MAX)) ['+ COLUMN_NAME +']' ELSE '['+COLUMN_NAME+']' END AS COLUMN_NAME FROM INFORMATION_SCHEMA.COLUMNS WHERE TABLE_NAME = 'Filters') z For XML PATH ('')),1,1,'') + ' FROM '+ @LiveDBName + '.dbo.VW_Filters '
			+ '
			SET IDENTITY_INSERT Filters OFF' +

			+ '
		
			SET IDENTITY_INSERT FilterGroup ON
			' +
			'INSERT INTO FilterGroup (' +
			STUFF((SELECT ', ' + COLUMN_NAME [text()] FROM  (SELECT '['+COLUMN_NAME+']' AS COLUMN_NAME FROM INFORMATION_SCHEMA.COLUMNS WHERE TABLE_NAME = 'FilterGroup') x For XML PATH ('')),1,1,'') +') 
			SELECT '+ STUFF((SELECT ', ' + COLUMN_NAME [text()] FROM (SELECT CASE WHEN Data_Type = 'XML' THEN 'CAST(['+COLUMN_NAME +'] AS NVARCHAR(MAX)) ['+ COLUMN_NAME +']' ELSE '['+COLUMN_NAME+']' END AS COLUMN_NAME FROM INFORMATION_SCHEMA.COLUMNS WHERE TABLE_NAME = 'FilterGroup') z For XML PATH ('')),1,1,'') + ' FROM '+ @LiveDBName + '.dbo.FilterGroup '
			+ '
			SET IDENTITY_INSERT FilterGroup OFF' +
		
			+ '
		
			SET IDENTITY_INSERT FilterDetails ON
			' +
			'INSERT INTO FilterDetails (' +
			STUFF((SELECT ', ' + COLUMN_NAME [text()] FROM  (SELECT '['+COLUMN_NAME+']' AS COLUMN_NAME FROM INFORMATION_SCHEMA.COLUMNS WHERE TABLE_NAME = 'FilterDetails') x For XML PATH ('')),1,1,'') +') 
			SELECT '+ STUFF((SELECT ', ' + COLUMN_NAME [text()] FROM (SELECT CASE WHEN Data_Type = 'XML' THEN 'CAST(['+COLUMN_NAME +'] AS NVARCHAR(MAX)) ['+ COLUMN_NAME +']' ELSE '['+COLUMN_NAME+']' END AS COLUMN_NAME FROM INFORMATION_SCHEMA.COLUMNS WHERE TABLE_NAME = 'FilterDetails') z For XML PATH ('')),1,1,'') + ' FROM '+ @LiveDBName + '.dbo.FilterDetails '
			+ '
			SET IDENTITY_INSERT FilterDetails OFF' +

			+ '
		
			SET IDENTITY_INSERT FilterSchedule ON
			' +
			'INSERT INTO FilterSchedule (' +
			STUFF((SELECT ', ' + COLUMN_NAME [text()] FROM  (SELECT '['+COLUMN_NAME+']' AS COLUMN_NAME FROM INFORMATION_SCHEMA.COLUMNS WHERE TABLE_NAME = 'FilterSchedule') x For XML PATH ('')),1,1,'') +') 
			SELECT '+ STUFF((SELECT ', ' + COLUMN_NAME [text()] FROM (SELECT CASE WHEN Data_Type = 'XML' THEN 'CAST(['+COLUMN_NAME +'] AS NVARCHAR(MAX)) ['+ COLUMN_NAME +']' ELSE '['+COLUMN_NAME+']' END AS COLUMN_NAME FROM INFORMATION_SCHEMA.COLUMNS WHERE TABLE_NAME = 'FilterSchedule') z For XML PATH ('')),1,1,'') + ' FROM '+ @LiveDBName + '.dbo.FilterSchedule '
			+ '
			SET IDENTITY_INSERT FilterSchedule OFF' +
		
			+ '
		
			SET IDENTITY_INSERT FilterExportField ON
			' +
			'INSERT INTO FilterExportField (' +
			STUFF((SELECT ', ' + COLUMN_NAME [text()] FROM  (SELECT '['+COLUMN_NAME+']' AS COLUMN_NAME FROM INFORMATION_SCHEMA.COLUMNS WHERE TABLE_NAME = 'FilterExportField') x For XML PATH ('')),1,1,'') +') 
			SELECT '+ STUFF((SELECT ', ' + COLUMN_NAME [text()] FROM (SELECT CASE WHEN Data_Type = 'XML' THEN 'CAST(['+COLUMN_NAME +'] AS NVARCHAR(MAX)) ['+ COLUMN_NAME +']' ELSE '['+COLUMN_NAME+']' END AS COLUMN_NAME FROM INFORMATION_SCHEMA.COLUMNS WHERE TABLE_NAME = 'FilterExportField') z For XML PATH ('')),1,1,'') + ' FROM '+ @LiveDBName + '.dbo.FilterExportField '
			+ '
			SET IDENTITY_INSERT FilterExportField OFF' +
		
			+ '
		
			SET IDENTITY_INSERT HeatMapLocations ON
			' +
			'INSERT INTO HeatMapLocations (' +
			STUFF((SELECT ', ' + COLUMN_NAME [text()] FROM  (SELECT '['+COLUMN_NAME+']' AS COLUMN_NAME FROM INFORMATION_SCHEMA.COLUMNS WHERE TABLE_NAME = 'HeatMapLocations') x For XML PATH ('')),1,1,'') +') 
			SELECT '+ STUFF((SELECT ', ' + COLUMN_NAME [text()] FROM (SELECT CASE WHEN Data_Type = 'XML' THEN 'CAST(['+COLUMN_NAME +'] AS NVARCHAR(MAX)) ['+ COLUMN_NAME +']' ELSE '['+COLUMN_NAME+']' END AS COLUMN_NAME FROM INFORMATION_SCHEMA.COLUMNS WHERE TABLE_NAME = 'HeatMapLocations') z For XML PATH ('')),1,1,'') + ' FROM '+ @LiveDBName + '.dbo.HeatMapLocations '
			+ '
			SET IDENTITY_INSERT HeatMapLocations OFF' +

			+ '
		
			SET IDENTITY_INSERT BrandDetails ON
			' +
			'INSERT INTO BrandDetails (' +
			STUFF((SELECT ', ' + COLUMN_NAME [text()] FROM  (SELECT '['+COLUMN_NAME+']' AS COLUMN_NAME FROM INFORMATION_SCHEMA.COLUMNS WHERE TABLE_NAME = 'BrandDetails') x For XML PATH ('')),1,1,'') +') 
			SELECT '+ STUFF((SELECT ', ' + COLUMN_NAME [text()] FROM (SELECT CASE WHEN Data_Type = 'XML' THEN 'CAST(['+COLUMN_NAME +'] AS NVARCHAR(MAX)) ['+ COLUMN_NAME +']' ELSE '['+COLUMN_NAME+']' END AS COLUMN_NAME FROM INFORMATION_SCHEMA.COLUMNS WHERE TABLE_NAME = 'BrandDetails') z For XML PATH ('')),1,1,'') + ' FROM '+ @LiveDBName + '.dbo.BrandDetails '
			+ '
			SET IDENTITY_INSERT BrandDetails OFF' +

			'
		
			INSERT INTO UserBrand (' +
			STUFF((SELECT ', ' + COLUMN_NAME [text()] FROM  (SELECT '['+COLUMN_NAME+']' AS COLUMN_NAME FROM INFORMATION_SCHEMA.COLUMNS WHERE TABLE_NAME = 'UserBrand') x For XML PATH ('')),1,1,'') +') 
			SELECT '+ STUFF((SELECT ', ' + COLUMN_NAME [text()] FROM (SELECT CASE WHEN Data_Type = 'XML' THEN 'CAST(['+COLUMN_NAME +'] AS NVARCHAR(MAX)) ['+ COLUMN_NAME +']' ELSE '['+COLUMN_NAME+']' END AS COLUMN_NAME FROM INFORMATION_SCHEMA.COLUMNS WHERE TABLE_NAME = 'UserBrand') z For XML PATH ('')),1,1,'') + ' FROM '+ @LiveDBName + '.dbo.UserBrand '
		
			+ '
		
			SET IDENTITY_INSERT Config ON
			' +
			'INSERT INTO Config (' +
			STUFF((SELECT ', ' + COLUMN_NAME [text()] FROM  (SELECT '['+COLUMN_NAME+']' AS COLUMN_NAME FROM INFORMATION_SCHEMA.COLUMNS WHERE TABLE_NAME = 'Config') x For XML PATH ('')),1,1,'') +') 
			SELECT '+ STUFF((SELECT ', ' + COLUMN_NAME [text()] FROM (SELECT CASE WHEN Data_Type = 'XML' THEN 'CAST(['+COLUMN_NAME +'] AS NVARCHAR(MAX)) ['+ COLUMN_NAME +']' ELSE '['+COLUMN_NAME+']' END AS COLUMN_NAME FROM INFORMATION_SCHEMA.COLUMNS WHERE TABLE_NAME = 'Config') z For XML PATH ('')),1,1,'') + ' FROM '+ @LiveDBName + '.dbo.Config '
			+ '
			SET IDENTITY_INSERT Config OFF' +

			 '
		
			SET IDENTITY_INSERT RecordViewField ON
			' +
			'INSERT INTO RecordViewField (' +
			STUFF((SELECT ', ' + COLUMN_NAME [text()] FROM  (SELECT '['+COLUMN_NAME+']' AS COLUMN_NAME FROM INFORMATION_SCHEMA.COLUMNS WHERE TABLE_NAME = 'RecordViewField') x For XML PATH ('')),1,1,'') +') 
			SELECT '+ STUFF((SELECT ', ' + COLUMN_NAME [text()] FROM (SELECT CASE WHEN Data_Type = 'XML' THEN 'CAST(['+COLUMN_NAME +'] AS NVARCHAR(MAX)) ['+ COLUMN_NAME +']' ELSE '['+COLUMN_NAME+']' END AS COLUMN_NAME FROM INFORMATION_SCHEMA.COLUMNS WHERE TABLE_NAME = 'RecordViewField') z For XML PATH ('')),1,1,'') + ' FROM '+ @LiveDBName + '.dbo.RecordViewField '
			+ '
			SET IDENTITY_INSERT RecordViewField OFF' +

			'
		
			SET IDENTITY_INSERT FilterPenetrationReports ON
			' +
			'INSERT INTO FilterPenetrationReports (' +
			STUFF((SELECT ', ' + COLUMN_NAME [text()] FROM  (SELECT '['+COLUMN_NAME+']' AS COLUMN_NAME FROM INFORMATION_SCHEMA.COLUMNS WHERE TABLE_NAME = 'FilterPenetrationReports') x For XML PATH ('')),1,1,'') +') 
			SELECT '+ STUFF((SELECT ', ' + COLUMN_NAME [text()] FROM (SELECT CASE WHEN Data_Type = 'XML' THEN 'CAST(['+COLUMN_NAME +'] AS NVARCHAR(MAX)) ['+ COLUMN_NAME +']' ELSE '['+COLUMN_NAME+']' END AS COLUMN_NAME FROM INFORMATION_SCHEMA.COLUMNS WHERE TABLE_NAME = 'FilterPenetrationReports') z For XML PATH ('')),1,1,'') + ' FROM '+ @LiveDBName + '.dbo.FilterPenetrationReports '
			+ '
			SET IDENTITY_INSERT FilterPenetrationReports OFF' +


			'
		
			SET IDENTITY_INSERT FilterPenetrationReportsDetails ON
			' +
			'INSERT INTO FilterPenetrationReportsDetails (' +
			STUFF((SELECT ', ' + COLUMN_NAME [text()] FROM  (SELECT '['+COLUMN_NAME+']' AS COLUMN_NAME FROM INFORMATION_SCHEMA.COLUMNS WHERE TABLE_NAME = 'FilterPenetrationReportsDetails') x For XML PATH ('')),1,1,'') +') 
			SELECT '+ STUFF((SELECT ', ' + COLUMN_NAME [text()] FROM (SELECT CASE WHEN Data_Type = 'XML' THEN 'CAST(['+COLUMN_NAME +'] AS NVARCHAR(MAX)) ['+ COLUMN_NAME +']' ELSE '['+COLUMN_NAME+']' END AS COLUMN_NAME FROM INFORMATION_SCHEMA.COLUMNS WHERE TABLE_NAME = 'FilterPenetrationReportsDetails') z For XML PATH ('')),1,1,'') + ' FROM '+ @LiveDBName + '.dbo.FilterPenetrationReportsDetails '
			+ '
			SET IDENTITY_INSERT FilterPenetrationReportsDetails OFF' +

		'
		
			SET IDENTITY_INSERT UserTracking ON
			' +
			'INSERT INTO UserTracking (' +
			STUFF((SELECT ', ' + COLUMN_NAME [text()] FROM  (SELECT '['+COLUMN_NAME+']' AS COLUMN_NAME FROM INFORMATION_SCHEMA.COLUMNS WHERE TABLE_NAME = 'UserTracking') x For XML PATH ('')),1,1,'') +') 
			SELECT '+ STUFF((SELECT ', ' + COLUMN_NAME [text()] FROM (SELECT CASE WHEN Data_Type = 'XML' THEN 'CAST(['+COLUMN_NAME +'] AS NVARCHAR(MAX)) ['+ COLUMN_NAME +']' ELSE '['+COLUMN_NAME+']' END AS COLUMN_NAME FROM INFORMATION_SCHEMA.COLUMNS WHERE TABLE_NAME = 'UserTracking') z For XML PATH ('')),1,1,'') + ' FROM '+ @LiveDBName + '.dbo.UserTracking '
			+ '
			SET IDENTITY_INSERT UserTracking OFF' +

			'
		
			--UPDATE ' + @LiveDBName + '_Refresh.dbo.Subscriptions
			UPDATE ' + @CurrentDBName + '.dbo.Subscriptions
			SET Latitude = s1.Latitude, Longitude = s1.Longitude, IsLatLonValid = s1.IsLatLonValid, LatLonMsg = s1.LatLonMsg
			FROM Subscriptions s2
				INNER JOIN ' + @LiveDBName + '.dbo.Subscriptions s1 ON s2.IGRP_NO = s1.IGRP_NO 
				AND ISNULL(s2.Address,'''')= ISNULL(s1.Address,'''') 
				AND ISNULL(s1.state,'''') = ISNULL(s2.state,'''') 
				AND ISNULL(S1.ZIP,'''') = ISNULL(S2.ZIP,'''')  '
			
			--PRINT @CopySQL

			EXEC (@CopySQL)


			EXEC SpUpdateBrandScore
	
	
			IF CHARINDEX('Stamats',@CurrentDBName) = 0			
			UPDATE Pubs SET Score = 1 WHERE ISNULL(Score,0) = 0
		
		
			PRINT 'spDataRefreshPart7 execution is done'
		END
	ELSE
		BEGIN
			PRINT 'Cannot Run DataRefresh in LIVE Database'
		END
END
GO