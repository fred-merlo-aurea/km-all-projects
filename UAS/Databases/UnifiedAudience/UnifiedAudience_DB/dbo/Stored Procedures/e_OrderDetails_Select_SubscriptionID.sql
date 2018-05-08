CREATE PROCEDURE [dbo].[e_OrderDetails_Select_SubscriptionID]
@SubscriptionID int
AS
BEGIN

	set nocount on

	select OrderDate 
	from OrderDetails od with(nolock) 
		join Orders o with(nolock) on od.OrderID = o.OrderID 
	where SubscriptionID = @SubscriptionID and o.OrderDate >(GETDATE() - 30)

END