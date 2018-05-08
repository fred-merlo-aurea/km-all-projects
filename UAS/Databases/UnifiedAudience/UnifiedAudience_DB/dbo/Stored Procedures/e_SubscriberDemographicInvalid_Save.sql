CREATE PROCEDURE [dbo].[e_SubscriberDemographicInvalid_Save]
@SDInvalidID int,
@PubID int,
@SORecordIdentifier uniqueidentifier,
@SIRecordIdentifier uniqueidentifier,
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

	IF @SDInvalidID > 0
		BEGIN
			IF @DateUpdated IS NULL
				BEGIN
					SET @DateUpdated = GETDATE();
				END
			
			UPDATE SubscriberDemographicInvalid
			SET SIRecordIdentifier = @SIRecordIdentifier,
				PubID = @PubID,
				SORecordIdentifier = @SORecordIdentifier,
				MAFField = @MAFField,
				Value = @Value,
				NotExists = @NotExists,
				DateUpdated = @DateUpdated,
				UpdatedByUserID = @UpdatedByUserID,
				DemographicUpdateCodeId = @DemographicUpdateCodeId,
				IsAdhoc = @IsAdhoc,
				ResponseOther = @ResponseOther
			WHERE SDInvalidID = @SDInvalidID;
		
			SELECT @SDInvalidID;
		END
	ELSE
		BEGIN
			IF @DateCreated IS NULL
				BEGIN
					SET @DateCreated = GETDATE();
				END
			INSERT INTO SubscriberDemographicInvalid (SIRecordIdentifier,PubID,SORecordIdentifier,MAFField,Value,NotExists,DateCreated,CreatedByUserID,DemographicUpdateCodeId,IsAdhoc,ResponseOther)
			VALUES(@SIRecordIdentifier,@PubID,@SORecordIdentifier,@MAFField,@Value,@NotExists,@DateCreated,@CreatedByUserID,@DemographicUpdateCodeId,@IsAdhoc,@ResponseOther);SELECT @@IDENTITY;
		END

END