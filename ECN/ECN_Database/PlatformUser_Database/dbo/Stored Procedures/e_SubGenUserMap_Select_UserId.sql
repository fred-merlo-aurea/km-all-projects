create procedure e_SubGenUserMap_Select_UserId
@UserId int
as
	select *
	from SubGenUserMap with(nolock)
	where UserID = @UserId
go
