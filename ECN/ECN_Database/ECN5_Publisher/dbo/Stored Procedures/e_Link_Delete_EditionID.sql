CREATE PROCEDURE [dbo].[e_Link_Delete_EditionID]   
@EditionID int,
@UserID int

AS

	Update Link  SET IsDeleted = 1, UpdatedUserID = @UserID, UpdatedDate = GETDATE() where PageID in (select pageID from Page where  EditionID = @EditionID)