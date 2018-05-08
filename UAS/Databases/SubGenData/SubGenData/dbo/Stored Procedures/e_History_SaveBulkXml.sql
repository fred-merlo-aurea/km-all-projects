create procedure e_History_SaveBulkXml
@xml xml
as
BEGIN
	set nocount on  	

	declare @docHandle int
    declare @insertcount int
    
	declare @import table    
	(  
		[history_id] [int] NOT NULL,
		[account_id] [int] NOT NULL,
		[user_id] [int] NULL,
		[notes] [varchar](50) NULL,
		[date] [datetime] NULL,
		[subscriber_id] [int] NULL
	)
	exec sp_xml_preparedocument @docHandle output, @xml  
	insert into @import 
	(
		history_id,account_id,user_id,notes,date,subscriber_id
	)  
	
	select history_id,account_id,user_id,notes,date,subscriber_id
	from openxml(@docHandle,N'/XML/History')
	with
	(
		history_id int 'history_id',
		account_id int 'account_id',
		user_id int 'user_id',
		notes varchar(50) 'notes',
		date datetime 'date',
		subscriber_id int 'subscriber_id'
	)
	
	exec sp_xml_removedocument @docHandle

	update @import
	set notes = replace(notes, '&amp;', '&')

	update @import
	set notes = replace(notes, '&quot;', '"' )

	update @import
	set notes = replace(notes, '&lt;', '<')

	update @import
	set notes = replace(notes, '&gt;', '>')

	update @import
	set notes = replace(notes, '&apos;', '''')

	insert into History(history_id,account_id,user_id,notes,date,subscriber_id)
	select i.history_id,i.account_id,i.user_id,i.notes,i.date,i.subscriber_id
	from @import i
	left join History x on i.history_id = x.history_id
	where x.history_id is null

	update x
	set x.account_id = i.account_id,
		x.user_id = i.user_id,
		x.notes = i.notes,
		x.date = i.date,
		x.subscriber_id = i.subscriber_id
	from History x
	join @import i on i.history_id = x.history_id

END
go