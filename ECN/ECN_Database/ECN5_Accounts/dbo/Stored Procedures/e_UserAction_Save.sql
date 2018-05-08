CREATE PROCEDURE [dbo].[e_UserAction_Save]    
(
	@UserActionID int,  -- ignore this column
	@UserID int,
	@ActionID int,
	@Active char(1)
)
AS  

BEGIN   
	SET NOCOUNT ON;  
	
	set @UserActionID = 0
	select @useractionID = UserActionID from UserActions where UserID = @UserID and ActionID = @ActionID
  
	if (@UserActionID > 0)
	Begin
		UPDATE [ecn5_accounts].[dbo].[UserActions]
		   SET [Active] = @Active
		 WHERE UserActionID = @UserActionID
		 
		 select @UserActionID

	End
	Else
	Begin
		
		INSERT INTO [ecn5_accounts].[dbo].[UserActions]
           ([UserID]
           ,[ActionID]
           ,[Active])
		VALUES
           (@UserID
           ,@ActionID
           ,@Active)
           
           select @@IDENTITY
	End
	  
END
