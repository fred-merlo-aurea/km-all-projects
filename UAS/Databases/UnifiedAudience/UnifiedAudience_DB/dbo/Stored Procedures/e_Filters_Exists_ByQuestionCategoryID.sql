CREATE PROCEDURE [dbo].[e_Filters_Exists_ByQuestionCategoryID] 
	@QuestionCategoryID int
AS       
BEGIN

	SET NOCOUNT ON
 		
	IF EXISTS  (
				Select Top 1 QuestionCategoryID 
				from Filters WITH (NOLOCK)
				where IsDeleted = 0 and 
				QuestionCategoryID = @QuestionCategoryID
				) SELECT 1 ELSE SELECT 0

END