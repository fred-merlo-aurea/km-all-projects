CREATE PROCEDURE [dbo].[e_Select_Actions_ActionID]
@ActionID int
AS
BEGIN	
	SET NOCOUNT ON;

    SELECT ActionID,
		  ProductID,
		  DisplayName,
		  ActionCode,
		  DisplayOrder		         
	FROM 
		[Action] WITH (NOLOCK) where ActionID = @ActionID
END
