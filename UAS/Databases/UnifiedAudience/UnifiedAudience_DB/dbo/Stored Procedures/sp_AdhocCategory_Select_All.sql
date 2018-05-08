CREATE PROCEDURE [dbo].[sp_AdhocCategory_Select_All]   

AS  
BEGIN
	
	SET NOCOUNT ON 
  
	SELECT  * 
	FROM AdhocCategory  WITH(NOLOCK)
	order by SortOrder

  END