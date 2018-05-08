CREATE PROCEDURE e_SubscriberAddKill_Save
@AddKillID int,
@PublicationID int,
@Count int,
@AddKillCount int,
@Type varchar(50),
@IsActive bit,
@DateCreated datetime,
@DateUpdated datetime,
@CreatedByUserID int,
@UpdatedByUserID int
as
BEGIN

	SET NOCOUNT ON

	IF @AddKillID > 0
		BEGIN
			IF @DateUpdated IS NULL
				BEGIN
					SET @DateUpdated = GETDATE();
				END
			UPDATE SubscriberAddKill
			SET PublicationID = @PublicationID,
				[Count] = @Count,
				AddKillCount = @AddKillCount,
				[Type] = @Type,
				IsActive = @IsActive,
				DateUpdated = @DateUpdated,
				UpdatedByUserID = @UpdatedByUserID
			WHERE AddKillID = @AddKillID;

			SELECT @AddKillID;
		END
	ELSE
		BEGIN
			IF @DateCreated IS NULL
				BEGIN
					SET @DateCreated = GETDATE();
				END
			INSERT INTO SubscriberAddKill (PublicationID,[Count],AddKillCount,[Type],IsActive,DateCreated,CreatedByUserID)
			VALUES(@PublicationID,@Count,@AddKillCount,@Type,@IsActive,@DateCreated,@CreatedByUserID);SELECT @@IDENTITY;
		END

END
go