CREATE PROCEDURE [dbo].[e_LinkAlias_Select_ContentID]   
@ContentID int = NULL
AS
	SELECT la.*, c.CustomerID 
	FROM LinkAlias la with (nolock) join Content c WITH (NOLOCK) on la.ContentID = c.ContentID 
	WHERE la.ContentID = @ContentID and la.IsDeleted = 0 and c.IsDeleted = 0