CREATE PROCEDURE [dbo].[e_LinkAlias_Select_Link]   
@ContentID int = NULL,
@Link varchar(300) = NULL
AS
	SELECT la.*, c.CustomerID 
	FROM LinkAlias la with (nolock) join Content c WITH (NOLOCK) on la.ContentID = c.ContentID 
	WHERE la.ContentID = @ContentID and la.Link = @Link and la.IsDeleted = 0 and c.IsDeleted = 0