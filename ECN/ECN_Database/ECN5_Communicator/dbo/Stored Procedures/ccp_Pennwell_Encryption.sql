CREATE PROCEDURE [dbo].[ccp_Pennwell_Encryption]
as
Begin

	set nocount on

	/* SSH Setup if we need to move to a another server

			--You'll need to run this part if CLR is not enabled on your database
		--EXEC sp_configure 'clr enabled', 1;
		--RECONFIGURE WITH OVERRIDE;
		--GO

 
		DECLARE @strDllPath VARCHAR(256)
		--Set this path to the path of the dll
		SET @strDllPath = 'C:\SQL DLLs\cryptohashCLR.dll'
 
		--Drop the assembly if it already exists
		IF EXISTS (SELECT * FROM sys.assemblies asms WHERE asms.name = N'cryptohashCLR')
			DROP ASSEMBLY [cryptohashCLR]
		--Create the assembly
		CREATE ASSEMBLY [cryptohashCLR] FROM @strDllPath WITH PERMISSION_SET = SAFE

		go

		IF EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[udf_hashBytes]') AND type in (N'FN', N'IF', N'TF', N'FS', N'FT'))
		DROP FUNCTION [dbo].[udf_hashBytes]
		GO
 
		CREATE FUNCTION [dbo].[udf_hashBytes]
			(
			@hashtype NVARCHAR(MAX),
			@input NVARCHAR(MAX)
			)
		RETURNS NVARCHAR(MAX) AS EXTERNAL NAME cryptohashCLR.[cryptohashCLR.SqlHash].CLRHash
		GO
 
		--Execute sample cases
		IF dbo.udf_hashbytes('SHA1','The quick brown fox jumps over the lazy dog') = '2FD4E1C67A2D28FCED849EE1BB76E7391B93EB12'
			PRINT 'Successful: Output of UDF test was as expected'
		ELSE
			PRINT 'Error: Inconsistent results were returned from UDF test'
	*/

	declare @basechannelID int = 139,
			@Currentdttime datetime = getdate(),
			@CustomerID int = 0,
			@i bigint = 1,
			@emailID int,
			@emailaddress varchar(100),
			@emailaddresssha512 varchar(255),
			@salt varchar(10) = 'PennWell',
			@EmailAddressSHA256 varchar(64)
		
	print (' Start : ' + convert(varchar(100), getdate(), 109))

	Create table #tmpE (emailID bigint, emailaddress varchar(100), user4 varchar(255), emailaddresssha512 varchar(255))

	select  top 1  @CustomerID = CustomerID from ecn5_accounts..Customer where basechannelID = @basechannelID order by customerID

	 while (@@ROWCOUNT > 0)
	 Begin 

		set @i = 1

		print ('Start @CustomerID ' + convert(varchar,@CustomerID) +  ' / ' + CONVERT(varchar(20), getdate(), 109))

		delete from #tmpE

		insert into #tmpE
		select  e.EmailID, e.emailaddress, isnull(user4,''), ''
		from 
				ecn5_communicator..emails e with (NOLOCK)
		where 
				customerID = @customerID and 
				(
					isnull(user4,'') = '' or 
					---- COMMENT BELOW LINES FOR FULL SYNC
					(
						DATEDIFF(MINUTE, isnull(e.DateUpdated, e.DateAdded), @Currentdttime) <= 480
					)
				)

		print ('insert #tmpE ' + convert(varchar,@@rowcount) + ' / '  + CONVERT(varchar(20), getdate(), 109))

		update #tmpE set emailaddresssha512 = [ecn_misc].dbo.udf_hashbytes('SHA512', lower(ltrim(rtrim(emailaddress))) + @salt)

		print ('Update #tmpE ' + convert(varchar,@@rowcount) + ' / '  + CONVERT(varchar(20), getdate(), 109))

		DECLARE c_emails CURSOR LOCAL FORWARD_ONLY STATIC READ_ONLY FOR
		select  EmailID, lower(ltrim(rtrim(emailaddress))), emailaddresssha512
		from 
				#tmpE
		where 
				user4 <> emailaddresssha512 -- or emailaddress <> LOWER(emailaddress) COLLATE Latin1_General_CS_AI

		OPEN c_emails    
  
		FETCH NEXT FROM c_emails INTO @emailID,  @emailaddress, @emailaddresssha512
		WHILE @@FETCH_STATUS = 0    
		BEGIN    

			if (@i % 10000 = 0)
				print (convert(varchar(100), @CustomerID) + ' / counter : ' + convert(varchar(100), @i) + ' / ' + convert(varchar(100), getdate(), 109))

			Update ecn5_communicator..emails
			set 
					--emailaddress = @emailaddress,
					user4 = @emailaddresssha512
			where
					emailID = @emailID

			set @i = @i + 1

			FETCH NEXT FROM c_emails INTO  @emailID,  @emailaddress, @emailaddresssha512
		End    
     
		CLOSE c_emails    
		DEALLOCATE c_emails   

		print ('End @CustomerID ' + convert(varchar,@CustomerID) +  ' / ' + CONVERT(varchar(20), getdate(), 109))

		/* Get Next Customer */
		select top 1  @CustomerID =  CustomerID from ecn5_accounts..Customer where basechannelID = @basechannelID and customerID > @customerID order by customerID

	End

	drop table #tmpE

	print (' END : ' + convert(varchar(100), getdate(), 109))
END