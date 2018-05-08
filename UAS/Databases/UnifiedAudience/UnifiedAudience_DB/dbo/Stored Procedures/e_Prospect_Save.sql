CREATE PROCEDURE e_Prospect_Save
@ProspectID int,
@PublicationID int,
@SubscriberID int,
@IsProspect bit,
@IsVerifiedProspect bit,
@DateCreated datetime,
@DateUpdated datetime,
@CreatedByUserID int,
@UpdatedByUserID int
AS
BEGIN

	SET NOCOUNT ON

	IF EXISTS(Select PublicationID From Prospect With(NoLock) Where PublicationID = @PublicationID AND SubscriberID = @SubscriberID) 
		BEGIN
			IF @DateUpdated IS NULL
				BEGIN
					SET @DateUpdated = GETDATE();
				END
			
			UPDATE Prospect
			SET IsProspect = @IsProspect,
				IsVerifiedProspect = @IsVerifiedProspect,
				DateUpdated = @DateUpdated,
				UpdatedByUserID = @UpdatedByUserID
			WHERE PublicationID = @PublicationID AND SubscriberID = @SubscriberID;
		END
	ELSE
		BEGIN
			IF @DateCreated IS NULL
				BEGIN
					SET @DateCreated = GETDATE();
				END
			INSERT INTO Prospect (PublicationID,SubscriberID,IsProspect,IsVerifiedProspect,DateCreated,CreatedByUserID)
			VALUES(@PublicationID,@SubscriberID,@IsProspect,@IsVerifiedProspect,@DateCreated,@CreatedByUserID);SELECT @@IDENTITY;
		END

END