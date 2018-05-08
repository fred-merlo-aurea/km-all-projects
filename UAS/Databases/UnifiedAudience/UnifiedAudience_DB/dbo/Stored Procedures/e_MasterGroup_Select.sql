CREATE PROCEDURE e_MasterGroup_Select
AS
BEGIN

	SET NOCOUNT ON

	SELECT *
	FROM MasterGroups With(NoLock)
	ORDER BY SortOrder

END
GO