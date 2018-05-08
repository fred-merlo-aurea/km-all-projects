CREATE proc [dbo].[e_RoleAction_Save] 
(
	@RoleActionID int,
	@RoleID  int,
	@ActionID int,
	@Active	char(1),
	@UserID int
)
as
Begin
			if @RoleActionID <= 0 
			Begin
				INSERT INTO [RoleAction]
				( 
					[RoleID], [ActionID],[Active],
					[CreatedUserID],[CreatedDate],[IsDeleted]
				)
				VALUES
				(
					@RoleID, @ActionID, @Active,
					@UserID, getdate(),0
				)
				set @RoleActionID = @@IDENTITY
			End
			Else
			Begin
				Update [RoleAction]
				Set [RoleID] = @RoleID, 
					[ActionID] = @ActionID, 
					[Active] = @Active, 
					[UpdatedUserID] = @UserID,
					[UpdatedDate] = getdate()
				where
					RoleActionID = @RoleActionID
			End
				select @RoleActionID as ID
End
