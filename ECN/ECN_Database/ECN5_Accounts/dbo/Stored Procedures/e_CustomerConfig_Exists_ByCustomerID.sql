-- Procedure-- Procedure
CREATE PROCEDURE [dbo].[e_CustomerConfig_Exists_ByCustomerID] 
	@CustomerID int
AS     
BEGIN     		
	IF EXISTS (SELECT TOP 1 CustomerConfigID FROM CustomerConfig WHERE CustomerID = @CustomerID and ProductID = 100 and ConfigName = 'PickupPath' AND IsDeleted = 0) SELECT 1 ELSE SELECT 0
END
