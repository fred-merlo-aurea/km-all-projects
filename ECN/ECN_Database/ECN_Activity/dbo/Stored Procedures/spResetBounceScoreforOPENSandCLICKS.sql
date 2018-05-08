create proc [dbo].[spResetBounceScoreforOPENSandCLICKS]
as
Begin

	declare @emailID int,
			@i int

	set @i = 0
	set nocount on

	DECLARE c_Emails CURSOR FOR 
	select distinct emailID from BlastActivityopens with (NOLOCK) where convert(date,opentime) = CONVERT(date, dateadd(dd, -1, getdate()) )
	union
	select distinct emailID from BlastActivityClicks with (NOLOCK) where convert(date,ClickTime) = CONVERT(date, dateadd(dd, -1, getdate()) )


	OPEN c_Emails  

	FETCH NEXT FROM c_Emails INTO @emailID

	WHILE @@FETCH_STATUS = 0  
	BEGIN  

			Update ecn5_communicator..Emails set bouncescore = 0, softbouncescore = 0 where emailID = @emailID

			set @i = @i + 1

			if @i % 1000 = 0
				print ('                   ' + convert(varchar,@i) + ' - ' + convert(varchar, getdate(), 108)) 

			FETCH NEXT FROM c_Emails INTO @emailID
	End

	CLOSE c_Emails  
	DEALLOCATE c_Emails 			

End
