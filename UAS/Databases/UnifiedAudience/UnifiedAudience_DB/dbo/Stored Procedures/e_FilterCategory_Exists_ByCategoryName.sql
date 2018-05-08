CREATE PROCEDURE [dbo].[e_FilterCategory_Exists_ByCategoryName]
	@FilterCategoryID int,
	@CategoryName varchar(50)
AS        
BEGIN

	SET NOCOUNT ON
 		
	IF EXISTS  (
		Select Top 1 FilterCategoryID 
		from FilterCategory WITH (NOLOCK) 
		where IsDeleted = 0 and 
			CategoryName = @CategoryName and filterCategoryID <> @FilterCategoryID
		) SELECT 1 ELSE SELECT 0
END