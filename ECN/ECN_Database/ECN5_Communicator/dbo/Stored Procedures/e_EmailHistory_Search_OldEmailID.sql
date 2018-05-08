CREATE PROCEDURE [dbo].[e_EmailHistory_Search_OldEmailID]
	@OldEmailID int
AS
	declare @Done bit = 0
	declare @NewEmailID int = -1
	
	while(@Done = 0 )
	BEGIN
		
		Select @NewEmailID = NewEmailID from EmailHistory eg with(nolock)
		where eg.OldEmailID = @OldEmailID and eg.Action = 'merge'
		
		IF @NewEmailID > 0
		BEGIN
			declare @TempEmailID int = -1
			SELECT @TempEmailID = ISNULL(NewEmailID,-1) from EmailHistory eg with(nolock) where eg.OldEmailID = @NewEmailID  and eg.Action = 'merge'
			if @TempEmailID is not null and @TempEmailID > 0
			BEGIN
				
				Select @OldEmailID = @NewEmailID
				
			END
			ELSE
			BEGIN
				
				Set @Done = 1
			END
		END
		ELSE
		BEGIN
			
			Set @Done = 1
		END
	END

	Select @NewEmailID