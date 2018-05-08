--2014-10-24 MK Added  WITH (NOLOCK) hints

CREATE PROCEDURE dbo.e_EmailDataValues_GetTransUDFDataValues
	(
		@GroupID int,
		@CustomerID int,
		@UDFEmailID varchar(10),
		@DatafieldSetID int
		
		--SET @GroupID = 8656
		--SET @CustomerID = 1764
		--SET @UDFEmailID = '18499939'
		--SET @DatafieldSetID = 13
	)
AS

BEGIN
	SET NOCOUNT ON
	
	/* Local Variables */
	DECLARE @FieldName varchar(250)
	
	DECLARE @EmailID int,
		@ShortName varchar(50),
		@GroupDataFieldsID int,
		@Datavalue varchar(500),
		@strColumns varchar(500),
		@strValues varchar(500),
		@sqlQuery varchar(8000),
		@EntryID varchar(50),
		@LastModifiedDate varchar(50)
	
	/* 
	CREATE TEMP TABLE WITH Emails COLUMNS 
	LATER ATTACH CUSTOM COLUMNS 
	*/
	
	create table #Email
	(
		EmailID	int,
		EntryID varchar(50)
	)
	
	/* Create table variable to store the UDF's */
	declare @tblGroupField TABLE 
		(    	
			GroupFieldID int identity(1,1),
			GroupDataFields int,
			GroupFieldName varchar(50)
		)
	
	/* insert UDF record into table variable */
	insert into @tblGroupField (
		GroupDataFields, 
		GroupFieldName)
	select 
		GroupDataFieldsID, 
		ShortName 
	from 
		GroupDataFields WITH (NOLOCK) 
		JOIN Groups WITH (NOLOCK) on GroupDataFields.GroupID = Groups.GroupID 
	where 
		Groups.groupID = @GroupID 
		and GroupDataFields.datafieldsetID = @DatafieldSetID 
		and GroupDatafields.IsDeleted = 0
	
	/* if no UDF's exists return EmailID from Emails table */
	if @@RowCount = 0
	Begin
		exec ('ALTER Table #Email ADD LastModifiedDate varchar(50) NULL')
		
		insert into #Email
		(
			EmailID, 
			EntryID,
			LastModifiedDate
		)
		SELECT DISTINCT 
			Emails.EmailID, 
			null,
			ISNULL(Emails.DateUpdated, Emails.DateAdded)
		FROM 
			Groups WITH (NOLOCK) 
			join EmailGroups WITH (NOLOCK) on Groups.groupID = EmailGroups.groupID 
			join Emails WITH (NOLOCK) on Emails.EmailID = EmailGroups.EmailID 
		WHERE 	
			Groups.groupID = @GroupID 
			and Groups.CustomerID = @CustomerID 
		ORDER BY 
			Emails.EmailID
	
	End
	Else
	Begin
		/* if UDF's exists*/
	
		/* CURSOR TO CREATE THE TABLE WITH ALL COLUMNS */
		DECLARE UDF_GROUPS CURSOR
		READ_ONLY
		FOR SELECT 'Alter table #Email ADD [' + SHORTNAME + '] varchar(500) NULL' FROM GROUPDATAFIELDS WITH (NOLOCK) WHERE GROUPID = @GroupID and GroupDataFields.datafieldsetID = @DatafieldSetID and GroupDataFields.IsDeleted = 0
		
		OPEN UDF_GROUPS
		FETCH NEXT FROM UDF_GROUPS INTO @FieldName
		WHILE (@@fetch_status <> -1)
		BEGIN
			IF (@@fetch_status <> -2)
			Begin
				exec (@FieldName)
			end
			FETCH NEXT FROM UDF_GROUPS INTO @FieldName
		END
		
		CLOSE UDF_GROUPS
		DEALLOCATE UDF_GROUPS

		exec ('ALTER Table #Email ADD LastModifiedDate varchar(50) NULL')
		
		/* Insert records from Email Table - which does't have any records in EMAILDATAVALUES */
		insert into #Email (EmailID)
		select 	
			Emails.EmailID
		from 
			Groups WITH (NOLOCK) 
			join EmailGroups WITH (NOLOCK) on Groups.groupID = EmailGroups.groupID 
			join Emails WITH (NOLOCK) on Emails.EmailID = EmailGroups.EmailID and Emails.EmailID = @UDFEmailID
		where 	
			Groups.groupID = @GroupID 
			and Groups.CustomerID = @CustomerID 
			and Emails.EmailID not in 
			(
				select distinct EmailID  
				from 
					EMAILDATAVALUES WITH (NOLOCK) 
					join GroupDataFields WITH (NOLOCK) on EMAILDATAVALUES.GroupDataFieldsid = GroupDataFields.GroupDataFieldsID and GroupDataFields.datafieldsetID = @DatafieldSetID and EMAILDATAVALUES.EmailID = @UDFEmailID 
					join Groups WITH (NOLOCK) on Groups.GroupID = GroupDataFields.GroupID
				where 
					Groups.groupID = @GroupID 
					and Groups.CustomerID = @CustomerID 
					and GroupDatafields.IsDeleted = 0 
			)



		/* CURSOR TO LOAD THE CUSTOM COLUMN VALUES IN TEMPORARY TABLE */
		DECLARE UDF_CUSTOMVALUES CURSOR
		READ_ONLY
		FOR 
		select 	Emails.EmailID, 
			GroupDatafields.ShortName, 
			GroupDatafields.groupDataFieldsID,
			EMAILDATAVALUES.DataValue as datavalue, EntryID,IsNull(MAX(EmailDataValues.ModifiedDate), ISNULL(MAX(EMAILDATAVALUES.CreatedDate), ''))
		from 
			Groups WITH (NOLOCK) 
			join EmailGroups WITH (NOLOCK) on Groups.groupID = EmailGroups.groupID 
			join Emails WITH (NOLOCK) on Emails.EmailID = EmailGroups.EmailID and Emails.EmailID = @UDFEmailID 
			join EMAILDATAVALUES WITH (NOLOCK) on Emails.EmailID = EMAILDATAVALUES.EmailID 
			join GroupDataFields WITH (NOLOCK) on EMAILDATAVALUES.groupDataFieldsID = GroupDataFields.groupDataFieldsID AND GroupDataFields.GroupID = Groups.GroupID AND GroupDataFields.datafieldsetID = @DatafieldSetID
		where 	
			Groups.groupID = @GroupID 
			and Groups.CustomerID = @CustomerID 
			and  GroupDataFields.IsDeleted = 0
		group by
			Emails.EmailID, 
			GroupDatafields.ShortName, 
			GroupDatafields.groupDataFieldsID,
			EMAILDATAVALUES.DataValue, 
			EntryID
		
		Insert Into #Email (EmailID, EntryID)
		select 	distinct Emails.EmailID,EntryID
		from 
			Groups WITH (NOLOCK) 
			join EmailGroups WITH (NOLOCK) on Groups.groupID = EmailGroups.groupID 
			join Emails WITH (NOLOCK) on Emails.EmailID = EmailGroups.EmailID and Emails.EmailID = @UDFEmailID join 
			EMAILDATAVALUES WITH (NOLOCK) on Emails.EmailID = EMAILDATAVALUES.EmailID join
			GroupDataFields WITH (NOLOCK) on EMAILDATAVALUES.groupDataFieldsID = GroupDataFields.groupDataFieldsID AND GroupDataFields.GroupID = Groups.GroupID
			AND GroupDataFields.datafieldsetID = @DatafieldSetID
		where 	
			Groups.groupID = @GroupID 
			and Groups.CustomerID = @CustomerID 
			and GroupDataFields.IsDeleted = 0 

		OPEN UDF_CUSTOMVALUES
		
		FETCH NEXT FROM UDF_CUSTOMVALUES INTO @EmailID, @ShortName, @GroupDataFieldsID, @Datavalue, @EntryID,@LastModifiedDate
		WHILE (@@fetch_status <> -1)
		BEGIN
			IF (@@fetch_status <> -2)
			BEGIN
					-- removed /  (case when isDate(@Datavalue) = 1 THEN convert(varchar(10),isnull(Convert(datetime,@DataValue),''),101) else @DataValue END)-- converts 4 digit number to date.
					--- 4444 to 01/01/4444
					
					
					set @sqlQuery = 'update #Email set [' + @ShortName + '] = ''' +  Replace(@DataValue,'''','''''') + ''', LastModifiedDate = ''' + @LastModifiedDate + ''' where EntryID= ''' + @EntryID + ''''
					
					--print (@sqlquery)
					exec (@sqlquery)
	
			END
			FETCH NEXT FROM UDF_CUSTOMVALUES INTO @EmailID, @ShortName, @GroupDataFieldsID, @Datavalue, @EntryID,@LastModifiedDate
		END
		
		CLOSE UDF_CUSTOMVALUES
		DEALLOCATE UDF_CUSTOMVALUES
	
	END
	
	declare @FilterQry varchar(4000)
 
	Begin

		Alter table #Email drop column EntryID
		set @FilterQry = 
				'select #Email.*
				from 
					#Email 
					join Emails WITH (NOLOCK) on #Email.EmailID = Emails.EmailID AND Emails.EmailID = '+@UDFEmailID+'
					join EmailGroups WITH (NOLOCK) on EmailGroups.EmailID = Emails.EmailID
				where 
					EmailGroups.GroupID = ' + convert(varchar(10),@GroupID) +  
				'  order by 
					#Email.EmailID '
		--print (@FilterQry)
		exec (@FilterQry)
	end
	drop table #Email
END