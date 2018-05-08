CREATE PROCEDURE dbo.sp_DelMarketPenetrationReport
(
@ReportID int
)
	
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;
	
	delete 
	from PenetrationReports_Markets 
	where ReportID=@ReportID;

	delete 
	from PenetrationReports 
	where ReportID=@ReportID;

END