CREATE PROCEDURE [dbo].[o_GetIssueDates]
	@ProductID int
AS
BEGIN

	SET NOCOUNT ON

	SELECT (CASE WHEN getdate() > convert(datetime,YearStartDate + '/' + convert(varchar,year(getdate()))) 
	THEN convert(varchar(5),year(GETDATE())) ELSE convert(varchar(5),year(GETDATE()) - 1) END)
	as YearStartDate, year(dateadd(yy,1,(CASE WHEN getdate() > convert(datetime,YearStartDate + '/' + convert(varchar,year(getdate()))) 
	THEN convert(varchar(5),year(GETDATE())) ELSE convert(varchar(5),year(GETDATE()) - 1) END)))
	as YearEndDate, YearStartDate as 'MonthDayStartDate', YearEndDate as 'MonthDayEndDate', PubID
	FROM Pubs
	WHERE IsCirc = 1 AND PubID = @ProductID

END