create procedure e_ProductAudit_Select_ProductId
@ProductId int
as
BEGIN

	set nocount on

	select *
	from ProductAudit with(nolock)
	where ProductId = @ProductId
	order by AuditField

END
go