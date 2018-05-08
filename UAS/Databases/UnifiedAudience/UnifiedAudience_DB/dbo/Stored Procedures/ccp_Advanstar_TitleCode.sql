CREATE PROCEDURE [dbo].[ccp_Advanstar_TitleCode]
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
	SELECT @PubID as PubID,st.SORecordIdentifier,st.STRecordIdentifier,sdt.MAFField,--'TITLE_CODE' as MAFField,
	(SELECT TOP 1 d.DimensionValue 
		FROM AdHocDimension d With(NoLock)
		WHERE d.ClientID = @ClientID 
		AND d.IsActive = 'true'
		AND d.StandardField = 'TITLE'
		AND d.CreatedDimension = 'TITLE_CODE'
		AND st.Title = d.MatchValue
	)as Value,
	'false' as NotExists,sdt.DateCreated as DateCreated,1 as CreatedByUser
	FROM SubscriberTransformed st With(NoLock)
	FULL JOIN SubscriberDemographicTransformed sdt With(NoLock) on st.SORecordIdentifier = sdt.SORecordIdentifier
	WHERE sdt.MAFField = 'TITLE_CODE' AND (sdt.Value = '' OR sdt.Value is NULL)
	) as A
	WHERE (A.Value != '' OR A.Value is not NULL)

END
GO