CREATE PROCEDURE [dbo].[e_Select_Channel_ProductTypeCode] 
	@ProductTypeCode varchar(100),
	@BaseChannelID int
AS
BEGIN	
	SET NOCOUNT ON;

    SELECT  ChannelID,	 
			BaseChannelID,
			ChannelName,
			AssetsPath,
			VirtualPath,
			PickupPath,
			HeaderSource, 
			FooterSource,
			ChannelTypeCode,
			Active,
			MailingIP
     FROM [Channel] WITH(NOLOCK)
	 WHERE ChannelTypeCode = @ProductTypeCode AND BaseChannelID =@BaseChannelID		
END
