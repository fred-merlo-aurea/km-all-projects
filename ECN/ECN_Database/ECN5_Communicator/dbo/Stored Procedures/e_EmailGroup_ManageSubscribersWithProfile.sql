CREATE PROCEDURE [dbo].[e_EmailGroup_ManageSubscribersWithProfile]
	@CustomerID int,
	@UserID int, 
	@EmailTable ManageSubscribersProfileInputTableType READONLY,
	@StandaloneUdfTable ManageSubscribersStandaloneUdfInputTableType READONLY,
	@TransactionUdfTable ManageSubscribersTransactionalUdfInputTableType READONLY,
	@filename varchar(200) ='',
    @TEST_MODE bit = 0
AS
	BEGIN
	set nocount on  	

	-- create temp table from designs stored as in User Defined Table Types
	DECLARE @result As ManageSubscribersProfileResultTableType;
	DECLARE @StandaloneUDF As ManageSubscribersStandaloneUdfResultTableType;
	DECLARE @TransactionalUDF As ManageSubscribersTransactionalUdfResultTableType	
	SELECT * INTO #result from @result WHERE 2=1;
	SELECT * INTO #StandaloneUDF from @StandaloneUDF WHERE 2=1;
	SELECT * INTO #TransactionalUDF from @TransactionalUDF WHERE 2=1;

	DECLARE  @dt datetime = getdate();

	DECLARE @BaseChannelID int = (SELECT BaseChannelID from ECN5_ACCOUNTS.DBO.Customer (NOLOCK) where customerID = @CustomerID and IsDeleted = 0);

	-- TODO: ImportEmailTracking table needs to change this process will result in several rows being added
	-- INSERT INTO ImportEmailTracking (CustomerID, GroupID, StartTime) VALUES (@CustomerID, @GroupID, @dt)
	-- SELECT @TrackingID = @@IDENTITY

	----------------------------
	-- Result calculation
	WITH GroupTransactionalFieldPrimaryKey AS
	     (
			SELECT GroupDatafieldsID,GroupID FROM GroupDatafields (NOLOCK) WHERE IsPrimaryKey = 1
	     ),
	     EmailCount AS
	     ( 
			SELECT EmailAddress, COUNT(EmailID) EmailAddressCount 
			FROM Emails 
			GROUP BY EmailAddress, CustomerID
			HAVING -- COUNT(EmailID) > 1 AND 
				   CustomerID = @CustomerID
	     )
	INSERT INTO #result
	     ( SubscriberInputTableRowID,
	       EmailAddress,
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
		   TransactionalPrimaryKeyGroupDataFieldId,
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
		  	Notes,
		  	Password,
		   
		   [Status],
		   Result
		 )
	SELECT ET.SubscriberInputTableRowID
	      ,ET.EmailAddress EmailAddress
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
	      , TPK.GroupDatafieldsID TransactionalPrimaryKeyGroupDataFieldId
	      , ISNULL(ET.Title,E.Title) Title
          , ISNULL(ET.FirstName,E.FirstName) FirstName
          , ISNULL(ET.LastName,E.LastName) LastName
          , ISNULL(ET.FullName,E.FullName) FullName
          , ISNULL(ET.Company,E.Company) Company
          , ISNULL(ET.Occupation,E.Occupation) Occupation
          , ISNULL(ET.Address,E.Address) Address
          , ISNULL(ET.Address2,E.Address2) Address2
          , ISNULL(ET.City,E.City) City
          , ISNULL(ET.State,E.State) State
          , ISNULL(ET.Zip,E.Zip) Zip
          , ISNULL(ET.Country,E.Country) Country
          , ISNULL(ET.Voice,E.Voice) Voice
          , ISNULL(ET.Mobile,E.Mobile) Mobile
          , ISNULL(ET.Fax,E.Fax) Fax
          , ISNULL(ET.Website,E.Website) Website
          , ISNULL(ET.Age,E.Age) Age
          , ISNULL(ET.Income,E.Income) Income
          , ISNULL(ET.Gender,E.Gender) Gender
          , ISNULL(ET.User1,E.User1) User1
          , ISNULL(ET.User2,E.User2) User2
          , ISNULL(ET.User3,E.User3) User3
          , ISNULL(ET.User4,E.User4) User4
          , ISNULL(ET.User5,E.User5) User5
          , ISNULL(ET.User6,E.User6) User6
          , ISNULL(ET.Birthdate,E.Birthdate) Birthdate
          , ISNULL(ET.UserEvent1,E.UserEvent1) UserEvent1
          , ISNULL(ET.UserEvent1Date,E.UserEvent1Date) UserEvent1Date
          , ISNULL(ET.UserEvent2,E.UserEvent2) UserEvent2
          , ISNULL(ET.UserEvent2Date,E.UserEvent2Date) UserEvent2Date
          , ISNULL(ET.Notes,E.Notes) Notes
          , ISNULL(RTRIM(LTRIM(ET.Password)),E.Password) Password

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
	        -- for profile version allow this; we might be updating profile fields w/o change subscribe status
	        -- WHEN UPPER(LEFT(ET.SubscribeTypeCode,1)) = UPPER(LEFT(ISNULL(EG.SubscribeTypeCode,''),1)) 
	        --  AND UPPER(ET.FormatTypeCode) = UPPER(ISNULL(EG.FormatTypeCode,'')) THEN 'Skipped, Duplicate'
	        
	        -- skip email addresses listed in any master suppression group
	        WHEN GMS.GSID IS NOT NULL OR CMS.CMSID IS NOT NULL OR MSG.EmailID IS NOT NULL THEN 'Skipped, MasterSuppressed'

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
	  LEFT OUTER JOIN GroupTransactionalFieldPrimaryKey TPK ON ( G.GroupID IS NOT NULL AND G.GroupID = TPK.GroupID )

	INSERT INTO #StandaloneUDF 
	     (
			SubscriberInputTableRowID,
			FieldShortName,
			FieldValue,
			EmailID,
			GroupDataFieldsID,
			OldFieldValue
	      ) 
	 SELECT UT.SubscriberInputTableRowID, UT.FieldShortName, UT.FieldValue
	      , RT.EmailID
	      , GD.GroupDatafieldsID
	      , DV.DataValue
	   FROM @StandaloneUdfTable        UT
	        INNER JOIN #result         RT ON ( UT.SubscriberInputTableRowID = RT.SubscriberInputTableRowID )
	   LEFT OUTER JOIN GroupDatafields GD ON ( RT.GroupID IS NOT NULL 
										 AND GD.GroupID = RT.GroupID 
										 AND LOWER(GD.ShortName) = LOWER(UT.FieldShortName) 
										 AND GD.IsDeleted = 0 
										 AND GD.DatafieldSetID IS NULL) -- allowing a transactional UDF here would cause a cartesian11
	   LEFT OUTER JOIN EmailDataValues DV ON ( RT.EmailID IS NOT NULL 
										 AND GD.GroupDatafieldsID IS NOT NULL 
										 AND RT.EmailID = DV.EmailID 
										 AND DV.GroupDatafieldsID = GD.GroupDatafieldsID)

	-- update Result for unprocessable due to CustomField issues
    UPDATE #result 
       SET Result = CASE 
           WHEN R.Result LIKE 'Skipped%' THEN R.Result + ', UnknownCustomField' 
		   ELSE 'Skipped, UnknownCustomField'
           END
      FROM #StandaloneUDF U
      JOIN #result R ON(R.SubscriberInputTableRowID = U.SubscriberInputTableRowID)
     WHERE GroupDataFieldsID IS NULL;

	-- collect GroupDataFieldID for valid Transactional fields
	INSERT INTO #TransactionalUDF
		 (
			SubscriberInputTableRowID,
			TransactionID,
			TransactionSequenceNumber,
			FieldShortName,
			FieldValue,
			EmailID,
			GroupDataFieldsID
	      ) 
	 SELECT UT.SubscriberInputTableRowID, UT.TransactionID, UT.TransactionSequenceNumber, UT.FieldShortName, UT.FieldValue
	      , RT.EmailID
	      , GD.GroupDatafieldsID
	   FROM @TransactionUdfTable       UT
	        INNER JOIN #result         RT ON ( UT.SubscriberInputTableRowID = RT.SubscriberInputTableRowID )
	   LEFT OUTER JOIN GroupDatafields GD ON ( RT.GroupID IS NOT NULL 
	                                     AND GD.GroupID = RT.GroupID 
	                                     AND LOWER(GD.ShortName) = LOWER(UT.FieldShortName) 
	                                     AND GD.IsDeleted = 0 
	                                     AND GD.DatafieldSetID IS NOT NULL)
	      ;

	-- skip result rows with unknown transactional fields
    UPDATE #result 
       SET Result = CASE 
			   WHEN R.Result LIKE 'Skipped%' THEN R.Result + ', UnknownTransactionalField' 
			   ELSE 'Skipped, UnknownTransactionalField'
           END
      FROM #TransactionalUDF U
      JOIN #result R ON(R.SubscriberInputTableRowID = U.SubscriberInputTableRowID)
     WHERE U.GroupDataFieldsID IS NULL;	
     
     -- Error condition: skip result rows when
     --   group defines transactional UDF with primary key
     --   AND transactional fields provided 
     --   AND pkey NOT provided
     UPDATE #result
        SET Result = CASE 
			   WHEN R.Result LIKE 'Skipped%' THEN R.Result + ', MissingTransactionalPrimaryKeyField' 
			   ELSE 'Skipped, MissingTransactionalPrimaryKeyField'
           END
       FROM #result R
       JOIN #TransactionalUDF U ON( U.SubscriberInputTableRowID = R.SubscriberInputTableRowID )
       LEFT OUTER JOIN #TransactionalUDF E ON(E.SubscriberInputTableRowID = R.SubscriberInputTableRowID 
										  AND E.TransactionID = U.TransactionID       
										  AND E.GroupDataFieldsID = R.TransactionalPrimaryKeyGroupDataFieldId)
      WHERE R.TransactionalPrimaryKeyGroupDataFieldId IS NOT NULL
        AND E.GroupDataFieldsID IS NULL;

	-- update transaction id from existing entity if pkey value already exists
	UPDATE #TransactionalUDF
	   SET TransactionID = X.EntryID
	  FROM #TransactionalUDF T
	  JOIN (
		SELECT T.TransactionSequenceNumber, T.FieldShortName, T.TransactionID , V.EntryID --, *
		  FROM #TransactionalUDF T
		  JOIN #result R ON( R.SubscriberInputTableRowID = T.SubscriberInputTableRowID )
		  JOIN EmailDataValues V ON( R.EmailID = V.EmailID AND R.TransactionalPrimaryKeyGroupDataFieldId = V.GroupDatafieldsID )
		 WHERE 
			   R.Result NOT LIKE 'Skipped%'
		   AND R.TransactionalPrimaryKeyGroupDataFieldId IS NOT NULL
		   AND LTRIM(RTRIM(NULLIF(V.DataValue,''))) = LTRIM(RTRIM(NULLIF(T.FieldValue,'')))
	   ) X ON( X.TransactionID = T.TransactionID )
	   ;
	-- END OF Result calculation
	----------------------------
	
	IF @TEST_MODE <> 1 BEGIN
	-- unsubscribe/resubscribe
	UPDATE EmailGroups  
	   SET LastChanged = @dt
	     , SubscribeTypeCode = RT.NewSubscribeTypeCode
	     , FormatTypeCode = RT.NewFormatTypeCode
	  FROM #result RT
	 WHERE RT.Result IN ('Updated, Subscribed', 'Updated, Unsubscribed')
	   AND RT.EmailGroupID IS NOT NULL
	   AND RT.EmailGroupID = EmailGroups.EmailGroupID
	
	-- profile update
	UPDATE Emails
	   SET [DateUpdated] = @dt
		  ,[Title] = RT.Title
		  ,[FirstName] = RT.FirstName
		  ,[LastName] = RT.LastName
		  ,[FullName] = RT.FullName
		  ,[Company] = RT.Company
		  ,[Occupation] = RT.Occupation
		  ,[Address] = RT.Address
		  ,[Address2] = RT.Address2
		  ,[City] = RT.City
		  ,[State] = RT.State
		  ,[Zip] = RT.Zip
		  ,[Country] = RT.Country
		  ,[Voice] = RT.Voice
		  ,[Mobile] = RT.Mobile
		  ,[Fax] = RT.Fax
		  ,[Website] = RT.Website
		  ,[Age] = RT.Age
		  ,[Income] = RT.Income
		  ,[Gender] = RT.Gender
		  ,[User1] = RT.User1
		  ,[User2] = RT.User2
		  ,[User3] = RT.User3
		  ,[User4] = RT.User4
		  ,[User5] = RT.User5
		  ,[User6] = RT.User6
		  ,[Birthdate] = RT.Birthdate
		  ,[UserEvent1] = RT.UserEvent1
		  ,[UserEvent1Date] = RT.UserEvent1Date
		  ,[UserEvent2] = RT.UserEvent2
		  ,[UserEvent2Date] = RT.UserEvent2Date
		  ,[Notes] = RT.Notes
		  ,[Password] = RT.Password
	  FROM #result RT
	 WHERE RT.Result IN ('Updated, Subscribed', 'Updated, Unsubscribed')
	   AND RT.EmailID IS NOT NULL
	   AND RT.EmailID = Emails.EmailID
	   
	-- email not previously known to the customer
	INSERT 
	  INTO Emails --(EmailAddress, CustomerID, DateAdded)
	      ([DateAdded],[CustomerID]
	       ,[EmailAddress]           
           ,[Title]
           ,[FirstName]
           ,[LastName]
           ,[FullName]
           ,[Company]
           ,[Occupation]
           ,[Address]
           ,[Address2]
           ,[City]
           ,[State]
           ,[Zip]
           ,[Country]
           ,[Voice]
           ,[Mobile]
           ,[Fax]
           ,[Website]
           ,[Age]
           ,[Income]
           ,[Gender]
           ,[User1]
           ,[User2]
           ,[User3]
           ,[User4]
           ,[User5]
           ,[User6]
           ,[Birthdate]
           ,[UserEvent1]
           ,[UserEvent1Date]
           ,[UserEvent2]
           ,[UserEvent2Date]
           ,[Notes]
           ,[Password])
	SELECT @dt, @CustomerID
		   ,[EmailAddress]
           ,[Title]
           ,[FirstName]
           ,[LastName]
           ,[FullName]
           ,[Company]
           ,[Occupation]
           ,[Address]
           ,[Address2]
           ,[City]
           ,[State]
           ,[Zip]
           ,[Country]
           ,[Voice]
           ,[Mobile]
           ,[Fax]
           ,[Website]
           ,[Age]
           ,[Income]
           ,[Gender]
           ,[User1]
           ,[User2]
           ,[User3]
           ,[User4]
           ,[User5]
           ,[User6]
           ,[Birthdate]
           ,[UserEvent1]
           ,[UserEvent1Date]
           ,[UserEvent2]
           ,[UserEvent2Date]
           ,[Notes]
           ,[Password]
	  FROM #result
	 WHERE EmailID IS NULL 
	   AND Result = 'New, Subscribed'
	     ;
	UPDATE #result
	   SET EmailID = E.EmailID
      FROM Emails (NOLOCK) E, 
           #result RT
     WHERE E.CustomerID = @CustomerID 
       AND E.EmailAddress = RT.EmailAddress
       AND RT.EmailID IS NULL
	   AND RT.Result = 'New, Subscribed'
	     ;

	-- adding the email/group association
	INSERT
	  INTO EmailGroups (GroupID, EmailID, SubscribeTypeCode, FormatTypeCode, CreatedOn, LastChanged, SMSEnabled)
	SELECT RT.GroupID, RT.EMailID, RT.NewSubscribeTypeCode, RT.NewFormatTypeCode, @dt, @dt, 0
	  FROM #result RT
	 WHERE RT.EmailGroupID IS NULL
	   AND RT.EmailID IS NOT NULL
	   AND RT.Result = 'New, Subscribed'
	UPDATE #result
	   SET EmailGroupID = EG.EmailGroupID
      FROM EmailGroups (NOLOCK) EG, 
           #result RT
     WHERE EG.GroupID = RT.GroupID
       AND EG.EmailID = RT.EmailID
       AND RT.EmailGroupID IS NULL
	   AND RT.Result = 'New, Subscribed'
	     ;
	
	-- merge for standalone UDFs
	MERGE EmailDataValues AS target
    USING (
			SELECT U.FieldValue,R.EmailID,U.GroupDataFieldsID 
			  FROM #StandaloneUDF U
			  JOIN #result R ON(
								U.SubscriberInputTableRowID = R.SubscriberInputTableRowID 
			                    -- AND R.Result NOT LIKE 'Skipped%'
			                    )
			  WHERE R.Result LIKE '%, Subscribed'
          ) AS source (FieldValue,EmailID,GroupDataFieldsID)
    ON (target.GroupDataFieldsID = source.GroupDataFieldsID AND target.EmailID = source.EmailID)
    WHEN MATCHED THEN 
        UPDATE SET DataValue = source.FieldValue
                 , ModifiedDate=@DT
                 , UpdatedUserID=@UserID
	WHEN NOT MATCHED AND source.EmailID IS NOT NULL THEN
		INSERT (EmailID, GroupDataFieldsID, DataValue, CreatedDate, CreatedUserID)
		VALUES (source.EmailID, source.GroupDataFieldsID, source.FieldValue, @dt, @UserID)
	;
	
	-- Merge for transactional UDFs
	MERGE EmailDataValues AS target
	USING (
			SELECT U.FieldValue,R.EmailID,U.GroupDataFieldsID,U.TransactionID 
			  FROM #TransactionalUDF U
			  JOIN #result R ON(
								U.SubscriberInputTableRowID = R.SubscriberInputTableRowID 
			                    -- AND R.Result NOT LIKE 'Skipped%'
			                    )
			  WHERE R.Result LIKE '%, Subscribed'
			  
	      ) As source (FieldValue,EmailID,GroupDataFieldsID,TransactionID)
	   ON ( target.GroupDataFieldsID = source.GroupDataFieldsID 
	        AND target.EmailID = source.EmailID 
	        AND target.EntryID = source.TransactionID )
    WHEN MATCHED THEN 
        UPDATE SET DataValue = source.FieldValue
                 , ModifiedDate=@DT
                 , UpdatedUserID=@UserID
	WHEN NOT MATCHED AND source.EmailID IS NOT NULL THEN
		INSERT (EmailID, GroupDataFieldsID, EntryID, DataValue, CreatedDate, CreatedUserID)
		VALUES (source.EmailID, source.GroupDataFieldsID, source.TransactionID, source.FieldValue, @dt, @UserID)
	;	   
	
	END --END TestMode
	
	-- collect current status of all result records that represent existing emailAddress/Group associations
	-- this will be expensive for large sets with many different group IDs however it improves testablity 
	-- by providing a cross check on Result (e.g. Status should be "S" anytime Result like "%Subscribed", etc.)
	UPDATE #result
	   SET Status = EG.SubscribeTypeCode
	  FROM #result RT,
	       EmailGroups EG 
	 WHERE RT.EmailGroupID IS NOT NULL
	   AND RT.EmailGroupID = EG.EmailGroupID;
	
	-- fix status for master-suppressed subscribers
	UPDATE #result
	   SET Status = 'M'
	 WHERE LOWER(Result) like '%suppressed';

	-- Output the result set
	SELECT 	EmailAddress
	     , ISNULL(GroupID, OriginalGroupID) GroupID
	     , EmailID
	     , EmailGroupID
	     , [Status]
	     , Result 
	FROM #result;
	
	DROP TABLE #result;
	DROP TABLE #StandaloneUDF;
	DROP TABLE #TransactionalUDF;
END
RETURN 0
