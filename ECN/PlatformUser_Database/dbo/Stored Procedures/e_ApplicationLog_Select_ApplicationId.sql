CREATE PROCEDURE [dbo].[e_ApplicationLog_Select_ApplicationId]
@ApplicationId int
AS
	select *
	from ApplicationLog with(nolock)
	where ApplicationId = @ApplicationId
	order by LogAddedDate, LogAddedTime
go

