CREATE PROC [dbo].[MovedToActivity_sp_getSoftBounceBadOrNoOpenEmails]  (
	@BounceThreshold INT,
	@BounceCheckDay INT,
	@BounceLastOpenDay INT
)

AS
	BEGIN
		INSERT INTO ECN_ACTIVITY..OldProcsInUse (ProcName, DateUsed) VALUES ('sp_getSoftBounceBadOrNoOpenEmails', GETDATE())
		DECLARE @sbCount INT, 
			@bounceStDt DATETIME, @bounceEndDt DATETIME,
			@lastOpenStDt DATETIME, @lastOpenEndDt DATETIME

		SET @bounceStDt = CONVERT(VARCHAR(10),DATEADD(d, @BounceCheckDay, GETDATE()),101)
		SET @bounceEndDt = DATEADD(s , @BounceCheckDay, DATEADD(d, 1, @bounceStDt))
		SET @lastOpenStDt = CONVERT(VARCHAR(10),DATEADD(d, -(@BounceLastOpenDay), GETDATE()),101)
		SET @lastOpenEndDt = DATEADD(s , -1, DATEADD(d, 1, @lastOpenStDt))

		CREATE TABLE #SBouncedEmails (EmailID INT, BlastID INT, LastOpen DATETIME, BounceCount INT)
		CREATE TABLE #SBouncedEmailsLastOpen (EmailID INT, LastOpen DATETIME)
		CREATE TABLE #SBouncedBounceCountEmails (EmailID INT, BounceCount INT)
		
		-- Select all Soft & SpamNotification Bounced emails for previous day.
		INSERT INTO #SBouncedEmails        
		SELECT TOP 7000 EmailID, BlastID, null, 0 
		FROM EmailActivityLog
		WHERE ActionDate BETWEEN @bounceStDt AND @bounceEndDt AND
		Actiontypecode = 'bounce' AND (ActionValue = 'softbounce' OR ActionValue = 'spamnotification') AND Processed = 'n'
		GROUP BY EmailID, BlastID
		
		-- get the count of #SBouncedEmails. If no SB's don't proceed further
		SET @sbCount = @@RowCount
		IF @sbCount > 0
		BEGIN
			-- Get Last Open Date for all the SBounced Emails from the above List. Upate the Master Temp Table.
			INSERT INTO #SBouncedEmailsLastOpen
			SELECT eal.EmailID, MAX(eal.ActionDate) 
			FROM EmailActivitylog eal JOIN #SBouncedEmails sbe ON eal.EmailID = sbe.EmailID 
			WHERE ActiONtypecode = 'open' 
			GROUP BY eal.EmailID
			UPDATE #SBouncedEmails SET LastOpen = sblo.LastOpen 
			FROM #SBouncedEmails sbe JOIN #SBouncedEmailsLastOpen sblo ON sbe.EmailID = sblo.EmailID 
			
			-- Get the Count of # of times the Email bounced as Soft / SPAM, Update the Master Temp Table.
			INSERT INTO #SBouncedBounceCountEmails
			SELECT eal.EmailID, COUNT(eal.EmailID) as 'BounceCount' 
			FROM EmailActivitylog eal 
			JOIN #SBouncedEmails sbe ON eal.EmailID = sbe.EmailID 
			WHERE eal.Actiontypecode = 'bounce' AND (ActionValue = 'softbounce' OR ActionValue = 'spamnotification') AND Processed = 'n'
			GROUP BY eal.EmailID
			UPDATE #SBouncedEmails SET BounceCount = sbbc.BounceCount 
			FROM #SBouncedEmails sbe JOIN #SBouncedBounceCountEmails sbbc ON sbe.EmailID = sbbc.EmailID 
			
			--SELECT * FROM #SBouncedEmails
			-- Get the details on the emails which 'DID NOT' open any email / last open date was less than X days / if the Bounce count is X or more.	
			SELECT e.EmailAddress, e.CustomerID, sbe.EmailID, MAX(sbe.BlastID)AS BlastID, sbe.LastOpen, sbe.BounceCount 
			FROM Emails e JOIN #SBouncedEmails sbe ON e.EmailID = sbe.EmailID WHERE (((LastOpen BETWEEN @lastOpenStDt AND @lastOpenEndDt) OR LastOpen is NULL) AND BounceCount >= @BounceThreshold )
			GROUP BY e.EmailAddress, e.CustomerID, sbe.EmailID, sbe.LastOpen, sbe.BounceCount
			ORDER BY sbe.EmailID, e.CustomerID
		END
		ELSE
			SELECT * FROM #SBouncedEmails

		DROP TABLE #SBouncedBounceCountEmails
		DROP TABLE #SBouncedEmailsLastOpen
		DROP TABLE #SBouncedEmails
	END
