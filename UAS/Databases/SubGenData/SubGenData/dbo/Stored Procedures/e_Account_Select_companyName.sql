
create procedure e_Account_Select_companyName
@companyName varchar(50)
as
BEGIN

	set nocount on

	select * 
	from Account with(nolock)
	where company_name = @companyName

END