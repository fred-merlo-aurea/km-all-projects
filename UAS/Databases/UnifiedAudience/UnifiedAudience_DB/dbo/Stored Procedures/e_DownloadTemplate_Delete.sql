CREATE PROCEDURE [dbo].[e_DownloadTemplate_Delete]
@DownloadTemplateID int,
@UserID int
AS
BEGIN

	SET NOCOUNT ON

	Update DownloadTemplate 
	set IsDeleted = 1, 
		UpdatedDate = GETDATE(), 
		UpdatedUserID = @UserID  
	where DownloadTemplateID = @DownloadTemplateID

End