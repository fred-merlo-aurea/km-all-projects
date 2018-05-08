CREATE PROCEDURE [dbo].[e_Select_BaseChannels]
@BaseChannelID int = 0
AS
BEGIN	
	SET NOCOUNT ON;

    if(@BaseChannelID = 0)
    begin
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
         FooterSource ,
         BaseChannelGuid                     
		FROM 
			[BaseChannel] WITH (NOLOCK)
	 end
	 else
	 begin		
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
         FooterSource ,
         BaseChannelGuid                     
		FROM 
			[BaseChannel] WITH (NOLOCK) 
		WHERE
			BaseChannelID = @BaseChannelID
	 end	 			
END
