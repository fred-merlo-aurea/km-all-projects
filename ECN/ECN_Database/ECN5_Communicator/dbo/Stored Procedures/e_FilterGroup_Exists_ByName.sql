CREATE PROCEDURE [dbo].[e_FilterGroup_Exists_ByName] 
	@FilterID int = NULL,
	@FilterGroupID int = NULL,
	@CustomerID int = NULL,
	@Name varchar(50) = NULL
AS     
BEGIN     		
	IF EXISTS (
				SELECT TOP 1 fg.FilterID 
				FROM FilterGroup fg WITH (NOLOCK) join Filter f WITH (NOLOCK) on fg.FilterID = f.FilterID
				WHERE 
					f.CustomerID = @CustomerID AND 
					fg.FilterGroupID != ISNULL(@FilterGroupID, -1) AND 
					fg.FilterID = @FilterID AND 
					fg.Name = @Name AND 
					fg.IsDeleted = 0 AND
					f.IsDeleted = 0
				) SELECT 1 ELSE SELECT 0
END