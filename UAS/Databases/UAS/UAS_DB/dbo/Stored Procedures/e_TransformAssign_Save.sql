CREATE PROCEDURE [e_TransformAssign_Save]
@TransformAssignID int,
@TransformationID int,
@Value varchar(200),
@IsActive bit,
@HasPubID bit,
@DateCreated datetime,
@DateUpdated datetime,
@CreatedByUserID int,
@UpdatedByUserID int,
@PubID int
AS
BEGIN

	set nocount on

	IF @TransformAssignID > 0
		BEGIN
			IF @DateUpdated IS NULL
				BEGIN
					SET @DateUpdated = GETDATE();
				END
			UPDATE TransformAssign
			SET TransformationID = @TransformationID,
				  Value = @Value,
				  IsActive = @IsActive,
				  HasPubID = @HasPubID,
				  DateUpdated = @DateUpdated,
				  UpdatedByUserID = @UpdatedByUserID,
				  PubID = @PubID           
			WHERE TransformAssignID = @TransformAssignID;

			SELECT @TransformAssignID;
		END
	ELSE
		BEGIN
			IF @DateCreated IS NULL
				BEGIN
					SET @DateCreated = GETDATE();
				END
			INSERT INTO TransformAssign (TransformationID, Value, IsActive, HasPubID, DateCreated, CreatedByUserID, PubID)
			VALUES(@TransformationID, @Value, @IsActive, @HasPubID, @DateCreated, @CreatedByUserID, @PubID);SELECT @@IDENTITY;
		END

END