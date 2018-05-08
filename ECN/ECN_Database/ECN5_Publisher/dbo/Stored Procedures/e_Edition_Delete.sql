create PROCEDURE [dbo].[e_Edition_Delete]   
@EditionID int,
@UserID int

AS
	Update Edition SET IsDeleted = 1, UpdatedUserID = @UserID, UpdatedDate = GETDATE()  where  EditionID = @EditionID