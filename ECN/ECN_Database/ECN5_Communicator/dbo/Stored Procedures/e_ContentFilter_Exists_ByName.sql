CREATE PROCEDURE [dbo].[e_ContentFilter_Exists_ByName] 
	@LayoutID int = NULL,
	@CustomerID int = NULL,
	@FilterID int = NULL,
	@FilterName varchar(50) = NULL
AS     
BEGIN     		
	IF EXISTS (SELECT TOP 1 cf.FilterID 
				FROM ContentFilter cf WITH (NOLOCK)
					join Content c WITH (NOLOCK) on cf.ContentID = c.ContentID
				WHERE c.CustomerID = @CustomerID AND 
					cf.FilterID != ISNULL(@FilterID, -1) AND 
					cf.LayoutID = @LayoutID AND 
					cf.FilterName = @FilterName AND 
					cf.IsDeleted = 0 AND
					c.IsDeleted = 0) SELECT 1 ELSE SELECT 0
END