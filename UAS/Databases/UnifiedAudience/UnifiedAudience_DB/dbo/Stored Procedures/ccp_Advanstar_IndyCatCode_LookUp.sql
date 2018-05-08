CREATE PROCEDURE [dbo].[ccp_Advanstar_IndyCatCode_LookUp]
@SourceFileID int,
@ClientID int
AS
BEGIN

	set nocount on

	DECLARE @PubID int = CASE WHEN EXISTS(SELECT TOP 1 ISNULL(sdo.PubID, 0) 
								FROM SubscriberDemographicOriginal sdo 
								JOIN SubscriberOriginal so ON sdo.SORecordIdentifier = so.SORecordIdentifier
								)
							THEN(SELECT TOP 1 ISNULL(sdo.PubID, 0) 
								FROM SubscriberDemographicOriginal sdo 
								JOIN SubscriberOriginal so ON sdo.SORecordIdentifier = so.SORecordIdentifier
								)
							ELSE 0
						END	
			   
	INSERT INTO SubscriberDemographicTransformed (PubID,SORecordIdentifier,STRecordIdentifier,MAFField,Value,NotExists,DateCreated,CreatedByUserID)
	SELECT * from
	(
	SELECT 0 as PubID,st.SORecordIdentifier,st.STRecordIdentifier,'IndyCode' as MAFField,
	(SELECT TOP 1 d.DimensionValue 
		FROM UAS..AdHocDimensionGroup g With(NoLock)
		JOIN UAS..AdHocDimension d with(nolock) on g.AdHocDimensionGroupId = d.AdHocDimensionGroupId 
		WHERE g.ClientID = @ClientID 
		AND d.IsActive = 'true'
		AND g.StandardField = 'Sequence'
		AND g.CreatedDimension = 'IndyCode'
		AND st.Sequence = SUBSTRING(d.MatchValue, PATINDEX('%[^0]%', d.MatchValue+'.'), LEN(d.MatchValue))
		AND st.PubCode = 'CBIATTEN'
	)as Value,
	'false' as NotExists,GETDATE() as DateCreated,1 as CreatedByUser
	FROM SubscriberTransformed st With(NoLock)
	) as A
	WHERE A.Value != ''
	
	INSERT INTO SubscriberDemographicTransformed (PubID,SORecordIdentifier,STRecordIdentifier,MAFField,Value,NotExists,DateCreated,CreatedByUserID)
	SELECT * from
	(
	SELECT @PubID as PubID,st.SORecordIdentifier,st.STRecordIdentifier,'CatCode' as MAFField,
	(SELECT TOP 1 d.DimensionValue 
		FROM UAS..AdHocDimensionGroup g With(NoLock)
		JOIN UAS..AdHocDimension d with(nolock) on g.AdHocDimensionGroupId = d.AdHocDimensionGroupId 
		WHERE g.ClientID = @ClientID 
		AND d.IsActive = 'true'
		AND g.StandardField = 'Sequence'
		AND g.CreatedDimension = 'CatCode'
		AND st.Sequence = SUBSTRING(d.MatchValue, PATINDEX('%[^0]%', d.MatchValue+'.'), LEN(d.MatchValue))
		AND st.PubCode = 'CBIATTEN'
	)as Value,
	'false' as NotExists,GETDATE() as DateCreated,1 as CreatedByUser
	FROM SubscriberTransformed st With(NoLock)
	) as A
	WHERE A.Value != ''

END
GO