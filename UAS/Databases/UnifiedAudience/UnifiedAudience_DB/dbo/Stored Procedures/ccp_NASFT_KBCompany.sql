CREATE PROCEDURE [dbo].[ccp_NASFT_KBCompany]
@SourceFileID int,
@ClientID int
AS
BEGIN

	set nocount on

	--This is the old way of doing the Adhoc dimension - should be done earlier now in Validator class JW:06/24/2015
		--DECLARE @PubID int = CASE WHEN EXISTS(SELECT ISNULL(sdo.PubID, 0) 
		--							FROM SubscriberDemographicOriginal sdo 
		--							JOIN SubscriberOriginal so ON sdo.SORecordIdentifier = so.SORecordIdentifier
		--							)
		--						THEN(SELECT ISNULL(sdo.PubID, 0) 
		--							FROM SubscriberDemographicOriginal sdo 
		--							JOIN SubscriberOriginal so ON sdo.SORecordIdentifier = so.SORecordIdentifier
		--							)
		--						ELSE 0
		--					END		
						   
		--INSERT INTO SubscriberDemographicTransformed (PubID,SORecordIdentifier,STRecordIdentifier,MAFField,Value,DateCreated,CreatedByUserID)
		--SELECT * FROM
		--(
		--SELECT @PubID as PubID,st.SORecordIdentifier,st.STRecordIdentifier,'KBFLAG' as MAFField,
		--(	SELECT CASE
		--		WHEN EXISTS(SELECT TOP 1 d.DimensionValue 
		--			FROM AdHocDimension d With(NoLock)
		--			WHERE d.ClientID = @ClientID
		--			AND d.IsActive = 'true'
		--			AND d.StandardField = 'COMPANY'
		--			AND d.CreatedDimension = 'KBFLAG'
		--			AND (dbo.fn_Levenshtein(st.Company, d.MatchValue) > 75))
		--		THEN 'Y'
		--		ELSE 'N'
		--		END
		--)as VAlue,
		--GETDATE() as DateCreated,1 as CreatedByUser
		--FROM SubscriberTransformed st With(NoLock)
		--) as A
		--WHERE A.Value != ''

END
GO