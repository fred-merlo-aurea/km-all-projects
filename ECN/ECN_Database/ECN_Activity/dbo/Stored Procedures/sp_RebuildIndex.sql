CREATE Procedure [dbo].[sp_RebuildIndex] 
(
	@tables varchar(100) = 'all'
)
As      
Begin      
	Declare @TableName varchar(50),  
			@startTime datetime 

	set nocount on

	Declare CURSOR_Table Cursor FORWARD_ONLY For       
		select distinct Name from sysobjects where type = 'U'  and name in (case when @tables = 'all' then name else @tables end) order by Name      
	 
	 select distinct Name from sysobjects where type = 'U'and  name in (case when @tables = 'all' then name else @tables end) order by Name  
	 
	Open Cursor_table      
	Fetch Next From cursor_Table into @TableName       
	 
	While (@@Fetch_status =0)      
	Begin      
		set @startTime = getdate()  
		
		If Exists (Select Rows from sysindexes where indid=1 And id=object_id(@TableName))        
		Begin      
			--if (@tables = 'all' or @tables = 'Emails' or @tables = 'EmailGroups' or @TableName = 'EmailActivityLog' or @TableName = 'EmailDataValues')        
			Begin      
				print ('
go
dbcc dbreindex(' + @TableName + ','''',80)  WITH NO_INFOMSGS
go')
				 --dbcc dbreindex(@TableName,'',80)  WITH NO_INFOMSGS      
				print('print(''' + @TableName + ''' + '' / '' + convert(varchar(20), getdate(), 114))')
			end      
		End      

		Fetch Next From cursor_Table into @TableName      
	End

	Close CURSOR_Table      
	Deallocate CURSOR_Table      
       Print ('EXEC sp_updatestats 
       go')
       
       --EXEC sp_updatestats
end
