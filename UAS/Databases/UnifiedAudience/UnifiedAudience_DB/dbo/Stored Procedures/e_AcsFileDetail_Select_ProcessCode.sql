create procedure e_AcsFileDetail_Select_ProcessCode
@ProcessCode varchar(50)
as
BEGIN

	set nocount on

	select *
	from AcsFileDetail with(nolock)
	where ProcessCode = @ProcessCode

END
go