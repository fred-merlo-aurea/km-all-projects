create proc [dbo].[e_Rule_Save]   
(  
	@RuleID    int,  
	@RuleName   varchar(100),  
	@PublicationID   int,  
	@EditionID    int,
	@UserID int  
)  
as  
Begin  
		if @RuleID <= 0   
		Begin  
			Insert into Rules   
				(RuleName, PublicationID, EditionID, CreatedUserID, CreatedDate, IsDeleted)   
			values  
				(@RuleName, @PublicationID, @EditionID, @UserID, getdate(), 0)
  
			set @RuleID = @@IDENTITY  
		End  
		Else  
		Begin  
			Update Rules 
				Set RuleName = @RuleName,  
					PublicationID = @PublicationID,  
					EditionID = @EditionID,  
					UpdatedUserID = @UserID,
					UpdatedDate = getdate() 
			where  
				RuleID   = @RuleID  
		End  
		
		select @RuleID as ID  

End