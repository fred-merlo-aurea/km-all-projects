CREATE PROCEDURE sp_DeleteConditionGroup
(
      @GroupID INT
) AS
DECLARE gr_cur CURSOR LOCAL FOR( SELECT ConditionGroup_Seq_ID FROM ConditionGroup WHERE MainGroup_ID = @GroupID ) 
DECLARE @id INT
OPEN gr_cur
FETCH NEXT FROM gr_cur INTO @id
WHILE @@FETCH_STATUS = 0 BEGIN
      EXEC sp_DeleteConditionGroup @id
      FETCH NEXT FROM gr_cur INTO @id
END
CLOSE gr_cur
DEALLOCATE gr_cur
DELETE ConditionGroup WHERE ConditionGroup_Seq_ID = @GroupID