create procedure e_AcsFileDetail_Select_ProcessCode
@ProcessCode varchar(50)
as
	select *
	from AcsFileDetail with(nolock)
	where ProcessCode = @ProcessCode
go
