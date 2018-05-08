CREATE proc [dbo].[MAF_AdvanstarMasterSuppressSYNC]
as
Begin

	set nocount on

	print 'START :  '+ ' / ' + convert(varchar(30), getdate() ,113 )

	declare @msEmails table (emailaddress varchar(100), UNIQUE CLUSTERED (emailaddress))
	declare @mafEmails table (subscriptionID int, emailaddress varchar(100), UNIQUE CLUSTERED (subscriptionID, emailaddress))

	declare @subscriptionID table (subscriptionID int, emailaddress varchar(100), UNIQUE CLUSTERED (subscriptionID))

	insert into @msemails
	select distinct emailaddress from ecn5_communicator.dbo.emailgroups eg join ECN5_COMMUNICATOR..Emails e on eg.EmailID = e.EmailID
	where groupID in (select groupID from ecn5_communicator.dbo.groups where customerID in (select customerID from [ECN5_ACCOUNTS].[DBO].[CUSTOMER]  where BaseChannelID = 65) and MasterSupression = 1)
	union 
	select distinct emailaddress from ecn5_communicator.dbo.ChannelMasterSuppressionList where BaseChannelID = 65
	union 
	select distinct emailaddress from ecn5_communicator.dbo.GlobalMasterSuppressionList

	print '1  '+ ' / ' + convert(varchar(30), getdate() ,113 )

	insert into @mafemails
	select distinct subscriptionID, EMAIL from [10.10.41.251].AdvanstarMasterDB.dbo.subscriptions where emailexists = 1

	print '2  '+ ' / ' + convert(varchar(30), getdate() ,113 )

	insert into @subscriptionID
	select subscriptionID, s.emailaddress  from @mafemails s join @msEmails m on s.emailaddress = m.emailaddress

	print '3  '+ ' / ' + convert(varchar(30), getdate() ,113)  + ' / rowcount : ' + convert(varchar(10), @@rowcount)

	--declare @xml xml
	--set @xml = (select s.subscriptionID as sID from @mafemails s join @msEmails m on s.emailaddress = m.emailaddress  FOR XML PATH(''), ELEMENTS, ROOT('XML') )
	--exec [10.10.41.251].AdvanstarMasterDB.dbo.spUpdateSuppression @xml

	declare @sID int, @emailaddress varchar(50), @i int, @dt date

	set @dt = GETDATE()
	set @i = 0

	DECLARE c_update CURSOR FOR select subscriptionID, emailaddress from @subscriptionID

	OPEN c_update  
	FETCH NEXT FROM c_update INTO @sID, @emailaddress

	WHILE @@FETCH_STATUS = 0  
	BEGIN  
		
		--update [10.10.41.251].AdvanstarMasterDB.dbo.subscriptions
		--set SuppressedEmail = @emailaddress, email = '', emailexists = 0, SuppressedDate = @dt, inSuppression = 1
		--where subscriptionID = @sID

		exec [10.10.41.251].AdvanstarMasterDB.dbo.spUpdateSuppression @sID, @emailaddress

		print (convert(varchar,@i) + '  /  ' + convert(varchar(20), getdate(), 114))
		
		set @i = @i + 1
		
		FETCH NEXT FROM c_update INTO @sID, @emailaddress
	END

	CLOSE c_update  
	DEALLOCATE c_update  

	print ('total Suppressed  :' + convert(varchar(10), @i))

	print 'END '+ ' / ' + convert(varchar(30), getdate() ,113 )



End
