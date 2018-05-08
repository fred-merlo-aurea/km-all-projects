CREATE PROCEDURE [dbo].[sp_GetBillingReceipt]
	@PaymentID int,
	@Type varchar(50)
AS
BEGIN
	
	SET NOCOUNT ON

	if @Type = 'Subscription'
		begin
			select 
				RegID as PaymentID, 
				DateAdded as Paymentdate, 
				SubscriptionFee as Amount,   
				('xxxxxxxx' + CardNo) as CardNo, 
				CardType, 
				CardHolderName, 
				'Subscription Fee ' + CONVERT(varchar(10), DateAdded, 101) + ' - ' +  CONVERT(varchar(10),(DateAdd(Month, 1, DateAdded)-1),101) as description 
			from UsersRegistration WITH(NOLOCK)
			where RegID = @PaymentID
		end	
	else if @Type = 'Order'
		begin
			select 
				OrderID as PaymentID, 
				OrderDate as Paymentdate, 
				OrderTotal as amount,   
				('xxxxxxxx' + CardNo) as CardNo,
				CardType, 
				CardHolderName, 
				'Download Charges'  as description 
			from Orders WITH(NOLOCK)
			where OrderID = @PaymentID 
		end
	else if @Type = 'SubMonthly'
		Begin
			select 
				PaymentID as PaymentID, 
				DateAdded as Paymentdate, 
				SubscriptionFee as Amount,   
				('xxxxxxxx' + CardNo) as CardNo,
				CardType, 
				CardHolderName, 
				'Subscription Fee ' as description 
			from BillingHistory WITH(NOLOCK)
			where PaymentID = @PaymentID	
		end

END