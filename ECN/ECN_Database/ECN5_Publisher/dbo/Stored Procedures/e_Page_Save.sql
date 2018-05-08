create proc [dbo].[e_Page_Save]   
(  
	@PageID    int,  
	@EditionID   int,  
	@PageNumber   int,  
	@DisplayNumber    varchar(5),  
	@Width  int,  
	@Height  int,  
	@TextContent  text,  
	@UserID int  
)  
as  
Begin  
		if @PageID <= 0   
		Begin  
			Insert into Page   
				(EditionID, PageNumber, DisplayNumber, Width, Height, TextContent, CreatedUserID, CreatedDate, IsDeleted)   
			values  
				(@EditionID, @PageNumber, @DisplayNumber, @Width, @Height, @TextContent, @UserID, getdate(), 0)
  
			set @PageID = @@IDENTITY  
		End  
		Else  
		Begin  
			Update Page 
				Set EditionID = @EditionID,  
					PageNumber = @PageNumber,  
					DisplayNumber = @DisplayNumber,  
					Width = @Width,  
					Height = @Height,  
					TextContent = @TextContent,  
					UpdatedUserID = @UserID,
					UpdatedDate = getdate() 
			where  
				PageID   = @PageID  
		End  
		
		select @PageID as ID  

End