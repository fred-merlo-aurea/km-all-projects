CREATE PROCEDURE [dbo].[e_HistoryResponseMap_Save]
@PubSubscriptionDetailID int,
@PubSubscriptionID int,
@SubscriptionID int,
@CodeSheetID int,
@IsActive bit,
@DateCreated datetime,
@CreatedByUserID int,
@ResponseOther varchar(300),
@HistorySubscriptionID int = 0
AS
BEGIN

	SET NOCOUNT ON

	IF @DateCreated IS NULL
		BEGIN
			SET @DateCreated = GETDATE();
		END
	
	INSERT INTO HistoryResponseMap (PubSubscriptionDetailID,PubSubscriptionID,SubscriptionID,CodeSheetID,IsActive,DateCreated,CreatedByUserID,ResponseOther,HistorySubscriptionID)

	VALUES(@PubSubscriptionDetailID,@PubSubscriptionID,@SubscriptionID,@CodeSheetID,@IsActive,@DateCreated,@CreatedByUserID,@ResponseOther,@HistorySubscriptionID);SELECT @@IDENTITY;

END