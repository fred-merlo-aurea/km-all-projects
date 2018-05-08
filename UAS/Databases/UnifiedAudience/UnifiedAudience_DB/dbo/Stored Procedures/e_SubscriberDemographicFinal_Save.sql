CREATE PROCEDURE [e_SubscriberDemographicFinal_Save]
@SDFinalID int,
@PubID int,
@SFRecordIdentifier uniqueidentifier,
@MAFField varchar(255),
@Value varchar(max),
@NotExists bit,
@DateCreated datetime,
@DateUpdated datetime,
@CreatedByUserID int,
@UpdatedByUserID int,
@DemographicUpdateCodeId int,
@IsAdhoc bit,
@ResponseOther varchar(256) = '',
@IsDemoDate bit
AS
BEGIN

	SET NOCOUNT ON

	IF @SDFinalID > 0
		BEGIN
			IF @DateUpdated IS NULL
				BEGIN
					SET @DateUpdated = GETDATE();
				END
			
			UPDATE SubscriberDemographicFinal
			SET SFRecordIdentifier = @SFRecordIdentifier,
				PubID = @PubID,
				MAFField = @MAFField,
				Value = @Value,
				NotExists = @NotExists,
				DateUpdated = @DateUpdated,
				UpdatedByUserID = @UpdatedByUserID,
				DemographicUpdateCodeId = @DemographicUpdateCodeId,
				IsAdhoc = @IsAdhoc,
				ResponseOther = @ResponseOther,
				IsDemoDate = @IsDemoDate
			WHERE SDFinalID = @SDFinalID;
		
			SELECT @SDFinalID;
		END
	ELSE
		BEGIN
			IF @DateCreated IS NULL
				BEGIN
					SET @DateCreated = GETDATE();
				END
			INSERT INTO SubscriberDemographicFinal (SFRecordIdentifier,PubID,MAFField,Value,NotExists,DateCreated,CreatedByUserID,DemographicUpdateCodeId,IsAdhoc,ResponseOther,IsDemoDate)
			VALUES(@SFRecordIdentifier,@PubID,@MAFField,@Value,@NotExists,@DateCreated,@CreatedByUserID,@DemographicUpdateCodeId,@IsAdhoc,@ResponseOther,@IsDemoDate);SELECT @@IDENTITY;
		END

END