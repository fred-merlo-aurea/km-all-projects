create PROCEDURE [dbo].[e_EditionActivityLog_Delete_EditionID]   
@EditionID int,
@UserID int

AS

	delete from EditionActivityLog where EditionID = @EditionID