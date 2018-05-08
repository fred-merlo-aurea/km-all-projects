CREATE proc [dbo].[e_CustomerConfig_Save] 
(
	@CustomerConfigID 	int,
	@CustomerID 	int,
	@ProductID	int,
	@ConfigName	varchar(50),
	@ConfigValue varchar(255),
	@UserID	int
)
as
Begin
			if @CustomerConfigID <= 0 
			Begin
				INSERT INTO CustomerConfig
				( 
					[CustomerID], [ProductID], [ConfigName], [ConfigValue],
					[CreatedUserID],[CreatedDate],[IsDeleted]
				)
				VALUES
				(
					@CustomerID, @ProductID, @ConfigName, @ConfigValue,
					@UserID, getdate(),0
				)
				set @CustomerConfigID = @@IDENTITY
			End
			Else
			Begin
				Update CustomerConfig
				Set [CustomerID] = @CustomerID, 
					[ProductID] = @ProductID, 
					[ConfigName] = @ConfigName, 
					[ConfigValue] = @ConfigValue, 
					[UpdatedUserID] = @UserID,
					[UpdatedDate] = getdate()
				where
					CustomerConfigID = @CustomerConfigID
			End
				select @CustomerConfigID as ID
End
