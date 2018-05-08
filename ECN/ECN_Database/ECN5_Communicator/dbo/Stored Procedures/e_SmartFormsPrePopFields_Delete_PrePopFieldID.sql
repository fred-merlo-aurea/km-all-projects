CREATE  PROC [dbo].[e_SmartFormsPrePopFields_Delete_PrePopFieldID] 
(
	@SFID int = NULL,
	@PrePopFieldID int = NULL,
    @CustomerID int = NULL,
    @UserID int = NULL
)
AS 
BEGIN
	UPDATE sfppf 
		SET sfppf.IsDeleted = 1, sfppf.UpdatedDate = GETDATE(), sfppf.UpdatedUserID = @UserID
	FROM 
		SmartFormsPrePopFields sfppf WITH (NOLOCK)
		JOIN SmartFormsHistory sfh WITH (NOLOCK) ON sfppf.SFID = sfh.SmartFormID
		JOIN [Groups] g WITH (NOLOCK) ON sfh.GroupID = g.GroupID
	WHERE 
		g.CustomerID = @CustomerID AND 
		sfppf.SFID = @SFID AND
		sfppf.PrePopFieldID = @PrePopFieldID
END
