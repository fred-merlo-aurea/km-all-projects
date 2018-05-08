CREATE PROCEDURE [dbo].[e_Filter_Exists_ByName] 
	@FilterID int = NULL,
	@CustomerID int = NULL,
	@GroupID int = NULL,
	@FilterName varchar(50) = NULL
AS     
BEGIN     		
	IF EXISTS (SELECT TOP 1 FilterID FROM Filter WITH (NOLOCK) WHERE CustomerID = @CustomerID AND FilterID != ISNULL(@FilterID, -1) AND GroupID = @GroupID AND FilterName = @FilterName AND IsDeleted = 0) SELECT 1 ELSE SELECT 0
END