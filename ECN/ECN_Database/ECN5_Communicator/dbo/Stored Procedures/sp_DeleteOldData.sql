CREATE PROC [dbo].[sp_DeleteOldData]

AS

BEGIN
	SET NOCOUNT ON

	DECLARE 
		@dt1Year datetime,
		@dt2Years datetime,
		@blastID int,
		@sampleID int
			      
	SET @dt1Year  = CONVERT(VARCHAR(10), DATEADD(MM, -12, GETDATE()), 101)
	SET @dt2Years = CONVERT(VARCHAR(10), DATEADD(MM, -24, GETDATE()), 101)

	CREATE TABLE #tblCampaignItem (CampaignItemID int NOT NULL PRIMARY KEY)
	
	TRUNCATE TABLE ECN_Temp..BlastDelete
	
	INSERT INTO 
		ECN_Temp..BlastDelete
	SELECT 
		b.blastID, 
		b.SampleID  
	FROM 
		ECN5_Communicator..Blast b 
		INNER JOIN ecn5_accounts..Customer c ON b.CustomerID = c.CustomerID
	WHERE 
		sendtime < @dt1Year 
		AND BaseChannelID not in (94) -- do not DELETE GOODDONOR customer accounts blasts
		AND basechannelID not in (3,4,6,15,16,34,39,42,46,50,55,63,65,68,69,71,72,73,74,75,76,77,78,79,80,81,82,84,85,86,87,88,89,91,99,103,104,105,106,107,108,109,110,111,112,113,114,127,128,130,131,132)
		AND basechannelID in (select BaseChannelID from ECN5_ACCOUNTS..Basechannel where BounceDomain = 'bounce2.com')
		AND b.blastID not in (SELECT BlastID FROM ECN5_Communicator..LayoutPlans)
		AND b.customerID <> 1735 and b.StatusCode <> 'deleted'
	UNION
	SELECT
		b.blastID, 
		b.SampleID  
	FROM 
		ECN5_Communicator..Blast b 
		INNER JOIN ecn5_accounts..Customer c ON b.CustomerID = c.CustomerID
	WHERE 
		BaseChannelID not in (94)-- do not DELETE GOODDONOR customer accounts blasts
		AND sendtime < @dt2Years
		AND b.blastID not in (SELECT blastID FROM ECN5_Communicator..LayoutPlans)
		AND b.customerID <> 1735  and b.StatusCode <> 'deleted'
	ORDER BY 
		BlastID ASC

	INSERT INTO #tblCampaignItem
	SELECT DISTINCT CampaignItemID FROM CampaignItemBlast cib with (NOLOCK) join ECN_Temp..BlastDELETE b on cib.BlastID = b.blastID
	UNION 
	SELECT DISTINCT CampaignItemID FROM CampaignItemTestBlast  citb with (NOLOCK) join ECN_Temp..BlastDELETE b on citb.BlastID = b.blastID		

	DELETE FROM CampaignItemBlastRefBlast	WHERE CampaignItemBlastID in (SELECT cib.CampaignItemBlastID FROM CampaignItemBlast cib join #tblCampaignItem tci on cib.CampaignItemID = tci.CampaignItemID)

	--DELETE FROM CampaignItemMetaTag			WHERE CampaignItemID in (SELECT tci.CampaignItemID FROM #tblCampaignItem tci)	
	--DELETE FROM CampaignItemOptOutGroup		WHERE CampaignItemID in (SELECT tci.CampaignItemID FROM #tblCampaignItem tci)	
	--DELETE FROM CampaignItemSocialMedia		WHERE CampaignItemID in (SELECT tci.CampaignItemID FROM #tblCampaignItem tci)	
	--DELETE FROM SimpleShareDetail			WHERE CampaignItemID in (SELECT tci.CampaignItemID FROM #tblCampaignItem tci)	
	--DELETE FROM CampaignItemLinkTracking	WHERE CampaignItemID in (SELECT tci.CampaignItemID FROM #tblCampaignItem tci)
	--DELETE FROM CampaignItemSuppression		WHERE CampaignItemID in (SELECT tci.CampaignItemID FROM #tblCampaignItem tci)
	--DELETE FROM CampaignItemTestBlast		WHERE CampaignItemID in (SELECT tci.CampaignItemID FROM #tblCampaignItem tci)
	--DELETE FROM CampaignItemBlast			WHERE CampaignItemID in (SELECT tci.CampaignItemID FROM #tblCampaignItem tci)
	--DELETE FROM CampaignItem				WHERE CampaignItemID in (SELECT tci.CampaignItemID FROM #tblCampaignItem tci)

	Update CampaignItem set IsDeleted = 1, UpdatedDate = GETDATE() where  CampaignItemID in (SELECT tci.CampaignItemID FROM #tblCampaignItem tci)

	/* delete test blast older than 3 months */
	INSERT INTO 
		ECN_Temp..BlastDelete
	SELECT
		b.blastID, 
		b.SampleID 
	FROM 
		ECN5_Communicator..Blast b 
	WHERE 
		sendtime < DATEADD(MONTH,-3,GETDATE())
		AND TestBlast = 'Y' and StatusCode = 'sent'
		
	/* added missing blast table - 8/8/2014 */

	DELETE FROM SampleBlasts  			WHERE SampleID in (SELECT tb.sampleID FROM ECN_Temp..BlastDELETE tb WHERE SampleID is not null)	
	DELETE FROM SampleBlasts  			WHERE BlastID in (SELECT tb.blastID FROM ECN_Temp..BlastDELETE tb) 
	DELETE FROM [Sample]  				WHERE SampleID in (SELECT tb.sampleID FROM ECN_Temp..BlastDELETE tb WHERE SampleID is not null)
	DELETE FROM [Sample]  				WHERE BlastID in (SELECT tb.blastID FROM ECN_Temp..BlastDELETE tb) 
	
	DELETE from AutoResponders WHERE BlastID  in (SELECT tb.blastID FROM ECN_Temp..BlastDELETE tb)  
	DELETE from BlastDynamicContents WHERE BlastID  in (SELECT tb.blastID FROM ECN_Temp..BlastDELETE tb)  
	DELETE from BlastEmails WHERE BlastID  in (SELECT tb.blastID FROM ECN_Temp..BlastDELETE tb)  
	DELETE from BlastFields WHERE BlastID  in (SELECT tb.blastID FROM ECN_Temp..BlastDELETE tb)  
	DELETE from BlastLinks WHERE BlastID  in (SELECT tb.blastID FROM ECN_Temp..BlastDELETE tb)  
	DELETE from BlastPlans WHERE BlastID  in (SELECT tb.blastID FROM ECN_Temp..BlastDELETE tb)  
	DELETE from BlastRSS WHERE BlastID  in (SELECT tb.blastID FROM ECN_Temp..BlastDELETE tb)  
	DELETE from BlastSeedlistHistory WHERE BlastID  in (SELECT tb.blastID FROM ECN_Temp..BlastDELETE tb)  
	DELETE from BlastSingles WHERE BlastID  in (SELECT tb.blastID FROM ECN_Temp..BlastDELETE tb)  
	DELETE from BlastSummary WHERE BlastID  in (SELECT tb.blastID FROM ECN_Temp..BlastDELETE tb)  
	DELETE from BlastSuppressedEmails WHERE BlastID  in (SELECT tb.blastID FROM ECN_Temp..BlastDELETE tb)  
	DELETE from CampaignItemBlast WHERE BlastID  in (SELECT tb.blastID FROM ECN_Temp..BlastDELETE tb)  
	DELETE from CampaignItemTestBlast WHERE BlastID  in (SELECT tb.blastID FROM ECN_Temp..BlastDELETE tb)  
	DELETE from ChampionBlasts WHERE BlastID  in (SELECT tb.blastID FROM ECN_Temp..BlastDELETE tb)  
	DELETE from EmailActivityLog WHERE BlastID  in (SELECT tb.blastID FROM ECN_Temp..BlastDELETE tb)  
	DELETE from EmailPreview WHERE BlastID  in (SELECT tb.blastID FROM ECN_Temp..BlastDELETE tb)  
	DELETE from EmailQueue WHERE BlastID  in (SELECT tb.blastID FROM ECN_Temp..BlastDELETE tb)  
	DELETE from LayoutPlans WHERE BlastID  in (SELECT tb.blastID FROM ECN_Temp..BlastDELETE tb)  
	--DELETE from PageWatchHistory WHERE BlastID  in (SELECT tb.blastID FROM ECN_Temp..BlastDELETE tb)  
	DELETE from ProcedureLogging WHERE BlastID  in (SELECT tb.blastID FROM ECN_Temp..BlastDELETE tb)  
	DELETE from ReportSchedule WHERE BlastID  in (SELECT tb.blastID FROM ECN_Temp..BlastDELETE tb)  
	DELETE from TriggerPlans WHERE BlastID  in (SELECT tb.blastID FROM ECN_Temp..BlastDELETE tb)  
	DELETE from UniqueLink WHERE BlastID  in (SELECT tb.blastID FROM ECN_Temp..BlastDELETE tb)  
	--DELETE from Wizard WHERE BlastID  in (SELECT tb.blastID FROM ECN_Temp..BlastDELETE tb)  
	--DELETE from WizardBlasts WHERE BlastID  in (SELECT tb.blastID FROM ECN_Temp..BlastDELETE tb)  
	--DELETE from Blast WHERE BlastID  in (SELECT tb.blastID FROM ECN_Temp..BlastDELETE tb)  
	
	UPDATE 
		Blast
	SET 
		StatusCode='Deleted' ,
		UpdatedDate = GETDATE(),
		UpdatedUserID = -1
	WHERE
		BlastID IN (SELECT tb.blastID FROM ECN_Temp..BlastDELETE tb) 
		AND StatusCode != 'Deleted'
	
	/* DELETE orphans */
	
	DELETE FROM BlastScheduleDaysHistory
	WHERE BlastScheduleDaysID in
	(
		  SELECT bsd.BlastScheduleDaysID FROM BlastScheduleDays bsd
		  WHERE blastscheduleID not in 
		  (
			  SELECT bs.BlastScheduleID FROM BlastSchedule bs
			  WHERE blastscheduleID not in (SELECT distinct b.BlastScheduleID FROM Blast b WHERE BlastScheduleID is not null)
		  )
	)

	DELETE FROM BlastScheduleHistory	WHERE blastscheduleID NOT IN (SELECT distinct b.BlastScheduleID FROM Blast b WHERE BlastScheduleID IS NOT NULL)
	DELETE FROM BlastScheduleDays		WHERE blastscheduleID NOT IN (SELECT distinct b.BlastScheduleID FROM Blast b WHERE BlastScheduleID IS NOT NULL)
	DELETE FROM BlastSchedule			WHERE blastscheduleID NOT IN (SELECT distinct b.BlastScheduleID FROM Blast b WHERE BlastScheduleID IS NOT NULL)

	DROP TABLE #tblCampaignItem
	
	/* DELETE Activity */
	/* Removed, this is in a separate bathing process in ECN_Activity, sp_DeleteActivityBulk */
	
	DELETE bsr FROM ECN5_Warehouse.dbo.BlastSendRangeByDate bsr where bsr.SendDate < @dt2Years


END
