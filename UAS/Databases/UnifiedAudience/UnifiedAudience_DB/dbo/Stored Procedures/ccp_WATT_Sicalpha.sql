CREATE PROCEDURE [dbo].[ccp_WATT_Sicalpha]
@SourceFileID int,
@ClientID int
AS
BEGIN

	set nocount on

--This is the old way of doing the Adhoc dimension - should be done earlier now in Validator class JW:06/24/2015

	--DECLARE @PubID int = (SELECT TOP 1 ISNULL(PubID,-1)
	--					  FROM SubscriberDemographicTransformed d With(NoLock)
	--					  JOIN SubscriberTransformed st With(NoLock) ON d.STRecordIdentifier = st.STRecordIdentifier)
						   
	--INSERT INTO SubscriberDemographicTransformed (PubID,SORecordIdentifier,STRecordIdentifier,MAFField,Value,DateCreated,CreatedByUserID)
	--SELECT * from
	--(
	--	SELECT @PubID as PubID,st.SORecordIdentifier,st.STRecordIdentifier,sdt.MAFField,
	--	(
	--		SELECT TOP 1 d.DimensionValue 
	--		FROM AdHocDimension d With(NoLock)		
	--		WHERE d.ClientID = @ClientID
	--		AND d.IsActive = 'true'
	--		AND d.StandardField = 'SIC'
	--		AND d.CreatedDimension = 'SICALPHA'
	--		AND st.Title = d.MatchValue		
	--	) as Value,
	--	sdt.DateCreated,sdt.CreatedByUserId
	--	FROM SubscriberTransformed st With(NoLock)
	--	FULL JOIN SubscriberDemographicTransformed sdt With(NoLock) on st.SORecordIdentifier = sdt.SORecordIdentifier
	--	WHERE sdt.MAFField = 'SICALPHA' 
	--) as A
END
GO