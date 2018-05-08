CREATE PROCEDURE Usp_UpdateSuppressedEmailFromSubRecord

AS

SET NOCOUNT ON

------------------------------------------------------------------------
--CREATE Logging Table if not exists
------------------------------------------------------------------------
IF NOT EXISTS (SELECT 1 FROM SYS.TABLES WHERE NAME = 'Archive_EmailStatus') 

CREATE TABLE Archive_EmailStatus(
	Archive_EmailStatusId INT IDENTITY(1,1),
	SubscriptionId INT,
	Email VARCHAR(100),
	EmailExists BIT,
	ArchivedDate DATETIME
PRIMARY KEY (Archive_EmailStatusId))


------------------------------------------------------------------------
--Reset All Subscription Emails Where Email is MasterSuppressed
------------------------------------------------------------------------

DECLARE @ActiveId INT
DECLARE @MasterSuppressedId INT

SELECT @ActiveId = EmailStatusID FROM advanstarmasterdb.dbo.EmailStatus WHERE Status = 'Active'
SELECT @MasterSuppressedId = EmailStatusID FROM advanstarmasterdb.dbo.EmailStatus WHERE Status = 'MasterSuppressed'

INSERT INTO Archive_EmailStatus(
	SubscriptionId,
	Email,
	EmailExists,
	ArchivedDate)
SELECT DISTINCT
	s.SubscriptionId,
	s.Email,
	s.EmailExists,
	GETDATE() AS ArchivedDate
FROM
	Subscriptions s
	JOIN Pubsubscriptions ps ON s.Subscriptionid = ps.subscriptionid AND s.Email = ps.Email
WHERE
	ps.EmailstatusID = @MasterSuppressedId;


UPDATE 
	s
SET 
	EmailExists = 0,
	Email = ''
FROM
	Subscriptions s
	JOIN Pubsubscriptions ps ON s.Subscriptionid = ps.subscriptionid AND s.Email = ps.Email
WHERE
	ps.EmailstatusID = @MasterSuppressedId;


-----------------------------------------------------------------------------------------
--Assign the most recent Email from Pubsubscriptions where the Emailstatusid =1 (Active)
-----------------------------------------------------------------------------------------
WITH cte_ActiveEmail 
	(
	SubscriptionId,
	Email,
	EmailstatusId,
	StatusUpdatedDate
	)

AS
(
SELECT DISTINCT
	ps.SubscriptionId,
	ps.Email,
	ps.EmailstatusId,
	ps.StatusUpdatedDate
 FROM 
	PubSubscriptions ps
	JOIN (	SELECT 
				SubscriptionId,
				MAX(StatusUpdatedDate) AS StatusUpdatedDate 
			FROM 
				PubSubscriptions 
			WHERE 
				EmailstatusId =@ActiveId
				AND ISNULL(Email,'') <> ''
			GROUP BY 
				SubscriptionID) ae ON ps.subscriptionid = ae.subscriptionId AND ps.StatusUpdatedDate = ae.StatusUpdatedDate
 WHERE 
	Emailstatusid = @ActiveId)

UPDATE 
	S
SET 
	Email = cte_ActiveEmail.Email,
	EmailExists = 1
FROM 
    subscriptions s 
	JOIN cte_ActiveEmail ON s.SubscriptionID = cte_ActiveEmail.SubscriptionID
WHERE
	EmailExists = 0;



-----------------------------------------------------------------------------------------
--Assign the most recent Email from Pubsubscriptions where the Emailstatusid NOT =4
-----------------------------------------------------------------------------------------
WITH cte_ActiveEmail 
	(
	SubscriptionId,
	Email,
	EmailstatusId,
	StatusUpdatedDate
	)
AS
(
SELECT DISTINCT
	ps.SubscriptionId,
	ps.Email,
	ps.EmailstatusId,
	ps.StatusUpdatedDate
 FROM 
	PubSubscriptions ps
	JOIN (	SELECT 
				SubscriptionId,
				MAX(StatusUpdatedDate) AS StatusUpdatedDate 
			FROM 
				PubSubscriptions 
			WHERE 
				EmailstatusId !=@MasterSuppressedId
				AND ISNULL(Email,'') <> ''
			GROUP BY 
				SubscriptionID) ae ON ps.subscriptionid = ae.subscriptionId AND ps.StatusUpdatedDate = ae.StatusUpdatedDate
 WHERE 
	Emailstatusid !=@MasterSuppressedId)

UPDATE
	S
SET 
	Email = cte_ActiveEmail.Email,
	EmailExists = 1
FROM 
    subscriptions s 
	JOIN cte_ActiveEmail ON s.SubscriptionID = cte_ActiveEmail.SubscriptionID
WHERE
	EmailExists = 0;
	--AND NOT EXISTS (SELECT 
	--					1 
	--				FROM 
	--					Pubsubscriptions ps 
	--				WHERE 
	--					ps.Email = cte_ActiveEmail.Email 
	--					AND ps.SubscriptionId = s.SubscriptionId 
	--					AND ps.EmailStatusID = @MasterSuppressedId)


-----------------------------------------------------------------------------------------
--Assign the most recent Email from Pubsubscriptions regardless of status
-----------------------------------------------------------------------------------------

WITH cte_RemainingEmail 
	(
	SubscriptionId,
	Email,
	EmailstatusId,
	StatusUpdatedDate
	)
AS
(
SELECT DISTINCT
	ps.SubscriptionId,
	ps.Email,
	ps.EmailstatusId,
	ps.StatusUpdatedDate
 FROM 
	PubSubscriptions ps
	JOIN (	SELECT 
				SubscriptionId,
				MAX(StatusUpdatedDate) AS StatusUpdatedDate 
			FROM 
				PubSubscriptions 
			WHERE 
				--EmailstatusId = @MasterSuppressedId
				 ISNULL(Email,'') <> ''
			GROUP BY 
				SubscriptionID) ae ON ps.subscriptionid = ae.subscriptionId AND ps.StatusUpdatedDate = ae.StatusUpdatedDate
 --WHERE 	Emailstatusid = @MasterSuppressedId
 )

UPDATE
	S
SET 
	Email = cte_RemainingEmail.Email,
	EmailExists = 1
FROM 
    subscriptions s 
	JOIN cte_RemainingEmail ON s.SubscriptionID = cte_RemainingEmail.SubscriptionID
WHERE
	EmailExists = 0
GO
