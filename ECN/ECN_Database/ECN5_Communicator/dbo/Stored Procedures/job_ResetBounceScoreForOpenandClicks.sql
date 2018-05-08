CREATE Proc [dbo].[job_ResetBounceScoreForOpenandClicks]
as
Begin
	declare @Emails table (EmailID int)
	declare	@i int,
			@emailID int,
			@dt date,
			@PreviousDayMaxEAID int

	set nocount on
			
	set @dt =  CONVERT(date, dateadd(dd,-1, getdate()))
	set @i = 0		
	
	select @PreviousDayMaxEAID = MAX(EAID)
	from EmailActivityLog with (NOLOCK)  
	where ActionDate <  @dt

	insert into @Emails
	select distinct emailID 
	from EmailActivityLog with (NOLOCK) 
	where EAID > @PreviousDayMaxEAID and (ActionTypeCode = 'open' or ActionTypeCode = 'click') and  ActionDate between @dt and CONVERT(date, getdate())

	DECLARE c_Emails CURSOR FOR select emailID from @Emails 

	OPEN c_Emails  
	FETCH NEXT FROM c_Emails INTO @emailID

	WHILE @@FETCH_STATUS = 0  
	BEGIN  
		update Emails set BounceScore = 0 where EmailID = @emailID

		if @i % 1000  = 0
			print (convert(varchar,@i) + '  /  ' + convert(varchar(20), getdate(), 114))
		
		set @i = @i + 1
		
		FETCH NEXT FROM c_Emails INTO @emailID
	END

	CLOSE c_Emails  
	DEALLOCATE c_Emails  
End
