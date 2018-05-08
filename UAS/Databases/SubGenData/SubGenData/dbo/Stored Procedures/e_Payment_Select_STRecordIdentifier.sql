create procedure e_Payment_Select_STRecordIdentifier
@STRecordIdentifier uniqueidentifier
as
BEGIN

	set nocount on

	select *
	from Payment with(nolock)
	where STRecordIdentifier = @STRecordIdentifier

END
go