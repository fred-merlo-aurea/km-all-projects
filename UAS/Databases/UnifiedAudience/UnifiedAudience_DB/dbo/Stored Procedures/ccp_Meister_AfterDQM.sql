CREATE PROCEDURE [dbo].[ccp_Meister_AfterDQM]
	@SourceFileId int,
	@ClientId int = 12
AS
BEGIN
	SET NOCOUNT ON;
	--IF EXISTS(SELECT * FROM SubscriberDemographicTransformed sdt INNER JOIN SubscriberTransformed st ON sdt.STRecordIdentifier = st.STRecordIdentifier
	--WHERE MAFFIeld IN ('Amount') and SourceFileID = @SourceFileId)
	--BEGIN

	--	SELECT IGRP_NO
	--			,SUM(CASE WHEN [Amounts]='' THEN 0.00
	--					  WHEN [Amounts] like '%[^0-9.]%' THEN 0.00 
	--					  ELSE CAST([Amounts] AS DECIMAL(12,2)) END) AS [AmountCollected]
	--			,0 as isInserted
	--	INTO #tmp_AMOUNT
	--	FROM  (SELECT igrp_no, sdt.Value AS Amounts FROM SubscriberTransformed st 
	--			INNER JOIN SubscriberDemographicTransformed sdt ON st.SORecordIdentifier = sdt.SORecordIdentifier
	--			WHERE sdt.MAFField='Amount') t1
	--	GROUP BY IGrp_No
	--END	

	
	--CREATE TABLE #tmp_SORecordIdentifierAmount(STRecordIdentifier uniqueidentifier,STRecordIdentifier uniqueidentifier,AmountCollected decimal(12,2),PubId int,SubscriberTable varchar(256))
	--CREATE INDEX idx_SORecAmt ON #tmp_SORecordIdentifierAmount(SORecordIdentifier,STRecordIdentifier,AmountCollected,PubId,SubscriberTable)
	
	--INSERT INTO #tmp_SORecordIdentifierAmount
	--	SELECT sdt.sorecordidentifier,st.STRecordIdentifier,ta.[AmountCollected],sdt.PubID,'SubscriberTransformed'
	--	FROM SubscriberDemographicTransformed sdt JOIN SubscriberTransformed st ON sdt.SORecordIdentifier = st.SORecordIdentifier
	--											  JOIN #tmp_AMOUNT ta ON ta.IGrp_No = st.IGrp_No
	--	WHERE sdt.MAFField = 'Amount Collected' and st.IGrp_Rank = 'M' and ClientID = 12 and SourceFileID = @SourceFileId
	--	GROUP BY sdt.SORecordIdentifier,st.STRecordIdentifier,ta.[AmountCollected],sdt.PubID
		
	--	UNION
		
	--	SELECT sdf.sorecordidentifier,sf.SFRecordIdentifier,ta.[AmountCollected],sdf.PubID,'SubscriberFinal'
	--	FROM SubscriberDemographicFinal sdf JOIN SubscriberFinal sf ON sdf.SFRecordIdentifier = sf.SFRecordIdentifier
	--										JOIN #tmp_AMOUNT ta ON ta.IGrp_No = sf.IGrp_No
	--	WHERE sdf.MAFField = 'Amount Collected' and sf.IGrp_Rank = 'M'
	--	GROUP BY sdf.SORecordIdentifier,sf.SFRecordIdentifier,ta.[AmountCollected],sdf.PubID


	--IF(SELECT COUNT(*) FROM #tmp_SORecordIdentifierAmount) > 0															
	--	BEGIN
	--		UPDATE sdt
	--		SET value = sri.AmountCollected
	--		FROM SubscriberDemographicTransformed AS sdt
	--			INNER JOIN #tmp_SORecordIdentifierAmount sri ON sdt.SORecordIdentifier = sri.SORecordIdentifier
	--		WHERE sdt.MAFField='Amount Collected' 
			
	--		UPDATE sdt
	--		SET value = sri.AmountCollected,DateUpdated = GETDATE()
	--		FROM SubscriberDemographicFinal AS sdt
	--			INNER JOIN #tmp_SORecordIdentifierAmount sri ON sdt.SORecordIdentifier = sri.SORecordIdentifier
	--		WHERE sdt.MAFField='Amount Collected' 
			
	--		UPDATE SubscriberFinal
	--		SET IsUpdatedInLive = 0
	--		WHERE sfrecordidentifier IN (SELECT sfrecordidentifier FROM #tmp_SORecordIdentifierAmount)
	
	--		UPDATE #tmp_AMOUNT	
	--		SET isInserted = 1
	--		FROM #tmp_AMOUNT ta JOIN SubscriberTransformed st on ta.IGrp_No = st.IGrp_No
	--							JOIN #tmp_SORecordIdentifierAmount sri on sri.SORecordIdentifier = st.SORecordIdentifier
	--		WHERE st.SORecordIdentifier NOT IN (Select SORecordIdentifier FROM SubscriberFinal Where ClientID = 12)			
			
	--		UPDATE #tmp_AMOUNT	
	--		SET isInserted = 1
	--		FROM #tmp_AMOUNT ta JOIN SubscriberFinal sf on ta.IGrp_No = sf.IGrp_No
	--							JOIN #tmp_SORecordIdentifierAmount sri on sri.SORecordIdentifier = sf.SORecordIdentifier
					
	--	END

		
	--INSERT INTO SubscriberDemographicTransformed(PubID,SORecordIdentifier,STRecordIdentifier,MAFField,Value,NotExists,DateCreated,DateUpdated,CreatedByUserID,UpdatedByUserID)
	--	SELECT PubID,st.SORecordIdentifier,st.STRecordIdentifier,'Amount Collected',ta.AmountCollected,0,GETDATE(),null,1,null
	--	FROM SubscriberTransformed st JOIN SubscriberDemographicTransformed sdt on st.SORecordIdentifier = sdt.SORecordIdentifier
	--								  JOIN #tmp_AMOUNT ta on ta.IGrp_No = st.IGrp_No
	--	WHERE IGrp_Rank = 'M' AND ta.isInserted = 0 AND ClientID = 12 AND SourceFileID = @SourceFileId and sdt.SORecordIdentifier NOT IN(Select SORecordIdentifier FROM SubscriberFinal WHere ClientID = 12)
	--	GROUP BY PubID,st.SORecordIdentifier,st.STRecordIdentifier,ta.AmountCollected


	--DECLARE @SFR table(SFrecordIdentifier uniqueidentifier)

	--INSERT INTO SubscriberDemographicFinal(PubID,SFRecordIdentifier,MAFField,Value,NotExists,DateCreated,DateUpdated,CreatedByUserID,UpdatedByUserID)
	--	OUTPUT Inserted.SFRecordIdentifier INTO @SFR
		
	--	SELECT PubID,sf.SFRecordIdentifier,'Amount Collected',ta.AmountCollected,0,GETDATE(),null,1,null
	--	FROM SubscriberFinal sf JOIN SubscriberDemographicFinal sdf on sf.SFRecordIdentifier = sdf.SFRecordIdentifier
	--								  JOIN #tmp_AMOUNT ta on ta.IGrp_No = sf.IGrp_No
	--	WHERE sf.IGrp_Rank = 'M' AND ta.isInserted = 0 AND sf.ClientID = 12
	--	GROUP BY PubID,sf.SORecordIdentifier,sf.SFRecordIdentifier,ta.AmountCollected		

	--UPDATE SubscriberFinal
	--SET IsUpdatedInLive = 0
	--WHERE SFRecordIdentifier in(Select SFRecordIdentifier from @SFR)
END