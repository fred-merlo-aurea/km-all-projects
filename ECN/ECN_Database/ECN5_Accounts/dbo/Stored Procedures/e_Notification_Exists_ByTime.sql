CREATE PROCEDURE [dbo].[e_Notification_Exists_ByTime]
@NotificationID int,
@StartDate varchar(10),
@StartTime varchar(10),
@EndDate varchar(10), 
@EndTime varchar(10)
AS
BEGIN
	if exists (select top 1 NotificationID FROM Notification WITH (NOLOCK)  where IsDeleted = 0 and NotificationID <> @NotificationID and 
		(CAST(CAST(@StartDate AS DATE) AS DATETIME) + CAST(@StartTime AS TIME) between CAST(CAST(startdate AS DATE) AS DATETIME) + CAST(starttime AS TIME)  and  CAST(CAST(enddate AS DATE) AS DATETIME) + CAST(endtime AS TIME) or
		CAST(CAST(@EndDate AS DATE) AS DATETIME) + CAST(@EndTime AS TIME) between CAST(CAST(startdate AS DATE) AS DATETIME) + CAST(starttime AS TIME)  and  CAST(CAST(enddate AS DATE) AS DATETIME) + CAST(endtime AS TIME))) select 1 else select 0 
END

