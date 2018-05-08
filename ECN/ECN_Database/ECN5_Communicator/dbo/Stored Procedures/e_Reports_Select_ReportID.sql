CREATE  PROC dbo.e_Reports_Select_ReportID 
(
@ReportID int
)
AS 
BEGIN
	select * from Reports with (NOLOCK)
	where ReportID=@ReportID
END