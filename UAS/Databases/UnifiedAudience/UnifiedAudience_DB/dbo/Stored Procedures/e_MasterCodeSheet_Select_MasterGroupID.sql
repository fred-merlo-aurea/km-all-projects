CREATE PROCEDURE [dbo].[e_MasterCodeSheet_Select_MasterGroupID]
	@MasterGroupID int
AS
BEGIN

	SET NOCOUNT ON

	SELECT * FROM MasterCodeSheet With(NoLock) 
	WHERE MasterGroupID = @MasterGroupID

END