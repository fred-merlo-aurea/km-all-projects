CREATE PROCEDURE [dbo].[e_Select_BaseChannels_BaseChannelID] 
	@BaseChannelID int
AS
BEGIN	
	SET NOCOUNT ON;

    SELECT 
		 BaseChannelID,
         BaseChannelName, 
         ChannelURL,
         BounceDomain, 
         AccessCommunicator,
         AccessCreator, 
         AccessCollector, 
         AccessPublisher,
         AccessCharity,
         DisplayAddress, 
		 [Address], 
         City,
         [State], 
         Country, 
         Zip,
         Salutation, 
         ContactName, 
         ContactTitle, 
         Phone,
         Fax,
         Email,
         WebAddress,
         InfoContentID,
         MasterCustomerID,
         ChannelPartnerType, 
         RatesXml,
         ChannelType, 
         IsBranding,
         EmailThreshold, 
         BounceThreshold,
         HeaderSource, 
         FooterSource                  
		FROM 
			[BaseChannel] WITH (NOLOCK)
		WHERE 
			BaseChannelID = @BaseChannelID			
END
