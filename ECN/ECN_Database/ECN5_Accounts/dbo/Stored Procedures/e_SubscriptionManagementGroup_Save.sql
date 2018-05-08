CREATE PROCEDURE [dbo].[e_SubscriptionManagementGroup_Save]
	@SMGID int,
	@SMID int,
	@GroupID int,
	@IsDeleted bit,
	@Label varchar(50),
	@UpdatedUserID int = null,
	@UpdatedDate datetime = null,
	@CreatedUserID int = null,
	@CreatedDate datetime = null
AS
	if @SMGID > 0
	BEGIN
		UPDATE SubscriptionManagementGroup
		SET GroupID = @GroupID, IsDeleted = @IsDeleted, Label = @Label, UpdatedDate = @UpdatedDate, UpdatedUserID = @UpdatedUserID
		WHERE SubscriptionManagementGroupID = @SMGID 
		SELECT @SMGID
	END
	ELSE
	BEGIN
		INSERT INTO SubscriptionManagementGroup(CreatedDate, CreatedUserID, SubscriptionManagementPageID, GroupID, IsDeleted, Label)
		VALUES(@CreatedDate, @CreatedUserID, @SMID, @GroupID, @IsDeleted, @Label)
		SELECT @@IDENTITY;
	END