CREATE PROCEDURE [dbo].[SPUpdateStats]
AS

Set Nocount on
Declare db Cursor For
Select name from master.dbo.sysdatabases where name not in ('master','TempDB', 'msdb', 'model')

Declare @dbname varchar(60)
Declare @execmd nvarchar(150)

Open db
Fetch Next from db into @dbname
While @@Fetch_status=0
   begin
	if @dbname is null 
	  Begin
   	    Print 'null Value'
	  end
	else 
	  Begin
	    PRINT '###########################################################################'
            PRINT 'Update Statistics in ' + @dbname
            SELECT @execmd = 'USE ' + @dbname + ' EXEC sp_updatestats'
            EXEC(@execmd)
	    PRINT ''
          End
     Fetch Next from db into @dbname	
   end
Close db
Deallocate db
