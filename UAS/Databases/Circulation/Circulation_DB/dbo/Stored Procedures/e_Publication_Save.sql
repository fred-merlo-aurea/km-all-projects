CREATE PROCEDURE e_Publication_Save
@PublicationID int,
@PublicationName varchar(50),
@PublicationCode varchar(50),
@PublisherID int,
@YearStartDate char(5),
@YearEndDate char(5),
@IssueDate date,
@IsImported bit,
@IsActive bit,
@AllowDataEntry bit,
@FrequencyID int,
@KMImportAllowed bit,
@ClientImportAllowed bit,
@AddRemoveAllowed bit,
@AcsMailerInfoId int,
@IsOpenCloseLocked int,
@DateCreated datetime,
@DateUpdated datetime,
@CreatedByUserID int,
@UpdatedByUserID int
AS

IF @PublicationID > 0
	BEGIN
		IF @DateUpdated IS NULL
			BEGIN
				SET @DateUpdated = GETDATE();
			END
			
		UPDATE Publication
		SET 
			PublicationName = @PublicationName,
			YearStartDate= @YearStartDate,
			YearEndDate = @YearEndDate,
			IssueDate = @IssueDate,
			IsImported = @IsImported,
			IsActive = @IsActive,
			AllowDataEntry = @AllowDataEntry,
			FrequencyID = @FrequencyID,
			KMImportAllowed = @KMImportAllowed, 
			ClientImportAllowed = @ClientImportAllowed, 
			AddRemoveAllowed = @AddRemoveAllowed,
			AcsMailerInfoId = @AcsMailerInfoId,
			IsOpenCloseLocked = @IsOpenCloseLocked,
			DateUpdated = @DateUpdated,
			UpdatedByUserID = @UpdatedByUserID
		WHERE PublicationID = @PublicationID;
		
		SELECT @PublicationID;
	END
ELSE
	BEGIN
		IF @DateCreated IS NULL
			BEGIN
				SET @DateCreated = GETDATE();
			END
		INSERT INTO Publication (PublicationName,PublicationCode,PublisherID,YearStartDate,YearEndDate,IssueDate,IsImported,IsActive,AllowDataEntry,FrequencyID,KMImportAllowed, ClientImportAllowed, AddRemoveAllowed,AcsMailerInfoId,IsOpenCloseLocked,DateCreated,CreatedByUserID)
		VALUES(@PublicationName,@PublicationCode,@PublisherID,@YearStartDate,@YearEndDate,@IssueDate,@IsImported,@IsActive,@AllowDataEntry,@FrequencyID,@KMImportAllowed,@ClientImportAllowed,@AddRemoveAllowed,@AcsMailerInfoId,@IsOpenCloseLocked,@DateCreated,@CreatedByUserID);SELECT @@IDENTITY;
	END
