CREATE PROCEDURE [dbo].[e_EmailGroup_ManageSubscribers]
	@CustomerID int,
	@UserID int,
	@EmailTable ManageSubscribersInputTableType READONLY,	
	@source varchar(200) = 'Unknown'
AS
Begin  
	set nocount on  	

	DECLARE @result As ManageSubscribersResultTableType;

	DECLARE  @dt datetime = getdate()

	DECLARE @BaseChannelID int = (SELECT BaseChannelID from ECN5_ACCOUNTS.DBO.Customer (NOLOCK) where customerID = @CustomerID and IsDeleted = 0);

	-- TODO: tracking; ImportEmailTracking table may need to change as we now allow multiple GroupIDs per invocation
	-- INSERT INTO ImportEmailTracking (CustomerID, GroupID, StartTime) VALUES (@CustomerID, @GroupID, @dt)
	-- SELECT @TrackingID = @@IDENTITY

	WITH EmailCount AS
	     ( 
			SELECT EmailAddress, COUNT(EmailID) EmailAddressCount 
			FROM Emails 
			GROUP BY EmailAddress, CustomerID
			HAVING -- COUNT(EmailID) > 1 AND 
				   CustomerID = @CustomerID
	     )
	INSERT INTO @result
	     ( EmailAddress,
		   EmailIsValid,
		   GroupID,
		   OriginalGroupID,  
		   EmailID, 
		   EmailGroupID,
		   OldSubscribeTypeCode,
		   OldFormatTypeCode,
		   NewSubscribeTypeCode,
		   NewFormatTypeCode,
		   GlobalMasterSuppressed,
		   ChannelMasterSuppressed,
		   GroupMasterSupressionSuppressed,
		   [Status],
		   Result
		 )
	SELECT ET.EmailAddress EmailAddress
	      ,ISNULL(dbo.fn_ValidateEmailAddress(ET.EmailAddress),0) EmailIsValid
	      ,G.GroupID GroupId
	      ,ET.GroupID
	      ,E.EmailID EmailID
   	      ,EG.EmailGroupID EmailGroupID
	      ,EG.SubscribeTypeCode OldSubscribeTypeCode
	      ,EG.FormatTypeCode OldFormatTypeCode
	      ,UPPER(LEFT(ISNULL(ET.SubscribeTypeCode,''),1)) NewSubscribeTypeCode
	      ,LOWER(ISNULL(ET.FormatTypeCode,'')) NewFormatTypeCode
	      , CASE WHEN GMS.GSID  IS NULL THEN 0 ELSE 1 END GlobalMasterSuppressed
	      , CASE WHEN CMS.CMSID IS NULL THEN 0 ELSE 1 END ChannelMasterSuppressed
	      , CASE WHEN MSG.EmailID IS NULL THEN 0 ELSE 1 END GroupMasterSupressionSuppressed
	      ,'' [Status]
	      ,CASE
			-- validate email address and group
			WHEN G.GroupID IS NULL AND ISNULL(dbo.fn_ValidateEmailAddress(ISNULL(ET.EmailAddress,'')),0) = 0 THEN 'Skipped, InvalidEmailAddress, InvalidGroupId'
			WHEN G.GroupID IS NULL  THEN 'Skipped, InvalidGroupId'
	        WHEN ISNULL(dbo.fn_ValidateEmailAddress(ISNULL(ET.EmailAddress,'')),0) = 0 THEN 'Skipped, InvalidEmailAddress'
			WHEN ISNULL(EC.EmailAddressCount,0) > 1 THEN 'Skipped, DuplicateEmailAddress'
	        
	        -- validate subscribe and format type-codes
	        WHEN UPPER(LEFT(ISNULL(ET.SubscribeTypeCode,''),1)) NOT IN ('S','U') THEN 'Skipped, InvalidSubscribeTypeCode'
	        WHEN UPPER(ISNULL(ET.FormatTypeCode,'')) NOT IN ('HTML','TEXT') THEN 'Skipped, InvalidFormatTypeCode'
	        
	        -- subscriber must exist in order to unsubscribe
	        WHEN UPPER(LEFT(ET.SubscribeTypeCode,1)) = 'U' AND EG.EmailGroupID IS NULL THEN 'Skipped, UnknownSubscriber'
	        
	        -- attempt to subscribe someone already subscribed or unsubscribe someone already unsubscribed
	        WHEN UPPER(LEFT(ET.SubscribeTypeCode,1)) = UPPER(LEFT(ISNULL(EG.SubscribeTypeCode,''),1)) 
	         AND UPPER(ET.FormatTypeCode) = UPPER(ISNULL(EG.FormatTypeCode,'')) THEN 'Skipped, Duplicate'
	        
	        -- skip email addresses listed in any master suppression group
	        WHEN GMS.GSID IS NOT NULL OR CMS.CMSID IS NOT NULL OR MSG.EmailID IS NOT NULL THEN 'MasterSuppressed'

	        WHEN LEFT(ET.SubscribeTypeCode,1) = 'S' AND EG.EmailGroupID IS NULL THEN 'New, Subscribed'
	        WHEN LEFT(ET.SubscribeTypeCode,1) = 'S' AND EG.EmailGroupID IS NOT NULL THEN 'Updated, Subscribed'
	        WHEN LEFT(ET.SubscribeTypeCode,1) = 'U' THEN 'Updated, Unsubscribed'
	        
	        -- this should be unreachable
	        ELSE 'Unknown'
	       END Result
	  FROM           @EmailTable  ET 
	  LEFT OUTER JOIN Groups      G  (NOLOCK) ON ( ET.GroupID IS NOT NULL AND ET.GroupID > 0 AND G.GroupID = ET.GroupID AND G.CustomerID = @CustomerID)
	  LEFT OUTER JOIN Emails      E  (NOLOCK) ON ( E.EmailAddress = ET.emailAddress  AND E.CustomerID = @CustomerID )
	  LEFT OUTER JOIN EmailCount  EC (NOLOCK) ON  (EC.EmailAddress = ET.EmailAddress )
	  LEFT OUTER JOIN EmailGroups EG (NOLOCK) ON ( E.EmailID IS NOT NULL AND E.EmailID = EG.EmailID AND EG.GroupID = G.GroupID  )
	  LEFT OUTER JOIN EmailGroups MSG(NOLOCK) ON ( E.EmailID = MSG.EmailID AND MSG.GroupID IN (SELECT GroupID From Groups Where CustomerID = @CustomerID AND MasterSupression = 1))
	  LEFT OUTER JOIN ChannelMasterSuppressionList CMS(NOLOCK) ON ( CMS.EmailAddress = ET.EmailAddress AND CMS.BaseChannelID = @BaseChannelID AND CMS.IsDeleted <> 1  )
	  LEFT OUTER JOIN GlobalMasterSuppressionList  GMS(NOLOCK) ON (GMS.EmailAddress = ET.EmailAddress AND GMS.IsDeleted <> 1)
	  
	---- unsubscribe/resubscribe
	--UPDATE EmailGroups  
	--   SET LastChanged = @dt
	--     , SubscribeTypeCode = RT.NewSubscribeTypeCode
	--     , FormatTypeCode = RT.NewFormatTypeCode
	--	 , LastChangedSource = @source
	--  FROM @result RT 
	-- WHERE RT.Result IN ('Updated, Subscribed', 'Updated, Unsubscribed')
	--   AND RT.EmailGroupID IS NOT NULL
	--   AND RT.EmailGroupID = EmailGroups.EmailGroupID
	
	---- email not previously known to the customer
	--INSERT 
	--  INTO Emails (EmailAddress, CustomerID)
	--SELECT EmailAddress, @CustomerID
	--  FROM @result
	-- WHERE EmailID IS NULL 
	--   AND Result = 'New, Subscribed'
	--     ;
	--UPDATE @result
	--   SET EmailID = E.EmailID
 --     FROM Emails (NOLOCK) E, 
 --          @result RT
 --    WHERE E.CustomerID = @CustomerID 
 --      AND E.EmailAddress = RT.EmailAddress
 --      AND RT.EmailID IS NULL
	--   AND RT.Result = 'New, Subscribed'
	--     ;

	---- adding the email/group association
	--INSERT
	--  INTO EmailGroups (GroupID, EmailID, SubscribeTypeCode, FormatTypeCode, CreatedOn, LastChanged, SMSEnabled, CreatedSource)
	--SELECT RT.GroupID, RT.EMailID, RT.NewSubscribeTypeCode, RT.NewFormatTypeCode, @dt, @dt, 0, @source
	--  FROM @Result RT
	-- WHERE RT.EmailGroupID IS NULL
	--   AND RT.Result = 'New, Subscribed'
	--UPDATE @result
	--   SET EmailGroupID = EG.EmailGroupID
 --     FROM EmailGroups (NOLOCK) EG, 
 --          @result RT
 --    WHERE EG.GroupID = RT.GroupID
 --      AND EG.EmailID = RT.EmailID
 --      AND RT.EmailGroupID IS NULL
	--   AND RT.Result = 'New, Subscribed'
	--     ;
	
	-- collect current status of all result records that represent existing emailAddress/Group associations
	-- this will be expensive for large sets with many different group IDs however it improves testablity 
	-- by providing a cross check on Result (e.g. Status should be "S" anytime Result like "%Subscribed", etc.)
	UPDATE @result
	   SET Status = EG.SubscribeTypeCode
	  FROM @result RT,
	       EmailGroups EG 
	 WHERE RT.EmailGroupID IS NOT NULL
	   AND RT.EmailGroupID = EG.EmailGroupID;
	
	-- fix status for master-suppressed subscribers
	UPDATE @result
	   SET Status = 'M'
	 WHERE LOWER(Result) like '%suppressed';
	
	-- Output the result set
	SELECT 	EmailAddress
	     , ISNULL(GroupID, OriginalGroupID) GroupID
	     , EmailID
	     , EmailGroupID
	     , [Status]
	     , Result 
	FROM @result;
END
RETURN 0
