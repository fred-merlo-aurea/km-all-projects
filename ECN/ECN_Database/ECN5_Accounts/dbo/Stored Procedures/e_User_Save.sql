CREATE PROCEDURE [dbo].[e_User_Save] 
	@UserID int = NULL,
	@CustomerID int,
	@UserName varchar(250),
	@Password varchar(250),	
	@CommunicatorOptions varchar(200), 
	@CollectorOptions varchar(200), 
	@CreatorOptions varchar(200), 
	@AccountsOptions varchar(200), 
    @ActiveFlag char(2), 
    @AcceptTermsDate datetime, 
    @RoleID int	,
    @LoggingUserID int,
    @AccessKey uniqueidentifier = NULL
AS
BEGIN
	IF @UserID = NULL or @UserID <= 0
	BEGIN
		INSERT INTO [Users]
		(
			CustomerID, UserName, [Password], CommunicatorOptions, CollectorOptions, CreatorOptions, AccountsOptions, 
			ActiveFlag, AcceptTermsDate, RoleID, AccessKey, CreatedUserID, CreatedDate, IsDeleted
		)
		VALUES
		(
			@CustomerID, @UserName, @Password, @CommunicatorOptions, @CollectorOptions,@CreatorOptions, @AccountsOptions,
			@ActiveFlag, @AcceptTermsDate, @RoleID,	NEWID(), @LoggingUserID, GETDATE(), 0
		)
		SET @UserID = @@IDENTITY
	END
	ELSE
	BEGIN
		UPDATE [Users]
			SET CustomerID=@CustomerID, UserName=@UserName, [Password] = @Password, CommunicatorOptions= @CommunicatorOptions, 
				CollectorOptions= @CollectorOptions, CreatorOptions=@CreatorOptions, AccountsOptions=@AccountsOptions,
				RoleID=@RoleID, ActiveFlag=@ActiveFlag,  UpdatedUserID=@LoggingUserID, UpdatedDate=GETDATE()
		WHERE
			UserID = @UserID
	END

	SELECT @UserID
END
