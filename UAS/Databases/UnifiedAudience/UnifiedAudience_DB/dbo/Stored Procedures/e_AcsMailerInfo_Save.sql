CREATE PROCEDURE [dbo].[e_AcsMailerInfo_Save]
@AcsMailerInfoId int,
@MailerID int,
@AcsCode varchar(9),
@ImbSeqCounter int,
@DateCreated datetime,
@DateUpdated datetime,
@CreatedByUserID int,
@UpdatedByUserID int
AS
BEGIN

	set nocount on

	IF @AcsMailerInfoId > 0
		BEGIN
			IF @DateUpdated IS NULL
				BEGIN
					SET @DateUpdated = GETDATE();
				END
			UPDATE AcsMailerInfo
			SET AcsCode = @AcsCode,
				MailerID = @MailerID,
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
			INSERT into AcsMailerInfo(AcsCode,MailerID,ImbSeqCounter,DateCreated,CreatedByUserID)
			VALUES(@AcsCode,@MailerID,@ImbSeqCounter,@DateCreated,@CreatedByUserID);SELECT @@IDENTITY;
		END

END
GO