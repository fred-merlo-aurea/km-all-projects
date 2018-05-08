CREATE PROCEDURE [dbo].[e_DynamicTag_Select_CustomerID]   
@CustomerID int
AS


SELECT dt.* FROM DynamicTag dt WITH (NOLOCK)
WHERE dt.CustomerID = @CustomerID and dt.IsDeleted = 0