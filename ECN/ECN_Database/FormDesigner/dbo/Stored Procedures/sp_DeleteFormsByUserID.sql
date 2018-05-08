 
CREATE PROCEDURE sp_DeleteFormsByUserID
(
      @UserID INT
) AS
DECLARE @id INT
DECLARE f_cur CURSOR LOCAL FOR( SELECT Form_Seq_ID FROM Form WHERE UserID = @UserID ) 
OPEN f_cur
FETCH NEXT FROM f_cur INTO @id
WHILE @@FETCH_STATUS = 0 BEGIN
      EXEC sp_DeleteForm @id
      FETCH NEXT FROM f_cur INTO @id
END
CLOSE f_cur
DEALLOCATE f_cur