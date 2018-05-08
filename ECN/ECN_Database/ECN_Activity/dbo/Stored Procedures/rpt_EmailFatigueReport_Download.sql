
CREATE PROC [dbo].[rpt_EmailFatigueReport_Download]
      @CustomerId INT,
      @StartDate  DATE = NULL,
      @EndDate    DATE = NULL   ,
      @FilterField VARCHAR(100) = NULL ,
      @FilterValue VARCHAR(100) = NULL ,
      @Grouping   INT ,
      @ActionType VARCHAR(10),
      @NumberOfTouches INT
 
AS
 
SET NOCOUNT ON
     

DECLARE
      @MinSendId INT,
      @MinOpenId INT,
      @MinClickId INT,
      @NumberOfTouchesUpperBound INT,
      @GroupingUpperBound INT

set @NumberOfTouchesUpperBound = (select 	
CASE
	when @NumberOfTouches < 11 then @NumberOfTouches
	when @NumberOfTouches between 11 and 41 then @NumberOfTouches + 9
	when @NumberOfTouches = 51 then 10000
end
)
set @GroupingUpperBound = (select 	
CASE
	when @Grouping < 11 then @Grouping
	when @Grouping between 11 and 41 then @Grouping + 9
	when @Grouping = 51 then 100
end
)


SELECT @MinOpenId = MIN(Minopenid) FROM ECN5_Warehouse.dbo.BlastOpenRangeByDate WHERE CONVERT(DATE,OpenDate) >= @StartDate
SELECT @MinClickId = MIN(MinClickid) FROM ECN5_Warehouse.dbo.BlastClickRangeByDate WHERE CONVERT(DATE,ClickDate) >= @StartDate
SELECT @MinSendId =  MIN(MinSendid) FROM ECN5_Warehouse.dbo.BlastSEndRangeByDate WHERE CONVERT(DATE,SendDate) >= @StartDate
 
DECLARE @opens TABLE (EmailID INT, BlastID INT, UNIQUE CLUSTERED (EmailID,BlastID))
DECLARE @Clicks TABLE (EmailID INT, BlastID INT, UNIQUE CLUSTERED (EmailID,BlastID))
DECLARE @Sends TABLE (EmailID INT, BlastID INT, UNIQUE CLUSTERED (EmailID,BlastID))
 
 
IF @ActionType = 'Opens'
BEGIN
      INSERT INTO @opens
      SELECT DISTINCT
            bab.emailID,
            bab.BlastID
      FROM
            ECN_Activity.dbo.BlastActivityOpens bab WITH (NOLOCK)
            INNER JOIN ECN5_COMMUNICATOR..Blast b WITH (NOLOCK) ON bab.BlastID = b.blastID 
      WHERE
            CustomerID = @CustomerId
            AND TestBlast ='N'
            AND OpenID >= @MinOpenId
 
      IF @FilterField = 'GroupID'
      BEGIN
            SELECT
				EmailAddress,
				Title,
				FirstName,
				LastName,
				FullName,
				Company,
				Occupation,
				Address,
				Address2,
				City,
				State,
				Zip,
				Country,
				Voice,
				Mobile,
				Fax,
				Website,
				Age,
				Income,
				Gender,
				User1,
				User2,
				User3,
				User4,
				User5,
				User6,
				Birthdate,
				UserEvent1,
				UserEvent1Date,
				UserEvent2,
				UserEvent2Date,
				DateAdded as CreatedOn,
				DateUpdated as LastChanged
            FROM
                ECN_Activity.dbo.BlastActivitySends s WITH (NOLOCK)
                JOIN ECN5_COMMUNICATOR..Emails e WITH (NOLOCK) ON e.EmailID = s.EmailId
                JOIN ECN5_COMMUNICATOR..Blast b WITH (NOLOCK) ON b.BlastID = s.BlastID
                LEFT JOIN @opens o ON s.blastid = o.blastid and s.emailid = o.emailid
            WHERE
                b.CustomerID = @CustomerId
                and B.TestBlast ='N'               
                AND cast( b.SendTime as date) BETWEEN @StartDate AND @EndDate
                AND b.GroupID = @FilterValue
            GROUP BY
				EmailAddress,
				Title,
				FirstName,
				LastName,
				FullName,
				Company,
				Occupation,
				Address,
				Address2,
				City,
				State,
				Zip,
				Country,
				Voice,
				Mobile,
				Fax,
				Website,
				Age,
				Income,
				Gender,
				User1,
				User2,
				User3,
				User4,
				User5,
				User6,
				Birthdate,
				UserEvent1,
				UserEvent1Date,
				UserEvent2,
				UserEvent2Date,
				DateAdded ,
				DateUpdated 
            HAVING
                COUNT(DISTINCT(s.BlastID)) between @NumberOfTouches and @NumberOfTouchesUpperBound
                AND COUNT (DISTINCT(o.BlastID)) between @Grouping and @GroupingUpperBound
      END
 
      IF @FilterField = 'MessageTypeId'
      BEGIN
            SELECT
				EmailAddress,
				Title,
				FirstName,
				LastName,
				FullName,
				Company,
				Occupation,
				Address,
				Address2,
				City,
				State,
				Zip,
				Country,
				Voice,
				Mobile,
				Fax,
				Website,
				Age,
				Income,
				Gender,
				User1,
				User2,
				User3,
				User4,
				User5,
				User6,
				Birthdate,
				UserEvent1,
				UserEvent1Date,
				UserEvent2,
				UserEvent2Date,
				DateAdded as CreatedOn,
				DateUpdated as LastChanged
			FROM
				ECN_Activity.dbo.BlastActivitySends s WITH (NOLOCK)
				JOIN ECN5_COMMUNICATOR..Emails e WITH (NOLOCK) ON e.EmailID = s.EmailId
				JOIN ECN5_COMMUNICATOR..Blast b WITH (NOLOCK) ON b.BlastID = s.BlastID
				JOIN ECN5_Communicator.dbo.Layout l WITH (NOLOCK)  ON l.LayoutID = B.LayoutID
				JOIN ECN5_Communicator.dbo.MessageType mt WITH (NOLOCK)  ON mt.MessageTypeID = l.MessageTypeID
				LEFT JOIN @opens o ON s.blastid = o.blastid and s.emailid = o.emailid
            WHERE
				b.CustomerID = @CustomerId
				AND B.TestBlast = 'N'
				AND cast( b.SendTime as date) BETWEEN @StartDate AND @EndDate
				AND mt.MessageTypeID = @FilterValue
			GROUP BY
				EmailAddress,
				Title,
				FirstName,
				LastName,
				FullName,
				Company,
				Occupation,
				Address,
				Address2,
				City,
				State,
				Zip,
				Country,
				Voice,
				Mobile,
				Fax,
				Website,
				Age,
				Income,
				Gender,
				User1,
				User2,
				User3,
				User4,
				User5,
				User6,
				Birthdate,
				UserEvent1,
				UserEvent1Date,
				UserEvent2,
				UserEvent2Date,
				DateAdded ,
				DateUpdated 
            HAVING
				COUNT(DISTINCT(s.BlastID)) between @NumberOfTouches and @NumberOfTouchesUpperBound
				AND COUNT (DISTINCT(o.BlastID)) between @Grouping and @GroupingUpperBound
      END
     
      IF @FilterField = 'CampaignId'
      BEGIN
            SELECT
				EmailAddress,
				Title,
				FirstName,
				LastName,
				FullName,
				Company,
				Occupation,
				Address,
				Address2,
				City,
				State,
				Zip,
				Country,
				Voice,
				Mobile,
				Fax,
				Website,
				Age,
				Income,
				Gender,
				User1,
				User2,
				User3,
				User4,
				User5,
				User6,
				Birthdate,
				UserEvent1,
				UserEvent1Date,
				UserEvent2,
				UserEvent2Date,
				DateAdded as CreatedOn,
				DateUpdated as LastChanged
            FROM
				ECN_Activity.dbo.BlastActivitySends s WITH (NOLOCK)
				JOIN ECN5_COMMUNICATOR..Emails e WITH (NOLOCK) ON e.EmailID = s.EmailId
				JOIN ECN5_COMMUNICATOR..Blast b WITH (NOLOCK) ON b.BlastID = s.BlastID
				JOIN ECN5_Communicator.dbo.campaignItemBlast cib  WITH (NOLOCK) ON cib.BlastID = b.BlastID
				JOIN ECN5_Communicator.dbo.CampaignItem ci  WITH (NOLOCK) ON ci.CampaignItemID = cib.CampaignItemID
				JOIN ECN5_Communicator.dbo.Campaign cam  WITH (NOLOCK) ON ci.CampaignID = cam.CampaignID
				LEFT JOIN @opens o ON s.blastid = o.blastid and s.emailid = o.emailid
            WHERE
				b.CustomerID = @CustomerId
				and TestBlast ='N'                 
				AND cast( b.SendTime as date) BETWEEN @StartDate AND @EndDate
				AND cam.CampaignId = @FilterValue
            GROUP BY
				EmailAddress,
				Title,
				FirstName,
				LastName,
				FullName,
				Company,
				Occupation,
				Address,
				Address2,
				City,
				State,
				Zip,
				Country,
				Voice,
				Mobile,
				Fax,
				Website,
				Age,
				Income,
				Gender,
				User1,
				User2,
				User3,
				User4,
				User5,
				User6,
				Birthdate,
				UserEvent1,
				UserEvent1Date,
				UserEvent2,
				UserEvent2Date,
				DateAdded ,
				DateUpdated 
            HAVING
				COUNT(DISTINCT(s.BlastID)) between @NumberOfTouches and @NumberOfTouchesUpperBound
				AND COUNT (DISTINCT(o.BlastID)) between @Grouping and @GroupingUpperBound
      END
 
      IF @FilterField IS NULL
      BEGIN
            SELECT
				EmailAddress,
				Title,
				FirstName,
				LastName,
				FullName,
				Company,
				Occupation,
				Address,
				Address2,
				City,
				State,
				Zip,
				Country,
				Voice,
				Mobile,
				Fax,
				Website,
				Age,
				Income,
				Gender,
				User1,
				User2,
				User3,
				User4,
				User5,
				User6,
				Birthdate,
				UserEvent1,
				UserEvent1Date,
				UserEvent2,
				UserEvent2Date,
				DateAdded as CreatedOn,
				DateUpdated as LastChanged
            FROM
				ECN_Activity.dbo.BlastActivitySends s WITH (NOLOCK)
				JOIN ECN5_COMMUNICATOR..Blast b WITH (NOLOCK) ON b.BlastID = s.BlastID
				JOIN ECN5_COMMUNICATOR..Emails e WITH (NOLOCK) ON e.EmailID = s.EmailId
				LEFT JOIN @opens o ON s.blastid = o.blastid and s.emailid = o.emailid
            WHERE
				b.CustomerID = @CustomerId
				AND TestBlast ='N'
				AND cast( b.SendTime as date) BETWEEN @StartDate AND @EndDate
            GROUP BY
				EmailAddress,
				Title,
				FirstName,
				LastName,
				FullName,
				Company,
				Occupation,
				Address,
				Address2,
				City,
				State,
				Zip,
				Country,
				Voice,
				Mobile,
				Fax,
				Website,
				Age,
				Income,
				Gender,
				User1,
				User2,
				User3,
				User4,
				User5,
				User6,
				Birthdate,
				UserEvent1,
				UserEvent1Date,
				UserEvent2,
				UserEvent2Date,
				DateAdded ,
				DateUpdated 
            HAVING
				COUNT(DISTINCT(s.BlastID)) between @NumberOfTouches and @NumberOfTouchesUpperBound
				AND COUNT (DISTINCT(o.BlastID)) between @Grouping and @GroupingUpperBound
      END
END
 
 
IF @ActionType = 'Clicks'
BEGIN
      INSERT INTO @Clicks
      SELECT DISTINCT
            bac.emailID,
            bac.BlastID
      FROM
            ECN_Activity.dbo.BlastActivityClicks bac WITH (NOLOCK)
            INNER JOIN ECN5_COMMUNICATOR..Blast b WITH (NOLOCK) ON bac.BlastID = b.blastID 
      WHERE
            CustomerID = @CustomerId
            AND TestBlast ='N'
            AND  ClickID >= @MinClickId
 
      IF @FilterField = 'GroupID'
      BEGIN
            SELECT
				EmailAddress,
				Title,
				FirstName,
				LastName,
				FullName,
				Company,
				Occupation,
				Address,
				Address2,
				City,
				State,
				Zip,
				Country,
				Voice,
				Mobile,
				Fax,
				Website,
				Age,
				Income,
				Gender,
				User1,
				User2,
				User3,
				User4,
				User5,
				User6,
				Birthdate,
				UserEvent1,
				UserEvent1Date,
				UserEvent2,
				UserEvent2Date,
				DateAdded as CreatedOn,
				DateUpdated as LastChanged
            FROM
				ECN_Activity.dbo.BlastActivitySends s WITH (NOLOCK)
				JOIN ECN5_COMMUNICATOR..Emails e WITH (NOLOCK) ON e.EmailID = s.EmailId
				JOIN ECN5_COMMUNICATOR..Blast b WITH (NOLOCK) ON b.BlastID = s.BlastID
				LEFT JOIN @Clicks c ON s.blastid = c.blastid and s.emailid = c.emailid
            WHERE
				b.CustomerID = @CustomerId
				and B.TestBlast ='N'               
				AND cast( b.SendTime as date) BETWEEN @StartDate AND @EndDate
				AND b.GroupID = @FilterValue
            GROUP BY
				EmailAddress,
				Title,
				FirstName,
				LastName,
				FullName,
				Company,
				Occupation,
				Address,
				Address2,
				City,
				State,
				Zip,
				Country,
				Voice,
				Mobile,
				Fax,
				Website,
				Age,
				Income,
				Gender,
				User1,
				User2,
				User3,
				User4,
				User5,
				User6,
				Birthdate,
				UserEvent1,
				UserEvent1Date,
				UserEvent2,
				UserEvent2Date,
				DateAdded ,
				DateUpdated 
            HAVING
				COUNT(DISTINCT(s.BlastID)) between @NumberOfTouches and @NumberOfTouchesUpperBound
				AND COUNT (DISTINCT(c.BlastID)) between @Grouping and @GroupingUpperBound
      END
 
      IF @FilterField = 'MessageTypeId'
      BEGIN
            SELECT
				EmailAddress,
				Title,
				FirstName,
				LastName,
				FullName,
				Company,
				Occupation,
				Address,
				Address2,
				City,
				State,
				Zip,
				Country,
				Voice,
				Mobile,
				Fax,
				Website,
				Age,
				Income,
				Gender,
				User1,
				User2,
				User3,
				User4,
				User5,
				User6,
				Birthdate,
				UserEvent1,
				UserEvent1Date,
				UserEvent2,
				UserEvent2Date,
				DateAdded as CreatedOn,
				DateUpdated as LastChanged
            FROM
				ECN_Activity.dbo.BlastActivitySends s WITH (NOLOCK)
				JOIN ECN5_COMMUNICATOR..Emails e WITH (NOLOCK) ON e.EmailID = s.EmailId
				JOIN ECN5_COMMUNICATOR..Blast b WITH (NOLOCK) ON b.BlastID = s.BlastID
				JOIN ECN5_Communicator.dbo.Layout l WITH (NOLOCK)  ON l.LayoutID = B.LayoutID
				JOIN ECN5_Communicator.dbo.MessageType mt WITH (NOLOCK)  ON mt.MessageTypeID = l.MessageTypeID
				LEFT JOIN @Clicks c ON s.blastid = c.blastid and s.emailid = c.emailid
            WHERE
				b.CustomerID = @CustomerId
				AND B.TestBlast = 'N'
				AND cast( b.SendTime as date) BETWEEN @StartDate AND @EndDate
				AND mt.MessageTypeID = @FilterValue
            GROUP BY
				EmailAddress,
				Title,
				FirstName,
				LastName,
				FullName,
				Company,
				Occupation,
				Address,
				Address2,
				City,
				State,
				Zip,
				Country,
				Voice,
				Mobile,
				Fax,
				Website,
				Age,
				Income,
				Gender,
				User1,
				User2,
				User3,
				User4,
				User5,
				User6,
				Birthdate,
				UserEvent1,
				UserEvent1Date,
				UserEvent2,
				UserEvent2Date,
				DateAdded ,
				DateUpdated 
            HAVING
				COUNT(DISTINCT(s.BlastID)) between @NumberOfTouches and @NumberOfTouchesUpperBound
				AND COUNT (DISTINCT(c.BlastID)) between @Grouping and @GroupingUpperBound
      END
     
      IF @FilterField = 'CampaignId'
      BEGIN
            SELECT
				EmailAddress,
				Title,
				FirstName,
				LastName,
				FullName,
				Company,
				Occupation,
				Address,
				Address2,
				City,
				State,
				Zip,
				Country,
				Voice,
				Mobile,
				Fax,
				Website,
				Age,
				Income,
				Gender,
				User1,
				User2,
				User3,
				User4,
				User5,
				User6,
				Birthdate,
				UserEvent1,
				UserEvent1Date,
				UserEvent2,
				UserEvent2Date,
				DateAdded as CreatedOn,
				DateUpdated as LastChanged
            FROM
				ECN_Activity.dbo.BlastActivitySends s WITH (NOLOCK)
				JOIN ECN5_COMMUNICATOR..Emails e WITH (NOLOCK) ON e.EmailID = s.EmailId
				JOIN ECN5_COMMUNICATOR..Blast b WITH (NOLOCK) ON b.BlastID = s.BlastID
				JOIN ECN5_Communicator.dbo.campaignItemBlast cib  WITH (NOLOCK) ON cib.BlastID = b.BlastID
				JOIN ECN5_Communicator.dbo.CampaignItem ci  WITH (NOLOCK) ON ci.CampaignItemID = cib.CampaignItemID
				JOIN ECN5_Communicator.dbo.Campaign cam  WITH (NOLOCK) ON ci.CampaignID = cam.CampaignID
				LEFT JOIN @Clicks c ON s.blastid = c.blastid and s.emailid = c.emailid
            WHERE
				b.CustomerID = @CustomerId
				and TestBlast ='N'                 
				AND cast( b.SendTime as date) BETWEEN @StartDate AND @EndDate
				AND cam.CampaignId = @FilterValue
            GROUP BY
				EmailAddress,
				Title,
				FirstName,
				LastName,
				FullName,
				Company,
				Occupation,
				Address,
				Address2,
				City,
				State,
				Zip,
				Country,
				Voice,
				Mobile,
				Fax,
				Website,
				Age,
				Income,
				Gender,
				User1,
				User2,
				User3,
				User4,
				User5,
				User6,
				Birthdate,
				UserEvent1,
				UserEvent1Date,
				UserEvent2,
				UserEvent2Date,
				DateAdded ,
				DateUpdated 
            HAVING
				COUNT(DISTINCT(s.BlastID)) between @NumberOfTouches and @NumberOfTouchesUpperBound
				AND COUNT (DISTINCT(c.BlastID)) between @Grouping and @GroupingUpperBound
      END
 
      IF @FilterField IS NULL
      BEGIN
            SELECT
				EmailAddress,
				Title,
				FirstName,
				LastName,
				FullName,
				Company,
				Occupation,
				Address,
				Address2,
				City,
				State,
				Zip,
				Country,
				Voice,
				Mobile,
				Fax,
				Website,
				Age,
				Income,
				Gender,
				User1,
				User2,
				User3,
				User4,
				User5,
				User6,
				Birthdate,
				UserEvent1,
				UserEvent1Date,
				UserEvent2,
				UserEvent2Date,
				DateAdded as CreatedOn,
				DateUpdated as LastChanged
            FROM
				ECN_Activity.dbo.BlastActivitySends s WITH (NOLOCK)
				JOIN ECN5_COMMUNICATOR..Blast b WITH (NOLOCK) ON b.BlastID = s.BlastID
				JOIN ECN5_COMMUNICATOR..Emails e WITH (NOLOCK) ON e.EmailID = s.EmailId
				LEFT JOIN @Clicks c ON s.blastid = c.blastid and s.emailid = c.emailid
            WHERE
				b.CustomerID = @CustomerId
				AND TestBlast ='N'
				AND cast( b.SendTime as date) BETWEEN @StartDate AND @EndDate
            GROUP BY
				EmailAddress,
				Title,
				FirstName,
				LastName,
				FullName,
				Company,
				Occupation,
				Address,
				Address2,
				City,
				State,
				Zip,
				Country,
				Voice,
				Mobile,
				Fax,
				Website,
				Age,
				Income,
				Gender,
				User1,
				User2,
				User3,
				User4,
				User5,
				User6,
				Birthdate,
				UserEvent1,
				UserEvent1Date,
				UserEvent2,
				UserEvent2Date,
				DateAdded ,
				DateUpdated 
            HAVING
				COUNT(DISTINCT(s.BlastID)) between @NumberOfTouches and @NumberOfTouchesUpperBound
				AND COUNT (DISTINCT(c.BlastID)) between @Grouping and @GroupingUpperBound
      END
END
 
IF @ActionType = 'Sent'
BEGIN
      IF @FilterField = 'GroupID'
      BEGIN
            SELECT
				EmailAddress,
				Title,
				FirstName,
				LastName,
				FullName,
				Company,
				Occupation,
				Address,
				Address2,
				City,
				State,
				Zip,
				Country,
				Voice,
				Mobile,
				Fax,
				Website,
				Age,
				Income,
				Gender,
				User1,
				User2,
				User3,
				User4,
				User5,
				User6,
				Birthdate,
				UserEvent1,
				UserEvent1Date,
				UserEvent2,
				UserEvent2Date,
				DateAdded as CreatedOn,
				DateUpdated as LastChanged
			FROM
				ECN_Activity.dbo.BlastActivitySends s WITH (NOLOCK)
				JOIN ECN5_COMMUNICATOR..Blast b WITH (NOLOCK) on b.BlastID = s.BlastID
				join ecn5_communicator..Emails e with (nolock) on s.EmailID = e.EmailID
			WHERE 
				b.CustomerID = @CustomerId 
				and TestBlast ='N'
				AND cast( b.SendTime as date) BETWEEN @StartDate AND @EndDate
				and b.GroupID = @FilterValue
			GROUP BY
				EmailAddress,
				Title,
				FirstName,
				LastName,
				FullName,
				Company,
				Occupation,
				Address,
				Address2,
				City,
				State,
				Zip,
				Country,
				Voice,
				Mobile,
				Fax,
				Website,
				Age,
				Income,
				Gender,
				User1,
				User2,
				User3,
				User4,
				User5,
				User6,
				Birthdate,
				UserEvent1,
				UserEvent1Date,
				UserEvent2,
				UserEvent2Date,
				DateAdded ,
				DateUpdated 
            HAVING
				COUNT(DISTINCT(s.BlastID)) between @NumberOfTouches and @NumberOfTouchesUpperBound
      END
 
      IF @FilterField = 'MessageTypeId'
      BEGIN
            SELECT
				EmailAddress,
				Title,
				FirstName,
				LastName,
				FullName,
				Company,
				Occupation,
				Address,
				Address2,
				City,
				State,
				Zip,
				Country,
				Voice,
				Mobile,
				Fax,
				Website,
				Age,
				Income,
				Gender,
				User1,
				User2,
				User3,
				User4,
				User5,
				User6,
				Birthdate,
				UserEvent1,
				UserEvent1Date,
				UserEvent2,
				UserEvent2Date,
				DateAdded as CreatedOn,
				DateUpdated as LastChanged
            FROM
				ECN_Activity.dbo.BlastActivitySends s WITH (NOLOCK)
				JOIN ECN5_COMMUNICATOR..Blast b WITH (NOLOCK) on b.BlastID = s.BlastID
				join ecn5_communicator..Layout l with (Nolock) on l.LayoutID = b.LayoutID
				join ecn5_communicator..Emails e with (nolock) on s.EmailID = e.EmailID
			WHERE 
				b.CustomerID = @CustomerId 
				and TestBlast ='N'
				AND cast( b.SendTime as date) BETWEEN @StartDate AND @EndDate
				and l.MessageTypeID = @FilterValue
            GROUP BY
				EmailAddress,
				Title,
				FirstName,
				LastName,
				FullName,
				Company,
				Occupation,
				Address,
				Address2,
				City,
				State,
				Zip,
				Country,
				Voice,
				Mobile,
				Fax,
				Website,
				Age,
				Income,
				Gender,
				User1,
				User2,
				User3,
				User4,
				User5,
				User6,
				Birthdate,
				UserEvent1,
				UserEvent1Date,
				UserEvent2,
				UserEvent2Date,
				DateAdded ,
				DateUpdated 
            HAVING
				COUNT(DISTINCT(s.BlastID)) between @NumberOfTouches and @NumberOfTouchesUpperBound
      END
     
      IF @FilterField = 'CampaignId'
      BEGIN
			SELECT
				EmailAddress,
				Title,
				FirstName,
				LastName,
				FullName,
				Company,
				Occupation,
				Address,
				Address2,
				City,
				State,
				Zip,
				Country,
				Voice,
				Mobile,
				Fax,
				Website,
				Age,
				Income,
				Gender,
				User1,
				User2,
				User3,
				User4,
				User5,
				User6,
				Birthdate,
				UserEvent1,
				UserEvent1Date,
				UserEvent2,
				UserEvent2Date,
				DateAdded as CreatedOn,
				DateUpdated as LastChanged
            FROM
				ECN_Activity.dbo.BlastActivitySends s WITH (NOLOCK)
				JOIN ECN5_COMMUNICATOR..Blast b WITH (NOLOCK) on b.BlastID = s.BlastID
				join ecn5_communicator..CampaignItemBlast cib with (nolock) on cib.BlastID = b.BlastID
				join ecn5_communicator..CampaignItem ci with (nolock) on ci.CampaignItemID = cib.CampaignItemID
				join ecn5_communicator..Campaign c with (nolock) on c.CampaignID = ci.CampaignID
				join ecn5_communicator..Emails e with (nolock) on s.EmailID = e.EmailID
			WHERE 
				b.CustomerID = @CustomerId 
				and TestBlast ='N'
				AND cast( b.SendTime as date) BETWEEN @StartDate AND @EndDate
				and c.CampaignID = @FilterValue
			GROUP BY
				EmailAddress,
				Title,
				FirstName,
				LastName,
				FullName,
				Company,
				Occupation,
				Address,
				Address2,
				City,
				State,
				Zip,
				Country,
				Voice,
				Mobile,
				Fax,
				Website,
				Age,
				Income,
				Gender,
				User1,
				User2,
				User3,
				User4,
				User5,
				User6,
				Birthdate,
				UserEvent1,
				UserEvent1Date,
				UserEvent2,
				UserEvent2Date,
				DateAdded ,
				DateUpdated 
            HAVING
				COUNT(DISTINCT(s.BlastID)) between @NumberOfTouches and @NumberOfTouchesUpperBound
      END
 
      IF @FilterField IS NULL
      BEGIN
			SELECT
				EmailAddress,
				Title,
				FirstName,
				LastName,
				FullName,
				Company,
				Occupation,
				Address,
				Address2,
				City,
				State,
				Zip,
				Country,
				Voice,
				Mobile,
				Fax,
				Website,
				Age,
				Income,
				Gender,
				User1,
				User2,
				User3,
				User4,
				User5,
				User6,
				Birthdate,
				UserEvent1,
				UserEvent1Date,
				UserEvent2,
				UserEvent2Date,
				DateAdded as CreatedOn,
				DateUpdated as LastChanged
			FROM
				ECN_Activity.dbo.BlastActivitySends s WITH (NOLOCK)
				JOIN ECN5_COMMUNICATOR..Blast b WITH (NOLOCK) on b.BlastID = s.BlastID
				join ecn5_communicator..Emails e with (nolock) on s.EmailID = e.EmailID
			WHERE 
				b.CustomerID = @CustomerId 
				and TestBlast ='N'
				AND cast( b.SendTime as date) BETWEEN @StartDate AND @EndDate
            GROUP BY
				EmailAddress,
				Title,
				FirstName,
				LastName,
				FullName,
				Company,
				Occupation,
				Address,
				Address2,
				City,
				State,
				Zip,
				Country,
				Voice,
				Mobile,
				Fax,
				Website,
				Age,
				Income,
				Gender,
				User1,
				User2,
				User3,
				User4,
				User5,
				User6,
				Birthdate,
				UserEvent1,
				UserEvent1Date,
				UserEvent2,
				UserEvent2Date,
				DateAdded ,
				DateUpdated 
            HAVING
				COUNT(DISTINCT(s.BlastID)) between @NumberOfTouches and @NumberOfTouchesUpperBound
      END
END
GO

