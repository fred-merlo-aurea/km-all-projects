CREATE PROCEDURE [dbo].[e_SubscriptionManagementReason_Save]
	@SubscriptionManagementReasonID int,
	@SubscriptionManagementID int,
	@Reason varchar(100),
	@IsDeleted bit,
	@CreatedUserID int = null,
	@UpdatedUserID int = null,
	@SortOrder int

AS
	if(@SubscriptionManagementReasonID > 0)
	BEGIN
		UPDATE SubscriptionManagementReason
		SET SubscriptionManagementID = @SubscriptionManagementID, Reason = @Reason, IsDeleted = @IsDeleted, UpdatedUserID = @UpdatedUserID, UpdatedDate = GETDATE(), SortOrder = @SortOrder
		WHERE SubscriptionManagementReasonID = @SubscriptionManagementReasonID
		SELECT @SubscriptionManagementReasonID
	END
	ELSE
	BEGIN
		INSERT INTO SubscriptionManagementReason(SubscriptionManagementID, Reason, IsDeleted, CreatedUserID, CreatedDate, SortOrder)
		VALUES(@SubscriptionManagementID, @Reason, @IsDeleted, @CreatedUserID,GETDATE(), @SortOrder) 
		SELECT @@IDENTITY;
	END
