create procedure e_Account_Select_KMFtpFolder
@KMFtpFolder varchar(50)
as
BEGIN

	set nocount on

	select * 
	from Account with(nolock)
	where KMFtpFolder = @KMFtpFolder

END
go