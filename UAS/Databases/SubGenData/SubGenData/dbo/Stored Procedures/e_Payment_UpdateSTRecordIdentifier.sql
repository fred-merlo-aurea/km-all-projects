create procedure e_Payment_UpdateSTRecordIdentifier
@orderId int,
@STRecordIdentifier uniqueidentifier
as
BEGIN

	set nocount on

	update Payment
	set STRecordIdentifier = @STRecordIdentifier
	where order_id = @orderId

END
go