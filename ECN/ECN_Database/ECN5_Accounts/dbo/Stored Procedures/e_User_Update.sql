CREATE PROCEDURE [dbo].[e_User_Update] 
	@UserID int, 
	@CustomerID int, 
	@UserName varchar(200),
	@Password varchar(200),	
	@CommunicatorOptions varchar(200), 
	@CollectorOptions varchar(200), 
	@CreatorOptions varchar(200), 
	@AccountsOptions varchar(200), 
    @ActiveFlag char(2),  
    @RoleID int 
AS
BEGIN
	UPDATE Users SET
		CustomerID=@CustomerID,
		UserName=@UserName,
		[Password] = @Password, 
		CommunicatorOptions= @CommunicatorOptions, 
		CollectorOptions= @CollectorOptions, 
		CreatorOptions=@CreatorOptions, 
		AccountsOptions=@AccountsOptions,
		RoleID=@RoleID, 
		ActiveFlag=@ActiveFlag
	WHERE 
		UserID=@UserID		
END
