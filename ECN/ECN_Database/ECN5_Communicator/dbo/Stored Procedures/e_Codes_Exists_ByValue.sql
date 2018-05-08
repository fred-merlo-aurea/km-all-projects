CREATE PROCEDURE [dbo].[e_Codes_Exists_ByValue] 
	@CodeID int = NULL,
	@CustomerID int = NULL,
	@CodeValue varchar(50) = NULL,
	@CodeType varchar (50) = NULL
AS     
BEGIN  
	IF EXISTS	(
					SELECT TOP 1 CodeID 
					FROM 
						Code 
					WHERE 
						CodeID = ISNULL(@CodeID, -1) and 
						CustomerID = @CustomerID and 
						CodeValue = @CodeValue and 
						CodeType = @CodeType and 
						IsDeleted = 0
				) 
	SELECT 1 ELSE SELECT 0
END
