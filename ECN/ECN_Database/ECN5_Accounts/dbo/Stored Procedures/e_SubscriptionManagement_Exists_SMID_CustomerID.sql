CREATE PROCEDURE [dbo].[e_SubscriptionManagement_Exists_SMID_CustomerID]
	@SMID int,
	@CustomerID int
AS
	if exists(SELECT TOP 1 * from SubscriptionManagement where SubscriptionManagementID = @SMID and IsDeleted = 0 and BaseChannelID = (SELECT BaseChannelID FROM Customer where CustomerID = @CustomerID))
	BEGIN
		SELECT 1
	END
	ELSE
	BEGIN
		SELECT 0
	END
