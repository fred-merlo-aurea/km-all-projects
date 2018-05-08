create PROCEDURE [dbo].[e_CodeSheet_Select_ResponseGroupID]  
@ResponseGroupID int  
AS  
BEGIN  
  
 set nocount on  
  
 SELECT  *  
 FROM CodeSheet With(NoLock)  
 WHERE ResponseGroupID = @ResponseGroupID  
  
END