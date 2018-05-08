CREATE PROCEDURE [dbo].[e_Gateway_Save]
	@GatewayID int = null,
	@Name varchar(100),
	@CustomerID int,
	@GroupID int,
	@PubCode varchar(20),
	@TypeCode varchar(20),
	@Header varchar(MAX),
	@Footer varchar(MAX),
	@ShowForgotPassword bit,
	@ForgotPasswordText varchar(1000),
	@ShowSignup bit,
	@SignupText varchar(1000),
	@SignupURL varchar(255),
	@SubmitText varchar(100),
	@UseStyleFrom varchar(50),
	@Style varchar(1000),
	@UseConfirmation bit,
	@UseRedirect bit,
	@RedirectURL varchar(255),
	@RedirectDelay int,
	@ConfirmationMessage varchar(1000),
	@ConfirmationText varchar(1000),
	@LoginOrCapture varchar(20),
	@ValidateEmail bit,
	@ValidatePassword bit,
	@ValidateCustom bit,
	@CreatedUserID int = null,
	@UpdatedUserID int = null,
	@IsDeleted bit
AS
	if(@GatewayID is not null)
	BEGIN
		Update Gateway
		set Name = @Name,CustomerID = @CustomerID,GroupID = @GroupID, PubCode = @PubCode, TypeCode = @TypeCode, Header = @Header, Footer = @Footer, ShowForgotPassword = @ShowForgotPassword, ForgotPasswordText = @ForgotPasswordText,
			ShowSignup = @ShowSignup, SignupText = @SignupText, SignupURL = @SignupURL, SubmitText = @SubmitText, UseStyleFrom = @UseStyleFrom, Style = @Style, UseConfirmation = @UseConfirmation,
			UseRedirect = @UseRedirect, RedirectURL = @RedirectURL, RedirectDelay = @RedirectDelay, ConfirmationMessage = @ConfirmationMessage, ConfirmationText = @ConfirmationText, LoginOrCapture = @LoginOrCapture,
			ValidateEmail = @ValidateEmail, ValidatePassword = @ValidatePassword, ValidateCustom = @ValidateCustom, UpdatedDate = GETDATE(), UpdatedUserID = @UpdatedUserID, IsDeleted = @IsDeleted
		WHERE GatewayID = @GatewayID
		SELECT @GatewayID
	END
	ELSE if(@GatewayID is null)
	BEGIN
		INSERT INTO Gateway(Name,CustomerID,GroupID, PubCode, TypeCode, Header, Footer, ShowForgotPassword, ForgotPasswordText, ShowSignup, SignupText, SignupURL, SubmitText, UseStyleFrom, Style, UseConfirmation, UseRedirect, RedirectURL, RedirectDelay,ConfirmationMessage, ConfirmationText, LoginOrCapture,ValidateEmail, ValidatePassword, ValidateCustom,CreatedDate, CreatedUserID, IsDeleted)
		VALUES(@Name,@CustomerID, @GroupID, @PubCode, @TypeCode, @Header, @Footer, @ShowForgotPassword, @ForgotPasswordText, @ShowSignup, @SignupText, @SignupURL, @SubmitText, @UseStyleFrom, @Style, @UseConfirmation, @UseRedirect, @RedirectURL, @RedirectDelay,@ConfirmationMessage, @ConfirmationText, @LoginOrCapture,@ValidateEmail, @ValidatePassword, @ValidateCustom,GETDATE(), @CreatedUserID, @IsDeleted)
		SELECT @@IDENTITY;
	END
