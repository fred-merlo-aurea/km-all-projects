create PROCEDURE [dbo].[e_Channel_Select_ChannelID]   
 @ChannelID int  
AS  
BEGIN   
 SET NOCOUNT ON;  
  
   SELECT  *
     FROM Channel WITH(NOLOCK)  
  WHERE  ChannelID = @ChannelID  and IsDeleted = 0
END
