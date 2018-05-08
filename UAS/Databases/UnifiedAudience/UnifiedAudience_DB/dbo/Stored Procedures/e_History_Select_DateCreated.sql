CREATE PROCEDURE e_History_Select_DateCreated
@StartDate date,
@EndDate date
AS
BEGIN

	SET NOCOUNT ON

	SELECT * 
	FROM History With(NoLock) 
	WHERE DateCreated BETWEEN @StartDate AND @EndDate

END