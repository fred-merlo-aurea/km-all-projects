CREATE PROCEDURE [dbo].[e_BaseChannel_Save]    
(
@BaseChannelID int, 
@PlatformClientGroupID int,
@BaseChannelName varchar(100),
@ChannelPartnerType int,
@RatesXML varchar(2000),
@Salutation varchar(50), 
@ContactName varchar(255), 
@ContactTitle varchar(255), 
@Phone varchar(255), 
@Fax varchar(50), 
@Email varchar(255), 
@Address varchar(255),
@City varchar(255),
@State varchar(255), 
@Country varchar(50), 
@Zip varchar(50), 
@BounceDomain varchar(100), 
@EmailThreshold int = 0,
@ChannelURL varchar(50), 
@WebAddress varchar(255),
@ChannelType varchar(50),		
@UserID int,
@MSCustomerID int = null,
@BaseChannelGuid uniqueidentifier
)
AS  

BEGIN   
	SET NOCOUNT ON;  
	
	if (@BaseChannelID > 0)
	Begin
			Update BaseChannel
			SET BaseChannelName = @BaseChannelName, 
			ChannelPartnerType = @channelPartnerType, 
			RatesXML = @ratesXML, 
			Salutation = @salutation, 
			ContactName = @contactName, 
			ContactTitle = @contactTitle, 
			Phone = @phone, 
			Fax = @fax, 
			Email = @email, 
			Address = @Address, 
			City = @city, 
			State = @state, 
			Country = @country, 
			Zip = @zip,
			--BounceDomain=@bounceDomain, 
			ChannelURL =@channelURL, 
			WebAddress=@webAddress, 
			channelType=@channelType,
			[UpdatedUserID] = @UserID,
			[UpdatedDate] = getdate(),
			MSCustomerID = @MSCustomerID,
			EmailThreshold = @EmailThreshold,
			BaseChannelGuid = @BaseChannelGuid,	 
			PlatformClientGroupID = @PlatformClientGroupID
			Where BaseChannelID = @baseChannelID
		 
		 select @baseChannelID
	End
	Else
	Begin
		INSERT INTO BaseChannel	
			(BaseChannelName,
			PlatformClientGroupID, 
			ChannelPartnerType, 
			RatesXML, 
			Salutation, 
			ContactName, 
			ContactTitle, 
			Phone, 
			Fax, 
			Email, 
			Address, 
			City, 
			State, 
			Country, 
			Zip, 
			--BounceDomain, 
			EmailThreshold,
			ChannelURL, 
			WebAddress, 
			channelType,
			CreatedUserID,
			CreatedDate,
			IsDeleted,
			MSCustomerID,
			BasechannelGuid, 
			AccessKey) 		 
		VALUES 
		(	@BaseChannelName,
			@PlatformClientGroupID,
			@ChannelPartnerType,
			@RatesXML, 
			@Salutation,
			@ContactName,
			@ContactTitle,
			@Phone,
			@Fax,
			@Email,
			@Address,
			@City,
			@State,
			@Country,
			@Zip, 
			--@BounceDomain, 
			0,
			@ChannelURL, 
			@WebAddress, 
			@ChannelType,		
			@UserID,
			getdate(),
			0,
			@MSCustomerID,
			@BasechannelGuid,	
			NEWID())		
		SELECT @@IDENTITY;
	End
END