CREATE PROCEDURE [dbo].[e_SubscriptionManagement_Save]
	@SMID int = null,
	@UpdatedDate datetime = null,
	@UpdatedUserID int = null,
	@CreatedDate datetime = null,
	@CreatedUserID int = null,
	@Name varchar(100),
	@AdminEmail varchar(255),
	@BaseChannelID int,
	@EmailFooter varchar(MAX),
	@EmailHeader varchar(MAX),
	@PageHeader varchar(MAX),
	@PageFooter varchar(MAX),
	@MSMessage varchar(MAX),
	@IncludeMSGroups bit,
	@IsDeleted bit,
	@UseReasonDropDown bit,
	@ReasonVisible bit,
	@ReasonLabel varchar(MAX),
	@UseThankYou bit,
	@UseRedirect bit,
	@ThankYouLabel varchar(MAX),
	@RedirectURL varchar(100),
	@RedirectDelay int
AS
	IF @SMID is null
	BEGIN
		INSERT INTO SubscriptionManagement(AdminEmail, BaseChannelID, CreatedDate, CreatedUserID, EmailFooter, EmailHeader, Footer, Header,MSMessage,IncludeMSGroups, IsDeleted, Name, UseReasonDropDown, ReasonVisible, ReasonLabel,UseThankYou, UseRedirect, ThankYouLabel, RedirectURL, RedirectDelay)
		VALUES(@AdminEmail, @BaseChannelID, @CreatedDate, @CreatedUserID, @EmailFooter, @EmailHeader, @PageFooter, @PageHeader,@MSMessage,@IncludeMSGroups, @IsDeleted, @Name, @UseReasonDropDown, @ReasonVisible, @ReasonLabel, @UseThankYou, @UseRedirect, @ThankYouLabel, @RedirectURL, @RedirectDelay)
		SELECT @@IDENTITY;
	END
	ELSE
	BEGIN
		UPDATE SubscriptionManagement
		SET AdminEmail = @AdminEmail, BaseChannelID = @BaseChannelID, EmailFooter = @EmailFooter, EmailHeader = @EmailHeader, Footer = @PageFooter, Header = @PageHeader,MSMessage = @MSMessage, IncludeMSGroups = @IncludeMSGroups, IsDeleted = @IsDeleted, Name = @Name, UpdatedDate = @UpdatedDate, UpdatedUserID = @UpdatedUserID, UseReasonDropDown = @UseReasonDropDown, ReasonVisible = @ReasonVisible, ReasonLabel = @ReasonLabel, UseThankYou = @UseThankYou, UseRedirect = @UseRedirect, ThankYouLabel = @ThankYouLabel, RedirectURL = @RedirectURL, RedirectDelay = @RedirectDelay
		WHERE SubscriptionManagementID = @SMID and IsDeleted = 0
		SELECT @SMID
	END
RETURN 0
