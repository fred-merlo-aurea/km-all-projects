create PROCEDURE [dbo].[e_Link_Delete]   
@LinkID int,
@UserID int

AS
	Update Link SET IsDeleted = 1, UpdatedUserID = @UserID, UpdatedDate = GETDATE()  where  LinkID = @LinkID