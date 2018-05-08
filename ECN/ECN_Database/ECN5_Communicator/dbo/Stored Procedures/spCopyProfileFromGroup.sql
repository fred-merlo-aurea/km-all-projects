------------------------------------------------------
-- 2014-01-28 MK Changed EmailDataValueId to BIGINT
--
--
--
------------------------------------------------------

CREATE proc [dbo].[spCopyProfileFromGroup]
(
	@sourcegroupID int,
		@destinationgroupID int,
		@EmailID int
)
as
Begin		
	declare 
		@DestinationEmailID int,
		@sourceCustomerID int,
		@destinationCustomerID int,
		@emailaddress varchar(100),
		@emailgroupID int
			
	--set @sourcegroupID = 14092		
	--set @destinationgroupID = 53169	
	--set @EmailID = 111164974
	
	set @DestinationEmailID =0 
	set @emailgroupID = 0

	set nocount on

	select @sourceCustomerID = CustomerID from Groups where GroupID = @sourcegroupID
	select @destinationCustomerID = CustomerID from Groups where GroupID = @destinationgroupID
	select @emailaddress = emailaddress from [Emails] where EmailID = @EmailID and CustomerID = @sourceCustomerID


	if @sourceCustomerID > 0 and @destinationCustomerID > 0 and LEN(@emailaddress) > 0
	Begin
		
		if exists (select top 1 eg.emailID from [EmailGroups] eg where GroupID = @sourcegroupID and EmailID = @EmailID)
		Begin
		
			--check if all UDFs exists in destination group.
			insert into GroupDatafields (GroupID,ShortName,LongName,SurveyID,DatafieldSetID,IsPublic,IsPrimaryKey,CreatedUserID,CreatedDate,UpdatedUserID,UpdatedDate,IsDeleted)
			select	@destinationgroupID, gdf1.shortname, gdf1.LongName, null, null, 'n', 0 , null, getdate(), null, getdate(), isnull(gdf1.isDeleted,0)
			from	GroupDatafields gdf1 left outer join 
					GroupDatafields gdf2 on gdf1.ShortName = gdf2.ShortName and gdf2.GroupID = @destinationgroupID and gdf2.IsDeleted = 0
			where 
					gdf1.GroupID= @sourcegroupID and 
					gdf2.GroupDatafieldsID is null and
					gdf1.IsDeleted = 0
			
			if (@sourceCustomerID = @destinationCustomerID)
			Begin
			
				set @DestinationEmailID = @EmailID
				
				select @emailgroupID = emailgroupID from [EmailGroups] where GroupID = @destinationgroupID and EmailID = @EmailID
								
				if @emailgroupID = 0
				Begin
					insert into [EmailGroups] (EmailID,GroupID,FormatTypeCode,SubscribeTypeCode,CreatedOn,LastChanged)
					values (@EmailID, @destinationgroupID, 'html','S',GETDATE(), GETDATE())
				End
				else
				Begin
					update [EmailGroups] set LastChanged = GETDATE() where EmailGroupID = @emailgroupID
				End
			End
			Else
			Begin
				select @DestinationEmailID = emailID from [Emails] where CustomerID = @destinationCustomerID and EmailAddress = @EmailAddress
				
				if (@DestinationEmailID = 0)
				Begin
					insert into [Emails] (Emailaddress, CustomerID, Title, FirstName, LastName, FullName, Company,  
						Occupation, Address, Address2, City, State, Zip, Country, Voice, Mobile, Fax,  
						Website, Age, Income, Gender, User1, User2, User3, User4, User5, User6, Birthdate, UserEvent1, UserEvent1Date, UserEvent2, UserEvent2Date, 
						notes, password, DateAdded)  
					SELECT el.EmailAddress, @destinationCustomerID, el.Title, el.FirstName, el.LastName, el.FullName, el.Company,  
						el.Occupation, el.Address, el.Address2, el.City, el.State, el.Zip, el.Country, el.Voice, el.Mobile, el.Fax,  
						el.Website, el.Age, el.Income, el.Gender, el.User1, el.User2, el.User3, el.User4, el.User5, el.User6,   
						el.Birthdate, el.UserEvent1, el.UserEvent1Date, el.UserEvent2, el.UserEvent2Date,
						'copied from Group : ' + convert(varchar(100),@sourcegroupID),el.password, getdate()
					from [Emails] el 
					where emailID = @EmailID
					
					set @DestinationEmailID = @@IDENTITY
					
					insert into [EmailGroups] (EmailID,GroupID,FormatTypeCode,SubscribeTypeCode,CreatedOn,LastChanged)
					values (@DestinationEmailID, @destinationgroupID, 'html','S',GETDATE(), GETDATE())
					
				end	
				else
				Begin
				
					select @emailgroupID = emailgroupID from [EmailGroups] where GroupID = @destinationgroupID and EmailID = @DestinationEmailID
					
					if @emailgroupID = 0
					Begin
						insert into [EmailGroups] (EmailID,GroupID,FormatTypeCode,SubscribeTypeCode,CreatedOn,LastChanged)
						values (@DestinationEmailID, @destinationgroupID, 'html','S',GETDATE(), GETDATE())
					End
					else
					Begin
						update [EmailGroups] set LastChanged = GETDATE() where EmailGroupID = @emailgroupID
					End
					
				End
			End
			
			select	
				@EmailID as emailID, 
				gdf.ShortName, 
				edv.DataValue, 
				CAST(0 AS BIGINT) as edvID 
			into #UDF 
			from	[EMAILDATAVALUES] edv join GroupDatafields gdf on edv.GroupDatafieldsID = gdf.GroupDatafieldsID 
			where	GroupID = @sourcegroupID and EmailID = @EmailID and gdf.IsDeleted = 0
			
			if exists (select top 1 EmailID from #uDF)
			Begin
				update #UDF
				set edvID = edv.EmailDataValuesID
				from [EMAILDATAVALUES] edv join groupdatafields gdf on edv.GroupDatafieldsID = gdf.GroupDatafieldsID join #UDF t on gdf.ShortName = t.shortname
				where gdf.GroupID = @destinationgroupID and edv.EmailID = @DestinationEmailID and gdf.IsDeleted = 0

				update EmailDataValues
				set DataValue = u.datavalue, ModifiedDate = GETDATE()
				from #UDF u join [EMAILDATAVALUES] edv on edv.EmailDataValuesID = u.edvID
				where u.edvID > 0
				
				insert into [EMAILDATAVALUES] (EmailID, GroupDatafieldsID, DataValue, ModifiedDate, SurveyGridID, EntryID)
				select @DestinationEmailID, gdf.GroupDatafieldsID, u.DataValue, GETDATE(), null, null
				from #UDF u join groupdatafields gdf on gdf.ShortName = u.shortname
				where gdf.GroupID = @destinationgroupID and u.edvID = 0 and gdf.IsDeleted = 0
			End
						
			drop table #UDF
			
		End
		Else
		Begin
			RAISERROR ('ERROR : Email Address not in Source Group.',10,1)
		End
	End
	Else
	Begin
		RAISERROR ('ERROR : Invalid parameters.',10,1)
	End
End