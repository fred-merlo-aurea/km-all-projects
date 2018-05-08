CREATE PROCEDURE e_History_Select_DateCreated
@StartDate date,
@EndDate date
AS
	SELECT * FROM History With(NoLock) WHERE DateCreated BETWEEN @StartDate AND @EndDate
