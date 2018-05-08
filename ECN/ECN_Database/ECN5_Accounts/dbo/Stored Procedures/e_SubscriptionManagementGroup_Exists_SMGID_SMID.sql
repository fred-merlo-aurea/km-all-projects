CREATE PROCEDURE [dbo].[e_SubscriptionManagementGroup_Exists_SMGID_SMID]
	@SMGID int,
	@SMID int
AS
	if exists(SELECT TOP 1 * from SubscriptionManagementGroup WHERE SubscriptionManagementGroupID = @SMGID and SubscriptionManagementPageID = @SMID and IsDeleted = 0)
	BEGIN
		SELECT 1
	END
	ELSE
	BEGIN
		SELECT 0
	END
