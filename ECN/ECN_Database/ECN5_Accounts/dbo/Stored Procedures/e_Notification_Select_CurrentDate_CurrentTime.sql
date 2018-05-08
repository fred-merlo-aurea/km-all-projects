CREATE PROCEDURE [dbo].[e_Notification_Select_CurrentDate_CurrentTime]
AS
BEGIN
	SELECT *
	FROM Notification  WITH(NOLOCK)
	where IsDeleted = 0 and 
		(CAST(CAST(startdate AS DATE) AS DATETIME) +
		CAST(starttime AS TIME) <= GETDATE() and CAST(CAST(enddate AS DATE) AS DATETIME) + CAST(endtime AS TIME) >= GETDATE())
END

