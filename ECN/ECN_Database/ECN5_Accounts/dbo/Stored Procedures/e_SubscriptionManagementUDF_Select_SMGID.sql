CREATE PROCEDURE [dbo].[e_SubscriptionManagementUDF_Select_SMGID]
	@SMGID int
AS
	SELECT * 
	FROM SubscriptionManagementUDF
	WHERE SubscriptionManagementGroupID = @SMGID and IsDeleted = 0