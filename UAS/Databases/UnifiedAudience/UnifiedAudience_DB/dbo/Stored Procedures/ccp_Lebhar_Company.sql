CREATE PROCEDURE [dbo].[ccp_Lebhar_Company]
@SourceFileID int,
@ClientId int = 10
AS
BEGIN

	set nocount on

	--This is the old way of doing the Adhoc dimension - should be done earlier now in Validator class JW:06/24/2015

	--DECLARE @ClientID int = (SELECT ClientID FROM UAS..Client With(NoLock) WHERE ClientName = 'Lebhar')
	--DECLARE @PubID int = CASE WHEN EXISTS(SELECT TOP 1 ISNULL(sdo.PubID, 0) 
	--							FROM SubscriberDemographicOriginal sdo 
	--							JOIN SubscriberOriginal so ON sdo.SORecordIdentifier = so.SORecordIdentifier
	--							)
	--						THEN(SELECT TOP 1 ISNULL(sdo.PubID, 0) 
	--							FROM SubscriberDemographicOriginal sdo 
	--							JOIN SubscriberOriginal so ON sdo.SORecordIdentifier = so.SORecordIdentifier
	--							)
	--						ELSE 0
	--					END		   
	--INSERT INTO SubscriberDemographicTransformed (PubID,SORecordIdentifier,STRecordIdentifier,MAFField,Value,DateCreated,CreatedByUserID)
	--SELECT * FROM
	--(
	--SELECT @PubID as PubID,st.SORecordIdentifier,st.STRecordIdentifier,'TOP_100_RANKING' as MAFField,
	--(	SELECT TOP 1 d.DimensionValue 
	--	FROM AdHocDimension d With(NoLock)
	--	WHERE d.ClientID = @ClientID
	--	AND d.IsActive = 'true'
	--	AND d.StandardField = 'COMPANY'
	--	AND d.CreatedDimension = 'TOP_100_RANKING'
	--	AND (dbo.fn_Levenshtein(st.Company, d.MatchValue) > 75)
	--)as Value,
	--GETDATE() as DateCreated,1 as CreatedByUser
	--FROM SubscriberTransformed st With(NoLock)
	--) as A
	--WHERE A.VAlue != ''

END
GO