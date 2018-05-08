
CREATE  PROC [dbo].[e_Reports_Select_ReportName] 
(
@ReportName varChar(50)
)
AS 
BEGIN
	select * from Reports with (NOLOCK)
	where ReportName=@ReportName
END


GO

