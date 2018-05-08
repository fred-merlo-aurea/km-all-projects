CREATE proc dbo.sp_InsertActivityLog      
(      
	@EditionID int,      
	@EmailID int,      
	@BlastID int,      
	@PageNo varchar(10),      
	@LinkID int,      
	@ActionTypeCode varchar(10),      
	@ActionValue varchar(255),      
	@IP varchar(20) ,  
	@SessionID varchar(36)    
)      
as       
Begin      
      
	declare @PageID int,  
			@Exists bit,
			@PageStart int,
			@PageEnd int  
      
	set @PageID = NULL      
	set @Exists = 0  
       
	if (@PageNo > 0)      
		select @PageID = pageID from [PAGE] where editionID = @EditionID and PageNumber = @PageNo and IsDeleted=0   
  
	if @ActionTypeCode = 'subscribe'  
	Begin  
		if exists(select EAID from editionactivitylog where ActionTypeCode = 'subscribe' and EmailID=@EmailID and EditionID = @EditionID)  
			set @Exists = 1  
	End  
  
	if @Exists = 0  and @actionTypeCode <> 'Print'
	Begin  

		insert into EditionActivitylog       
		(
				EditionID, EmailID, BlastID, PageID, LinkID, ActionDate, ActionTypeCode, ActionValue, IPAddress, IsAnonymous, SessionID
		)      
		values      
		( 
			@EditionID,       
			case when @EmailID > 0 then @EmailID else NULL end,      
			@BlastID,       
			@PageID,       
			case when @LinkID > 0 then @LinkID else NULL end,       
			getdate(),      
			@ActionTypeCode,       
			@ActionValue,      
			@IP,      
			case when @EmailID > 0 then 0 else 1 end,  
			@SessionID     
		)       
	end      
	else if @actionTypeCode = 'Print'
	Begin

		set @PageStart = substring(@ActionValue, 1, charindex(',',@ActionValue)-1)
		set @PageEnd = substring(@ActionValue, charindex(',',@ActionValue)+1, len(@ActionValue))

		insert into EditionActivitylog       
		(
				EditionID, EmailID, BlastID, PageID, LinkID, ActionDate, ActionTypeCode, ActionValue, IPAddress, IsAnonymous, SessionID, PageStart, PageEnd
		)      
		values      
		( 
			@EditionID,       
			case when @EmailID > 0 then @EmailID else NULL end,      
			@BlastID,       
			@PageID,       
			case when @LinkID > 0 then @LinkID else NULL end,       
			getdate(),      
			@ActionTypeCode,       
			@ActionValue,      
			@IP,      
			case when @EmailID > 0 then 0 else 1 end,  
			@SessionID,
			@PageStart,
			@PageEnd     
		)     

	End      
End