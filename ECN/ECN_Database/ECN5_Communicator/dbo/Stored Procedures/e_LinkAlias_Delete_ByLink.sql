CREATE  PROC [dbo].[e_LinkAlias_Delete_ByLink] 
(
	@ContentID int = NULL,
	@Link varchar(300) = NULL,
    @CustomerID int = NULL,
    @UserID int = NULL
)
AS 
BEGIN
	UPDATE la SET la.IsDeleted = 1, la.UpdatedDate = GETDATE(), la.UpdatedUserID = @UserID
	FROM 
		LinkAlias la WITH (NOLOCK) 
		JOIN Content c WITH (NOLOCK) ON la.ContentID = c.ContentID
	WHERE 
		la.ContentID = @ContentID and la.Link = @Link and c.CustomerID = @CustomerID
END
