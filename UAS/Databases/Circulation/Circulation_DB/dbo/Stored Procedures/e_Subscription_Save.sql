
CREATE PROCEDURE [dbo].[e_Subscription_Save]
@SubscriptionID int,
@SequenceID int = 0,
@SubscriberID int,
@PublisherID int,
@PublicationID int,
@ActionID_Current int,
@ActionID_Previous int,
@SubscriptionStatusID int,
@IsPaid bit,
@QSourceID int,
@QSourceDate datetime,
@DeliverabilityID int,
@IsSubscribed bit,
@SubscriberSourceCode varchar(256)='',
@Copies int,
@OriginalSubscriberSourceCode varchar(256)='',
@DateCreated datetime,
@DateUpdated datetime,
@CreatedByUserID int,
@UpdatedByUserID int,
@Par3cID int,
@SubsrcTypeID int,
@AccountNumber varchar(50),
@OnBehalfOf varchar(256),
@MemberGroup varchar(256),
@Verify varchar(256),
@AddRemoveID int,
@IMBSeq varchar(10),
@IsActive BIT = 'true'

AS

IF @SubscriptionID > 0
	BEGIN
		IF @DateUpdated IS NULL
			BEGIN
				SET @DateUpdated = GETDATE();
			END
		
		--IF @SequenceID = 0
		--	BEGIN
		--		SET @SequenceID = (Select MAX(SequenceID) + 1 FROM Subscription WHERE PublicationID = @PublicationID)
		--	END
			
		UPDATE Subscription
		SET 
			--SequenceID = @SequenceID,
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
			DateUpdated = @DateUpdated,
			UpdatedByUserID = @UpdatedByUserID,
			Par3cID = @Par3cID,
			SubsrcTypeID = @SubsrcTypeID,
			AccountNumber = @AccountNumber,
			OnBehalfOf = @OnBehalfOf,
			MemberGroup = @MemberGroup,
			Verify = @Verify,
			IMBSeq = @IMBSeq,
			IsActive = @IsActive
		WHERE SubscriptionID = @SubscriptionID;
		
		SELECT @SubscriptionID;
	END
ELSE
	BEGIN
		IF @DateCreated IS NULL
			BEGIN
				SET @DateCreated = GETDATE();
			END
		IF @SequenceID = 0
			BEGIN
				SET @SequenceID = (Select MAX(SequenceID) + 1 FROM Subscription WHERE PublicationID = @PublicationID)
				IF(@SequenceID) IS NULL
				BEGIN
					SET @SequenceID = 1
				END
			END	
		--INSERT INTO PublicationSequence (PublicationID,SequenceID, DateCreated, CreatedByUserID)
		--	VALUES(@PublicationID,@SequenceID, @DateCreated, @CreatedByUserID)
			
		INSERT INTO Subscription (SequenceID,PublisherID,SubscriberID,PublicationID,ActionID_Current,ActionID_Previous,SubscriptionStatusID,IsPaid,QSourceID,QSourceDate,DeliverabilityID,
									IsSubscribed,SubscriberSourceCode,Copies,OriginalSubscriberSourceCode,DateCreated,CreatedByUserID,Par3cID,SubsrcTypeID,AccountNumber,OnBehalfOf,MemberGroup,Verify, AddRemoveID, IMBSeq,IsActive)
		VALUES(@SequenceID,@PublisherID,@SubscriberID,@PublicationID,@ActionID_Current,@ActionID_Previous,@SubscriptionStatusID,@IsPaid,@QSourceID,@QSourceDate,@DeliverabilityID,
				@IsSubscribed,@SubscriberSourceCode,@Copies,@OriginalSubscriberSourceCode,@DateCreated,@CreatedByUserID,@Par3cID,@SubsrcTypeID,@AccountNumber,@OnBehalfOf,@MemberGroup,@Verify, @AddRemoveID, @IMBSeq,@IsActive);SELECT @@IDENTITY;
	END

