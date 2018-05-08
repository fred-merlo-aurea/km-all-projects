CREATE FUNCTION [dbo].[fn_getGroupNames]
(
	@GroupIDs varchar(2000)
)
RETURNS varchar(4000)
AS
BEGIN
	declare @groupnames varchar(4000)
	set @groupnames = ''

	select @groupnames = @groupnames + (case when @groupnames='' then'' else '<BR>' end) + COALESCE(GroupName,' ')   
	from Groups 
	where GroupID in ((SELECT Items FROM dbo.fn_Split(@GroupIDs, ',')))

	return @groupnames

END
