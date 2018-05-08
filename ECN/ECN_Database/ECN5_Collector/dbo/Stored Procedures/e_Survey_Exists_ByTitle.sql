CREATE PROCEDURE [dbo].[e_Survey_Exists_ByTitle] 
	@SurveyID int = NULL,
	@CustomerID int = NULL,
	@SurveyTitle varchar(50) = NULL
AS     
BEGIN     		
	IF EXISTS (SELECT TOP 1 SurveyID FROM [Survey] WITH (NOLOCK) WHERE CustomerID = @CustomerID AND SurveyID != ISNULL(@SurveyID, -1) AND SurveyTitle = @SurveyTitle) SELECT 1 ELSE SELECT 0
END