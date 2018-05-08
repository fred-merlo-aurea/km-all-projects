CREATE PROCEDURE [dbo].[sp_CleanUP]        
AS        
Begin     
	print ''
	--create table #tmpDeleteEmails (EID int)

	--insert into #tmpDeleteEmails
	--select e. emailID from emails e left outer join emailgroups eg on e.emailID = eg.emailID
	--where eg.emailID is null and e.DateAdded < DATEADD(dd, -1, getdate())

	--delete from emails where emailID in (select EID from #tmpDeleteEmails)        
 --   print (' emails ' + convert(varchar(20), @@rowcount) + ' / ' + convert(varchar(20), getdate(), 114 ))      

	--delete from emaildatavalues where ltrim(rtrim(isnull(datavalue,''))) = ''         
	--print (' emaildatavalues ' + convert(varchar(20), @@rowcount) + ' / ' + convert(varchar(20), getdate(), 114 ))   

	--delete from emaildatavalues where emailID not in (select emailID from Emails)        
	--print (' emaildatavalues 1 ' + convert(varchar(20), @@rowcount) + ' / ' + convert(varchar(20), getdate(), 114 ))      

	--delete from emaildatavalues where emailID in (select EID from #tmpDeleteEmails)        
	--print (' emaildatavalues 2 ' + convert(varchar(20), @@rowcount) + ' / ' + convert(varchar(20), getdate(), 114 ))      

	--drop table #tmpDeleteEmails
	 
	----delete from emailactivitylog where blastID not in (select blastID from [BLAST])        
	----print (' emailactivitylog ' + convert(varchar(20), @@rowcount) + ' / ' + convert(varchar(20), getdate(), 114 ))  
	   
   
end
