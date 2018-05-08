CREATE PROCEDURE [dbo].[e_LinkOwnerIndex_Exists_ByOwnerID]   
(
@LinkOwnerIndexID int = NULL,
@CustomerID int = NULL
)
AS
	IF EXISTS ( SELECT TOP 1 LinkOwnerIndexID FROM LinkOwnerIndex WITH (NOLOCK) WHERE LinkOwnerIndexID = @LinkOwnerIndexID AND CustomerID = @CustomerID AND IsDeleted = 0) SELECT 1 ELSE SELECT 0