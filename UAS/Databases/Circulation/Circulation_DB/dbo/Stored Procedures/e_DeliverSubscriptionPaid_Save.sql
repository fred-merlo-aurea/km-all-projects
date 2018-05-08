CREATE PROCEDURE [dbo].[e_DeliverSubscriptionPaid_Save]
@DeliverID int,
@DeliverName nvarchar(50),
@DeliverCode nchar(10),
@IsActive bit,
@DateCreated datetime,
@DateUpdated datetime,
@CreatedByUserID int,
@UpdatedByUserID int
AS

IF @DeliverID > 0
	BEGIN
		IF @DateUpdated IS NULL
			BEGIN
				SET @DateUpdated = GETDATE();
			END
			
		UPDATE DeliverSubscriptionPaid
		SET 
			DeliverName = @DeliverName,
			IsActive = @IsActive,
			DateUpdated = @DateUpdated,
			UpdatedByUserID = @UpdatedByUserID
		WHERE DeliverID = @DeliverID;
		
		SELECT @DeliverID;
	END
ELSE
	BEGIN
		IF @DateCreated IS NULL
			BEGIN
				SET @DateCreated = GETDATE();
			END
		INSERT INTO CreditCardType (CreditCardName,CreditCardCode,IsActive,DateCreated,CreatedByUserID)
		VALUES(@DeliverName,@DeliverCode,@IsActive,@DateCreated,@CreatedByUserID);SELECT @@IDENTITY;
	END
