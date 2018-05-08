CREATE PROCEDURE [dbo].[ccp_Advanstar_BadPhoneFaxMobile]
@SourceFileID int,
@ClientID int
AS
BEGIN
	SET NOCOUNT ON
	DECLARE @temp TABLE (a varchar(250), b varchar(250), c varchar(250), d varchar(250), e varchar(250), f datetime, g int)
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
	DECLARE @Device nvarchar(125)
	DECLARE @Value nvarchar(125)
	DECLARE cur CURSOR FOR
	SELECT dbo.getRow(1,1, d.MatchValue), dbo.getRow(2,2, d.MatchValue) 
	from UAS..AdHocDimensionGroup g with(nolock)
	join UAS..AdHocDimension d with(nolock) on g.AdHocDimensionGroupId = d.AdHocDimensionGroupId
	WHERE g.ClientID = @ClientID 
		AND d.IsActive = 'true'
		AND g.CreatedDimension = 'DEMO33'
		AND g.StandardField = 'MOBILE'

	OPEN cur
	
	FETCH NEXT FROM cur INTO @Device, @Value;
	WHILE @@FETCH_STATUS = 0
		BEGIN   
			DELETE @temp
			IF @Device = 'FAX'
				BEGIN
					insert into @temp
					Select * from 
					(
					SELECT @PubID as PubID, st.SORecordIdentifier, st.STRecordIdentifier,'DEMO32' as MAFField,
					(SELECT TOP 1 d.DimensionValue 
						from UAS..AdHocDimensionGroup g with(nolock)
						join UAS..AdHocDimension d with(nolock) on g.AdHocDimensionGroupId = d.AdHocDimensionGroupId
						WHERE g.ClientID = @ClientID 
						AND d.IsActive = 'true'
						AND g.StandardField = 'MOBILE'
						AND g.CreatedDimension = 'DEMO33'
						AND @Value = st.Fax
					) as Value,
					GETDATE() as DateCreated,1 as CreatedByUserID
					FROM SubscriberTransformed st With(NoLock)
					) as A
					Where A.Value != ''
				END
			ELSE
				insert into @temp
				Select * from
				(
				SELECT @PubID as PubID, st.SORecordIdentifier, st.STRecordIdentifier,'DEMO33' as MAFField,
				(SELECT TOP 1 d.DimensionValue 
					FROM UAS..AdHocDimensionGroup g With(NoLock)
					join UAS..AdHocDimension d with(nolock) on g.AdHocDimensionGroupId = d.AdHocDimensionGroupId
					WHERE g.ClientID = @ClientID 
					AND d.IsActive = 'true'
					AND g.StandardField = 'MOBILE'
					AND g.CreatedDimension = 'DEMO33'
					AND (@Device = 'MOBILE' AND @Value = st.Mobile
					OR (@Device = 'PHONE' AND @Value = st.Phone))
				) as Value,
				GETDATE() as DateCreated,1 as CreatedByUserID
				FROM SubscriberTransformed st With(NoLock)
				) as A
				Where A.Value != ''
		
			IF  @Device = 'MOBILE' AND (select count(*) from @temp) > 0
				BEGIN
					PRINT('M ' + @Value)
					INSERT INTO SubscriberDemographicTransformed (PubID,SORecordIdentifier,STRecordIdentifier,MAFField,Value,DateCreated,CreatedByUserID)
					SELECT * FROM @temp
					INSERT INTO SubscriberDemographicTransformed (PubID,SORecordIdentifier,STRecordIdentifier,MAFField,Value,DateCreated,CreatedByUserID)
					SELECT @PubID as PubID, st.SORecordIdentifier, st.STRecordIdentifier,'MOBILE' as MAFField, '' as Value, GETDATE() as DateCreated,1 as CreatedByUserID
					FROM SubscriberTransformed st JOIN @temp t ON t.b = st.SORecordIdentifier
					DELETE @temp
				END
			ELSE IF @Device = 'FAX' AND (select count(*) from @temp) > 0
				BEGIN
					PRINT('F ' + @Value)
					INSERT INTO SubscriberDemographicTransformed (PubID,SORecordIdentifier,STRecordIdentifier,MAFField,Value,DateCreated,CreatedByUserID)
					SELECT * FROM @temp
					INSERT INTO SubscriberDemographicTransformed (PubID,SORecordIdentifier,STRecordIdentifier,MAFField,Value,DateCreated,CreatedByUserID)
					SELECT @PubID as PubID, st.SORecordIdentifier, st.STRecordIdentifier,'FAX' as MAFField, '' as Value, GETDATE() as DateCreated,1 as CreatedByUserID
					FROM SubscriberTransformed st JOIN @temp t ON t.b = st.SORecordIdentifier
					DELETE @temp
				END
			ELSE IF @Device = 'PHONE' AND (select count(*) from @temp) > 0
				INSERT INTO SubscriberDemographicTransformed (PubID,SORecordIdentifier,STRecordIdentifier,MAFField,Value,DateCreated,CreatedByUserID)
				SELECT * FROM @temp
				INSERT INTO SubscriberDemographicTransformed (PubID,SORecordIdentifier,STRecordIdentifier,MAFField,Value,DateCreated,CreatedByUserID)
				SELECT @PubID as PubID, st.SORecordIdentifier, st.STRecordIdentifier,'PHONE' as MAFField, '' as Value, GETDATE() as DateCreated,1 as CreatedByUserID
				FROM SubscriberTransformed st JOIN @temp t ON t.b = st.SORecordIdentifier
				DELETE @temp
				DELETE @temp
				SELECT * FROM @temp
				FETCH NEXT FROM cur INTO @Device, @Value;
		END
	
	CLOSE cur;
	DEALLOCATE cur;
	

	--DECLARE cur CURSOR FOR
	--SELECT MatchValue from AdHocDimension d with(nolock)
	--WHERE d.ClientID = 2 
	--	AND d.IsActive = 'true'
	--	AND d.StandardField = 'MOBILE'
	--	AND d.CreatedDimension = 'DEMO33'
	--	AND d.MatchValue LIKE '%MOBILE%'
	--	--AND (dbo.getRow(1,1, d.MatchValue) = 'FAX') --AND dbo.getRow(2,2, d.MatchValue) = 'DO NOT CALL')

	--OPEN cur

	--FETCH NEXT FROM cur INTO @name;
	--WHILE @@FETCH_STATUS = 0
	--BEGIN   
	--PRINT dbo.getRow(2,2,@name)
	--FETCH NEXT FROM cur INTO @name;
	--END

	--CLOSE cur;
	--DEALLOCATE cur;
END
GO