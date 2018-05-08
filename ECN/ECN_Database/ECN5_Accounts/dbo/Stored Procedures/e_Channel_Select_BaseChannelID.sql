CREATE PROCEDURE [dbo].[e_Channel_Select_BaseChannelID]   
 @BaseChannelID int  
AS  
BEGIN   
 SET NOCOUNT ON;  
  
   SELECT  *
     FROM Channel WITH(NOLOCK)  
  WHERE  BaseChannelID = @BaseChannelID  and IsDeleted = 0
END
