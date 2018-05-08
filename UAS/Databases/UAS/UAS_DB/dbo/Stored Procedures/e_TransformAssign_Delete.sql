CREATE PROCEDURE [dbo].[e_TransformAssign_Delete]
	@TransformAssignID int
AS
BEGIN

	set nocount on

	Delete from TransformAssign
	where TransformAssignID = @TransformAssignID;
	Select @TransformAssignID;
	
END
