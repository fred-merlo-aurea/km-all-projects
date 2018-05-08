create proc [dbo].[spGetMultipleGroupEmailProfilesWithUDF_export] 
	@GroupID VARCHAR(4000),    
	@Filter varchar(8000) = ' ',
	@Filter2 varchar(8000) = ' '  
as
Begin


print ('EXECUTE MASTER.dbo.xp_cmdshell ''SQLCMD -S 10.10.41.198 -U sa -P dcran9755 -Q"execute ecn5_communicator..spGetMultipleGroupEmailProfilesWithUDF ''''' + @groupID + '''''" -o"C:\temp\report2.txt"  -s"	"  ''') 
end
