CREATE PROCEDURE [dbo].[e_QuestionCategory_Select]
AS
BEGIN

	SET NOCOUNT ON

	SELECT * 
	from QuestionCategory With(NoLock)

END