CREATE PROCEDURE [dbo].[e_SubscriberDemographicArchive_Save]
@SDArchiveID int,
@PubID int,
@SARecordIdentifier uniqueidentifier,
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

	IF @SDArchiveID > 0
		BEGIN
			IF @DateUpdated IS NULL
				BEGIN
					SET @DateUpdated = GETDATE();
				END
			
			UPDATE SubscriberDemographicArchive
			SET SARecordIdentifier = @SARecordIdentifier,
				PubID = @PubID,
				MAFField = @MAFField,
				Value = @Value,
				NotExists = @NotExists,
				DateUpdated = @DateUpdated,
				UpdatedByUserID = @UpdatedByUserID,
				DemographicUpdateCodeId = @DemographicUpdateCodeId,
				IsAdhoc = @IsAdhoc,
				ResponseOther = @ResponseOther
			WHERE SDArchiveID = @SDArchiveID;
		
			SELECT @SDArchiveID;
		END
	ELSE
		BEGIN
			IF @DateCreated IS NULL
				BEGIN
					SET @DateCreated = GETDATE();
				END
			INSERT INTO SubscriberDemographicArchive (SARecordIdentifier,PubID,MAFField,Value,NotExists,DateCreated,CreatedByUserID,DemographicUpdateCodeId,IsAdhoc,ResponseOther)
			VALUES(@SARecordIdentifier,@PubID,@MAFField,@Value,@NotExists,@DateCreated,@CreatedByUserID,@DemographicUpdateCodeId,@IsAdhoc,@ResponseOther);SELECT @@IDENTITY;
		END

END