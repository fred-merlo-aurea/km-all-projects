CREATE PROCEDURE [dbo].[e_Action_Select]  
AS
BEGIN	
	SET NOCOUNT ON;

    SELECT *	         
	FROM 
		Action WITH (NOLOCK) 
	WHERE IsDeleted = 0
END