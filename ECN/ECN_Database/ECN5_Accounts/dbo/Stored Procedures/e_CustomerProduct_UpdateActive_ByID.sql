create proc [dbo].[e_CustomerProduct_UpdateActive_ByID] 
(
	@CustomerProductID 	int,
	@UserID	int
)
as
Begin
			declare @Active char(1)
			
			select @Active = Active from CustomerProduct where CustomerProductID = @CustomerProductID

			if @Active = 'y'
			Begin
				Update CustomerProduct
				Set	[Active] = 'n', 
					[UpdatedUserID] = @UserID,
					[UpdatedDate] = getdate()
				where
					CustomerProductID = @CustomerProductID
			End
			Else
			Begin
				Update CustomerProduct
				Set	[Active] = 'y', 
					[UpdatedUserID] = @UserID,
					[UpdatedDate] = getdate()
				where
					CustomerProductID = @CustomerProductID
			End
				select @CustomerProductID as ID
End
