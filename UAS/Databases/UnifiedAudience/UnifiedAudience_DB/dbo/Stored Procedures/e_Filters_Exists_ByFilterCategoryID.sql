CREATE PROCEDURE [dbo].[e_Filters_Exists_ByFilterCategoryID] 
	@FilterCategoryID int
AS     
BEGIN

	SET NOCOUNT ON
   		
	IF EXISTS  (
				Select Top 1 FilterCategoryID 
				from Filters  WITH (NOLOCK)
				where IsDeleted = 0 and 
					FilterCategoryID = @FilterCategoryID
				) SELECT 1 ELSE SELECT 0

END