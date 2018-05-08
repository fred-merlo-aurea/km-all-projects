CREATE PROCEDURE [dbo].[e_DynamicTag_Select_DynamicTagID]   
@DynamicTagID int
AS


SELECT dt.* FROM DynamicTag dt WITH (NOLOCK)
WHERE dt.DynamicTagID = @DynamicTagID and dt.IsDeleted = 0