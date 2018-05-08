CREATE PROCEDURE [dbo].[e_CustomerTemplate_Delete]   
@CTID int, 
@UserID int

AS
	Update CustomerTemplate SET IsDeleted = 1, UpdatedUserID =  @UserID, UpdatedDate = GETDATE() where  CTID = @CTID
