create procedure e_PaidOrder_Save
@PaidOrderId int,
@SubscriptionId int,
@ImportName varchar(30),
@OrderDate date,
@IsGift bit,
@SubTotal money,
@TaxTotal money,
@GrandTotal money,
@PaymentAmount money,
@PaymentNote varchar(50),
@PaymentTransactionId varchar(50),
@PaymentTypeCodeId int,
@UserId int,
@SubGenOrderId int,
@SubGenSubscriberId int,
@SubGenUserId int,
@DateCreated DateTime,
@DateUpdated DateTime,
@CreatedByUserId int,
@UpdatedByUserId int
as
BEGIN

	set nocount on

	if (@PaidOrderId > 0)
		begin
			IF @DateUpdated IS NULL
				BEGIN
					SET @DateUpdated = GETDATE();
				END
		
			update PaidOrder
			set SubscriptionId = @SubscriptionId,
				ImportName = @ImportName,
				OrderDate = @OrderDate,
				IsGift = @IsGift,
				SubTotal = @SubTotal,
				TaxTotal = @TaxTotal,
				GrandTotal = @GrandTotal,
				PaymentAmount = @PaymentAmount,
				PaymentNote = @PaymentNote,
				PaymentTransactionId = @PaymentTransactionId,
				PaymentTypeCodeId = @PaymentTypeCodeId,
				UserId = @UserId,
				SubGenOrderId = @SubGenOrderId,
				SubGenSubscriberId = @SubGenSubscriberId,
				SubGenUserId = @SubGenUserId,
				DateUpdated = @DateUpdated,
				UpdatedByUserId = @UpdatedByUserId
			where PaidOrderId = @PaidOrderId
		
			select @PaidOrderId;
		
		end
	else
		begin
			IF @DateCreated IS NULL
				BEGIN
					SET @DateCreated = GETDATE();
				END
		
			insert into PaidOrder (SubscriptionId,ImportName,OrderDate,IsGift,SubTotal,TaxTotal,GrandTotal,PaymentAmount,PaymentNote,PaymentTransactionId,PaymentTypeCodeId,
									UserId,SubGenOrderId,SubGenSubscriberId,SubGenUserId,DateCreated,CreatedByUserId)
			values(@SubscriptionId,@ImportName,@OrderDate,@IsGift,@SubTotal,@TaxTotal,@GrandTotal,@PaymentAmount,@PaymentNote,@PaymentTransactionId,@PaymentTypeCodeId,
									@UserId,@SubGenOrderId,@SubGenSubscriberId,@SubGenUserId,@DateCreated,@CreatedByUserId);select @@IDENTITY;
		end

END
go