CREATE PROCEDURE [dbo].[e_FilterCategory_Exists_ByParentID]
	@ParentID int
AS
BEGIN

	SET NOCOUNT ON
    		
	IF EXISTS  (
		Select Top 1 FilterCategoryID 
		from FilterCategory WITH (NOLOCK)
		where IsDeleted = 0 and 
			ParentID = @ParentID
		) SELECT 1 ELSE SELECT 0
END
