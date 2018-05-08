CREATE PROCEDURE [dbo].[e_HistoryPaid_Save]
@SubscriptionPaidID int,
@PubSubscriptionID int,
@PriceCodeID int,
@StartIssueDate date,
@ExpireIssueDate date,
@CPRate decimal(10,2),
@Amount decimal(10,2),
@AmountPaid decimal(10,2),
@BalanceDue decimal(10,2),
@PaidDate datetime,
@TotalIssues int,
@CheckNumber char(20)='',
@CCNumber char(16)='',
@CCExpirationMonth char(2)='',
@CCExpirationYear char(4)='',
@CCHolderName varchar(100)='',
@CreditCardTypeID int='',
@PaymentTypeID int='',
@DateCreated datetime='',
@DateUpdated datetime='',
@CreatedByUserID int='',
@UpdatedByUserID int=''
AS
BEGIN

	SET NOCOUNT ON

	IF @DateCreated IS NULL
		BEGIN
			SET @DateCreated = GETDATE();
		END
	
	INSERT INTO HistoryPaid (SubscriptionPaidID,PubSubscriptionID,PriceCodeID,StartIssueDate,ExpireIssueDate,CPRate,Amount,AmountPaid,
								  BalanceDue,PaidDate,TotalIssues,CheckNumber,CCNumber,CCExpirationMonth,CCExpirationYear,CCHolderName,
								  CreditCardTypeID,PaymentTypeID,DateCreated,CreatedByUserID)

	VALUES(@SubscriptionPaidID,@PubSubscriptionID,@PriceCodeID,@StartIssueDate,@ExpireIssueDate,@CPRate,@Amount,@AmountPaid,
		   @BalanceDue,@PaidDate,@TotalIssues,@CheckNumber,@CCNumber,@CCExpirationMonth,@CCExpirationYear,@CCHolderName,
		   @CreditCardTypeID,@PaymentTypeID,@DateCreated,@CreatedByUserID);SELECT @@IDENTITY;

END