create  PROC [dbo].[e_Action_Exists_ByID] 
(
	@ActionID int = NULL
	)
AS 
BEGIN
	IF EXISTS (
		SELECT TOP 1 ActionID
		from 
			[Action]  with (nolock)
		where 
			ActionID = @ActionID AND IsDeleted = 0 
	) SELECT 1 ELSE SELECT 0
END
