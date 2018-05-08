create procedure e_PaidOrderDetail_Save
@PaidOrderDetailId int,
@PaidOrderId int,
@ProductSubscriptionId int,
@ProductId int,
@RefundDate date,
@FulfilledDate date,
@SubTotal money,
@TaxTotal money,
@GrandTotal money,
@SubGenBundleId int,
@SubGenOrderItemId int,
@DateCreated DateTime,
@DateUpdated DateTime,
@CreatedByUserId int,
@UpdatedByUserId int
as
BEGIN

	set nocount on

	if (@PaidOrderDetailId > 0)
		begin
			IF @DateUpdated IS NULL
				BEGIN
					SET @DateUpdated = GETDATE();
				END
		
			update PaidOrderDetail
			set PaidOrderId = @PaidOrderId,
				ProductSubscriptionId = @ProductSubscriptionId,
				ProductId = @ProductId,
				RefundDate = @RefundDate,
				FulfilledDate = @FulfilledDate,
				SubTotal = @SubTotal,
				TaxTotal = @TaxTotal,
				GrandTotal = @GrandTotal,
				SubGenBundleId = @SubGenBundleId,
				SubGenOrderItemId = @SubGenOrderItemId,
				DateUpdated = @DateUpdated,
				UpdatedByUserId = @UpdatedByUserId
			where PaidOrderDetailId = @PaidOrderDetailId
		
			select @PaidOrderDetailId;
		
		end
	else
		begin
			IF @DateCreated IS NULL
				BEGIN
					SET @DateCreated = GETDATE();
				END
		
			insert into PaidOrderDetail (PaidOrderId,ProductSubscriptionId,ProductId,RefundDate,FulfilledDate,SubTotal,TaxTotal,GrandTotal,SubGenBundleId,SubGenOrderItemId,
											DateCreated,CreatedByUserId)
			values(@PaidOrderId,@ProductSubscriptionId,@ProductId,@RefundDate,@FulfilledDate,@SubTotal,@TaxTotal,@GrandTotal,@SubGenBundleId,@SubGenOrderItemId,
											@DateCreated,@CreatedByUserId);select @@IDENTITY;
		end

END
go