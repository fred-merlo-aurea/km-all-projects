CREATE PROCEDURE [dbo].[ccp_Watt_LookupMarketPubcode]
@SourceFileID int,
@ProcessCode varchar(50) = '',
@ClientId int = 19
AS
BEGIN

	set nocount on

	DECLARE @PubCode varchar(100)
	DECLARE @SOR uniqueidentifier
	DECLARE @STR uniqueidentifier
	DECLARE @Company varchar(100)

	DECLARE c CURSOR
	FOR 
		SELECT PubCode, SORecordIdentifier, STRecordIdentifier, Company
		FROM SubscriberTransformed t With(NoLock)
		WHERE SourceFileID = @SourceFileID
	OPEN c
		FETCH NEXT FROM c INTO @PubCode,@SOR,@STR,@Company
	WHILE @@FETCH_STATUS = 0
	BEGIN
		DECLARE @markets TABLE
		(
			Market varchar(500)
		)
		INSERT INTO @markets(Market)
		SELECT DimensionValue FROM AdHocDimension With(NoLock) WHERE StandardField = 'PubCode' AND MatchValue = @PubCode

		--If pubcode matches pubcode column in KM_WATT_LOOKUP_Market_Pubcode.xlsx, assign value in Market column to temp field Market.
		IF (SELECT COUNT(*) FROM @markets) > 0
			BEGIN
				--Note: a pubcode can belong to more than one Market.  As a result a record can have comma separated values such as F,P,T in TOP_COMPANY.
				DECLARE @topCo nvarchar(500)
				DECLARE @m nvarchar(500)
				DECLARE mc CURSOR
				FOR
					SELECT Market FROM @markets
				OPEN mc
					FETCH NEXT FROM mc INTO @m
				WHILE @@FETCH_STATUS = 0
				BEGIN
					
					IF (SELECT MAX(dbo.fn_Levenshtein(MatchValue,@Company)) FROM AdHocDimension With(NoLock) WHERE StandardField = 'Company' AND CreatedDimension = @m AND ClientID = @ClientID) >= 75
					BEGIN
						DECLARE @Value varchar(500) = (SELECT Distinct DimensionValue FROM AdHocDimension With(NoLock) WHERE  StandardField = 'Company' AND CreatedDimension = @m AND ClientID = @ClientID)
						DECLARE @PubID int = (SELECT Distinct ISNULL(sdo.PubID,0) 
											  FROM SubscriberDemographicOriginal sdo 
											  JOIN SubscriberOriginal so ON sdo.SORecordIdentifier = so.SORecordIdentifier
											  WHERE so.PubCode = @PubCode)
					  
						INSERT INTO SubscriberDemographicTransformed (PubID,SORecordIdentifier,STRecordIdentifier,MAFField,Value,NotExists,DateCreated,CreatedByUserID)
						VALUES(@PubID,@SOR,@STR,'TOP_COMPANY',@Value,'false',GETDATE(),1)
					END

					FETCH NEXT FROM mc INTO @m
				END
				CLOSE mc
				DEALLOCATE mc
			END
	
		FETCH NEXT FROM c INTO @PubCode,@SOR,@STR,@Company
	END
	CLOSE c
	DEALLOCATE c

END
GO