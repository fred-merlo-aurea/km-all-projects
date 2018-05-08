CREATE proc [dbo].[sp_SaveUsers](
@ExportPermissionIDs varchar(200), 
@Permission varchar(50), 
@UserID int,
@ShowSalesView bit)
as
BEGIN

	SET NOCOUNT ON

	if exists(select * from Users where userID = @UserID)
		begin
			update [Users] 
			set [ExportPermissionIDs] = @ExportPermissionIDs, 
				[Permission] = @Permission,  
				[ShowSalesView] =  @ShowSalesView 
			where userID = @UserID

			select @UserID;
		end
	else
		begin
			INSERT INTO [Users]
				([ExportPermissionIDs], 
				[Permission],
				[UserID], 
				[ShowSalesView]) 
			VALUES (@ExportPermissionIDs, 
				@Permission, 
				@UserID, 
				@ShowSalesView)

			select @UserID;
		end

End