create procedure e_DataCompareCostUser_Select_UserId
@UserId int
as
BEGIN

	set nocount on

	select * 
	from DataCompareCostUser with(nolock)
	where UserId = @UserId

END
go