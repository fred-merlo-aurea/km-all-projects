
create procedure e_ImportSubscriber_Select_accountId_IsMergedToUAD
@accountId int,
@IsMergedToUAD int
as
BEGIN

	set nocount on

	select * 
	from ImportSubscriber with(nolock)
	where account_id = @accountId
	and IsMergedToUAD = @IsMergedToUAD

END