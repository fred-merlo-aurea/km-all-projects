create procedure e_Subscriber_Select_Dynamic
@whereClause varchar(max)
as
BEGIN

	set nocount on

	declare @sql varchar(max) = 'select * from Subscriber with(nolock) ' + @whereClause
	exec (@sql)

END
go
