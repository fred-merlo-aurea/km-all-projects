CREATE PROCEDURE [dbo].[ccp_Advanstar_RegCode_LookUp]
@SourceFileID int,
@ClientID int = 2 
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
	SELECT 0 as PubID,st.SORecordIdentifier,st.STRecordIdentifier,'RegCode' as MAFField,
	(SELECT TOP 1 d.DimensionValue 
		FROM AdHocDimension d With(NoLock)
		WHERE d.ClientID = @ClientID 
		AND d.IsActive = 'true'
		AND d.StandardField = 'Sequence'
		AND d.CreatedDimension = 'RegCode'
		AND st.Sequence = SUBSTRING(d.MatchValue, PATINDEX('%[^0]%', d.MatchValue+'.'), LEN(d.MatchValue))
		AND st.PubCode = 'CBIATTEN'
	)as Value,
	'false' as NotExists,GETDATE() as DateCreated,1 as CreatedByUser
	FROM SubscriberTransformed st With(NoLock)
	) as A
	WHERE A.Value != ''

END
GO