CREATE PROCEDURE job_UpdateMasterEmailStatus
AS
Begin

	SET NOCOUNT ON
	
	SET ANSI_WARNINGS OFF

	DECLARE @ActiveId INT
	SELECT @ActiveId = EmailStatusID FROM EmailStatus WHERE Status = 'Active';

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
	SELECT DISTINCT ps.SubscriptionId,
		ps.Email,
		ps.EmailstatusId,
		ps.StatusUpdatedDate
	 FROM PubSubscriptions ps
		JOIN (	SELECT SubscriptionId,
					MAX(StatusUpdatedDate) AS StatusUpdatedDate 
				FROM PubSubscriptions 
				WHERE EmailstatusId = @ActiveId
					AND ISNULL(Email,'') <> ''
				GROUP BY SubscriptionID) ae ON ps.subscriptionid = ae.subscriptionId AND ISNULL(ps.StatusUpdatedDate,'1900-01-01') = ISNULL(ae.StatusUpdatedDate,'1900-01-01') AND ISNULL(ps.Email,'') <> ''
				WHERE Emailstatusid = @ActiveId
				)

	UPDATE S
	SET Email = cte_ActiveEmail.Email,
		EmailExists = 1
	FROM Subscriptions s 
		JOIN cte_ActiveEmail ON s.SubscriptionID = cte_ActiveEmail.SubscriptionID;

	-----------------------------------------------------------------------------------------
	--If no active email exists then
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
	SELECT DISTINCT ps.SubscriptionId,
		ps.Email,
		ps.EmailstatusId,
		ps.StatusUpdatedDate
	FROM PubSubscriptions ps
		JOIN (	SELECT SubscriptionId,
					MAX(StatusUpdatedDate) AS StatusUpdatedDate 
				FROM PubSubscriptions ps1
				WHERE ISNULL(Email,'') <> ''
					 AND NOT EXISTS (SELECT 1 FROM pubSubscriptions ps2 WHERE ps1.SubscriptionID = ps2.SubscriptionId AND ps2.EmailStatusId = @ActiveId)
				GROUP BY SubscriptionID) ae ON ps.subscriptionid = ae.subscriptionId AND ISNULL(ps.StatusUpdatedDate,'1900-01-01') = ISNULL(ae.StatusUpdatedDate,'1900-01-01')   AND ISNULL(ps.Email,'') <> ''
	)
	UPDATE S
	SET Email = cte_RemainingEmail.Email,
		EmailExists = 1
	FROM subscriptions s 
		JOIN cte_RemainingEmail ON s.SubscriptionID = cte_RemainingEmail.SubscriptionID
		

	-----------------------------------------------------------------------------------------
	--If no email exists at all then remove any email already on the master record and flag as no email
	-----------------------------------------------------------------------------------------
	UPDATE s
	SET Email = NULL,
		EmailExists = 0
	FROM Subscriptions s
	WHERE s.emailexists = 1
		AND NOT EXISTS (SELECT 1 FROM PubSubscriptions ps WHERE ps.SubscriptionID = s.SubscriptionId AND ISNULL(ps.Email,'') != '')

	UPDATE Subscriptions
	SET EmailExists = 0
	WHERE emailexists = 1 and ISNULL(Email,'') = ''

END