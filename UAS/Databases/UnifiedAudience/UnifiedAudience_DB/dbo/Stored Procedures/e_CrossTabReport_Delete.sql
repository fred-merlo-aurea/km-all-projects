CREATE PROCEDURE [dbo].[e_CrossTabReport_Delete]
@CrossTabReportID int,
@UserID int
AS
BEGIN

	SET NOCOUNT ON

	Update CrossTabReport 
	set IsDeleted = 1, 
		UpdatedDate = GETDATE(), 
		UpdatedUserID = @UserID  
	where CrossTabReportID = @CrossTabReportID

End