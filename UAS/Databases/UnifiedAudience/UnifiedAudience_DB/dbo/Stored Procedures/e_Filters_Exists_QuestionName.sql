CREATE PROCEDURE [dbo].[e_Filters_Exists_QuestionName]
	@FilterID int,
	@QuestionName varchar(50)
AS      
BEGIN

	SET NOCOUNT ON
  		
	IF EXISTS  (Select Top 1 FilterID 
				from Filters  WITH (NOLOCK)
				where IsDeleted = 0 and 
				FilterID <> @FilterID and QuestionName = @QuestionName
				) SELECT 1 ELSE SELECT 0

END