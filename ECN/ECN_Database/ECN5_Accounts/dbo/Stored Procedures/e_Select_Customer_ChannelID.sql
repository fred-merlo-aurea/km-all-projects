CREATE PROCEDURE [dbo].[e_Select_Customer_ChannelID]
	@ChannelID int		
AS
BEGIN	
	SET NOCOUNT ON;

    SELECT 
		*     	
    FROM 
		Customer WITH(NOLOCK) WHERE BaseChannelID = @ChannelID order by CustomerName
END
