create procedure e_Product_Select_PubCode
@PubCode varchar(50)
as
BEGIN

	set nocount on

	select *
	from Pubs with(nolock)
	where PubCode = @PubCode

END
go