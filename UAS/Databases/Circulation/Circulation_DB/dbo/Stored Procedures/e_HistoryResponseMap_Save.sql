
CREATE PROCEDURE [dbo].[e_HistoryResponseMap_Save]
@SubscriptionID int,
@ResponseID int,
@IsActive bit,
@DateCreated datetime,
@CreatedByUserID int,
@ResponseOther varchar(300)
AS
IF @DateCreated IS NULL
	BEGIN
		SET @DateCreated = GETDATE();
	END
	
INSERT INTO HistoryResponseMap (SubscriptionID,ResponseID,IsActive,DateCreated,CreatedByUserID,ResponseOther)

VALUES(@SubscriptionID,@ResponseID,@IsActive,@DateCreated,@CreatedByUserID,@ResponseOther);SELECT @@IDENTITY;

