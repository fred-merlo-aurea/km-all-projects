CREATE PROCEDURE [dbo].[e_DynamicTagRule_Select_DynamicTagID]   
@DynamicTagID int
AS


SELECT dtr.* FROM DynamicTagRule dtr WITH (NOLOCK)
WHERE dtr.DynamicTagID = @DynamicTagID and dtr.IsDeleted = 0