--IF EXISTS (SELECT 1 FROM Sysobjects where name = 'job_DomainTracking_CreateEmail')
--DROP PROC job_DomainTracking_CreateEmail


--SET ANSI_NULLS ON
--GO

--SET QUOTED_IDENTIFIER ON
--GO


CREATE PROCEDURE [dbo].[job_DomainTracking_CreateEmail]

AS
Begin

	SET NoCount ON
	
	Declare @subID table (SubscriptionID int)			
	
	DECLARE @EmailAddress varchar(100),
			@Reason varchar(200),
			@RequestDate datetime,
			@CurrentDate datetime,
			@newSubscriptionID int,
			@newPubSubscriptionID int,
			@ErrorCode int,
			@PubID	integer,
			@SubRecCount integer,
			@PubSubRecCount integer


	SELECT @PubID = IsNull(PubID, 0) from pubs where PubCode = 'Domain_Tracking'
	IF @@ROWCOUNT = 0 OR @PubID = 0 
	BEGIN
		-- This client is not setup for creating emails
		return 0
	END 
	SET @CurrentDate = GETDATE()
	SET @Reason = 'Domain_Tracking'

	UPDATE tmp_domainActiveEmails
		SET SkippedRecord = 0
		
	UPDATE tmp_domainActiveEmails
		SET SkippedRecord = 1
	WHERE UAD_Temp.dbo.fn_ValidateEmailAddress(Email) = 0

BEGIN TRY
	DECLARE c cursor
	FOR 
		-- result of select will be those emails that are missing
		-- tmp_domain... contains email addresses from ECN in the past X days
		SELECT DISTINCT 
			LOWER(t.Email) AS Email
		FROM tmp_domainActiveEmails t 
			 LEFT JOIN Subscriptions s with (NOLOCK) on t.Email = s.Email
			 LEFT JOIN PubSubscriptions ps with (NOLOCK) on t.Email = ps.Email 
		WHERE
			s.SubscriptionID is NULL 
			AND ps.SubscriptionID is NULL 
			AND t.Email is not NULL 
			AND t.SkippedRecord = 0
		ORDER BY
			LOWER(t.Email)
	OPEN c

	-- Get List of Emails without Subscrptions
	FETCH NEXT FROM c INTO @EmailAddress
	WHILE @@FETCH_STATUS = 0
	BEGIN
		DECLARE @MaxSeq int = (Select isnull(MAX(SEQUENCE),0) +1 From Subscriptions)

		BEGIN TRANSACTION ;
					
		INSERT INTO Subscriptions 
		(	SEQUENCE,
			EMAIL,
			EmailExists,
			PhoneExists,
			FaxExists,
			IsExcluded,
			Latitude,
			Longitude,
			IsLatLonValid,
			LatLonMsg,
			Igrp_No,
			categoryID,
			TransactionID,
			Qdate,
			Transactiondate
		)
		VALUES
		(	@MaxSeq,
			@EmailAddress,
			'true',
			'false',
			'false',
			'false',
			0,
			0,
			'false',
			'No Address',
			NEWID(),
			10,
			10,
			@CurrentDate,
			@CurrentDate
		)

		-- Create a new PubSubscriptiopns Record
		SET @newSubscriptionID  = SCOPE_IDENTITY()
		INSERT INTO PubSubscriptions
		(	SubscriptionID,
			PubID, 
			Email, 
			EmailStatusID,
			StatusUpdatedDate,
			StatusUpdatedReason, 
			Qualificationdate 
		)
		VALUES
		(	@newSubscriptionID,
			@pubID,
			@EmailAddress, 
			1,									-- defaulted to 1
			@CurrentDate,						-- What Date should be used ?
			@Reason,							-- 'Domain_Tracking'
			@CurrentDate
		)
		-- Create a new PubSubscriptiopns Record
		SET @newPubSubscriptionID  = SCOPE_IDENTITY()

		INSERT INTO PubSubscriptionDetail (PubSubscriptionID,SubscriptionID,CodesheetID)
			SELECT ps.pubsubscriptionID, s.SubscriptionID, c.codesheetID
			FROM 
					Subscriptions s with (NOLOCK) 
					join PubSubscriptions ps with (NOLOCK) on s.SubscriptionID = ps.SubscriptionID 
					join ResponseGroups rg with (NOLOCK) on rg.PubID = ps.PubID
					join CodeSheet c with (NOLOCK) on c.ResponseGroupID  = rg.ResponseGroupID
			WHERE
				s.SubscriptionID = @NewSubscriptionID and ps.PubID = @PubID and rg.ResponseGroupName = 'PubCode'

		INSERT INTO SubscriptionDetails (SubscriptionID,MasterID)
			SELECT DISTINCT s.SubscriptionID, cmb.MasterID
			FROM 
					Subscriptions s 
					join PubSubscriptions ps with (NOLOCK) on s.SubscriptionID = ps.SubscriptionID 
					join ResponseGroups rg with (NOLOCK) on rg.PubID = ps.PubID
					join CodeSheet c with (NOLOCK) on c.ResponseGroupID  = rg.ResponseGroupID
					join CodeSheet_Mastercodesheet_Bridge cmb with (NOLOCK) on cmb.codesheetID = c.codesheetID
			WHERE
				s.SubscriptionID = @NewSubscriptionID and ps.PubID = @PubID and rg.ResponseGroupName = 'PubCode'
		COMMIT TRANSACTION ;
		
	FETCH NEXT FROM c INTO @EmailAddress
	END
END TRY
BEGIN CATCH
	IF @@trancount > 0 ROLLBACK TRANSACTION
	EXEC workspace.dbo.UserErrorHandler 
	RETURN 55555
END CATCH

BEGIN TRY
	CLOSE c
	DEALLOCATE c
END TRY
BEGIN CATCH
	-- do nothing
END CATCH
	
END

GO


