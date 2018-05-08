CREATE PROCEDURE [dbo].[e_History_Save]
@HistoryID int,
@BatchID int,
@BatchCountItem int,
@PublicationID int,
@SubscriptionID int,
@PubSubscriptionID int,
@HistorySubscriptionID int,
@HistoryPaidID int,
@HistoryPaidBillToID int,
@DateCreated datetime,
@CreatedByUserID  int
AS
BEGIN

	SET NOCOUNT ON

	IF @HistoryID > 0
		BEGIN
			UPDATE History
			SET 
				BatchID = @BatchID,
				BatchCountItem = @BatchCountItem,
				PublicationID = @PublicationID,
				SubscriptionID = @SubscriptionID,
				PubSubscriptionID = @PubSubscriptionID,
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
			INSERT INTO History (BatchID,BatchCountItem,PublicationID,SubscriptionID,PubSubscriptionID,HistorySubscriptionID,HistoryPaidID,HistoryPaidBillToID,DateCreated,CreatedByUserID)
			VALUES(@BatchID,@BatchCountItem,@PublicationID,@SubscriptionID,@PubSubscriptionID,@HistorySubscriptionID,@HistoryPaidID,@HistoryPaidBillToID,@DateCreated,@CreatedByUserID);SELECT @@IDENTITY;
		END

END