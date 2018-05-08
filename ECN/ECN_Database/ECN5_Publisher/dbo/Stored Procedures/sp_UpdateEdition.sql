CREATE proc [dbo].[sp_UpdateEdition]     
(    
 @EditionID    int,    
 @filename   varchar(100),    
 @Totalpages   int,    
 @TOC    Text    
)    
as    
Begin  
	
	declare @versionNo int,
			@OriginalEditionID int,
			@NewEditionID int
  
	if @EditionID > 0     
	Begin    

		select @OriginalEditionID = isnull(OriginalEditionID,0)
		from 
				[EDITION] where EditionID = @EditionID

		if @OriginalEditionID = 0 
		Begin
			set @OriginalEditionID = @EditionID
			set @versionNo = 1
		end
		Else
		Begin
			select @versionNo = Max(Version) + 1 from [EDITION] where OriginalEditionID = @OriginalEditionID
		End	

		Insert into [EDITION]     
			(EditionName, PublicationID, Status, FileName, Pages, EnableDate, DisableDate, IsSearchable, IsLoginRequired, xmlTOC, OriginalEditionID, CreatedDate)     
		select 
			 EditionName, PublicationID, status, @filename, @totalpages, EnableDate, DisableDate, IsSearchable, IsLoginRequired, @TOC, @OriginalEditionID, getdate()
		From [EDITION] 
		where EditionID = @EditionID    
 
		set @NewEditionID = @@IDENTITY    

		Update [EDITION]    
		Set EditionName = EditionName + ' (v' + convert(varchar(2),@VersionNo) + ')',    
			Status = 'Updated',    
			OriginalEditionID = @OriginalEditionID,
			Version = @versionNo,
			UpdatedDate = getdate()    
		where    
			EditionID = @EditionID    
	End    
	select @NewEditionID as ID    
  
End
