CREATE PROCEDURE [dbo].[sp_PreviewFilteredEmails]
(
	@GroupID int,
	@CustomerID int,
	@Filter varchar(8000)
)
AS
BEGIN
	set NOCOUNT ON
	declare 
	  @Col1 varchar(8000),
	  @Col2 varchar(8000),
	  @SqlString  varchar(8000)
	
	set @SqlString = ''
	set @Col1  = ''
	set @Col2  = ''

	create table #g(
		GID int,
		ShortName	varchar(50)
		)


	create table #E(EmailID int, GID int, DataValue varchar(500), EntryID uniqueidentifier)        
	CREATE UNIQUE CLUSTERED INDEX E_ind on  #E(EmailID,EntryID,GID) with ignore_dup_key	


	if len(rtrim(ltrim(@Filter))) = 0
	begin
		insert into #g 
		select GroupDatafieldsID, ShortName from groupdatafields
		where GroupDatafields.groupID = @GroupID
	end
	else
	begin
		insert into #g 
		select distinct gdf.GroupDatafieldsID, gdf.ShortName
		from 	Groups join 
				GroupDataFields gdf on gdf.GroupID = Groups.GroupID 
		where 	Groups.groupID = @GroupID and 
				Groups.customerID = @CustomerID and
			  	@Filter like '%' + ShortName + '%'

	end

 	if not exists (select GroupDatafieldsID from GroupDataFields JOIN Groups on GroupDataFields.GroupID = Groups.GroupID where Groups.groupID = @GroupID) or not exists(select * from #g)
	Begin

  		exec (	' select distinct Emails.EmailID, EmailAddress ' +
			  	' from  Emails join EmailGroups on EmailGroups.EmailID = Emails.EmailID ' +
		     	' where Emails.CustomerID = ' + @CustomerID + ' and Emails.EmailAddress like ''%@%.%'' and EmailGroups.SubscribeTypeCode = ''S''' +
		     	' and EmailGroups.GroupID = ' + @GroupID + 
				' ' + @Filter + ' order by Emails.EmailID ')
	End
	Else
	Begin
		/* if UDF's exists*/
		select  @Col1  = @Col1 + coalesce('max([' + RTRIM(ShortName)  + ']) as ''' + RTRIM(ShortName)  + ''',',''),
			@Col2 = @Col2 + coalesce('case when E.GID = ' + convert(varchar(10),G.GID) + ' then E.DataValue else null end as [' + ShortName  + '],', '')
		from #G G
		
		set @Col1 = substring(@Col1,0,len(@Col1))
		set @Col2 = substring(@Col2,0,len(@Col2))

		insert into #E 
		select EmailDataValues.EmailID, EmailDataValues.GroupDataFieldsID, DataValue, EntryID
		from EmailDataValues join #G on #G.GID = EmailDataValues.GroupDataFieldsID

		exec (	' select distinct Emails.EmailID, EmailAddress  from Emails left outer join ' +
				' ( select InnerTable1.EmailID as tmp_EmailID, InnerTable1.EntryID, ' + @Col1 + ' from ' +
				' ( select Emails.EmailID, E.EntryID, ' + @Col2 + 
				' from EmailGroups join  Emails on Emails.EmailID = EmailGroups.EmailID join '+
				' #E E on Emails.EmailID = E.EmailID '+
				' where emailgroups.groupID = ' + @GroupID + ' and EmailGroups.SubscribeTypeCode = ''S'') as InnerTable1 ' + 
				' Group by InnerTable1.EmailID, InnerTable1.EntryID ' +
				' ) as InnerTable2 on Emails.EmailID = InnerTable2.tmp_EmailID ' +
				' join EmailGroups on EmailGroups.EmailID = Emails.EmailID ' +
				' where Emails.CustomerID = ' + @CustomerID + ' and Emails.EmailAddress like ''%@%.%'' and EmailGroups.SubscribeTypeCode = ''S'' ' +
				' and EmailGroups.GroupID = ' + @GroupID + @Filter + ' order by EmailAddress ')	 

	END

	drop table #g
	drop table #E
END
