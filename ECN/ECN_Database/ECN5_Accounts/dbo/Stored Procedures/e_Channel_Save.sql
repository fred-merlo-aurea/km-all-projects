CREATE PROCEDURE [dbo].[e_Channel_Save]    
(
@ChannelID int, 
@BaseChannelID int,
@ChannelName varchar(50),
@AssetsPath varchar(255),
@VirtualPath varchar(100), 
@HeaderSource text, 
@FooterSource text, 
@ChannelTypeCode varchar(50), 
@Active char(1), 
@MailingIP varchar(50), 
@PickupPath Varchar(255),
@UserID int
)
AS  

BEGIN   
	SET NOCOUNT ON;  
	
	if (@ChannelID > 0)
	Begin
			Update Channel
			SET BaseChannelID = @BaseChannelID, 
			ChannelName = @ChannelName, 
			AssetsPath = @AssetsPath, 
			VirtualPath = @VirtualPath, 
			HeaderSource = @HeaderSource, 
			FooterSource = @FooterSource, 
			ChannelTypeCode = @ChannelTypeCode, 
			Active = @Active, 
			MailingIP = @MailingIP,
			PickupPath = @PickupPath,
			[UpdatedUserID] = @UserID,
			[UpdatedDate] = getdate()		   
			Where ChannelID = @ChannelID
		 
		 select @ChannelID
	End
	Else
	Begin
		INSERT INTO Channel	
			(BaseChannelID, 
			ChannelName, 
			AssetsPath, 
			VirtualPath, 
			HeaderSource, 
			FooterSource, 
			ChannelTypeCode, 
			Active, 
			MailingIP,
			PickupPath,				
			CreatedUserID,
			CreatedDate,
			IsDeleted) 		 
		VALUES 
		(	@BaseChannelID,
			@ChannelName,
			@AssetsPath,
			@VirtualPath,
			@HeaderSource,
			@FooterSource,
			@ChannelTypeCode,
			@Active,			
			@MailingIP,
			@PickupPath,			
			@UserID,
			getdate()
			,0)
		
		SELECT @@IDENTITY;
	End
END
