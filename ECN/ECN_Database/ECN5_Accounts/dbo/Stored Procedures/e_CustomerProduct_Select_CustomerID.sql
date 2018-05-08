CREATE PROCEDURE [dbo].[e_CustomerProduct_Select_CustomerID]      
(  
 @CustomerID int  
)  
AS    
  
BEGIN     
   
    SELECT	*  
	FROM     
			CustomerProduct WITH (NOLOCK) 
	Where  
			CustomerID = @CustomerID  and IsDeleted = 0
END
