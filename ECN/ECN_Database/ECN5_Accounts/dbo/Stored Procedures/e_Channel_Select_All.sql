CREATE PROCEDURE [dbo].[e_Channel_Select_All]   

AS  
BEGIN   
 SET NOCOUNT ON;  
  
   SELECT  *
     FROM Channel WITH(NOLOCK)  
  WHERE IsDeleted = 0
END
