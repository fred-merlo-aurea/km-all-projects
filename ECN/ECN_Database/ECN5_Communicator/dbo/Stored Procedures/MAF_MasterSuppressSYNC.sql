CREATE proc [dbo].[MAF_MasterSuppressSYNC] 
(
	@Database varchar(50),
	@BasechannelID int = 0,
	@CustomerID int = 0
)
as
Begin

	set nocount on

	print 'START :  '+ ' / ' + convert(varchar(30), getdate() ,113 )

	Create Table #msEmails (emailaddress varchar(100), UNIQUE CLUSTERED (emailaddress))
	Create Table #mafEmails (subscriptionID int, emailaddress varchar(100), UNIQUE CLUSTERED (subscriptionID, emailaddress))

	Create Table #subscriptionID (subscriptionID int, emailaddress varchar(100), UNIQUE CLUSTERED (subscriptionID))

	BEGIN TRY

		insert into #msEmails
		select distinct emailaddress from ecn5_communicator.dbo.emailgroups eg join ECN5_COMMUNICATOR..Emails e on eg.EmailID = e.EmailID
		where groupID in (select groupID from ecn5_communicator.dbo.groups where customerID in (select customerID from [ECN5_ACCOUNTS].[DBO].[CUSTOMER]  
		where (BaseChannelID = @BasechannelID or CustomerID = @CustomerID)) and MasterSupression = 1)
		union 
		select distinct emailaddress from ecn5_communicator.dbo.ChannelMasterSuppressionList where BaseChannelID = @BasechannelID

		print '1  '+ ' / ' + convert(varchar(30), getdate() ,113) + ' / rowcount : ' + convert(varchar(10), @@rowcount)

		exec ('insert into #mafEmails select distinct subscriptionID, EMAIL from [10.10.41.251].' + @Database + '.dbo.subscriptions where emailexists = 1')

		print '2  '+ ' / ' + convert(varchar(30), getdate() ,113) + ' / rowcount : ' + convert(varchar(10), @@rowcount)

		insert into #subscriptionID
		select subscriptionID, replace(s.emailaddress, '''','''''')  from #mafEmails s join #msEmails m on s.emailaddress = m.emailaddress

		print '3  '+ ' / ' + convert(varchar(30), getdate() ,113)  + ' / rowcount : ' + convert(varchar(10), @@rowcount)

		--declare @xml xml
		--set @xml = (select s.subscriptionID as sID from #mafEmails s join #msEmails m on s.emailaddress = m.emailaddress  FOR XML PATH(''), ELEMENTS, ROOT('XML') )
		--exec [10.10.41.251].AdvanstarMasterDB.dbo.spUpdateSuppression @xml

		declare @sID int, @emailaddress varchar(50), @i int, @dt date

		set @dt = GETDATE()
		set @i = 0

		DECLARE c_update CURSOR FOR select subscriptionID, emailaddress from #subscriptionID

		OPEN c_update  
		FETCH NEXT FROM c_update INTO @sID, @emailaddress

		WHILE @@FETCH_STATUS = 0  
		BEGIN  
			
			--update [10.10.41.251].AdvanstarMasterDB.dbo.subscriptions
			--set SuppressedEmail = @emailaddress, email = '', emailexists = 0, SuppressedDate = @dt, inSuppression = 1
			--where subscriptionID = @sID
			
			exec ('exec [10.10.41.251].' + @Database + '.dbo.spUpdateSuppression '+ @sID + ',''' + @emailaddress + '''')

			print (convert(varchar,@i) + '  /  ' + convert(varchar(20), getdate(), 114))
			
			set @i = @i + 1
			
			FETCH NEXT FROM c_update INTO @sID, @emailaddress
		END

		CLOSE c_update  
		DEALLOCATE c_update  

		--print ('total Suppressed  :' + convert(varchar(10), @i))

		print 'END '+ ' / ' + convert(varchar(30), getdate() ,113 )
		
	END TRY
	BEGIN CATCH
    
		 DECLARE 
        @ErrorMessage    NVARCHAR(4000),
        @ErrorNumber     INT,
        @ErrorSeverity   INT,
        @ErrorState      INT,
        @ErrorLine       INT,
        @ErrorProcedure  NVARCHAR(200);

    -- Assign variables to error-handling functions that 
    -- capture information for RAISERROR.
    SELECT 
        @ErrorNumber = ERROR_NUMBER(),
        @ErrorSeverity = ERROR_SEVERITY(),
        @ErrorState = ERROR_STATE(),
        @ErrorLine = ERROR_LINE(),
        @ErrorProcedure = ISNULL(ERROR_PROCEDURE(), '-');

    -- Build the message string that will contain original
    -- error information.
    SELECT @ErrorMessage = 
        N'Error %d, Level %d, State %d, Procedure %s, Line %d, ' + 
            'Message: '+ ERROR_MESSAGE();

	Print @errormessage
        
	END CATCH


End


