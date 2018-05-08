CREATE PROCEDURE [dbo].[e_QuestionCategory_Exists_ByCategoryName]
	@QuestionCategoryID int,
	@CategoryName varchar(50)
AS
BEGIN

	SET NOCOUNT ON
     		
	IF EXISTS  (
				Select Top 1 QuestionCategoryID 
				from QuestionCategory with(nolock) 
				where IsDeleted = 0 and CategoryName = @CategoryName and QuestionCategoryID <> @QuestionCategoryID
				) SELECT 1 ELSE SELECT 0
END