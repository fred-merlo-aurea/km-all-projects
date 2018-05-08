CREATE TABLE [dbo].[SubscriberLogin]
(
	[SubscriberLoginID] INT IDENTITY (1, 1) NOT NULL,
	[FormID] INT NOT NULL,
	[LoginRequired] bit NOT NULL,
	[OtherIdentification] NVARCHAR(200) NOT NULL,
	[PasswordRequired] bit NOT NULL,
	[AutoLoginAllowed] bit NOT NULL, 
    [LoginModalHTML] NVARCHAR(MAX) NOT NULL, 
    [LoginButtonText] NVARCHAR(100) NOT NULL,
	[SignUpButtonText] NVARCHAR(100) NOT NULL,
	[ForgotPasswordButtonText] NVARCHAR(100) NOT NULL,
	[NewSubscriberButtonText] NVARCHAR(100) NOT NULL,
	[ExistingSubscriberButtonText] NVARCHAR(100) NOT NULL,
	[ForgotPasswordMessageHTML] NVARCHAR(MAX) NOT NULL, 
	[ForgotPasswordNotificationHTML] NVARCHAR(MAX) NOT NULL, 
    [ForgotPasswordFromName] NVARCHAR(100) NOT NULL,
	[ForgotPasswordSubject] NVARCHAR(100) NOT NULL,
	[EmailAddressQuerystringName] NVARCHAR(50) NOT NULL,
	[OtherQuerystringName] NVARCHAR(50) NOT NULL,
	[PasswordQuerystringName] NVARCHAR(50) NOT NULL,
	CONSTRAINT [PK_SubscriberLogin] PRIMARY KEY CLUSTERED ([SubscriberLoginID] ASC),
    CONSTRAINT [FK_SubscriberLogin_Form] FOREIGN KEY ([FormID]) REFERENCES [dbo].[Form] ([Form_Seq_ID])
)
