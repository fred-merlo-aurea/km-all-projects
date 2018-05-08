CREATE PROCEDURE [dbo].[e_Action_Select_ActionID]
@ActionID int
AS    
BEGIN

	set nocount on

	SELECT *
	FROM [Action] With(NoLock)
	WHERE ActionID = @ActionID

END