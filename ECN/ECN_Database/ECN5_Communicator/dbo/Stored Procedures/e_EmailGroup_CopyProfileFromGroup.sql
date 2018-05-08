------------------------------------------------------
-- 2014-01-28 MK Changed EmailDataValueId to BIGINT
-- 2014-10-24 MK Added  WITH (NOLOCK) hints
--
--
------------------------------------------------------

CREATE proc [dbo].[e_EmailGroup_CopyProfileFromGroup]
(
	@sourcegroupID int,
	@destinationgroupID int,
	@EmailID int
)
AS
BEGIN		
	DECLARE 
		@DestinationEmailID int,
		@sourceCustomerID int,
		@destinationCustomerID int,
		@emailaddress varchar(100),
		@emailgroupID int
			
	--set @sourcegroupID = 14092		
	--set @destinationgroupID = 53169	
	--set @EmailID = 111164974
	
	SET @DestinationEmailID =0 
	SET @emailgroupID = 0

	SET NOCOUNT ON

	SELECT @sourceCustomerID = CustomerID FROM Groups WITH (NOLOCK) where GroupID = @sourcegroupID
	SELECT @destinationCustomerID = CustomerID FROM Groups WITH (NOLOCK) where GroupID = @destinationgroupID
	SELECT @emailaddress = emailaddress FROM [Emails] WITH (NOLOCK) where EmailID = @EmailID AND CustomerID = @sourceCustomerID


	if @sourceCustomerID > 0 AND @destinationCustomerID > 0 AND LEN(@emailaddress) > 0
	BEGIN
		
		if exists (SELECT top 1 eg.emailID FROM [EmailGroups] eg  WITH (NOLOCK) where GroupID = @sourcegroupID AND EmailID = @EmailID)
		BEGIN
		
			--check if all UDFs exists in destination group.
			--First do standalone udfs
			INSERT INTO GroupDatafields (GroupID,ShortName,LongName,SurveyID,DatafieldSetID,IsPublic,IsPrimaryKey,CreatedUserID,CreatedDate,UpdatedUserID,UpdatedDate,IsDeleted)
			SELECT	@destinationgroupID, gdf1.shortname, gdf1.LongName, null, null, 'N', 0 , null, getdate(), null, getdate(), isnull(gdf1.isDeleted,0)
			FROM
				GroupDatafields gdf1  WITH (NOLOCK) 
				LEFT OUTER JOIN GroupDatafields gdf2  WITH (NOLOCK) on gdf1.ShortName = gdf2.ShortName AND gdf2.GroupID = @destinationgroupID AND gdf2.IsDeleted = 0
			WHERE 
				gdf1.GroupID= @sourcegroupID 
				AND gdf2.GroupDatafieldsID IS NULL 
				and	gdf1.DatafieldSetID IS NULL 
				and	gdf1.IsDeleted = 0
			
			--then do transaction udfs
			if exists(SELECT top 1 * FROM GroupDatafields gdf with(nolock) where gdf.GroupID = @sourcegroupID AND gdf.IsDeleted = 0 AND gdf.DatafieldSetID IS NOT NULL)
			BEGIN

					INSERT INTO DatafieldSets(GroupID, MultivaluedYN, Name)
					SELECT @destinationgroupID, dfs1.MultiValuedYN, dfs1.Name
					FROM 
						DatafieldSets dfs1 with(nolock) 
						left outer join DatafieldSets dfs2 with(nolock) on dfs1.Name = dfs2.Name AND dfs2.GroupID = @destinationgroupID
					WHERE 
						dfs1.GroupID = @sourcegroupID 
						AND dfs2.DatafieldSetID IS NULL
					
					INSERT INTO GroupDatafields(GroupID, ShortName, LongName, SurveyID, DatafieldSetID, IsPublic, IsPrimaryKey, CreatedUserID, CreatedDate, UpdatedUserID, UpdatedDate, IsDeleted)
					SELECT @destinationgroupID, gdf1.ShortName, gdf1.LongName, null, dfs2.DataFieldSetID, gdf1.IsPublic, gdf1.IsPrimaryKey, null, GETDATE(), null, GETDATE(), gdf1.IsDeleted
					FROM 
						GroupDatafields gdf1 with(nolock)
						LEFT OUTER JOIN GroupDatafields gdf2 with(nolock) on gdf1.ShortName = gdf2.ShortName AND gdf2.GroupID = @destinationgroupID AND gdf2.IsDeleted = 0
						LEFT OUTER JOIN DatafieldSets dfs1 with(nolock) on gdf1.GroupID = dfs1.GroupID AND gdf1.DatafieldSetID = dfs1.DataFieldsetID 
						LEFT OUTER JOIN DatafieldSets dfs2 with(nolock) on dfs1.Name = dfs2.Name AND dfs2.GroupID = @destinationgroupID
					WHERE
						gdf1.GroupID= @sourcegroupID 
						AND gdf2.GroupDatafieldsID IS NULL
						AND gdf1.DatafieldSetID IS NOT NULL
						AND gdf1.IsDeleted = 0

			END
			
			if (@sourceCustomerID = @destinationCustomerID)
			BEGIN
			
				SET @DestinationEmailID = @EmailID
				
				SELECT @emailgroupID = emailgroupID FROM [EmailGroups] WITH (NOLOCK) WHERE GroupID = @destinationgroupID AND EmailID = @EmailID
								
				if @emailgroupID = 0
				BEGIN
					INSERT INTO [EmailGroups] (EmailID,GroupID,FormatTypeCode,SubscribeTypeCode,CreatedOn,LastChanged)
					VALUES (@EmailID, @destinationgroupID, 'html','S',GETDATE(), GETDATE())
				End
				else
				BEGIN
					update [EmailGroups] set LastChanged = GETDATE() where EmailGroupID = @emailgroupID
				End
			End
			Else
			BEGIN
				SELECT @DestinationEmailID = emailID FROM [Emails]  WITH (NOLOCK) WHERE CustomerID = @destinationCustomerID AND EmailAddress = @EmailAddress
				
				if (@DestinationEmailID = 0)
				BEGIN
					INSERT INTO [Emails] (Emailaddress, CustomerID, Title, FirstName, LastName, FullName, Company,  
						Occupation, Address, Address2, City, State, Zip, Country, Voice, Mobile, Fax,  
						Website, Age, Income, Gender, User1, User2, User3, User4, User5, User6, Birthdate, UserEvent1, UserEvent1Date, UserEvent2, UserEvent2Date, 
						notes, password, DateAdded)  
					SELECT el.EmailAddress, @destinationCustomerID, el.Title, el.FirstName, el.LastName, el.FullName, el.Company,  
						el.Occupation, el.Address, el.Address2, el.City, el.State, el.Zip, el.Country, el.Voice, el.Mobile, el.Fax,  
						el.Website, el.Age, el.Income, el.Gender, el.User1, el.User2, el.User3, el.User4, el.User5, el.User6,   
						el.Birthdate, el.UserEvent1, el.UserEvent1Date, el.UserEvent2, el.UserEvent2Date,
						'copied FROM Group : ' + convert(varchar(100),@sourcegroupID),el.password, getdate()
					FROM 
						[Emails] el  WITH (NOLOCK) 
					WHERE 
						emailID = @EmailID
					
					SET @DestinationEmailID = @@IDENTITY
					
					INSERT INTO [EmailGroups] (EmailID,GroupID,FormatTypeCode,SubscribeTypeCode,CreatedOn,LastChanged)
					VALUES (@DestinationEmailID, @destinationgroupID, 'html','S',GETDATE(), GETDATE())
					
				end	
				else
				BEGIN
				
					SELECT @emailgroupID = emailgroupID FROM [EmailGroups] WITH (NOLOCK) WHERE GroupID = @destinationgroupID AND EmailID = @DestinationEmailID
					
					IF @emailgroupID = 0
					BEGIN
						INSERT INTO [EmailGroups] (
							EmailID,
							GroupID,
							FormatTypeCode,
							SubscribeTypeCode,
							CreatedOn,
							LastChanged)
						VALUES (
							@DestinationEmailID, 
							@destinationgroupID, 
							'html',
							'S',
							GETDATE(), 
							GETDATE())
					END
					ELSE
					BEGIN
						UPDATE 
							[EmailGroups] 
						SET 
							LastChanged = GETDATE() 
						WHERE 
							EmailGroupID = @emailgroupID
					END
					
				END
			END
			
			SELECT	
				@EmailID as emailID, 
				gdf.ShortName, 
				edv.DataValue, 
				CAST(0 AS BIGINT) as edvID 
			INTO 
				#UDF 
			FROM
				[EMAILDATAVALUES] edv  WITH (NOLOCK) 
				join GroupDatafields gdf WITH (NOLOCK) on edv.GroupDatafieldsID = gdf.GroupDatafieldsID 
			WHERE
				GroupID = @sourcegroupID 
				AND EmailID = @EmailID 
				AND gdf.IsDeleted = 0 
				AND gdf.DatafieldSetID IS NULL
			
			SELECT 
				@EmailID as emailID,
				gdf.ShortName,
				edv.DataValue,
				edv.EntryID
			INTO 
				#UDFTrans
			FROM 
				[EmailDataVALUES] edv WITH (NOLOCK) 
				join GroupDatafields gdf WITH (NOLOCK) on edv.GroupDatafieldsID = gdf.GroupDatafieldsID
			WHERE 
				GroupID = @sourcegroupID 
				AND EmailID = @EmailID 
				AND gdf.IsDeleted = 0 
				AND gdf.DatafieldSetID IS NOT NULL
			ORDER BY 
				edv.EntryID
			
			--inserting the standalone data VALUES
			IF EXISTS (SELECT TOP 1 EmailID FROM #uDF)
			BEGIN
				UPDATE 
					#UDF
				SET 
					edvID = edv.EmailDataVALUESID
				FROM 
					[EMAILDATAVALUES] edv WITH (NOLOCK) 
					join groupdatafields gdf WITH (NOLOCK) on edv.GroupDatafieldsID = gdf.GroupDatafieldsID AND gdf.DataFieldSetID IS NULL
					join #UDF t on gdf.ShortName = t.shortname
				WHERE 
					gdf.GroupID = @destinationgroupID 
					AND edv.EmailID = @DestinationEmailID 
					AND gdf.IsDeleted = 0

				UPDATE 
					EmailDataVALUES
				SET 
					DataValue = u.datavalue, 
					ModifiedDate = GETDATE()
				FROM 
					#UDF u 
					join [EMAILDATAVALUES] edv WITH (NOLOCK) on edv.EmailDataVALUESID = u.edvID
				WHERE
					u.edvID > 0
				
				INSERT INTO [EMAILDATAVALUES] (EmailID, GroupDatafieldsID, DataValue, ModifiedDate, SurveyGridID, EntryID)
				SELECT @DestinationEmailID, gdf.GroupDatafieldsID, u.DataValue, GETDATE(), null, null
				from 
					#UDF u 
					join groupdatafields gdf  WITH (NOLOCK) on gdf.ShortName = u.shortname AND gdf.DatafieldSetID IS NULL
				where 
					gdf.GroupID = @destinationgroupID 
					AND u.edvID = 0 
					AND gdf.IsDeleted = 0
				
				
				
			End
			--inserting the transaction data VALUES
			if exists(SELECT top 1 EmailID FROM #UDFTrans)
			BEGIN
				Declare @TransTable table(oldEntryID uniqueidentifier, newEntryID uniqueidentifier)
				INSERT INTO 
					@TransTable(oldEntryID)
				SELECT 
					Distinct EntryID 
				FROM 
					#UDFTrans
				
				UPDATE 
					@TransTable
				SET 
					newEntryID = NEWID()

				INSERT INTO [EmailDataVALUES](
					EmailID, 
					GroupDatafieldsID, 
					DataValue, 
					ModifiedDate, 
					SurveyGridID, 
					EntryID)
				SELECT
					@DestinationEmailID, 
					gdf.GroupDataFieldsID, 
					u.DataValue, 
					GETDATE(), 
					null, 
					tt.newEntryID
				FROM 
					#UDFTrans u
					join GroupDatafields gdf  WITH (NOLOCK) on gdf.ShortName = u.ShortName AND gdf.DatafieldSetID IS NOT NULL
					join @TransTable tt on u.EntryID = tt.oldEntryID
				WHERE
					gdf.GroupID = @destinationgroupID 
					AND gdf.IsDeleted = 0
			END			
						
						
			DROP TABLE #UDF
			DROP TABLE #UDFTRANS
			
		END
		ELSE
		BEGIN
			RAISERROR ('ERROR : Email Address not in Source Group.',10,1)
		END
	END
	ELSE
	BEGIN
		RAISERROR ('ERROR : Invalid parameters.',10,1)
	END
END