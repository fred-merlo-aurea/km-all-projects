
Create  proc [dbo].[sp_GetColumnLegend]
(
	@ReportID int
)
as
Begin
	select	ResponseCode as column_value,
			DisplayName as column_description
	from response rs join PublicationReports r on rs.PublicationID = r.PublicationID and rs.ResponseName = r.[Column]
	where r.reportID = @reportID order by DisplayName
end