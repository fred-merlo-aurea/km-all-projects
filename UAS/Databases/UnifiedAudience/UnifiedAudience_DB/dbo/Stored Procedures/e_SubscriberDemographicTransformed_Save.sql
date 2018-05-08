CREATE PROCEDURE [dbo].[e_SubscriberDemographicTransformed_Save]
@SubscriberDemographicTransformedID int,
@PubID int,
@SORecordIdentifier uniqueidentifier,
@STRecordIdentifier uniqueidentifier,
@MAFField varchar(255),
@Value varchar(max),
@NotExists bit,
@NotExistReason varchar(50),
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

	IF @SubscriberDemographicTransformedID > 0
		BEGIN
			IF @DateUpdated IS NULL
				BEGIN
					SET @DateUpdated = GETDATE();
				END
			
			UPDATE SubscriberDemographicTransformed
			SET STRecordIdentifier = @STRecordIdentifier,
				PubID = @PubID,
				SORecordIdentifier = @SORecordIdentifier,
				MAFField = @MAFField,
				Value = @Value,
				NotExists = @NotExists,
				NotExistReason = @NotExistReason,
				DateUpdated = @DateUpdated,
				UpdatedByUserID = @UpdatedByUserID,
				DemographicUpdateCodeId = @DemographicUpdateCodeId,
				IsAdhoc = @IsAdhoc,
				ResponseOther = @ResponseOther,
				IsDemoDate = @IsDemoDate
			WHERE SubscriberDemographicTransformedID = @SubscriberDemographicTransformedID;
		
			SELECT @SubscriberDemographicTransformedID;
		END
	ELSE
		BEGIN
			IF @DateCreated IS NULL
				BEGIN
					SET @DateCreated = GETDATE();
				END
			INSERT INTO SubscriberDemographicTransformed (STRecordIdentifier,PubID,SORecordIdentifier,MAFField,Value,NotExists,NotExistReason,DateCreated,CreatedByUserID,DemographicUpdateCodeId,IsAdhoc,ResponseOther,IsDemoDate)
			VALUES(@STRecordIdentifier,@PubID,@SORecordIdentifier,@MAFField,@Value,@NotExists,@NotExistReason,@DateCreated,@CreatedByUserID,@DemographicUpdateCodeId,@IsAdhoc,@ResponseOther,@IsDemoDate);SELECT @@IDENTITY;
		END

END