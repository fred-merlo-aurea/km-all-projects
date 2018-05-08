CREATE PROCEDURE e_PriceCode_Save
@PriceCodeID int,
@PublicationID int,
@PriceCode varchar(50),
@Term int,
@USCopyRate decimal,
@CANCopyRate decimal,
@FORCopyRate decimal,
@USPrice decimal(18,2),
@CANPrice decimal(18,2),
@FORPrice decimal(18,2),
@QFOfferCode varchar(255),
@FoxProPriceCode varchar(255),
@Description varchar(255),
@DeliverabilityID varchar(50),
@TotalIssues int,
@IsActive bit,
@DateCreated datetime,
@DateUpdated datetime,
@CreatedByUserID int,
@UpdatedByUserID int
AS

IF @PriceCodeID > 0
	BEGIN
		IF @DateUpdated IS NULL
			BEGIN
				SET @DateUpdated = GETDATE();
			END
			
		UPDATE PriceCode
		SET 
			PublicationID = @PublicationID,
			PriceCodes = @PriceCode,
			Term = @Term,
			US_CopyRate = @USCopyRate,
			CAN_CopyRate = @CANCopyRate,
			FOR_CopyRate = @FORCopyRate,
			US_Price= @USPrice,
			CAN_Price= @CANPrice,
			FOR_Price= @FORPrice,
			QFOfferCode = @QFOfferCode,
			FoxProPriceCode = @FoxProPriceCode,
			[Description] = @Description,
			DeliverabilityID = @DeliverabilityID,
			TotalIssues = @TotalIssues,
			IsActive = @IsActive,
			DateUpdated = @DateUpdated,
			UpdatedByUserID = @UpdatedByUserID
		WHERE PriceCodeID = @PriceCodeID;
		
		SELECT @PriceCodeID;
	END
ELSE
	BEGIN
		IF @DateCreated IS NULL
			BEGIN
				SET @DateCreated = GETDATE();
			END
		INSERT INTO PriceCode (PublicationID,PriceCodes,Term,US_CopyRate,CAN_CopyRate,FOR_CopyRate,US_Price,CAN_Price,FOR_Price,QFOfferCode,FoxProPriceCode,[Description],DeliverabilityID,TotalIssues,IsActive,DateCreated,CreatedByUserID)
		VALUES(@PublicationID,@PriceCode,@Term,@USCopyRate,@CANCopyRate,@FORCopyRate,@USPrice,@CANPrice,@FORPrice,@QFOfferCode,@FoxProPriceCode,@Description,@DeliverabilityID,@TotalIssues,@IsActive,@DateCreated,@CreatedByUserID);SELECT @@IDENTITY;
	END
