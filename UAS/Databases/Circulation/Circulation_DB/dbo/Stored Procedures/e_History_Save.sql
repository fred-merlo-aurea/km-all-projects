CREATE PROCEDURE [e_History_Save]
@HistoryID int,
@BatchID int,
@BatchCountItem int,
@PublisherID int,
@PublicationID int,
@SubscriberID int,
@SubscriptionID int,
@HistorySubscriptionID int,
@HistoryPaidID int,
@HistoryPaidBillToID int,
@DateCreated datetime,
@CreatedByUserID  int
AS

IF @HistoryID > 0
	BEGIN
		UPDATE History
		SET 
			BatchID = @BatchID,
			BatchCountItem = @BatchCountItem,
			PublisherID = @PublisherID,
			PublicationID = @PublicationID,
			SubscriberID = @SubscriberID,
			SubscriptionID = @SubscriptionID,
			HistorySubscriptionID = @HistorySubscriptionID,
			HistoryPaidID = @HistoryPaidID,
			HistoryPaidBillToID = @HistoryPaidBillToID,
			DateCreated = @DateCreated,
			CreatedByUserID = @CreatedByUserID
		WHERE HistoryID = @HistoryID 
		
		SELECT @HistoryID;
	END
ELSE
	BEGIN
		IF @DateCreated IS NULL
			BEGIN
				SET @DateCreated = GETDATE();
			END
		INSERT INTO History (BatchID,BatchCountItem,PublisherID,PublicationID,SubscriberID,SubscriptionID,HistorySubscriptionID,HistoryPaidID,HistoryPaidBillToID,DateCreated,CreatedByUserID)
		VALUES(@BatchID,@BatchCountItem,@PublisherID,@PublicationID,@SubscriberID,@SubscriptionID,@HistorySubscriptionID,@HistoryPaidID,@HistoryPaidBillToID,@DateCreated,@CreatedByUserID);SELECT @@IDENTITY;
	END
