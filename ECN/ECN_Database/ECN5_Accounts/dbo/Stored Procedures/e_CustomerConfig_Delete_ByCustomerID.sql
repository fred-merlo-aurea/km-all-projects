CREATE PROCEDURE [dbo].[e_CustomerConfig_Delete_ByCustomerID]   
@CustomerID int,
@UserID int

AS
	Update CustomerConfig SET IsDeleted = 1, UpdatedUserID = @UserID, UpdatedDate = GETDATE()  where  CustomerConfigID = @CustomerID and ProductID = 100 and (ConfigName = 'PickupPath' or ConfigName = 'MailingIP')
