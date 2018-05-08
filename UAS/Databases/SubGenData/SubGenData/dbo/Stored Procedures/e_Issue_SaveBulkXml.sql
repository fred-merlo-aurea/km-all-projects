create procedure e_Issue_SaveBulkXml
@xml xml
as
BEGIN
	set nocount on  	

	declare @docHandle int
    declare @insertcount int
    
	declare @import table    
	(  
		[issue_id] [int] NOT NULL,
		[account_id] [int] NOT NULL,
		[name] [varchar](50) NULL,
		[date] [datetime] NULL
	)
	exec sp_xml_preparedocument @docHandle output, @xml  
	insert into @import 
	(
		issue_id,account_id,name,date
	)  
	
	select issue_id,account_id,name,date
	from openxml(@docHandle,N'/XML/Issue')
	with
	(
		issue_id int 'issue_id',
		account_id int 'account_id',
		name varchar(50) 'name',
		date datetime 'date'
	)
	
	exec sp_xml_removedocument @docHandle

	update @import
	set name = replace(name, '&amp;', '&')

	update @import
	set name = replace(name, '&quot;', '"' )

	update @import
	set name = replace(name, '&lt;', '<')

	update @import
	set name = replace(name, '&gt;', '>')

	update @import
	set name = replace(name, '&apos;', '''')


	insert into Issue(issue_id,account_id,name,date)
	select i.issue_id,i.account_id,i.name,i.date
	from @import i
	left join Issue x on i.issue_id = x.issue_id
	where x.issue_id is null

	update x
	set x.account_id = i.account_id,
		x.name = i.name,
		x.date = i.date
	from Issue x
	join @import i on i.issue_id = x.issue_id

END
go