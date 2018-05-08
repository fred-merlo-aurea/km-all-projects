create PROCEDURE [dbo].[e_ResponseGroup_Select_ID]  
@ResponseGroupID int  
AS  
BEGIN  
  
 set nocount on  
  
 SELECT *  
 FROM ResponseGroups With(NoLock)  
 WHERE ResponseGroupID = @ResponseGroupID  
  
END