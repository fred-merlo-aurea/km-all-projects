Create FUNCTION [dbo].[fn_getResponseValues]
(
	@ResponseIDs varchar(800)
)
RETURNS varchar(4000)
AS
BEGIN
	declare @ResponseValues varchar(4000)
	set @ResponseValues = ''

	select @ResponseValues = @ResponseValues + (case when @ResponseValues='' then'' else ', ' end) + COALESCE(ResponseCode,' ')   
	from Response
	where ResponseID in ((SELECT Items FROM dbo.fn_Split(@ResponseIDs, ',')))

	return @ResponseValues

END