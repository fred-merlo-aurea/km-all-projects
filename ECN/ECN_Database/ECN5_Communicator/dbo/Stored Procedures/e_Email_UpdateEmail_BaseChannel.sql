CREATE PROCEDURE [dbo].[e_Email_UpdateEmail_BaseChannel]
	@OldEmail varchar(255),
	@NewEmail varchar(255),
	@BaseChannelID int,
	@UserID int,
	@Source varchar(255) = 'Unknown'
AS
 --   Declare 	@OldEmail varchar(255) = 'justin.welter@teamkm.com',
	--@NewEmail varchar(255) = 'testingteamkm@teamkm.com',
	--@BaseChannelID int = 92,
	--@UserID int = 5739
	Declare @CustIDsThatHaveOldEmail table(CustomerID int, OldEmailID int)
	delete from @CustIDsThatHaveOldEmail
	insert into @CustIDsThatHaveOldEmail(CustomerID, OldEmailID)
	Select distinct e.CustomerID, MAX(e.EmailID)
	from Emails e with(nolock)
	join ECN5_Accounts..Customer c with(nolock) on e.CustomerID = c.CustomerID
	where e.EmailAddress = @OldEmail and c.BaseChannelID = @BaseChannelID and c.ActiveFlag = 'Y' and c.IsDeleted = 0
	group by e.CustomerID
	order by e.CustomerID
	
	declare @OldEmailID int, @CustID int
	Declare mycursor cursor
	FOR
	Select CustomerID, OldEmailID from @CustIDsThatHaveOldEmail
	OPEN mycursor
	FETCH NEXT FROM mycursor
	INTO @CustID, @OldEmailID
	WHILE @@FETCH_STATUS = 0
	BEGIN
		Declare @NewEmailIDToUpdate int = null
		if exists(Select top 1 EmailID from Emails e with(nolock) where e.CustomerID = @CustID and e.EmailAddress = @NewEmail)
		BEGIN
			--New email already exists so we're updating profile fields from old email if they don't already have a value
			
			Select @NewEmailIDToUpdate = EmailID from Emails e with(nolock) where e.EmailAddress = @NewEmail and e.CustomerID = @CustID
			declare @EmailProfileToCopyFrom table(EmailID int,Title varchar(50),FirstName varchar(50),LastName varchar(50),FullName varchar(50), Company varchar(100), Occupation varchar(50), Address varchar(255), Address2 varchar(255),
				City varchar(50), State varchar(50), Zip varchar(50), Country varchar(50), Voice varchar(50), Mobile varchar(50), Fax varchar(50), WebSite varchar(50), Age varchar(50), Income varchar(50), Gender varchar(50), 
				User1 varchar(255), User2 varchar(255), User3 varchar(255), User4 varchar(255), User5 varchar(255),User6 varchar(255), BirthDate datetime, UserEvent1 varchar(50), UserEvent1Date datetime, UserEvent2 varchar(50), UserEvent2Date datetime,
				Notes varchar(1000), DateUpdated datetime, Password varchar(25), CarrierCode varchar(10), SMSOptin varchar(10))
				insert into @EmailProfileToCopyFrom
				select top 1 EmailID, Title, FirstName, LastName, FullName, Company, Occupation, Address, Address2, City, State, Zip, Country, Voice, Mobile, Fax, Website, Age, Income, Gender, User1, User2, User3, User4, User5,User6, BirthDate, UserEvent1,
					UserEvent1Date, UserEvent2, UserEvent2Date, Notes, DateUpdated, Password, CarrierCode, SMSOptin
				FROM Emails e with(nolock)
				where e.emailID = @OldEmailID

			--Update new email profile if there isn't already a value in the field
			update Emails
			set Emails.Title = CASE WHEN ISNULL(Emails.Title,'') = '' then e.Title else Emails.Title end,
				Emails.FirstName = CASE WHEN ISNULL(Emails.FirstName, '') = '' then e.FirstName else Emails.FirstName end,
				Emails.LastName = CASE WHEN ISNULL(Emails.LastName, '') = '' then e.LastName else Emails.LastName end,
				Emails.FullName = CASE WHEN ISNULL(Emails.FullName, '') = '' then e.FullName else Emails.FullName end,
				Emails.Company = CASE WHEN ISNULL(Emails.Company, '') = '' then e.Company else Emails.Company end,
				Emails.Occupation = CASE WHEN ISNULL(Emails.Occupation, '') = '' then e.Occupation else Emails.Occupation end,
				Emails.Address = CASE WHEN ISNULL(Emails.Address, '') = '' then e.Address else Emails.Address end,
				Emails.Address2 = CASE WHEN ISNULL(Emails.Address2, '') = '' then e.Address2 else Emails.Address2 end,
				Emails.City = CASE WHEN ISNULL(Emails.City, '') = '' then e.City else Emails.City end,
				Emails.State = CASE WHEN ISNULL(Emails.State, '') = '' then e.State else Emails.State end,
				Emails.Zip = CASE WHEN ISNULL(Emails.Zip, '') = '' then e.Zip else Emails.Zip end,
				Emails.Country = CASE WHEN ISNULL(Emails.Country, '') = '' then e.Country else Emails.Country end,
				Emails.Voice = CASE WHEN ISNULL(Emails.Voice, '') = '' then e.Voice else Emails.Voice end,
				Emails.Mobile = CASE WHEN ISNULL(Emails.Mobile, '') = '' then e.Mobile else Emails.Mobile end,
				Emails.Fax = CASE WHEN ISNULL(Emails.Fax, '') = '' then e.Fax else Emails.Fax end,
				Emails.Website = CASE WHEN ISNULL(Emails.Website, '') = '' then e.Website else Emails.Website end,
				Emails.Age = CASE WHEN ISNULL(Emails.Age, '') = '' then e.Age else Emails.Age end,
				Emails.Income = CASE WHEN ISNULL(Emails.Income, '') = '' then e.Income else Emails.Income end,
				Emails.Gender = CASE WHEN ISNULL(Emails.Gender, '') = '' then e.Gender else Emails.Gender end,
				Emails.User1 = CASE WHEN ISNULL(Emails.User1, '') = '' then e.User1 else Emails.User1 end,
				Emails.User2 = CASE WHEN ISNULL(Emails.User2, '') = '' then e.User2 else Emails.User2 end,
				Emails.User3 = CASE WHEN ISNULL(Emails.User3, '') = '' then e.User3 else Emails.User3 end,
				Emails.User4 = CASE WHEN ISNULL(Emails.User4, '') = '' then e.User4 else Emails.User4 end,
				Emails.User5 = CASE WHEN ISNULL(Emails.User5, '') = '' then e.User5 else Emails.User5 end,
				Emails.User6 = CASE WHEN ISNULL(Emails.User6, '') = '' then e.User6 else Emails.User6 end,
				Emails.Birthdate = CASE WHEN ISNULL(Emails.Birthdate, '') = '' then e.Birthdate else Emails.Birthdate end,
				Emails.UserEvent1 = CASE WHEN ISNULL(Emails.UserEvent1, '') = '' then e.UserEvent1 else Emails.UserEvent1 end,
				Emails.UserEvent1Date = CASE WHEN ISNULL(Emails.UserEvent1Date, '') = '' then e.UserEvent1Date else Emails.UserEvent1Date end,
				Emails.UserEvent2 = CASE WHEN ISNULL(Emails.UserEvent2, '') = '' then e.UserEvent2 else Emails.UserEvent2 end,
				Emails.UserEvent2Date = CASE WHEN ISNULL(Emails.UserEvent2Date, '') = '' then e.UserEvent2Date else Emails.UserEvent2Date end,
				Emails.Notes = CASE WHEN ISNULL(Emails.Notes, '') = '' then e.Notes else Emails.Notes end,
				Emails.DateUpdated = GetDATE(),
				Emails.Password = CASE WHEN ISNULL(Emails.Password, '') = '' then e.Password else Emails.Password end,
				Emails.CarrierCode = CASE WHEN ISNULL(Emails.CarrierCode, '') = '' then e.CarrierCode else Emails.CarrierCode end,
				Emails.SMSOptIn = CASE WHEN ISNULL(Emails.SMSOptIn, '') = '' then e.SMSOptIn else Emails.SMSOptIn end
			FROM @EmailProfileToCopyFrom e 
			WHERE Emails.emailid = @NewEmailIDToUpdate


		END
		else
		BEGIN
			--Email doesn't already exist so were doing an insert
			
			INSERT INTO Emails(Address, Address2, Age,Birthdate, BounceScore, CarrierCode, City, Company, Country, CustomerID, DateAdded, EmailAddress,Fax, FirstName, FullName, Gender, Income, LastName, Mobile, Notes, Occupation, Password,State, Title, User1, User2, User3, User4, User5, User6, UserEvent1, UserEvent1Date, UserEvent2,UserEvent2Date,Voice, Website, Zip)
			Select Address, Address2, Age,Birthdate, BounceScore, CarrierCode, City, Company, Country, CustomerID, GETDATE(), @NewEmail,Fax, FirstName, FullName, Gender, Income, LastName, Mobile, Notes, Occupation, Password,State, Title, User1, User2, User3, User4, User5, User6, UserEvent1, UserEvent1Date, UserEvent2,UserEvent2Date,Voice, Website, Zip
			FROM Emails with(nolock)
			where CustomerID = @CustID and EmailID = @OldEmailID
			select @NewEmailIDToUpdate = @@IDENTITY;
		END



		--Insert old Email into Master Suppress if it's not already in there
		declare @CustMSGroupID int = null
		select @CustMSGroupID = GroupID from Groups g with(nolock) 
		where g.CustomerID = @CustID and ISNULL(g.MasterSupression,0) = 1
		
		if not exists(select top 1 EmailGroupID from EmailGroups with(nolock) where EmailID = @OldEmailID and GroupID = @CustMSGroupID)
		BEGIN
			INSERT INTO EmailGroups(EmailID,GroupID, FormatTypeCode, SubscribeTypeCode, CreatedOn,CreatedSource)
			VALUES(@OldEmailID,@CustMSGroupID, 'HTML','E',GETDATE(),@Source)
			--Doing an update on the old email id to update the UpdatedDate
			UPDATE Emails 
			set DateUpdated = GETDATE()
			where EmailID = @OldEmailID
		END



		--Do Email Groups
		Declare @OldEmailGroupIDs table(EmailGroupID int, GroupID int)
		delete from @OldEmailGroupIDs

		INSERT INTO @OldEmailGroupIDs(EmailGroupID, GroupID)
		Select EmailGroupID, eg.GroupID
		FROM EmailGroups eg with(nolock)
		join Groups g with(nolock) on eg.GroupID = g.GroupID
		where eg.EmailID = @OldEmailID  and eg.GroupID <> @CustMSGroupID and g.CustomerID = @CustID
		
		--loop through groups and insert/update emailgroup records and insert EmailDataValues if they don't already have values for the new email
		declare @currentGroupID int, @currentEmailGroupID int
		declare groupcursor cursor
		for 
		Select EmailGroupID, GroupID from @OldEmailGroupIDs
		OPEN groupcursor
		FETCH NEXT FROM groupcursor
		INTO @currentEmailGroupID, @currentGroupID
		WHILE @@FETCH_STATUS =0
		BEGIN
			--Insert/update EMailGroups
			if not exists( select top 1 EmailGroupID from EmailGroups eg with(nolock) where eg.GroupID = @currentGroupID and eg.Emailid = @NewEmailIDToUpdate)
			BEGIN
				INSERT INTO EmailGroups(EmailID, GroupID, FormatTypeCode, SubscribeTypeCode, CreatedOn, SMSEnabled,CreatedSource)
				SELECT @NewEmailIDToUpdate, @currentGroupID, FormatTypeCode, case when eg.SubscribeTypeCode = 'M' then 'S' else eg.SubscribeTypeCode end, GETDATE(), SMSEnabled,@Source
				FROM EmailGroups eg with(nolock)
				where eg.EmailGroupID = @currentEmailGroupID
			END
			ELSE
			BEGIN
				Update EmailGroups
				set LastChanged = GETDATE(),LastChangedSource = @Source
				WHERE EmailID = @NewEmailIDToUpdate and GroupID = @currentGroupID
			END


			--Update Old Email Group record to be Master SUppressed
			Update EmailGroups
			set SubscribeTypeCode = 'M', LastChanged = GETDATE(), LastChangedSource = @Source
			where EmailGroupID = @currentEmailGroupID and SubscribeTypeCode <> 'M'

			--transfer UDF values
			if exists(select top 1 GroupDataFieldsID from GroupDatafields gdf where gdf.groupid = @currentGroupID and gdf.IsDeleted = 0)
			BEGIN
				
				--Do Standalone
				declare @UDFIDs table (gdfID int)
				delete from @UDFIDs
				insert into @UDFIDs(gdfID)
				select GroupdataFieldsID 
				from GroupDatafields gdf with(nolock)
				where gdf.GroupID = @currentGroupID and gdf.DatafieldSetID is null

				--UDFs exist so transfer values if they don't already exist
				declare @EmailDataValues table(edvID bigint, datavalue varchar(500), groupdatafieldsID int, DateModified datetime)
				delete from @EmailDataValues
				insert into @EmailDataValues(edvID, datavalue, groupdatafieldsID, DateModified)
				select edv.EmailDataValuesID, edv.DataValue, edv.GroupDatafieldsID,ISNULL(edv.ModifiedDate, ISNULL(edv.CreatedDate, GETDATE()))
				FROM EmailDataValues edv with(nolock)
				join @UDFIDs u on edv.GroupDatafieldsID = u.gdfID
				WHERE edv.EmailID = @OldEmailID 
				
				--if they do exist, update to which ever is newer
				update EmailDataValues
				set DataValue = edv2.datavalue,ModifiedDate = GETDATE(), UpdatedUserID = @UserID
				FROM @EmailDataValues edv2
				WHERE EmailDataValues.GroupDatafieldsID = edv2.groupdatafieldsID and EmailDataValues.EmailID = @NewEmailIDToUpdate and edv2.DateModified > ISNULL(EmailDataValues.ModifiedDate, CreatedDate) 
				
				--otherwise do an insert
				insert into EmailDataValues(EmailID, GroupDatafieldsID,DataValue, CreatedDate, CreatedUserID,SurveyGridID)
				select @NewEmailIDToUpdate, u.gdfID, edv1.datavalue, GETDATE(), @UserID,edv2.SurveyGridID
				FROM @EmailDataValues edv1 
				left outer join @UDFIDs u on edv1.groupdatafieldsID = u.gdfID
				left outer join EmailDataValues edv2 with(nolock) on edv1.groupdatafieldsID = edv2.GroupDatafieldsID and edv2.EmailID = @NewEmailIDToUpdate
				where edv2.EmailDataValuesID is null 



				--Do Transactional
				if exists (select top 1 GroupDatafieldsID from GroupDatafields gdf where gdf.GroupID = @currentGroupID and gdf.IsDeleted = 0 and gdf.DatafieldSetID is not null)
				BEGIN
					declare @tranUDFs table(gdfID int, DFSID int)
					delete from @tranUDFs
					INsert into @tranUDFs(gdfID, DFSID)
					select GroupDataFieldsID,gdf.DatafieldSetID
					FROM GroupDatafields gdf with(nolock) 
					where gdf.GroupID = @currentGroupID and gdf.IsDeleted = 0 and gdf.DatafieldSetID is not null

					declare @tranEmailDataValues table(datavalue varchar(500), gdfID int, dfsID int,oldEntryID uniqueidentifier,newEntryID uniqueidentifier, CreatedDate datetime)
					delete from @tranEmailDataValues
					insert into @tranEmailDataValues(datavalue, gdfID, dfsID,oldEntryID,newEntryID, CreatedDate)
					select DataValue, GroupDataFieldsID, u.DFSID,edv.EntryID,null, edv.CreatedDate 
					from EmailDataValues edv with(nolock)
					left outer join @tranUDFs u on edv.GroupDatafieldsID = u.gdfID
					where edv.EmailID = @OldEmailID and edv.GroupDatafieldsID in (select gdfID from @tranUDFs)

					
					declare @UniqueEntryIDs table(EntryID uniqueidentifier, newEntryID uniqueidentifier)
					delete from @UniqueEntryIDs
					insert into @UniqueEntryIDs(EntryID,newEntryID)
					Select distinct oldEntryID,NEWID()
					from @tranEmailDataValues
					
					update @tranEmailDataValues 
					set newEntryID = e.newEntryID
					from @UniqueEntryIDs e
					where oldEntryID = e.EntryID

					--loop through DataFieldSets
					declare @currentDFS int
					declare trancursor cursor
					for 
					select distinct DFSID 
					from @tranUDFs
					open trancursor
					FETCH NEXT FROM trancursor
					INTO @currentDFS
					WHILE @@FETCH_STATUS = 0
					BEGIN

						
						INSERT INTO EmailDataValues(EmailID, GroupDatafieldsID, EntryID, DataValue, SurveyGridID, CreatedDate, CreatedUserID)
						Select @NewEmailIDToUpdate, u.gdfID, t.newEntryID, t.datavalue, SurveyGridID, GETDATE(), @UserID
						FROM @tranEmailDataValues t 
						left outer join @tranUDFs u on t.gdfID = u.gdfID
						left outer join EmailDataValues edv with(nolock) on t.gdfID = edv.GroupDatafieldsID and edv.EmailID = @NewEmailIDToUpdate
						where (edv.EmailDataValuesID is null or ISNULL(edv.ModifiedDate, ISNULL(edv.CreatedDate, t.CreatedDate)) <= t.CreatedDate) and t.dfsID = @currentDFS
						
						FETCH NEXT FROM trancursor
						INTO @currentDFS
					END
					close trancursor
					deallocate trancursor
				END
			END
			FETCH NEXT FROM groupcursor
			INTO @currentEmailGroupID, @currentGroupID
		END
		close groupcursor
		deallocate groupcursor

		FETCH NEXT from mycursor 
		INTo @CustID, @OldEmailID
	END
	Close mycursor
	Deallocate mycursor