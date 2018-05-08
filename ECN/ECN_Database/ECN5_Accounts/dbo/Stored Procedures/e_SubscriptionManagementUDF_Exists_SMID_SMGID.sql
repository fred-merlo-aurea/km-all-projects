CREATE PROCEDURE [dbo].[e_SubscriptionManagementUDF_Exists_SMID_SMGID]
	@SMID int,
	@SMGID int
AS
	if exists(SELECT TOP 1 * FROM SubscriptionManagementUDF WHERE IsDeleted = 0 and SubscriptionManagementGroupID in (SELECT SubscriptionManagementGroupID FROM SubscriptionManagementGroup WHERE SubscriptionManagementPageID = @SMID and IsDeleted = 0))
	BEGIN
		SELECT 1
	END
	ELSE
	BEGIN
		SELECT 0
	END
