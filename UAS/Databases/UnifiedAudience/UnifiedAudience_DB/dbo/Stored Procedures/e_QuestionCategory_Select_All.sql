CREATE PROCEDURE [dbo].[e_QuestionCategory_Select_All]
AS  
BEGIN

	SET NOCOUNT ON
  
	Select * 
	from QuestionCategory with(nolock) 
	where IsDeleted = 0  order by CategoryName

End