CREATE PROCEDURE [dbo].[e_SubscriptionManagementUDF_Save]
	@SMUDFID int,
	@GroupDataFieldsID int,
	@IsDeleted bit, 
	@StaticValue varchar(500),
	@SubscriptionManagementGroupID int,
	@UpdatedUserID int = null,
	@UpdatedDate datetime = null,
	@CreatedUserID int = null,
	@CreatedDate datetime = null
AS
	IF @SMUDFID > 0
	BEGIN
		UPDATE SubscriptionManagementUDF
		SET GroupDataFieldsID = @GroupDataFieldsID, IsDeleted = @IsDeleted, StaticValue = @StaticValue, SubscriptionManagementGroupID = @SubscriptionManagementGroupID, UpdatedUserID = @UpdatedUserID, UpdatedDate = @UpdatedDate
		WHERE SubscriptionManagementUDFID = @SMUDFID
	END
	ELSE
	BEGIN
		INSERT INTO SubscriptionManagementUDF(GroupDataFieldsID, IsDeleted, StaticValue, SubscriptionManagementGroupID, CreatedDate, CreatedUserID)
		VALUES(@GroupDataFieldsID, @IsDeleted, @StaticValue, @SubscriptionManagementGroupID, @CreatedDate, @CreatedUserID)
	END
