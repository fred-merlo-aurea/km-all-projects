CREATE PROCEDURE [dbo].[e_MasterGroup_Delete]
	@MasterGroupID int
AS
BEGIN

	SET NOCOUNT ON

	DELETE FROM MasterGroups WHERE MasterGroupID = @MasterGroupID

END