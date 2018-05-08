CREATE Procedure [dbo].[sp_RebuildIndex] As      
BEGIN

	SET NOCOUNT ON
     
	Declare @TableName varchar(50),  
	@startTime varchar(20)  
   
	Declare CURSOR_Table Cursor FORWARD_ONLY For       
	select distinct Name 
	from sysobjects 
	where type = 'U' 
	order by Name      --and status > 0 
     
	Open Cursor_table      
	Fetch Next From cursor_Table into @TableName       
     
	While (@@Fetch_status =0)      
		Begin      
			If Exists (Select Rows from sysindexes where indid=1 And id=object_id(@TableName))        
				Begin      
					set @startTime = convert(varchar(25),getdate())  
					dbcc dbreindex(@TableName,'',90)  WITH NO_INFOMSGS      
					print(@tablename + ' st :  ' +  @startTime + ' - end : ' + convert(varchar(25),getdate()))   
					--end      
				End      
			Fetch Next From cursor_Table into @TableName      
		End      
    
	Close CURSOR_Table      
	Deallocate CURSOR_Table      
      
	exec sp_updatestats
      
end