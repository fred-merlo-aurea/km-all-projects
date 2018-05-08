create PROCEDURE [dbo].[e_FilterPenetrationReports_Delete]
(
	@ReportID int
)	
AS
BEGIN

	SET NOCOUNT ON

	update FilterPenetrationReports 
	set IsDeleted  = 1 
	where ReportID = @ReportID;
	
END