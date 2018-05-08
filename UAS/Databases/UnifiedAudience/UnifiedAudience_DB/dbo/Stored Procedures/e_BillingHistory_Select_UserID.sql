CREATE PROCEDURE [dbo].[e_BillingHistory_Select_UserID]
	@UserID uniqueidentifier
	
AS
BEGIN

	set nocount on

	select 
		RegID as PaymentID, 
		DateAdded as Paymentdate, 
		SubscriptionFee as amount,   
		('xxxxxxxx' + CardNo) as CardNo, 
		CardType, CardHolderName, 
		'Subscription Fee ' + CONVERT(varchar(10), DateAdded, 101) + ' - ' +  CONVERT(varchar(10),(DateAdd(Month, 1, DateAdded)-1),101) as description, 
		'Subscription' as ptype
	from UsersRegistration with(nolock)  
	where UserID = @UserID union all
	select 
		PaymentID as PaymentID, 
		DateAdded as Paymentdate, 
		SubscriptionFee as amount,   
		('xxxxxxxx' + CardNo) as CardNo, 
		CardType, CardHolderName, 
		'Subscription Fee ' as description, 
		'SubMonthly' as ptype
	from BillingHistory with(nolock)   
	where UserID = @UserID union all	
	select 
		OrderID as PaymentID, 
		OrderDate as Paymentdate, 
		OrderTotal as amount,   
		('xxxxxxxx' + CardNo) as CardNo, 
		CardType, 
		CardHolderName, 
		'Download Charges'  as description, 
		'Order' as ptype
	from Orders with(nolock)
	where orderTotal > 0 and  UserID = @UserID order by 2 desc 

END