CREATE PROCEDURE e_IssueArchiveSubscription_Save
@IssueArchiveSubscriptionId int,
@IssueArchiveSubscriberId int,
@IsComp bit,
@CompId int,
@SubscriptionID int,
@SequenceID int,
@PublisherID int,
@SubscriberID int,
@PublicationID int,
@ActionID_Current int,
@ActionID_Previous int,
@SubscriptionStatusID int,
@IsPaid bit,
@QSourceID int,
@QSourceDate date,
@DeliverabilityID int,
@IsSubscribed bit,
@SubscriberSourceCode varchar(256),
@Copies int,
@OriginalSubscriberSourceCode varchar(256),
@Par3cID int,
@SubsrcTypeID int,
@AccountNumber varchar(50),
@GraceIssues	int,
@OnBehalfOf varchar(256), 
@MemberGroup varchar(256), 
@Verify varchar(256), 
@IsNewSubscription bit,
@DateCreated datetime,
@DateUpdated datetime,
@CreatedByUserID int,
@UpdatedByUserID int
AS
IF @IssueArchiveSubscriptionId > 0
	BEGIN
		IF @DateUpdated IS NULL
			BEGIN
				SET @DateUpdated = GETDATE();
			END
		UPDATE IssueArchiveSubscription
		SET IssueArchiveSubscriberId = @IssueArchiveSubscriberId,
			IsComp = @IsComp,
			CompId = @CompId,
			SubscriptionID = @SubscriptionID,
			SequenceID = @SequenceID,
			PublisherID = @PublisherID,
			SubscriberID = @SubscriberID,
			PublicationID = @PublicationID,
			ActionID_Current = @ActionID_Current,
			ActionID_Previous = @ActionID_Previous,
			SubscriptionStatusID = @SubscriptionStatusID,
			IsPaid = @IsPaid,
			QSourceID = @QSourceID,
			QSourceDate = @QSourceDate,
			DeliverabilityID = @DeliverabilityID,
			IsSubscribed = @IsSubscribed,
			SubscriberSourceCode = @SubscriberSourceCode,
			Copies = @Copies,
			OriginalSubscriberSourceCode = @OriginalSubscriberSourceCode,
			Par3cID = @Par3cID,
			SubsrcTypeID = @SubsrcTypeID,
			AccountNumber = @AccountNumber,
			GraceIssues = @GraceIssues,
			OnBehalfOf = @OnBehalfOf, 
			MemberGroup = @MemberGroup, 
			Verify = @Verify, 
			IsNewSubscription = @IsNewSubscription,
			DateCreated = @DateCreated,
			DateUpdated = @DateUpdated,
			CreatedByUserID = @CreatedByUserID,
			UpdatedByUserID = @UpdatedByUserID
		WHERE IssueArchiveSubscriptionId = @IssueArchiveSubscriptionId;

		SELECT @IssueArchiveSubscriptionId;
	END
ELSE
	BEGIN
		IF @DateCreated IS NULL
			BEGIN
				SET @DateCreated = GETDATE();
			END
		INSERT into IssueArchiveSubscription (IssueArchiveSubscriberId,IsComp,CompId,SubscriptionID,SequenceID,PublisherID,SubscriberID,PublicationID,
											  ActionID_Current,ActionID_Previous,SubscriptionStatusID,IsPaid,QSourceID,QSourceDate,DeliverabilityID,
											  IsSubscribed,SubscriberSourceCode,Copies,OriginalSubscriberSourceCode,Par3cID,SubsrcTypeID,AccountNumber,
											  GraceIssues,OnBehalfOf,MemberGroup,Verify,IsNewSubscription,DateCreated,CreatedByUserID)
		VALUES(@IssueArchiveSubscriberId,@IsComp,@CompId,@SubscriptionID,@SequenceID,@PublisherID,@SubscriberID,@PublicationID,
			   @ActionID_Current,@ActionID_Previous,@SubscriptionStatusID,@IsPaid,@QSourceID,@QSourceDate,@DeliverabilityID,
			   @IsSubscribed,@SubscriberSourceCode,@Copies,@OriginalSubscriberSourceCode,@Par3cID,@SubsrcTypeID,@AccountNumber,
			   @GraceIssues,@OnBehalfOf,@MemberGroup,@Verify,@IsNewSubscription,@DateCreated,@CreatedByUserID);SELECT @@IDENTITY;
	END
GO
