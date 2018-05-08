CREATE PROCEDURE [dbo].[ccp_TenMissions_Company]
@SourceFileID int,
@ClientId int = 1
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
	INSERT INTO SubscriberDemographicTransformed (PubID,SORecordIdentifier,STRecordIdentifier,MAFField,Value,DateCreated,CreatedByUserID)
	Select * from
	(
	SELECT @PubID as PubID,st.SORecordIdentifier,st.STRecordIdentifier,'BUSSINESS2' as MaFField,
	(	SELECT TOP 1 d.DimensionValue 
		FROM AdHocDimension d With(NoLock)
		WHERE d.ClientID = @ClientID 
		AND d.IsActive = 'true'
		AND d.StandardField = 'COMPANY'
		AND d.CreatedDimension = 'BUSSINESS2'
		AND (dbo.fn_Levenshtein(st.Company, d.MatchValue) > 75)
	) as Value,
	GETDATE() as DateCreated,1 as CreatedByUser
	FROM SubscriberTransformed st With(NoLock)
	WHERE st.PubCode = 'FNDR' OR st.PubCode = 'RCHT'
	) as A
	Where A.Value != ''

END
GO