CREATE PROCEDURE [dbo].[e_Content_Select_ContentID]   
@ContentID int
AS
	SELECT * FROM Content WITH (NOLOCK) WHERE ContentID = @ContentID and IsDeleted = 0