CREATE PROCEDURE [dbo].[e_Select_Actions]  
AS
BEGIN	
	SET NOCOUNT ON;

    SELECT ActionID,
		  ProductID,
		  DisplayName,
		  ActionCode,
		  DisplayOrder		         
	FROM 
		[Action] WITH (NOLOCK) 
END
