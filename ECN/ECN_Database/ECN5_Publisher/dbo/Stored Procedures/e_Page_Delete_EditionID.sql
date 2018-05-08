CREATE PROCEDURE [dbo].[e_Page_Delete_EditionID]   
@EditionID int,
@UserID int

AS

	Update page SET IsDeleted = 1, UpdatedUserID = @UserID, UpdatedDate = GETDATE() where  EditionID = @EditionID