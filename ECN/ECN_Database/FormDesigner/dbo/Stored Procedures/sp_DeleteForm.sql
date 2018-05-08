 
CREATE PROCEDURE sp_DeleteForm
(
      @FormID INT
) AS
 
DECLARE @gr_ids NVARCHAR(400)
DECLARE @data NVARCHAR(8)
SET @gr_ids = ''
DECLARE @id INT
DECLARE gr_cur CURSOR LOCAL FOR( SELECT ConditionGroup_Seq_ID FROM [Rule] 
                                                      WHERE Form_Seq_ID = @FormID OR Control_ID IN (SELECT Control_ID FROM Control WHERE Form_Seq_ID = @FormID) 
                                                 UNION SELECT ConditionGroup_Seq_ID FROM Notification 
                                                            WHERE Form_Seq_ID = @FormID AND ConditionGroup_Seq_ID IS NOT NULL ) 
OPEN gr_cur
FETCH NEXT FROM gr_cur INTO @id
WHILE @@FETCH_STATUS = 0 BEGIN
      SET @gr_ids = @gr_ids + CONVERT(NVARCHAR(8), @id) + ' '
      FETCH NEXT FROM gr_cur INTO @id
END
CLOSE gr_cur
DEALLOCATE gr_cur
 
DELETE [Rule] WHERE Form_Seq_ID = @FormID OR Control_ID IN (SELECT Control_ID FROM Control WHERE Form_Seq_ID = @FormID)
DELETE Notification WHERE Form_Seq_ID = @FormID
 
WHILE( LEN(@gr_ids) > 0 ) BEGIN
      SET @data = SUBSTRING(@gr_ids, 0, CHARINDEX(' ', @gr_ids))
      SET @id = CONVERT(INT, @data)
      EXEC sp_DeleteConditionGroup @id
      SET @gr_ids = SUBSTRING(@gr_ids, LEN(@data) + 2, LEN(@gr_ids))
END
 
DECLARE @cssId INT
SET @cssId = (SELECT CssFile_Seq_ID FROM Form WHERE Form_Seq_ID = @FormID)
UPDATE Form
SET CssFile_Seq_ID = NULL
WHERE Form_Seq_ID = @FormID
DELETE CssFile WHERE CssFile_Seq_ID = @cssId
DECLARE @childId INT
SET @childId = (SELECT Form_Seq_ID FROM Form WHERE ParentForm_ID = @FormID)
UPDATE Form
SET ParentForm_ID = Form_Seq_ID
WHERE Form_Seq_ID = @childId
DELETE Form WHERE Form_Seq_ID = @FormID
UPDATE Form
SET ParentForm_ID = NULL
WHERE Form_Seq_ID = @childId