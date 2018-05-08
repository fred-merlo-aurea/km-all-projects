CREATE PROCEDURE [dbo].[sp_GetUDFDataValues]
	(
		@GroupID int,
		@CustomerID int,
		@UDFEmailID varchar(10),
		@DatafieldSetID int
	)
AS

BEGIN
	set NOCOUNT ON
	
	/* Local Variables */
	DECLARE @FieldName varchar(250)
	
	DECLARE @EmailID int,
		@ShortName varchar(50),
		@GroupDataFieldsID int,
		@Datavalue varchar(500),
		@strColumns varchar(500),
		@strValues varchar(500),
		@sqlQuery varchar(8000),
		@EntryID varchar(50)
	
	/* 
	CREATE TEMP TABLE WITH EMAIL COLUMNS 
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
	insert into @tblGroupField (GroupDataFields, GroupFieldName)
	select GroupDataFieldsID, ShortName from GroupDataFields JOIN Groups on GroupDataFields.GroupID = Groups.GroupID 
	where Groups.groupID = @GroupID and GroupDataFields.datafieldsetID = @DatafieldSetID and GroupDataFields.IsDeleted = 0
	
	/* if no UDF's exists return EmailID from Emails table */
	if @@RowCount = 0
	Begin
		insert into #Email
		(
			EmailID, EntryID
		)
		select 	distinct Emails.EmailID, null
		from 
			Groups join EmailGroups on Groups.groupID = EmailGroups.groupID join
			Emails on Emails.EmailID = EmailGroups.EmailID 
		where 	
			Groups.groupID = @GroupID and Groups.CustomerID = @CustomerID
		order by Emails.EmailID
	
	End
	Else
	Begin
		/* if UDF's exists*/
	
		/* CURSOR TO CREATE THE TABLE WITH ALL COLUMNS */
		DECLARE UDF_GROUPS CURSOR
		READ_ONLY
		FOR SELECT 'Alter table #Email ADD [' + SHORTNAME + '] varchar(500) NULL' FROM GROUPDATAFIELDS WHERE GROUPID = @GroupID and GroupDataFields.datafieldsetID = @DatafieldSetID and IsDeleted = 0
		
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
		
		/* Insert records from Email Table - which does't have any records in EmailDataValues */
		insert into #Email (EmailID)
		select 	Emails.EmailID
		from 
			Groups join EmailGroups on Groups.groupID = EmailGroups.groupID join
			Emails on Emails.EmailID = EmailGroups.EmailID and Emails.EmailID = @UDFEmailID
		where 	
			Groups.groupID = @GroupID and Groups.CustomerID = @CustomerID
			and Emails.EmailID not in 
			(
				select distinct EmailID  
				from EmailDataValues join GroupDataFields on EmailDataValues.GroupDataFieldsid = GroupDataFields.GroupDataFieldsID 
					and GroupDataFields.datafieldsetID = @DatafieldSetID and EmailDataValues.EmailID = @UDFEmailID join
					Groups on Groups.GroupID = GroupDataFields.GroupID
				where Groups.groupID = @GroupID and Groups.CustomerID = @CustomerID and GroupDataFields.IsDeleted = 0
			)



		/* CURSOR TO LOAD THE CUSTOM COLUMN VALUES IN TEMPORARY TABLE */
		DECLARE UDF_CUSTOMVALUES CURSOR
		READ_ONLY
		FOR 
		select 	Emails.EmailID, 
			GroupDatafields.ShortName, 
			GroupDatafields.groupDataFieldsID,
			EmailDataValues.DataValue as datavalue, EntryID
		from 
			Groups join EmailGroups on Groups.groupID = EmailGroups.groupID join
			Emails on Emails.EmailID = EmailGroups.EmailID and Emails.EmailID = @UDFEmailID join 
			EmailDataValues on Emails.EmailID = EmailDataValues.EmailID join
			GroupDataFields on EmailDataValues.groupDataFieldsID = GroupDataFields.groupDataFieldsID AND GroupDataFields.GroupID = Groups.GroupID
			AND GroupDataFields.datafieldsetID = @DatafieldSetID
		where 	
			Groups.groupID = @GroupID and Groups.CustomerID = @CustomerID and GroupDataFields.IsDeleted = 0
		
		Insert Into #Email (EmailID, EntryID)
		select 	distinct Emails.EmailID,EntryID
		from 
			Groups join EmailGroups on Groups.groupID = EmailGroups.groupID join
			Emails on Emails.EmailID = EmailGroups.EmailID and Emails.EmailID = @UDFEmailID join 
			EmailDataValues on Emails.EmailID = EmailDataValues.EmailID join
			GroupDataFields on EmailDataValues.groupDataFieldsID = GroupDataFields.groupDataFieldsID AND GroupDataFields.GroupID = Groups.GroupID
			AND GroupDataFields.datafieldsetID = @DatafieldSetID
		where 	
			Groups.groupID = @GroupID and Groups.CustomerID = @CustomerID and GroupDataFields.IsDeleted = 0

		OPEN UDF_CUSTOMVALUES
		
		FETCH NEXT FROM UDF_CUSTOMVALUES INTO @EmailID, @ShortName, @GroupDataFieldsID, @Datavalue, @EntryID
		WHILE (@@fetch_status <> -1)
		BEGIN
			IF (@@fetch_status <> -2)
			BEGIN
					-- removed /  (case when isDate(@Datavalue) = 1 THEN convert(varchar(10),isnull(Convert(datetime,@DataValue),''),101) else @DataValue END)-- converts 4 digit number to date.
					--- 4444 to 01/01/4444
					
					
					set @sqlQuery = 'update #Email set [' + @ShortName + '] = ''' +  Replace(@DataValue,'''','''''') + ''' where EntryID= ''' + @EntryID + ''''
					--print (@sqlquery)
					exec (@sqlquery)
	
			END
			FETCH NEXT FROM UDF_CUSTOMVALUES INTO @EmailID, @ShortName, @GroupDataFieldsID, @Datavalue, @EntryID
		END
		
		CLOSE UDF_CUSTOMVALUES
		DEALLOCATE UDF_CUSTOMVALUES
	
	END
	
	declare @FilterQry varchar(4000)
 
	Begin

		Alter table #Email drop column EntryID
		set @FilterQry = 
				'select #Email.*
				from #Email join Emails on #Email.EmailID = Emails.EmailID AND Emails.EmailID = '+@UDFEmailID+'
					join EmailGroups on EmailGroups.EmailID = Emails.EmailID
				where EmailGroups.GroupID = ' + convert(varchar(10),@GroupID) +  
				' order by #Email.EmailID '
		--print (@FilterQry)
		exec (@FilterQry)
	end
	drop table #Email
END