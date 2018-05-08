CREATE PROCEDURE [dbo].[e_FilterCategory_Select_All]
AS  
BEGIN

	SET NOCOUNT ON
  
	Select * 
	from FilterCategory WITH (NOLOCK)
	where IsDeleted = 0  
	order by CategoryName

End