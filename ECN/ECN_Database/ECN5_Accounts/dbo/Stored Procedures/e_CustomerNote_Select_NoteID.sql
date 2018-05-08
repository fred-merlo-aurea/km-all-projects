create PROCEDURE [dbo].[e_CustomerNote_Select_NoteID]
@NoteID int
AS

SELECT * FROM CustomerNote WHERE NoteID = @NoteID  and IsDeleted=0
