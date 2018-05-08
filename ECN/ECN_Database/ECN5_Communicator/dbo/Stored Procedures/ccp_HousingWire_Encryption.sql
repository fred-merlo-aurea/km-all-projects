CREATE PROCEDURE [dbo].[ccp_HousingWire_Encryption]
as
Begin

	set nocount on

	declare @basechannelID int = 142,
			@Previousday datetime = dateadd(day, -1, getdate()),
			@i bigint = 1,
			@emailID int,
			@EmailAddressMD5 varchar(64)
		
	print (' Start : ' + convert(varchar(100), getdate(), 109))

	Create table #tmpMD5 (emailID int primary key, emailaddress varchar(100), EmailAddressMD5 varchar(64))

	insert into #tmpMD5
	select EmailID, EmailAddress, CONVERT(VARCHAR(32),HashBytes('MD5', EmailAddress),2)
	from
				ecn5_communicator..emails e  with (NOLOCK) 
	where 
				e.customerID in (select c.customerID from ECN5_ACCOUNTS..customer c with (NOLOCK) where basechannelID = @basechannelID) 
				and (isnull(user4,'') = '' or isnull(e.DateUpdated, e.DateAdded) > @Previousday)

	print (' #tmpMD5 insert : ' + convert(varchar(100), getdate(), 109))
	

	DECLARE c_emails CURSOR LOCAL FORWARD_ONLY STATIC READ_ONLY FOR
	select t.emailID, t.EmailAddressMD5 
	from #tmpMD5 t join ECN5_COMMUNICATOR..emails e on t.emailID = e.emailID where isnull(user4,'') <> EmailAddressMD5

	OPEN c_emails    
  
	FETCH NEXT FROM c_emails INTO @emailID,  @EmailAddressMD5
	WHILE @@FETCH_STATUS = 0    
	BEGIN    

		print (convert(varchar(100), @i)  + ' / emailID : ' + convert(varchar(100), @emailID)  + ' / EmailAddressMD5 : ' + convert(varchar(100), @EmailAddressMD5)  + ' / ' + convert(varchar(100), getdate(), 109))

		Update ecn5_communicator..emails
		set 
				user4 = @EmailAddressMD5
		where
				emailID = @emailID

		set @i = @i + 1

		FETCH NEXT FROM c_emails INTO  @emailID,  @EmailAddressMD5
	End    
     
	CLOSE c_emails    
	DEALLOCATE c_emails   

	drop table #tmpMD5

	print (' END : ' + convert(varchar(100), getdate(), 109))
END

