CREATE PROCEDURE e_DynamicTag_Select_ContentID
@ContentID int
AS
SELECT *
FROM DynamicTag With(NoLock)
WHERE ContentID = @ContentID
