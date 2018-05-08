CREATE proc [dbo].[e_CustomerProduct_Save] 
(
	@CustomerProductID 		int,
	@CustomerID 	int,
	@ProductDetailID	int,
	@Active	char(1),
	@UserID	int
)
as
Begin
			if @CustomerProductID <= 0 
			Begin
				INSERT INTO CustomerProduct
				( 
					[CustomerID], [ProductDetailID], [Active],
					[CreatedUserID],[CreatedDate],[IsDeleted]
				)
				VALUES
				(
					@CustomerID, @ProductDetailID, @Active,
					@UserID, getdate(),0
				)
				set @CustomerProductID = @@IDENTITY
			End
			Else
			Begin
				Update CustomerProduct
				Set [CustomerID] = @CustomerID, 
					[ProductDetailID] = @ProductDetailID, 
					[Active] = @Active, 
					[UpdatedUserID] = @UserID,
					[UpdatedDate] = getdate()
				where
					CustomerProductID = @CustomerProductID
			End
				select @CustomerProductID as ID
End
