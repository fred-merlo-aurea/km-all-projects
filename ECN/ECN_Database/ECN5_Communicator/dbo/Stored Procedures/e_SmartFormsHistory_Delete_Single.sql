CREATE  PROC [dbo].[e_SmartFormsHistory_Delete_Single] 
(
	@SmartFormID int = NULL,
    @CustomerID int = NULL,
    @UserID int = NULL
)
AS 
BEGIN
	UPDATE sfh 
		SET sfh.IsDeleted = 1, sfh.UpdatedDate = GETDATE(), sfh.UpdatedUserID = @UserID
	FROM 
		SmartFormsHistory sfh WITH (NOLOCK)
		JOIN [Groups] g WITH (NOLOCK) ON sfh.GroupID = g.GroupID
	WHERE 
		g.CustomerID = @CustomerID AND 
		sfh.SmartFormID = @SmartFormID
END
