CREATE PROCEDURE [dbo].[e_SubscriptionPaid_Save]
@SubscriptionPaidID INT,
@PubSubscriptionID INT,
@PriceCodeID INT,
@StartIssueDate DATE,
@ExpireIssueDate DATE,
@CPRate DECIMAL(10,2),
@Amount  DECIMAL(10,2),
@AmountPaid DECIMAL(10,2),
@BalanceDue DECIMAL(10,2),
@PaidDate DATETIME,
@TotalIssues INT,
@CheckNumber CHAR(20),
@CCNumber CHAR(16),
@CCExpirationMonth CHAR(2),
@CCExpirationYear CHAR(4),
@CCHolderName VARCHAR(100),
@CreditCardTypeID INT,
@PaymentTypeID INT,
@DeliverID INT,
@GraceIssues INT,
@WriteOffAmount DECIMAL(10,2),
@OtherType VARCHAR(256),
@DateCreated DATETIME,
@DateUpdated DATETIME,
@CreatedByUserID INT,
@UpdatedByUserID INT,
@Frequency INT,
@Term INT
AS
BEGIN
	
	SET NOCOUNT ON
	
	IF EXISTS(SELECT * FROM SubscriptionPaid with(nolock) WHERE PubSubscriptionID = @PubSubscriptionID)
		BEGIN
			IF @DateUpdated IS NULL
				BEGIN
					SET @DateUpdated = GETDATE();
				END
			
			UPDATE SubscriptionPaid
			SET PubSubscriptionID = @PubSubscriptionID,
				PriceCodeID = @PriceCodeID,
				StartIssueDate = @StartIssueDate,
				ExpireIssueDate = @ExpireIssueDate,
				CPRate = @CPRate,
				Amount = @Amount,
				AmountPaid = @AmountPaid,
				BalanceDue = @BalanceDue,
				PaidDate = @PaidDate,
				TotalIssues = @TotalIssues,
				CheckNumber = @CheckNumber,
				CCNumber = @CCNumber,
				CCExpirationMonth = @CCExpirationMonth,
				CCExpirationYear = @CCExpirationYear,
				CCHolderName = @CCHolderName,
				CreditCardTypeID = @CreditCardTypeID,
				PaymentTypeID = @PaymentTypeID,
				DeliverID = @DeliverID,
				GraceIssues = @GraceIssues,
				WriteOffAmount = @WriteOffAmount,
				OtherType = @OtherType,
				DateCreated = @DateCreated,
				DateUpdated = @DateUpdated,
				CreatedByUserID = @CreatedByUserID,
				UpdatedByUserID = @UpdatedByUserID,
				Frequency = @Frequency,
				Term = @Term
			WHERE PubSubscriptionID = @PubSubscriptionID
			SELECT @SubscriptionPaidID;
		END
	ELSE
		BEGIN
			IF @DateCreated IS NULL
				BEGIN
					SET @DateCreated = GETDATE();
				END

			INSERT INTO SubscriptionPaid (PubSubscriptionID,PriceCodeID,StartIssueDate,ExpireIssueDate,CPRate,Amount,AmountPaid,BalanceDue,PaidDate,TotalIssues,CheckNumber,CCNumber,CCExpirationMonth,CCExpirationYear,CCHolderName,CreditCardTypeID,PaymentTypeID,DeliverID,GraceIssues,WriteOffAmount,OtherType,DateCreated,DateUpdated,CreatedByUserID,UpdatedByUserID,Frequency,Term)
			VALUES(@PubSubscriptionID,@PriceCodeID,@StartIssueDate,@ExpireIssueDate,@CPRate,@Amount,@AmountPaid,@BalanceDue,@PaidDate,@TotalIssues,@CheckNumber,@CCNumber,@CCExpirationMonth,@CCExpirationYear,@CCHolderName,@CreditCardTypeID,@PaymentTypeID,@DeliverID,@GraceIssues,@WriteOffAmount,@OtherType,@DateCreated,@DateUpdated,@CreatedByUserID,@UpdatedByUserID,@Frequency,@Term);SELECT @@IDENTITY;
		END

END