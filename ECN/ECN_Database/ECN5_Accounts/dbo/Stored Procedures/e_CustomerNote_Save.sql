CREATE proc [dbo].[e_CustomerNote_Save] 
(
	@NoteID 		int,
	@CustomerID 	int,
	@Notes	varchar(2000),
	@IsBillingNotes	bit,
	@UserID	int
)
as
Begin
			if @NoteID = 0 
			Begin
				INSERT INTO CustomerNote
				( 
					[CustomerID], [Notes], [IsBillingNotes],
					[CreatedUserID],[CreatedDate],[IsDeleted]
				)
				VALUES
				(
					@CustomerID, @Notes, @IsBillingNotes,
					@UserID, getdate(),0
				)
				set @NoteID = @@IDENTITY
			End
			Else
			Begin
				Update CustomerNote
				Set [CustomerID] = @CustomerID, 
					[Notes] = @Notes, 
					[IsBillingNotes] = @IsBillingNotes, 
					[UpdatedUserID] = @UserID,
					[UpdatedDate] = getdate()
				where
					NoteID = @NoteID
			End
				select @NoteID as ID
End
