CREATE PROCEDURE [dbo].[ccp_Canon_Company]
@SourceFileID int,
@ClientID int = 7 
AS
BEGIN

	set nocount on

	DECLARE @PubID int = CASE WHEN EXISTS(SELECT ISNULL(sdo.PubID, 0) 
								FROM SubscriberDemographicOriginal sdo 
								JOIN SubscriberOriginal so ON sdo.SORecordIdentifier = so.SORecordIdentifier
								)
							THEN(SELECT ISNULL(sdo.PubID, 0) 
								FROM SubscriberDemographicOriginal sdo 
								JOIN SubscriberOriginal so ON sdo.SORecordIdentifier = so.SORecordIdentifier
								)
							ELSE 0
						END		
	
	DECLARE @temp TABLE
	(
		company2 varchar(500)
	) 
	
	INSERT INTO @temp(company2)
	SELECT * FROM
	(
	SELECT ( SELECT TOP 1 DimensionValue FROM AdHocDimension d With(NoLock) 
		WHERE d.ClientID = @ClientID 
		AND d.IsActive = 'true'
		AND d.StandardField = 'COMPANY'
		AND d.CreatedDimension = 'company2'
		AND (dbo.fn_Levenshtein(st.Company,d.MatchValue) > 75)
		) As Result
	FROM SubscriberTransformed st With(NoLock)
	) AS A
	WHERE A.Result != ''
						   
	INSERT INTO SubscriberDemographicTransformed (PubID,SORecordIdentifier,STRecordIdentifier,MAFField,Value,DateCreated,CreatedByUserID)
	SELECT * FROM
	(
	SELECT @PubID as PubID,st.SORecordIdentifier,st.STRecordIdentifier,'TOP100' as MAFField,
	(	SELECT CASE	
			WHEN EXISTS(SELECT TOP 1 d.DimensionValue 
			FROM AdHocDimension d With(NoLock)
			WHERE d.ClientID = @ClientID 
			AND d.IsActive = 'true'
			AND d.StandardField = 'company2'
			AND d.CreatedDimension = 'TOP100'
			AND EXISTS(SELECT TOP 1 t.company2
			FROM @temp t WHERE dbo.fn_Levenshtein(t.company2, d.MatchValue) > 75
			))
			THEN 'A'
			ELSE 'Z'
		END
	) as Value,
	GETDATE() as DateCreated,1 as CreatedByUser
	FROM SubscriberTransformed st With(NoLock)
	) as A
	WHERE A.Value != ''

END
GO