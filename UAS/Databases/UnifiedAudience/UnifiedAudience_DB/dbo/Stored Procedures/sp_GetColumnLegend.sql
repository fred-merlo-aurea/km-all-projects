Create  proc [dbo].[sp_GetColumnLegend]
(
	@ReportID int
)
as
BEGIN
	
	SET NOCOUNT ON

	select	ResponseCode as column_value, DisplayName as column_description
	from response rs  WITH(NOLOCK)
		join PublicationReports r WITH(NOLOCK) on rs.PublicationID = r.PublicationID and rs.ResponseName = r.[Column]
	where r.reportID = @reportID order by DisplayName

end