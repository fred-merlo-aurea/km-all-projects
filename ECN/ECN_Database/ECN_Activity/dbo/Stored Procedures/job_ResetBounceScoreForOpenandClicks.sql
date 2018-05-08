CREATE Proc [dbo].[job_ResetBounceScoreForOpenandClicks]
as
Begin
	declare @Emails table (EmailID int NOT NULL PRIMARY KEY WITH (IGNORE_DUP_KEY = ON)) 
	declare	@i int,
			@emailID int,
			@dt date,
			@PreviousDayMaxOpenID int,
			@PreviousDayMaxClickID int

	set nocount on
			
	set @dt =  CONVERT(date, dateadd(dd,-1, getdate()))
	set @i = 0		
	
	
	select @PreviousDayMaxOpenID = MAX(OpenID)
	from BlastActivityOpens with (NOLOCK)  
	where OpenTime <  @dt
	
	select @PreviousDayMaxClickID = MAX(ClickID)
	from BlastActivityClicks with (NOLOCK)  
	where ClickTime <  @dt
	
	insert into @Emails
	select distinct emailID 
	from BlastActivityOpens with (NOLOCK) 
	where OpenID > @PreviousDayMaxOpenID and OpenTime between @dt and CONVERT(date, getdate())
	
	PRINT ('Previou Day Open Count ' + Convert(varchar(10), @@ROWCOUNT))
	
	insert into @Emails
	select distinct emailID 
	from BlastActivityClicks with (NOLOCK) 
	where ClickID > @PreviousDayMaxClickID and ClickTime between @dt and CONVERT(date, getdate())

	PRINT ('Previou Day Click Count ' + Convert(varchar(10), @@ROWCOUNT))

	DECLARE c_Emails CURSOR FOR select emailID from @Emails 

	OPEN c_Emails  
	FETCH NEXT FROM c_Emails INTO @emailID

	WHILE @@FETCH_STATUS = 0  
	BEGIN  
		update ecn5_communicator..Emails set BounceScore = 0, SoftBounceScore = 0 where EmailID = @emailID

		if @i % 1000  = 0
			print (convert(varchar,@i) + '  /  ' + convert(varchar(20), getdate(), 114))
		
		set @i = @i + 1
		
		FETCH NEXT FROM c_Emails INTO @emailID
	END

	CLOSE c_Emails  
	DEALLOCATE c_Emails  
End
