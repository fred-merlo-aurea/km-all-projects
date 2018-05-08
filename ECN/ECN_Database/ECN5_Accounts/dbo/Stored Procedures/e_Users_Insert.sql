CREATE PROCEDURE [dbo].[e_Users_Insert] 
	@CustomerID int,
	@UserName varchar(250),
	@Password varchar(250),	
	@CommunicatorOptions varchar(200), 
	@CollectorOptions varchar(200), 
	@CreatorOptions varchar(200), 
	@AccountsOptions varchar(200), 
    @ActiveFlag char(2), 
    @CreatedDate datetime, 
    @AcceptTermsDate datetime, 
    @RoleID int	
AS
BEGIN
	INSERT INTO Users (CustomerID, UserName, Password, CommunicatorOptions, CollectorOptions, CreatorOptions, AccountsOptions, 
	ActiveFlag, CreatedDate, AcceptTermsDate, RoleID) 
	VALUES 
	(  
	@CustomerID, 
	@UserName, 
	@Password,
	@CommunicatorOptions, 
	@CollectorOptions,
	@CreatorOptions,
	@AccountsOptions,
	@ActiveFlag,
	@CreatedDate,
	@AcceptTermsDate,
	@RoleID
	)
	SELECT @@IDENTITY; 
END
