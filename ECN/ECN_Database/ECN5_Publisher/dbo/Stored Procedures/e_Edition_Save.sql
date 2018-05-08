CREATE proc [dbo].[e_Edition_Save]   
(  
	@EditionID    int,  
	@EditionName    varchar(100),  
	@PublicationID   int,  
	@Status    varchar(10),  
	@Filename   varchar(100),  
	@Pages   int,  
	@EnableDate  datetime,  
	@DisableDate  datetime,  
	@IsSearchable bit,
	@IsLoginRequired bit,  
	@xmlTOC    Text,
	@UserID int  
)  
as  
Begin  
 
		if @EditionID <= 0   
		Begin  
			Insert into Edition   
				(EditionName, PublicationID, Status, FileName, Pages, EnableDate, DisableDate, IsSearchable, IsLoginRequired, xmlTOC, CreatedUserID,CreatedDate,IsDeleted)   
			values  
				(@EditionName, @PublicationID, @Status, @Filename, @Pages,  @EnableDate, @DisableDate, @IsSearchable, @IsLoginRequired, @xmlTOC, @UserID,getdate(),0)
  
			set @EditionID = @@IDENTITY  
		End  
		Else  
		Begin  
			Update Edition 
				Set EditionName = @EditionName,  
					PublicationID = @PublicationID,  
					Status = @Status,  
					FileName = (case when @FileName = '' then FileName else @FileName end),  
					Pages = (case when @Pages = 0 then Pages else @Pages end),  
					EnableDate = @EnableDate,  
					DisableDate =  @DisableDate,  
					IsSearchable = @IsSearchable,  
					IsLoginRequired = @IsLoginRequired,
					xmlTOC = (case when Convert(varchar,@xmlTOC) = '' then xmlTOC else @xmlTOC end),  
					UpdatedUserID = @UserID,
					UpdatedDate = getdate() 
			where  
				EditionID = @EditionID  
		End  
		
		select @EditionID as ID  

End