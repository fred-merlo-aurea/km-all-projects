CREATE PROCEDURE [dbo].[ccp_WATT_Macro]
@SourceFileID int,
@ClientId int = 19
AS
BEGIN

	set nocount on


	DECLARE @PubID int = (SELECT TOP 1 ISNULL(PubID,-1)
						  FROM SubscriberDemographicTransformed d With(NoLock)
						  JOIN SubscriberTransformed st With(NoLock) ON d.STRecordIdentifier = st.STRecordIdentifier)
						   
	INSERT INTO SubscriberDemographicTransformed (PubID,SORecordIdentifier,STRecordIdentifier,MAFField,Value,DateCreated,CreatedByUserID)
	SELECT * from
	(
		SELECT @PubID as PubID,st.SORecordIdentifier,st.STRecordIdentifier,sdt.MAFField,
		(
			SELECT TOP 1 d.DimensionValue 
			FROM AdHocDimension d With(NoLock)		
			WHERE d.ClientID = @ClientID
			AND d.IsActive = 'true'
			AND d.StandardField = 'PubCode'
			AND d.CreatedDimension = 'MACRO'
			AND st.Title = d.MatchValue		
		) as Value,
		sdt.DateCreated,sdt.CreatedByUserId
		FROM SubscriberTransformed st With(NoLock)
		FULL JOIN SubscriberDemographicTransformed sdt With(NoLock) on st.SORecordIdentifier = sdt.SORecordIdentifier
		WHERE sdt.MAFField = 'MACRO' 
	) as A

END
GO