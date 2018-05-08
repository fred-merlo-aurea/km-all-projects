CREATE PROCEDURE [dbo].[e_LinkAlias_Select_OwnerID]   
@OwnerID int = NULL
AS
	SELECT la.*, c.CustomerID 
	FROM LinkAlias la with (nolock) join Content c WITH (NOLOCK) on la.ContentID = c.ContentID 
	WHERE la.LinkOwnerID = @OwnerID and la.IsDeleted = 0 and c.IsDeleted = 0