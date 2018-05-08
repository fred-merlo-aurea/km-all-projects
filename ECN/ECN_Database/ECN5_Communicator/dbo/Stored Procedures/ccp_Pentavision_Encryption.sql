CREATE PROCEDURE dbo.ccp_Pentavision_Encryption
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

	declare @basechannelID int = 144,
			@Currentdttime datetime = getdate(),
			@CustomerID int = 0,
			@i bigint = 1,
			@emailID int,
			@EmailAddressSHA256 varchar(64),
			@NPISHA256 varchar(64)
		
	print (' Start : ' + convert(varchar(100), getdate(), 109))

	Create table #tmpNPI (emailaddress varchar(100) primary key, npi varchar(100), EmailAddressSHA256 varchar(64), NPISHA256 varchar(64) )

	insert into #tmpNPI
	select x.EmailAddress, x.npi, [ecn_misc].dbo.udf_hashbytes('SHA256', x.EmailAddress) , [ecn_misc].dbo.udf_hashbytes('SHA256', x.npi) 
	from
	(
		select e.EmailAddress, DataValue npi, ROW_NUMBER() over (partition by e.emailaddress order by isnull(edv.ModifiedDate,edv.CreatedDate) desc) as rowN
		from 
				ecn5_communicator..emails e  with (NOLOCK) join 
				ecn5_communicator..Emaildatavalues edv with (NOLOCK) on edv.emailID = e.EmailID join
				ecn5_communicator..groupdatafields gdf with (NOLOCK) on gdf.groupdatafieldsID = edv.groupdatafieldsID

		where 
				e.customerID in (select c.customerID from ECN5_ACCOUNTS..customer c with (NOLOCK) where basechannelID = @basechannelID)  and 
				gdf.shortname in ('MAF_NPI_NUMBER','NPI','NPI_number') and 
				(datavalue <> '' and charindex(',', datavalue) = 0) 
				-- COMMENT BELOW LINES FOR FULL SYNC
				and
				(
					DATEDIFF(MINUTE, isnull(e.DateUpdated, e.DateAdded), @Currentdttime) <= 250 or
					DATEDIFF(MINUTE, isnull(edv.ModifiedDate, edv.CreatedDate), @Currentdttime) <= 250 
				)
	) x
	where
		rowN = 1

	print (' #tmpNPI insert : ' + convert(varchar(100), getdate(), 109))

	select count(*) from #tmpNPI

	select  top 1  @CustomerID = CustomerID from ecn5_accounts..Customer where basechannelID = @basechannelID order by customerID

	 while (@@ROWCOUNT > 0)
	 Begin 

		DECLARE c_emails CURSOR LOCAL FORWARD_ONLY STATIC READ_ONLY FOR
		select  e.EmailID, EmailAddressSHA256, NPISHA256
		from 
				ecn5_communicator..emails e with (NOLOCK) join
				#tmpNPI t with (NOLOCK) on t.emailaddress = e.EmailAddress 
		where 
				customerID = @customerID and 
				(isnull(user3,'') = '' or EmailAddressSHA256 <> isnull(user4,'') or  NPISHA256 <> isnull(user5,''))

		OPEN c_emails    
  
		FETCH NEXT FROM c_emails INTO @emailID,  @EmailAddressSHA256, @NPISHA256
		WHILE @@FETCH_STATUS = 0    
		BEGIN    

			print (convert(varchar(100), @CustomerID) + ' / counter : ' + convert(varchar(100), @i)  + ' / emailID : ' + convert(varchar(100), @emailID)   + ' / ' + convert(varchar(100), getdate(), 109))

			Update ecn5_communicator..emails
			set 
					user3 = [ecn_misc].dbo.udf_hashbytes('SHA1', EmailAddress),
					user4 = @EmailAddressSHA256,
					user5 = @NPISHA256
			where
					emailID = @emailID
		

			set @i = @i + 1

			FETCH NEXT FROM c_emails INTO  @emailID,  @EmailAddressSHA256, @NPISHA256
		End    
     
		CLOSE c_emails    
		DEALLOCATE c_emails   

		/* Get Next Customer */
		select top 1  @CustomerID =  CustomerID from ecn5_accounts..Customer where basechannelID = @basechannelID and customerID > @customerID order by customerID

	End

	drop table #tmpNPI

	print (' END : ' + convert(varchar(100), getdate(), 109))
END

GO
