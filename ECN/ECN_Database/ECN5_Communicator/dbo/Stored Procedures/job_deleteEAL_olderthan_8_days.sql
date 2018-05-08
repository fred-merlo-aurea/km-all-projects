
CREATE proc [dbo].[job_deleteEAL_olderthan_8_days]
as
Begin
	
	set nocount on
	declare @EAID int, @i int
	set @i = 1
	
	create table #temp1 (EAID int)
	
	--select COUNT(EAID) from emailactivitylog where actiondate < convert(date,DATEADD(dd, -8, getdate()))

	print (' start ' + convert(varchar(20), getdate(), 114))  

	insert into #temp1
	select EAID from emailactivitylog with (NOLOCK) where actiondate < convert(date,DATEADD(dd, -8, getdate()))

	print @@rowcount

	DECLARE c_EAL CURSOR FOR select EAID from #temp1
	
	OPEN c_EAL  

	FETCH NEXT FROM c_EAL INTO @EAID

	WHILE @@FETCH_STATUS = 0  
	BEGIN  
		if (@i % 10000 = 0)
			print (convert(varchar,(@i)) + ' / ' + convert(varchar(20), getdate(), 114)) 
		
		set @i = @i + 1
		
		delete from emailactivitylog where EAID = @EAID

		FETCH NEXT FROM c_EAL INTO @EAID
	End

	CLOSE c_EAL  
	DEALLOCATE c_EAL 		
	
	
	drop table #temp1
	

	--SET ROWCOUNT 20000

	--declare @EAID int,
	--		@j int

	--set @j=1

	--print (convert(varchar,@j) + ' - ' + convert(varchar(20), getdate(), 114))  

	--select COUNT(EAID) from emailactivitylog where actiondate < convert(date,DATEADD(dd, -8, getdate()))

	--WHILE @@ROWCOUNT > 0 
	--BEGIN
	--	set @j = @j + 1

	--	--if @j % 10 = 0
	--		print (convert(varchar,@j*20000) +  ' / ' + convert(varchar, getdate(), 108)) 

	--	Delete from emailactivitylog where actiondate < convert(date,DATEADD(dd, -8, getdate()))

	--END
End
