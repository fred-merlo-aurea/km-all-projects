CREATE proc [dbo].[sp_SavePage]     
(    
	@PageID   int,    
	@EditionID   int,    
	@PageNumber  int,    
	@DisplayNumber varchar(5),    
	@width   int,    
	@height   int,    
	@TextContent Text,    
	@xmlLinks  Text    
)    
as    
Begin    
	DECLARE @docHandle int      
	if @PageID = 0     
	Begin    
		Insert into [PAGE] (EditionID, PageNumber, DisplayNumber, Width, Height, TextContent)     
		values    
		(@EditionID, @PageNumber, @DisplayNumber, @width, @height, replace(convert(varchar(8000),@TextContent), '',''))    
  
		set @PageID = @@IDENTITY    
		
		EXEC sp_xml_preparedocument @docHandle OUTPUT, @xmlLinks    
    
		Insert into [LINK] (PageID, LinkType, LinkURL, x1, y1, x2, y2)    
		SELECT @PageID, Type, URL, x1, y1, x2, y2  FROM OPENXML(@docHandle, N'/Links/Link')     
		WITH 
		(     
			type varchar(10) '@type',     
			x1 INT '@x1',     
			y1 INT '@y1',     
			x2 INT '@x2',     
			y2 INT '@y2',    
			url varchar(500) '.'    
		)
      
		EXEC sp_xml_removedocument @docHandle    
	End    
 
	select @PageID as ID    

End
