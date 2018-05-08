CREATE PROCEDURE [dbo].[v_PerformanceByDayAndTimeReport] 

@CustomerID int,
@StartDate date,
@EndDate date,
@FilterOne varchar(20),
@FilterOneVal int,
@FilterTwo varchar(20),
@FilterTwoVal int = null
AS
SET NOCOUNT ON
BEGIN


Declare @BlastCounts table ([Day] int, Hour int, Emails Int default 0,opens decimal(11,2) default 0, clicks decimal(11,2) default 0)



INSERT INTO @BlastCounts ([Day], Hour) Values (1,0),(1,1),(1,2),(1,3),(1,4),(1,5),(1,6),(1,7),(1,8),(1,9),(1,10),(1,11),(1,12),(1,13),(1,14),(1,15),(1,16),(1,17),(1,18),(1,19),(1,20),(1,21),(1,22),(1,23)
INSERT INTO @BlastCounts ([Day], Hour) Values (2,0),(2,1),(2,2),(2,3),(2,4),(2,5),(2,6),(2,7),(2,8),(2,9),(2,10),(2,11),(2,12),(2,13),(2,14),(2,15),(2,16),(2,17),(2,18),(2,19),(2,20),(2,21),(2,22),(2,23)
INSERT INTO @BlastCounts ([Day], Hour) Values (3,0),(3,1),(3,2),(3,3),(3,4),(3,5),(3,6),(3,7),(3,8),(3,9),(3,10),(3,11),(3,12),(3,13),(3,14),(3,15),(3,16),(3,17),(3,18),(3,19),(3,20),(3,21),(3,22),(3,23)
INSERT INTO @BlastCounts ([Day], Hour) Values (4,0),(4,1),(4,2),(4,3),(4,4),(4,5),(4,6),(4,7),(4,8),(4,9),(4,10),(4,11),(4,12),(4,13),(4,14),(4,15),(4,16),(4,17),(4,18),(4,19),(4,20),(4,21),(4,22),(4,23)
INSERT INTO @BlastCounts ([Day], Hour) Values (5,0),(5,1),(5,2),(5,3),(5,4),(5,5),(5,6),(5,7),(5,8),(5,9),(5,10),(5,11),(5,12),(5,13),(5,14),(5,15),(5,16),(5,17),(5,18),(5,19),(5,20),(5,21),(5,22),(5,23)
INSERT INTO @BlastCounts ([Day], Hour) Values (6,0),(6,1),(6,2),(6,3),(6,4),(6,5),(6,6),(6,7),(6,8),(6,9),(6,10),(6,11),(6,12),(6,13),(6,14),(6,15),(6,16),(6,17),(6,18),(6,19),(6,20),(6,21),(6,22),(6,23)
INSERT INTO @BlastCounts ([Day], Hour) Values (7,0),(7,1),(7,2),(7,3),(7,4),(7,5),(7,6),(7,7),(7,8),(7,9),(7,10),(7,11),(7,12),(7,13),(7,14),(7,15),(7,16),(7,17),(7,18),(7,19),(7,20),(7,21),(7,22),(7,23)

Declare @Opens table ([Day] int, Hour int, Opens int)
Declare @Clicks table ([Day] int, Hour int, Clicks int)

IF UPPER(@FilterOne) = 'CAMPAIGNID'
Begin
	IF UPPER(@FilterTwo) = 'GROUPID'
	Begin
		Insert into @Opens
		Select 
			DATEPART(weekday,o.OpenTime),
			Datepart(hour, OpenTime) as hour, 
			COUNT(*) as Opens
			From
				ECN_Activity.dbo.BlastActivityOpens o WITH (NOLOCK)
				JOIN ECN5_COMMUNICATOR..Blast b WITH (NOLOCK) on b.BlastID = o.BlastID 
				JOIN ECN5_Communicator.dbo.campaignItemBlast cib  WITH (NOLOCK) ON cib.BlastID = b.BlastID
				JOIN ECN5_Communicator.dbo.CampaignItem ci  WITH (NOLOCK) on ci.CampaignItemID = cib.CampaignItemID
			where
				ci.CampaignID = @FilterOneVal and b.GroupID = @FilterTwoVal 
				and CAST(b.sendtime as date) between @StartDate and @EndDate 
				and b.CustomerID = @CustomerID
			Group By Datepart(hour, OpenTime), DATEPART(weekday,o.OpenTime)
			order by DATEPART(WEEKDAY,o.OpenTime)

		Insert into @Clicks
		Select 
			DATEPART(weekday,c.ClickTime),
			Datepart(hour, ClickTime) as hour, 
			COUNT(*) as Clicks
			From
				ECN_Activity.dbo.BlastActivityClicks c WITH (NOLOCK)
				JOIN ECN5_COMMUNICATOR..Blast b WITH (NOLOCK) on b.BlastID = c.BlastID 
				JOIN ECN5_Communicator.dbo.campaignItemBlast cib  WITH (NOLOCK) ON cib.BlastID = b.BlastID
				JOIN ECN5_Communicator.dbo.CampaignItem ci  WITH (NOLOCK) on ci.CampaignItemID = cib.CampaignItemID
			where
				ci.CampaignID = @FilterOneVal and b.GroupID = @FilterTwoVal
				and CAST(b.SendTime as date) between @StartDate and @EndDate and
				b.CustomerID = @CustomerID
			Group By Datepart(hour, ClickTime), DATEPART(weekday,c.ClickTime)
			order by DATEPART(WEEKDAY,c.ClickTime)
	End
	ELSE
	Begin
		Insert into @Opens
		Select 
			DATEPART(weekday,o.OpenTime),
			Datepart(hour, OpenTime) as hour, 
			COUNT(*) as Opens
			From
				ECN_Activity.dbo.BlastActivityOpens o WITH (NOLOCK)
				JOIN ECN5_COMMUNICATOR..Blast b WITH (NOLOCK) on b.BlastID = o.BlastID 
				JOIN ECN5_Communicator.dbo.campaignItemBlast cib  WITH (NOLOCK) ON cib.BlastID = b.BlastID
				JOIN ECN5_Communicator.dbo.CampaignItem ci  WITH (NOLOCK) on ci.CampaignItemID = cib.CampaignItemID
			where
				ci.CampaignID = @FilterOneVal
				and CAST(b.sendtime as date) between @StartDate and @EndDate 
				and b.CustomerID = @CustomerID
			Group By Datepart(hour, OpenTime), DATEPART(weekday,o.OpenTime)
			order by DATEPART(WEEKDAY,o.OpenTime)
		Insert into @Clicks
		Select 
			DATEPART(weekday,c.ClickTime),
			Datepart(hour, ClickTime) as hour, 
			COUNT(*) as Clicks
			From
				ECN_Activity.dbo.BlastActivityClicks c WITH (NOLOCK)
				JOIN ECN5_COMMUNICATOR..Blast b WITH (NOLOCK) on b.BlastID = c.BlastID 
				JOIN ECN5_Communicator.dbo.campaignItemBlast cib  WITH (NOLOCK) ON cib.BlastID = b.BlastID
				JOIN ECN5_Communicator.dbo.CampaignItem ci  WITH (NOLOCK) on ci.CampaignItemID = cib.CampaignItemID
			where
				ci.CampaignID = @FilterOneVal
				and CAST(b.sendtime as date) between @StartDate and @EndDate 
				and b.CustomerID = @CustomerID
			Group By Datepart(hour, ClickTime), DATEPART(weekday,c.ClickTime)
			order by DATEPART(WEEKDAY,c.ClickTime)
	End
End
ELSE IF UPPER(@FilterOne) = 'LayoutID'
Begin
	IF UPPER(@FilterTwo) = 'GROUPID'
	Begin
		Insert into @Opens
		Select 
			DATEPART(weekday,o.OpenTime),
			Datepart(hour, OpenTime) as hour, 
			COUNT(*) as Opens
			From
				ECN_Activity.dbo.BlastActivityOpens o WITH (NOLOCK)
				JOIN ECN5_COMMUNICATOR..Blast b WITH (NOLOCK) on b.BlastID = o.BlastID 
				JOIN ECN5_Communicator.dbo.Layout l  WITH (NOLOCK) ON l.LayoutID = b.LayoutID
			where
				l.LayoutID = @FilterOneVal and b.GroupID = @FilterTwoVal
				and CAST(b.sendtime as date) between @StartDate and @EndDate 
				and b.CustomerID = @CustomerID
			Group By Datepart(hour, OpenTime), DATEPART(weekday,o.OpenTime)
			order by DATEPART(WEEKDAY,o.OpenTime)
		Insert into @Clicks
		Select 
			DATEPART(weekday,c.ClickTime),
			Datepart(hour, ClickTime) as hour, 
			COUNT(*) as Clicks
			From
				ECN_Activity.dbo.BlastActivityClicks c WITH (NOLOCK)
				JOIN ECN5_COMMUNICATOR..Blast b WITH (NOLOCK) on b.BlastID = c.BlastID 
				join ECN5_COMMUNICATOR.dbo.Layout l WITH (NOLOCK) on l.LayoutID = b.LayoutID
			where
				l.LayoutID = @FilterOneVal and b.GroupID = @FilterTwoVal
				and CAST(b.sendtime as date) between @StartDate and @EndDate 
				and b.CustomerID = @CustomerID
			Group By Datepart(hour, ClickTime), DATEPART(weekday,c.ClickTime)
			order by DATEPART(WEEKDAY,c.ClickTime)
	End
	ELSE
	Begin
		Insert into @Opens
		Select 
			DATEPART(weekday,o.OpenTime),
			Datepart(hour, OpenTime) as hour, 
			COUNT(*) as Opens
			From
				ECN_Activity.dbo.BlastActivityOpens o WITH (NOLOCK)
				JOIN ECN5_COMMUNICATOR..Blast b WITH (NOLOCK) on b.BlastID = o.BlastID 
				JOIN ECN5_Communicator.dbo.Layout l  WITH (NOLOCK) ON l.LayoutID = b.LayoutID
			where
				l.LayoutID = @FilterOneVal
				and CAST(b.sendtime as date) between @StartDate and @EndDate 
				and b.CustomerID = @CustomerID
			Group By Datepart(hour, OpenTime), DATEPART(weekday,o.OpenTime)
			order by DATEPART(WEEKDAY,o.OpenTime)
		Insert into @Clicks
		Select 
			DATEPART(weekday,c.ClickTime),
			Datepart(hour, ClickTime) as hour, 
			COUNT(*) as Clicks
			From
				ECN_Activity.dbo.BlastActivityClicks c WITH (NOLOCK)
				JOIN ECN5_COMMUNICATOR..Blast b WITH (NOLOCK) on b.BlastID = c.BlastID 
				join ECN5_COMMUNICATOR.dbo.Layout l WITH (NOLOCK) on l.LayoutID = b.LayoutID
			where
				l.LayoutID = @FilterOneVal
				and CAST(b.sendtime as date) between @StartDate and @EndDate 
				and b.CustomerID = @CustomerID
			Group By Datepart(hour, ClickTime), DATEPART(weekday,c.ClickTime)
			order by DATEPART(WEEKDAY,c.ClickTime)
	End
End
ELSE IF UPPER(@FilterOne) = 'MESSAGETYPEID'
Begin
	IF UPPER(@FilterTwo) = 'GROUPID'
	Begin
		Insert into @Opens
		Select 
			DATEPART(weekday,o.OpenTime),
			Datepart(hour, OpenTime) as hour, 
			COUNT(*) as Opens
			From
				ECN_Activity.dbo.BlastActivityOpens o WITH (NOLOCK)
				JOIN ECN5_COMMUNICATOR..Blast b WITH (NOLOCK) on b.BlastID = o.BlastID 
				JOIN ECN5_Communicator.dbo.Layout l WITH (NOLOCK)  on l.LayoutID = B.LayoutID 
				JOIN ECN5_Communicator.dbo.MessageType mt WITH (NOLOCK)  on mt.MessageTypeID = l.MessageTypeID
			where
				mt.MessageTypeID = @FilterOneVal and b.GroupID = @FilterTwoVal 
				and CAST(b.sendtime as date) between @StartDate and @EndDate 
				and b.CustomerID = @CustomerID
			Group By Datepart(hour, OpenTime), DATEPART(weekday,o.OpenTime)
			order by DATEPART(WEEKDAY,o.OpenTime)

		Insert into @Clicks
		Select 
			DATEPART(weekday,c.ClickTime),
			Datepart(hour, ClickTime) as hour, 
			COUNT(*) as Clicks
			From
				ECN_Activity.dbo.BlastActivityClicks c WITH (NOLOCK)
				JOIN ECN5_COMMUNICATOR..Blast b WITH (NOLOCK) on b.BlastID = c.BlastID 
				JOIN ECN5_Communicator.dbo.Layout l WITH (NOLOCK)  on l.LayoutID = B.LayoutID 
				JOIN ECN5_Communicator.dbo.MessageType mt WITH (NOLOCK)  on mt.MessageTypeID = l.MessageTypeID
			where
				mt.MessageTypeID = @FilterOneVal and b.GroupID = @FilterTwoVal 
				and CAST(b.sendtime as date) between @StartDate and @EndDate 
				and b.CustomerID = @CustomerID
			Group By Datepart(hour, ClickTime), DATEPART(weekday,c.ClickTime)
			order by DATEPART(WEEKDAY,c.ClickTime)
	End
	ELSE
	begin
		Insert into @Opens
		Select 
			DATEPART(weekday,o.OpenTime),
			Datepart(hour, OpenTime) as hour, 
			COUNT(*) as Opens
			From
				ECN_Activity.dbo.BlastActivityOpens o WITH (NOLOCK)
				JOIN ECN5_COMMUNICATOR..Blast b WITH (NOLOCK) on b.BlastID = o.BlastID 
				JOIN ECN5_Communicator.dbo.Layout l WITH (NOLOCK)  on l.LayoutID = B.LayoutID 
				JOIN ECN5_Communicator.dbo.MessageType mt WITH (NOLOCK)  on mt.MessageTypeID = l.MessageTypeID
			where
				mt.MessageTypeID = @FilterOneVal
				and CAST(b.sendtime as date) between @StartDate and @EndDate 
				and b.CustomerID = @CustomerID
			Group By Datepart(hour, OpenTime), DATEPART(weekday,o.OpenTime)
			order by DATEPART(WEEKDAY,o.OpenTime)

		Insert into @Clicks
		Select 
			DATEPART(weekday,c.ClickTime),
			Datepart(hour, ClickTime) as hour, 
			COUNT(*) as Clicks
			From
				ECN_Activity.dbo.BlastActivityClicks c WITH (NOLOCK)
				JOIN ECN5_COMMUNICATOR..Blast b WITH (NOLOCK) on b.BlastID = c.BlastID 
				JOIN ECN5_Communicator.dbo.Layout l WITH (NOLOCK)  on l.LayoutID = B.LayoutID 
				JOIN ECN5_Communicator.dbo.MessageType mt WITH (NOLOCK)  on mt.MessageTypeID = l.MessageTypeID
			where
				mt.MessageTypeID = @FilterOneVal
				and CAST(b.sendtime as date) between @StartDate and @EndDate 
				and b.CustomerID = @CustomerID
			Group By Datepart(hour, ClickTime), DATEPART(weekday,c.ClickTime)
			order by DATEPART(WEEKDAY,c.ClickTime)
	end
End
ElSE IF UPPER(@FilterOne) = 'GROUPID'
Begin
	IF UPPER(@FilterTwo) = 'CAMPAIGNID'
	Begin
		Insert into @Opens
		Select 
			DATEPART(weekday,o.OpenTime),
			Datepart(hour, OpenTime) as hour, 
			COUNT(*) as Opens
			From
				ECN_Activity.dbo.BlastActivityOpens o WITH (NOLOCK)
				JOIN ECN5_COMMUNICATOR..Blast b WITH (NOLOCK) on b.BlastID = o.BlastID 
				JOIN ECN5_Communicator.dbo.campaignItemBlast cib  WITH (NOLOCK) ON cib.BlastID = b.BlastID
				JOIN ECN5_Communicator.dbo.CampaignItem ci  WITH (NOLOCK) on ci.CampaignItemID = cib.CampaignItemID
			where
				b.GroupID = @FilterOneVal and ci.CampaignID = @FilterTwoVal 
				and CAST(b.sendtime as date) between @StartDate and @EndDate 
				and b.CustomerID = @CustomerID
			Group By Datepart(hour, OpenTime), DATEPART(weekday,o.OpenTime)
			order by DATEPART(WEEKDAY,o.OpenTime)

		Insert into @Clicks
		Select 
			DATEPART(weekday,c.ClickTime),
			Datepart(hour, ClickTime) as hour, 
			COUNT(*) as Clicks
			From
				ECN_Activity.dbo.BlastActivityClicks c WITH (NOLOCK)
				JOIN ECN5_COMMUNICATOR..Blast b WITH (NOLOCK) on b.BlastID = c.BlastID 
				JOIN ECN5_Communicator.dbo.campaignItemBlast cib  WITH (NOLOCK) ON cib.BlastID = b.BlastID
				JOIN ECN5_Communicator.dbo.CampaignItem ci  WITH (NOLOCK) on ci.CampaignItemID = cib.CampaignItemID
			where
				b.GroupID = @FilterOneVal and ci.CampaignID = @FilterTwoVal
				and CAST(b.sendtime as date) between @StartDate and @EndDate 
				and b.CustomerID = @CustomerID
			Group By Datepart(hour, ClickTime), DATEPART(weekday,c.ClickTime)
			order by DATEPART(WEEKDAY,c.ClickTime)

	End
	ELSE IF UPPER(@FilterTwo) = 'LayoutID'
	Begin
		Insert into @Opens
		Select 
			DATEPART(weekday,o.OpenTime),
			Datepart(hour, OpenTime) as hour, 
			COUNT(*) as Opens
			From
				ECN_Activity.dbo.BlastActivityOpens o WITH (NOLOCK)
				JOIN ECN5_COMMUNICATOR..Blast b WITH (NOLOCK) on b.BlastID = o.BlastID 
				JOIN ECN5_Communicator.dbo.Layout l  WITH (NOLOCK) ON l.LayoutID = b.LayoutID
			where
				b.GroupID = @FilterOneVal and l.LayoutID = @FilterTwoVal
				and CAST(b.sendtime as date) between @StartDate and @EndDate 
				and b.CustomerID = @CustomerID
			Group By Datepart(hour, OpenTime), DATEPART(weekday,o.OpenTime)
			order by DATEPART(WEEKDAY,o.OpenTime)

		Insert into @Clicks
		Select 
			DATEPART(weekday,c.ClickTime),
			Datepart(hour, ClickTime) as hour, 
			COUNT(*) as Clicks
			From
				ECN_Activity.dbo.BlastActivityClicks c WITH (NOLOCK)
				JOIN ECN5_COMMUNICATOR..Blast b WITH (NOLOCK) on b.BlastID = c.BlastID 
				join ECN5_COMMUNICATOR.dbo.Layout l WITH (NOLOCK) on l.LayoutID = b.LayoutID
			where
				b.GroupID = @FilterOneVal and l.LayoutID = @FilterTwoVal
				and CAST(b.sendtime as date) between @StartDate and @EndDate 
				and b.CustomerID = @CustomerID
			Group By Datepart(hour, ClickTime), DATEPART(weekday,c.ClickTime)
			order by DATEPART(WEEKDAY,c.ClickTime)

	End
	ELSE IF UPPER(@FilterTwo) = 'MESSAGETYPEID'
	Begin
		Insert into @Opens
		Select 
			DATEPART(weekday,o.OpenTime),
			Datepart(hour, OpenTime) as hour, 
			COUNT(*) as Opens
			From
				ECN_Activity.dbo.BlastActivityOpens o WITH (NOLOCK)
				JOIN ECN5_COMMUNICATOR..Blast b WITH (NOLOCK) on b.BlastID = o.BlastID 
				JOIN ECN5_Communicator.dbo.Layout l WITH (NOLOCK)  on l.LayoutID = B.LayoutID 
				JOIN ECN5_Communicator.dbo.MessageType mt WITH (NOLOCK)  on mt.MessageTypeID = l.MessageTypeID
			where
				b.GroupID = @FilterOneVal and mt.MessageTypeID = @FilterTwoVal 
				and CAST(b.sendtime as date) between @StartDate and @EndDate 
				and b.CustomerID = @CustomerID
			Group By Datepart(hour, OpenTime), DATEPART(weekday,o.OpenTime)
			order by DATEPART(WEEKDAY,o.OpenTime)

		Insert into @Clicks
		Select 
			DATEPART(weekday,c.ClickTime),
			Datepart(hour, ClickTime) as hour, 
			COUNT(*) as Clicks
			From
				ECN_Activity.dbo.BlastActivityClicks c WITH (NOLOCK)
				JOIN ECN5_COMMUNICATOR..Blast b WITH (NOLOCK) on b.BlastID = c.BlastID 
				JOIN ECN5_Communicator.dbo.Layout l WITH (NOLOCK)  on l.LayoutID = B.LayoutID 
				JOIN ECN5_Communicator.dbo.MessageType mt WITH (NOLOCK)  on mt.MessageTypeID = l.MessageTypeID
			where
				b.GroupID = @FilterOneVal and mt.MessageTypeID = @FilterTwoVal
				and CAST(b.sendtime as date) between @StartDate and @EndDate 
				and b.CustomerID = @CustomerID
			Group By Datepart(hour, ClickTime), DATEPART(weekday,c.ClickTime)
			order by DATEPART(WEEKDAY,c.ClickTime)

	End
	ELSE
	Begin
		Insert into @Opens
		Select 
			DATEPART(weekday,o.OpenTime),
			Datepart(hour, OpenTime) as hour, 
			COUNT(*) as Opens
			From
				ECN_Activity.dbo.BlastActivityOpens o WITH (NOLOCK)
				JOIN ECN5_COMMUNICATOR..Blast b WITH (NOLOCK) on b.BlastID = o.BlastID 
			where
				b.GroupID = @FilterOneVal 
				and CAST(b.sendtime as date) between @StartDate and @EndDate 
				and b.CustomerID = @CustomerID
			Group By Datepart(hour, OpenTime), DATEPART(weekday,o.OpenTime)
			order by DATEPART(WEEKDAY,o.OpenTime)

		Insert into @Clicks
		Select 
			DATEPART(weekday,c.ClickTime),
			Datepart(hour, ClickTime) as hour, 
			COUNT(*) as Clicks
			From
				ECN_Activity.dbo.BlastActivityClicks c WITH (NOLOCK)
				JOIN ECN5_COMMUNICATOR..Blast b WITH (NOLOCK) on b.BlastID = c.BlastID 
			where
				b.GroupID = @FilterOneVal 
				and CAST(b.sendtime as date) between @StartDate and @EndDate 
				and b.CustomerID = @CustomerID
			Group By Datepart(hour, ClickTime), DATEPART(weekday,c.ClickTime)
			order by DATEPART(WEEKDAY,c.ClickTime)
	End
End
   
   
   
   
   UPDATE B
SET         
                Opens = o.Opens
FROM 
                @BlastCounts b
                JOIN @Opens o on b.Hour = o.Hour and b.[day] = o.[day]

UPDATE B
SET         
                Clicks = c.Clicks
FROM 
                @BlastCounts b
                JOIN @Clicks c on b.Hour = c.Hour and b.[day] = c.[day];

declare @openTotal decimal (15,2) = (Select SUM(opens) From @BlastCounts)
declare @clickTotal decimal (15,2) = (Select SUM(clicks) From @BlastCounts);


WITH OutputTable (DayGroup,HourGroup,Opens,Clicks) AS 
(
SELECT 
      case [Day] 
      when 2 then 'Mon' 
      when 3 then 'Tue' 
      when 4 then 'Wed' 
      when 5 then 'Thur' 
      when 6 then 'Fri' 
      when 7 then 'Sat' 
      when 1 then 'Sun' 
      end AS DayGroup,
      CASE 
      WHEN HOUR IN (0,1,2,3,4,5) THEN 'Midnight-6'
      WHEN HOUR IN (6,7) THEN '6-8 AM'
      WHEN HOUR IN (8,9) THEN '8-10 AM'
      WHEN HOUR IN (10,11) THEN '10-12 PM'
      WHEN HOUR IN (12,13) THEN '12-2 PM'
      WHEN HOUR IN (14,15) THEN '2-4 PM'
      WHEN HOUR IN (16,17) THEN '4-6 PM'
      WHEN HOUR IN (17,18) THEN '6-8 PM'
      WHEN HOUR IN (19,20,21,22,23) THEN '8-Midnight'
      END AS HourGroup,
      Case (@openTotal)
      when 0 then 0
      else cast(SUM(Opens)/(@openTotal) * 100 as decimal(5,2)) end As Opens,
      Case (@clickTotal)
      when 0 then 0
      else cast(SUM(clicks)/(@clickTotal) * 100 as decimal(5,2)) end As Clicks
FROM
      @BlastCounts
GROUP BY  
      case [Day] 
      when 2 then 'Mon' 
      when 3 then 'Tue' 
      when 4 then 'Wed' 
      when 5 then 'Thur' 
      when 6 then 'Fri' 
      when 7 then 'Sat' 
      when 1 then 'Sun' 
      end,
      CASE 
      WHEN HOUR IN (0,1,2,3,4,5) THEN 'Midnight-6'
      WHEN HOUR IN (6,7) THEN '6-8 AM'
      WHEN HOUR IN (8,9) THEN '8-10 AM'
      WHEN HOUR IN (10,11) THEN '10-12 PM'
      WHEN HOUR IN (12,13) THEN '12-2 PM'
      WHEN HOUR IN (14,15) THEN '2-4 PM'
      WHEN HOUR IN (16,17) THEN '4-6 PM'
      WHEN HOUR IN (17,18) THEN '6-8 PM'
      WHEN HOUR IN (19,20,21,22,23) THEN '8-Midnight'
      end
)   

SELECT 
	  DayGroup,
      HourGroup,
      convert(varchar(10),Opens) as Opens,
      convert(varchar(10),Clicks) as Clicks
FROM 
      OutputTable
ORDER BY
		case DayGroup
      when 'Mon' then 2 
      when 'Tue' then 3 
      when 'Wed' then 4 
      when 'Thur' then 5 
      when 'Fri' then 6
      when 'Sat' then 7 
      when 'Sun' then 1 
      end,
      CASE HourGroup WHEN 'midnight-6' THEN 1
      WHEN '6-8 AM' THEN 2
      WHEN '8-10 AM' THEN 3
      WHEN '10-12 PM' THEN 4
      WHEN '12-2 PM' THEN 5
      WHEN '2-4 PM' THEN 6
      WHEN '4-6 PM' THEN 7
      WHEN '6-8 PM' THEN 8
      WHEN '8-midnight' THEN 9
END

END