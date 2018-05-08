CREATE PROCEDURE [dbo].[e_AcsMailerInfo_Save]
@AcsMailerInfoId int,
@AcsMailerId varchar(9),
@ImbSeqCounter int,
@DateCreated datetime,
@DateUpdated datetime,
@CreatedByUserID int,
@UpdatedByUserID int
AS
IF @AcsMailerInfoId > 0
	BEGIN
		IF @DateUpdated IS NULL
			BEGIN
				SET @DateUpdated = GETDATE();
			END
		UPDATE AcsMailerInfo
		SET AcsMailerId = @AcsMailerId,
			ImbSeqCounter = @ImbSeqCounter,
			DateUpdated = @DateUpdated,
			UpdatedByUserID = @UpdatedByUserID
		WHERE AcsMailerInfoId = @AcsMailerInfoId;

		SELECT @AcsMailerInfoId;
	END
ELSE
	BEGIN
		IF @DateCreated IS NULL
			BEGIN
				SET @DateCreated = GETDATE();
			END
		INSERT into AcsMailerInfo(AcsMailerId,ImbSeqCounter,DateCreated,CreatedByUserID)
		VALUES(@AcsMailerId,@ImbSeqCounter,@DateCreated,@CreatedByUserID);SELECT @@IDENTITY;
	END
GO