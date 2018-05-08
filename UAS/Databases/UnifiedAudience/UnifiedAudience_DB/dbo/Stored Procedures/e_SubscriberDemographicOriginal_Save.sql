CREATE PROCEDURE e_SubscriberDemographicOriginal_Save
@SDOriginalID int,
@PubID int,
@SORecordIdentifier uniqueidentifier,
@MAFField varchar(255),
@Value varchar(max),
@NotExists bit,
@DateCreated datetime,
@DateUpdated datetime,
@CreatedByUserID int,
@UpdatedByUserID int,
@DemographicUpdateCodeId int,
@IsAdhoc bit,
@ResponseOther varchar(256) = ''
AS
BEGIN

	SET NOCOUNT ON

	IF @SDOriginalID > 0
		BEGIN
			IF @DateUpdated IS NULL
				BEGIN
					SET @DateUpdated = GETDATE();
				END
			
			UPDATE SubscriberDemographicOriginal
			SET PubID = @PubID,
				SORecordIdentifier = @SORecordIdentifier,
				MAFField = @MAFField,
				Value = @Value,
				NotExists = @NotExists,
				DateUpdated = @DateUpdated,
				UpdatedByUserID = @UpdatedByUserID,
				DemographicUpdateCodeId = @DemographicUpdateCodeId,
				IsAdhoc = @IsAdhoc,
				ResponseOther = @ResponseOther
			WHERE SDOriginalID = @SDOriginalID;
		
			SELECT @SDOriginalID;
		END
	ELSE
		BEGIN
			IF @DateCreated IS NULL
				BEGIN
					SET @DateCreated = GETDATE();
				END
			INSERT INTO SubscriberDemographicOriginal (PubID,SORecordIdentifier,MAFField,Value,NotExists,DateCreated,CreatedByUserID,DemographicUpdateCodeId,IsAdhoc,ResponseOther)
			VALUES(@PubID,@SORecordIdentifier,@MAFField,@Value,@NotExists,@DateCreated,@CreatedByUserID,@DemographicUpdateCodeId,@IsAdhoc,@ResponseOther);SELECT @@IDENTITY;
		END

END