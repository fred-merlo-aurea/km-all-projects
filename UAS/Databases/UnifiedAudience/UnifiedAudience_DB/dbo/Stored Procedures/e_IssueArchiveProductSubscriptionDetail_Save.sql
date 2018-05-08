CREATE PROCEDURE e_IssueArchiveProductSubscriptionDetail_Save
@IAProductSubscriptionDetailID int,
@IssueArchiveSubscriptionId int,
@PubSubscriptionDetailID int,
@PubSubscriptionID int,
@SubscriptionID  int,
@CodeSheetID int,
@ResponseOther varchar(300),
@DateCreated datetime,
@DateUpdated datetime,
@CreatedByUserID int,
@UpdatedByUserID int
AS
BEGIN

	SET NOCOUNT ON

	IF @IssueArchiveSubscriptionId > 0
		BEGIN
			IF @DateUpdated IS NULL
				BEGIN
					SET @DateUpdated = GETDATE();
				END
			UPDATE IssueArchiveProductSubscriptionDetail
			SET PubSubscriptionID = @PubSubscriptionID,
				SubscriptionID = @SubscriptionID,
				CodeSheetID = @CodeSheetID,
				ResponseOther = @ResponseOther,
				DateUpdated = @DateUpdated,
				UpdatedByUserID = @UpdatedByUserID
			WHERE IAProductSubscriptionDetailID = @IAProductSubscriptionDetailID;

			SELECT @IAProductSubscriptionDetailID;
		END
	ELSE
		BEGIN
			IF @DateCreated IS NULL
				BEGIN
					SET @DateCreated = GETDATE();
				END
			INSERT into IssueArchiveProductSubscriptionDetail(IssueArchiveSubscriptionId,PubSubscriptionID,SubscriptionID,CodeSheetID,ResponseOther,DateCreated,CreatedByUserID)
			VALUES(@IssueArchiveSubscriptionId,@PubSubscriptionID,@SubscriptionID,@CodeSheetID,@ResponseOther,@DateCreated,@CreatedByUserID);SELECT @@IDENTITY;
		END

END
GO