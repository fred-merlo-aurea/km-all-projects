create  PROC [dbo].[e_Template_Delete] 
(
	@UserID int = NULL,
    @TemplateID int = NULL
)
AS 
BEGIN
	Update Template SET IsDeleted = 1, UpdatedDate = GETDATE(), UpdatedUserID = @UserID WHERE TemplateID = @TemplateID 
END
