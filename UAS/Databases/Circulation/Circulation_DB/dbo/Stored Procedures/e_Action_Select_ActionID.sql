CREATE PROCEDURE [dbo].[e_Action_Select_ActionID]
@ActionID int
AS    
  SELECT *
  FROM [Action] With(NoLock)
  WHERE ActionID = @ActionID
