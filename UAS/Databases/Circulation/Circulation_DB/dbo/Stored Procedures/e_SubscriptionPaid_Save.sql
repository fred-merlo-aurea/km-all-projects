
CREATE PROCEDURE [dbo].[e_SubscriptionPaid_Save]
@SubscriptionPaidID int,
@SubscriptionID int,
@PriceCodeID int,
@StartIssueDate date,
@ExpireIssueDate date,
@CPRate decimal(10,2),
@Amount decimal(10,2),
@AmountPaid decimal(10,2),
@BalanceDue decimal(10,2),
@PaidDate datetime,
@TotalIssues int,
@CheckNumber char(20)=0,
@CCNumber char(16)=0,
@CCExpirationMonth char(2)='',
@CCExpirationYear char(4)='',
@CCHolderName varchar(100)='',
@CreditCardTypeID int=0,
@PaymentTypeID int,
@DateCreated datetime,
@DateUpdated datetime ,
@CreatedByUserID int=0,
@UpdatedByUserID int=0,
@DeliverID int=0,
@GraceIssues int=0,
@WriteOffAmount decimal(10,2) = '',
@OtherType varchar(256) = '',
@Frequency int = 0,
@Term int = 0
AS

IF @SubscriptionPaidID > 0
	BEGIN
		UPDATE SubscriptionPaid
		SET 
			SubscriptionID = @SubscriptionID,
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
			DateCreated = @DateCreated,
			DateUpdated = @DateUpdated,
			CreatedByUserID = @CreatedByUserID,
			UpdatedByUserID = @UpdatedByUserID,
			DeliverID = @DeliverID,
			GraceIssues = @GraceIssues,
			WriteOffAmount = @WriteOffAmount,
			OtherType = @OtherType,
			Frequency = @Frequency,
			Term = @Term
		WHERE SubscriptionPaidID = @SubscriptionPaidID 
		
		SELECT @SubscriptionPaidID;
	END
ELSE
	BEGIN
		IF @DateCreated IS NULL
			BEGIN
				SET @DateCreated = GETDATE();
			END
		INSERT INTO SubscriptionPaid (SubscriptionID,PriceCodeID,StartIssueDate,ExpireIssueDate,CPRate,Amount,AmountPaid,
									  BalanceDue,PaidDate,TotalIssues,CheckNumber,CCNumber,CCExpirationMonth,CCExpirationYear,CCHolderName,
									  CreditCardTypeID,PaymentTypeID,DateCreated,CreatedByUserID,DeliverID,GraceIssues,WriteOffAmount,OtherType,Frequency,Term)

		VALUES(@SubscriptionID,@PriceCodeID,@StartIssueDate,@ExpireIssueDate,@CPRate,@Amount,@AmountPaid,
			   @BalanceDue,@PaidDate,@TotalIssues,@CheckNumber,@CCNumber,@CCExpirationMonth,@CCExpirationYear,@CCHolderName,
			   @CreditCardTypeID,@PaymentTypeID,@DateCreated,@CreatedByUserID,@DeliverID,@GraceIssues,@WriteOffAmount,@OtherType,@Frequency,@Term);SELECT @@IDENTITY;
	END

