create proc [dbo].[e_Link_Save]   
(  
	@LinkID    int,  
	@PageID    int,  
	@LinkType   varchar(50),  
	@LinkURL    varchar(500),  
	@x1   int,  
	@y1  int,  
	@x2  int,  
	@y2  int,  
	@Alias varchar(100),
	@UserID int  
)  
as  
Begin  
		if @LinkID <= 0   
		Begin  
			Insert into Link   
				(PageID, LinkType, LinkURL, x1, y1, x2, y2, Alias, CreatedUserID, CreatedDate, IsDeleted)   
			values  
				(@PageID, @LinkType, @LinkURL, @x1, @y1, @x2, @y2, @Alias, @UserID, getdate(), 0)
  
			set @LinkID = @@IDENTITY  
		End  
		Else  
		Begin  
			Update Link 
				Set PageID = @PageID,  
					LinkType = @LinkType,  
					LinkURL = @LinkURL,  
					x1 = @x1,  
					y1 = @y1,  
					x2 = @x2,  
					y2 = @y2,  
					Alias = @Alias,  
					UpdatedUserID = @UserID,
					UpdatedDate = getdate() 
			where  
				LinkID = @LinkID  
		End  
		
		select @LinkID as ID  

End