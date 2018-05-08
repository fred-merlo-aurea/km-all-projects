CREATE PROCEDURE [dbo].[e_SubscriberAddKill_Save]
	@AddKillID int,
	@PublicationID int,
	@FilterID int,
	@Count int,
	@AddKillCount int,
	@Type varchar(10),
	@IsActive bit,
	@DateCreated datetime,
	@DateUpdated datetime,
	@CreatedByUserID int,
	@UpdatedByUserID int
AS
	IF @AddKillID > 0
	BEGIN
		IF @DateUpdated IS NULL
			BEGIN
				SET @DateUpdated = GETDATE();
			END
			
		UPDATE SubscriberAddKill
		SET 
			PublicationID = @PublicationID,
			FilterID = @FilterID,
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
		INSERT INTO SubscriberAddKill (PublicationID, FilterID, [Count], AddKillCount, [Type], IsActive, DateCreated, CreatedByUserID)
		VALUES(@PublicationID, @FilterID, @Count, @AddKillCount, @Type, @IsActive, @DateCreated, @CreatedByUserID);SELECT @@IDENTITY;
	END
