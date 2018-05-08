CREATE PROCEDURE [dbo].[e_LinkOwnerIndex_Exists_ByCustomerID]   
(
	@CustomerID int = NULL
)
AS
	IF EXISTS ( SELECT TOP 1 LinkOwnerIndexID FROM LinkOwnerIndex WITH (NOLOCK) WHERE CustomerID = @CustomerID AND IsDeleted = 0) SELECT 1 ELSE SELECT 0