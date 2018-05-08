CREATE proc [dbo].[e_EditionActivityLog_Save]   
(  
	@EAID int,
	@EditionID int,      
	@EmailID int,      
	@BlastID int,      
	@ActionTypeCode varchar(10),      
	@ActionValue varchar(255),	
	@IPAddress varchar(50) ,  
	@IsAnonymous bit,    
	@LinkID int,  
	@PageID varchar(10),  
	@PageNo varchar(10) = 0,     
    @SessionID varchar(36) ,     
	@PageEnd int,
	@PageStart int,
	@UserID int  
)  
as  
Begin  
		if (@PageNo > 0)  
		BEGIN    
		select @PageID = pageID from [PAGE] where editionID = @EditionID and PageNumber = @PageNo and IsDeleted=0
		END  
		
		if @EAID <= 0   
		Begin  
				insert into EditionActivityLog 
					(EditionID, 
					EmailID,
					BlastID,
					ActionDate,
					ActionTypeCode,
					ActionValue,
					IPAddress,
					IsAnonymous,
					LinkID,
					PageID,
					SessionID,
					PageEnd,
					PageStart,
					CreatedUserID,
					CreatedDate,
					IsDeleted) 
				values 
					(@EditionID, 
					@EmailID,
					@BlastID,
					GETDATE(),
					@ActionTypeCode,
					@ActionValue,
					@IPAddress,
					@IsAnonymous,
					@LinkID,
					@PageID,
					@SessionID,
					@PageEnd,
					@PageStart,					
					@UserID,
					getdate(),
					0)
					
				set @EAID = @@IDENTITY
		End  
		Else  
		Begin  
				update EditionActivityLog
					set 
					EditionID = @EditionID, 
					EmailID = @EmailID,
					BlastID = @BlastID,
					ActionTypeCode = @ActionTypeCode,
					ActionValue = @ActionValue,
					IPAddress = @IPAddress,
					IsAnonymous = @IsAnonymous,
					LinkID = @LinkID,
					PageID = @PageID,
					SessionID = @SessionID,
					PageEnd = @PageEnd,
					PageStart = @PageStart,
					UpdatedUserID = @UserID,
					UpdatedDate = getdate() 					
					where EAID = @EAID
		End  
		
		select @EAID as ID  
End