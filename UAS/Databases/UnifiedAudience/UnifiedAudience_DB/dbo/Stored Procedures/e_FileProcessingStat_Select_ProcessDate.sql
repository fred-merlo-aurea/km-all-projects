create procedure e_FileProcessingStat_Select_ProcessDate
@ProcessDate date
as
BEGIN

	SET NOCOUNT ON

	select *
	from FileProcessingStat with(nolock)
	where ProcessDate = @ProcessDate

END
go