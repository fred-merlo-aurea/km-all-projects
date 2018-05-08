CREATE PROCEDURE [dbo].[e_CustomerLicense_Delete]   
@CLID int,
@UserID int

AS
	Update CustomerLicense SET IsDeleted = 1, UpdatedUserID = @UserID, UpdatedDate = GETDATE()  where  CLID = @CLID
