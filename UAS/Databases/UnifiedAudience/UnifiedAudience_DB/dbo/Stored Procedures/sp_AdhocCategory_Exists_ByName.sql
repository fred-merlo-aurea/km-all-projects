CREATE  PROC [dbo].[sp_AdhocCategory_Exists_ByName] 
(
	@CategoryName varchar(50)
)
AS 
BEGIN
	
	SET NOCOUNT ON

	IF EXISTS (SELECT TOP 1 CategoryName from AdhocCategory WITH(NOLOCK) where CategoryName = @CategoryName) SELECT 1 ELSE SELECT 0

END