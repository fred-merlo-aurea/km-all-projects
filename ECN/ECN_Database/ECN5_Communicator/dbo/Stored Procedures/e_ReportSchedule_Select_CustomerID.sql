CREATE  PROC [dbo].[e_ReportSchedule_Select_CustomerID]
(
	@CustomerID int
) 
AS 
BEGIN
	select rs.*, r.ReportName from ReportSchedule rs with (NOLOCK)
	join Reports r with (NOLOCK)
	on r.ReportID= rs.ReportID
	where CustomerID = @CustomerID
	and IsDeleted=0
	order by ReportName 
END