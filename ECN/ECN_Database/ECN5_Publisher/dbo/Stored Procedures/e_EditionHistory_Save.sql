CREATE proc [dbo].[e_EditionHistory_Save]   
(  
	@EditionHistoryID    int,  
	@EditionID    int,  
	@ActivatedDate  datetime,  
	@ArchievedDate  datetime,  
	@DeActivatedDate datetime,
	@UserID int  
)  
as  
Begin  
		if @EditionHistoryID <= 0   
		Begin  
				insert into editionHistory 
					(EditionID, 
					ActivatedDate,
					ArchievedDate,
					DeActivatedDate,
					CreatedUserID,
					CreatedDate,
					IsDeleted) 
				values 
					(@EditionID, 
					@ActivatedDate, 
					@ArchievedDate, 
					@DeActivatedDate,
					@UserID,
					getdate(),
					0)
				set @EditionHistoryID = @@IDENTITY
		End  
		Else  
		Begin  
				update EditionHistory
					set 
						EditionID = @EditionID, 
						ActivatedDate = @ActivatedDate,
						ArchievedDate = @ArchievedDate,				
						DeActivatedDate = @DeActivatedDate,
						UpdatedUserID = @UserID,
						UpdatedDate = getdate() 					
					where editionhistoryID = @EditionHistoryID
		End  
		
		select @EditionHistoryID as ID  
End