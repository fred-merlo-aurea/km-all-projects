CREATE PROCEDURE [dbo].[ccp_NASFT_SUM_TOTS]
AS
BEGIN

	set nocount on

	/** NOTES **/
	-----
	---Process 1-5 will attach to record and if there are duplicates for one person
	---EXAMPLE: Steve Jobs PUBCODE: "CCE12" IGRP_NO: "D32K..." has 2 records with TOTALSPENT’s of 50 and 100
	---They will be summed to 150 and attached to both then when the duplicates are 
	---removed the master? record will be left with a TOTALSPENT of 150
	-----
	-----
	---Process 6 will attach to the “Master Record” for that specific IGRP_NO
	---EXAMPLE: IGRP_NO: "D32K..." has 4 records. TOTALSPENT's of 50, 75, 100, 25 sum to 250 and
	---attached in the TOTSPALL Field to the Master Record.
	-----

	/** Process 1: TOTALSPENT - IGRP_NO & Pubcode **/
	--IF EXISTS (SELECT * FROM tempdb.dbo.sysobjects o WHERE o.xtype in ('U') and o.id = object_id('tempdb..#tmp_TotalSpent'))
	--BEGIN
	--	DROP TABLE #tmp_TotalSpent;
	--END

	--SELECT SUM(CAST(Value AS Decimal)) AS SUMS, SF.PubCode, SF.IGrp_No INTO #tmp_TotalSpent
	--FROM SubscriberTransformed SF With(NoLock)
	--JOIN SubscriberDemographicTransformed SDF With(NoLock)
	--ON SF.SORecordIdentifier = SDF.SORecordIdentifier
	--WHERE SDF.MAFField = 'TOTALSPENT'
	--GROUP BY PubCode, IGrp_No

	--Update SubscriberDemographicTransformed
	--SET Value = t.SUMS
	--FROM SubscriberTransformed sf
	--JOIN #tmp_TotalSpent t ON sf.IGrp_No = t.IGrp_No and sf.PubCode = t.PubCode
	--JOIN SubscriberDemographicTransformed sdf ON sf.SORecordIdentifier = sdf.SORecordIdentifier
	--WHERE sf.IGrp_No = t.IGrp_No and sf.PubCode = t.PubCode AND sdf.MAFField = 'TOTALSPENT'

	--IF EXISTS (SELECT * FROM tempdb.dbo.sysobjects s WHERE s.xtype in ('U') and s.id = object_id('tempdb..#tmp_TotalSpent'))
	--BEGIN
	--	DROP TABLE #tmp_TotalSpent;
	--END



	--/** Process 2: BOOTHS - IGRP_NO & Pubcode **/
	--IF EXISTS (SELECT * FROM tempdb.dbo.sysobjects o WHERE o.xtype in ('U') and o.id = object_id('tempdb..#tmp_Booths'))
	--BEGIN
	--	DROP TABLE #tmp_Booths;
	--END

	--SELECT SUM(CAST(Value AS Decimal)) AS SUMS, SF.PubCode, SF.IGrp_No INTO #tmp_Booths
	--FROM SubscriberTransformed SF With(NoLock)
	--JOIN SubscriberDemographicTransformed SDF With(NoLock)
	--ON SF.SORecordIdentifier = SDF.SORecordIdentifier
	--WHERE SDF.MAFField = 'BOOTHS'
	--GROUP BY PubCode, IGrp_No

	--Update SubscriberDemographicTransformed
	--SET Value = t.SUMS
	--FROM SubscriberTransformed sf
	--JOIN #tmp_Booths t ON sf.IGrp_No = t.IGrp_No and sf.PubCode = t.PubCode
	--JOIN SubscriberDemographicTransformed sdf ON sf.SORecordIdentifier = sdf.SORecordIdentifier
	--WHERE sf.IGrp_No = t.IGrp_No and sf.PubCode = t.PubCode AND sdf.MAFField = 'BOOTHS'

	--IF EXISTS (SELECT * FROM tempdb.dbo.sysobjects o WHERE o.xtype in ('U') and o.id = object_id('tempdb..#tmp_Booths'))
	--BEGIN
	--	DROP TABLE #tmp_Booths;
	--END



	--/** Process 3: TOTWB - IGRP_NO & Pubcode **/
	--IF EXISTS (SELECT * FROM tempdb.dbo.sysobjects o WHERE o.xtype in ('U') and o.id = object_id('tempdb..#tmp_Totwb'))
	--BEGIN
	--	DROP TABLE #tmp_Totwb;
	--END

	--SELECT SUM(CAST(Value AS Decimal)) AS SUMS, SF.PubCode, SF.IGrp_No INTO #tmp_Totwb
	--FROM SubscriberTransformed SF With(NoLock)
	--JOIN SubscriberDemographicTransformed SDF With(NoLock)
	--ON SF.SORecordIdentifier = SDF.SORecordIdentifier
	--WHERE SDF.MAFField = 'TOTWB'
	--GROUP BY PubCode, IGrp_No

	--Update SubscriberDemographicTransformed
	--SET Value = t.SUMS
	--FROM SubscriberTransformed sf
	--JOIN #tmp_Totwb t ON sf.IGrp_No = t.IGrp_No and sf.PubCode = t.PubCode
	--JOIN SubscriberDemographicTransformed sdf ON sf.SORecordIdentifier = sdf.SORecordIdentifier
	--WHERE sf.IGrp_No = t.IGrp_No and sf.PubCode = t.PubCode AND sdf.MAFField = 'TOTWB'

	--IF EXISTS (SELECT * FROM tempdb.dbo.sysobjects o WHERE o.xtype in ('U') and o.id = object_id('tempdb..#tmp_Totwb'))
	--BEGIN
	--	DROP TABLE #tmp_Totwb;
	--END



	--/** Process 4: TOTWP - IGRP_NO & Pubcode **/
	--IF EXISTS (SELECT * FROM tempdb.dbo.sysobjects o WHERE o.xtype in ('U') and o.id = object_id('tempdb..#tmp_Totwp'))
	--BEGIN
	--	DROP TABLE #tmp_Totwp;
	--END

	--SELECT SUM(CAST(Value AS Decimal)) AS SUMS, SF.PubCode, SF.IGrp_No INTO #tmp_Totwp
	--FROM SubscriberTransformed SF With(NoLock)
	--JOIN SubscriberDemographicTransformed SDF With(NoLock)
	--ON SF.SORecordIdentifier = SDF.SORecordIdentifier
	--WHERE SDF.MAFField = 'TOTWP'
	--GROUP BY PubCode, IGrp_No

	--Update SubscriberDemographicTransformed
	--SET Value = t.SUMS
	--FROM SubscriberTransformed sf
	--JOIN #tmp_Totwp t ON sf.IGrp_No = t.IGrp_No and sf.PubCode = t.PubCode
	--JOIN SubscriberDemographicTransformed sdf ON sf.SORecordIdentifier = sdf.SORecordIdentifier
	--WHERE sf.IGrp_No = t.IGrp_No and sf.PubCode = t.PubCode AND sf.ClientID = 15 AND sdf.MAFField = 'TOTWP'

	--IF EXISTS (SELECT * FROM tempdb.dbo.sysobjects o WHERE o.xtype in ('U') and o.id = object_id('tempdb..#tmp_Totwp'))
	--BEGIN
	--	DROP TABLE #tmp_Totwp;
	--END



	--/** Process 5: RS - IGRP_NO & Pubcode **/
	--IF EXISTS (SELECT * FROM tempdb.dbo.sysobjects o WHERE o.xtype in ('U') and o.id = object_id('tempdb..#tmp_rs'))
	--BEGIN
	--	DROP TABLE #tmp_rs;
	--END

	--SELECT SUM(CAST(Value AS Decimal)) AS SUMS, SF.PubCode, SF.IGrp_No INTO #tmp_rs
	--FROM SubscriberTransformed SF With(NoLock)
	--JOIN SubscriberDemographicTransformed SDF With(NoLock)
	--ON SF.SORecordIdentifier = SDF.SORecordIdentifier
	--WHERE SDF.MAFField = 'RS'
	--GROUP BY PubCode, IGrp_No

	--Update SubscriberDemographicTransformed
	--SET Value = t.SUMS
	--FROM SubscriberTransformed sf
	--JOIN #tmp_rs t ON sf.IGrp_No = t.IGrp_No and sf.PubCode = t.PubCode
	--JOIN SubscriberDemographicTransformed sdf ON sf.SORecordIdentifier = sdf.SORecordIdentifier
	--WHERE sf.IGrp_No = t.IGrp_No and sf.PubCode = t.PubCode AND sdf.MAFField = 'RS'

	--IF EXISTS (SELECT * FROM tempdb.dbo.sysobjects o WHERE o.xtype in ('U') and o.id = object_id('tempdb..#tmp_rs'))
	--BEGIN
	--	DROP TABLE #tmp_rs;
	--END



	--/** Process 6: TOTALSPENT - IGRP_NO **/
	--IF EXISTS (SELECT * FROM tempdb.dbo.sysobjects o WHERE o.xtype in ('U') and o.id = object_id('tempdb..#tmp_TotalSpent2'))
	--BEGIN
	--	DROP TABLE #tmp_TotalSpent2;
	--END

	--SELECT SUM(CAST(Value AS Decimal)) AS SUMS, SF.IGrp_No INTO #tmp_TotalSpent2
	--FROM SubscriberTransformed SF With(NoLock)
	--JOIN SubscriberDemographicTransformed SDF With(NoLock)
	--ON SF.SORecordIdentifier = SDF.SORecordIdentifier
	--WHERE SDF.MAFField = 'TOTALSPENT'
	--GROUP BY IGrp_No

	--Update SubscriberDemographicTransformed
	--SET Value = t.SUMS
	--FROM SubscriberTransformed sf
	--JOIN #tmp_TotalSpent2 t ON sf.IGrp_No = t.IGrp_No
	--JOIN SubscriberDemographicTransformed sdf ON sf.SORecordIdentifier = sdf.SORecordIdentifier
	--WHERE sf.IGrp_No = t.IGrp_No AND sdf.MAFField = 'TOTSPALL'

	--IF EXISTS (SELECT * FROM tempdb.dbo.sysobjects o WHERE o.xtype in ('U') and o.id = object_id('tempdb..#tmp_TotalSpent2'))
	--BEGIN
	--	DROP TABLE #tmp_TotalSpent2;
	--END
	--GO

END