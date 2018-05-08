CREATE PROCEDURE [dbo].[e_Select_Customers_ChannelID]
	@ChannelID int		
AS
BEGIN	
	SET NOCOUNT ON;

    SELECT 
		*     	
    FROM 
		[Customer] WITH(NOLOCK) WHERE BaseChannelID = @ChannelID and IsDeleted=0 order by CustomerName
END