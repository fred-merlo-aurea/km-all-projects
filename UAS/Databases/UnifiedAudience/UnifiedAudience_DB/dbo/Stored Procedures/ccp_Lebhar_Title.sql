CREATE PROCEDURE [dbo].[ccp_Lebhar_Title]
@SourceFileID int,
@ProcessCode varchar(50),
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
	--INSERT INTO SubscriberDemographicTransformed (PubID,SORecordIdentifier,STRecordIdentifier,MAFField,Value,NotExists,DateCreated,CreatedByUserID)
	--SELECT * from
	--(
	--SELECT @PubID as PubID,st.SORecordIdentifier,st.STRecordIdentifier,'TITLE_CODE' as MAFField,
	--(SELECT TOP 1 d.DimensionValue 
	--	FROM AdHocDimension d With(NoLock)
	--	WHERE d.ClientID = @ClientID 
	--	AND d.IsActive = 'true'
	--	AND d.StandardField = 'TITLE'
	--	AND d.CreatedDimension = 'TITLE_CODE'
	--	AND st.Title LIKE '%' + d.MatchValue + '%'
	--)as Value,
	--'false' as NotExists,GETDATE() as DateCreated,1 as CreatedByUser
	--FROM SubscriberTransformed st With(NoLock)
	--) as A
	--WHERE A.Value != ''

END
GO