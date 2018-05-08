CREATE proc [dbo].[e_Role_Save] 
(
	@RoleID 		int,
	@CustomerID 	int,
	@RoleName	varchar(50),
	@UserID	int
)
as
Begin
			if @RoleID <= 0 
			Begin
				INSERT INTO [Role]
				( 
					[CustomerID], [RoleName],
					[CreatedUserID],[CreatedDate],[IsDeleted]
				)
				VALUES
				(
					@CustomerID, @RoleName,
					@UserID, getdate(),0
				)
				set @RoleID = @@IDENTITY
			End
			Else
			Begin
				Update [Role]
				Set [CustomerID] = @CustomerID, 
					[RoleName] = @RoleName, 
					[UpdatedUserID] = @UserID,
					[UpdatedDate] = getdate()
				where
					RoleID = @RoleID
			End
			
				select @RoleID as ID
End
