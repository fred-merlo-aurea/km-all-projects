CREATE PROCEDURE [dbo].[e_MasterGroup_Select_ID]
@MasterGroupID int
AS
BEGIN

	set nocount on

	SELECT *
	FROM MasterGroups With(NoLock)
	WHERE MasterGroupID = @MasterGroupID

END
