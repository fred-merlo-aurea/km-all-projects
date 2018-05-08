create procedure e_Publication_SaveBulkXml
@xml xml
as
BEGIN

	set nocount on  	

	declare @docHandle int
    declare @insertcount int
    
	declare @import table    
	(  
		[publication_id] [int] NOT NULL,
		[account_id] [int] NOT NULL,
		[name] [varchar](50) NULL,
		[issues_per_year] [int] NULL,
		[KMPubCode] [varchar](50) NULL,
		[KMPubID] [int] NULL
	)
	exec sp_xml_preparedocument @docHandle output, @xml  
	insert into @import 
	(
		publication_id,account_id,name,issues_per_year,KMPubCode,KMPubID
	)  
	
	select publication_id,account_id,name,issues_per_year,KMPubCode,KMPubID
	from openxml(@docHandle,N'/XML/Publication')
	with
	(
		publication_id int 'publication_id',
		account_id int 'account_id',
		name varchar(50) 'name',
		issues_per_year int 'issues_per_year',
		KMPubCode varchar(50) 'KMPubCode',
		KMPubID int 'KMPubID'
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


	insert into Publication(publication_id,account_id,name,issues_per_year,KMPubCode,KMPubID)
	select i.publication_id,i.account_id,i.name,i.issues_per_year,i.KMPubCode,i.KMPubID
	from @import i
	left join Publication x on i.publication_id = x.publication_id
	where x.publication_id is null

	update x
	set x.account_id = i.account_id,
		x.name = i.name,
		x.issues_per_year = i.issues_per_year,
		x.KMPubCode = i.KMPubCode,
		x.KMPubID = i.KMPubID
	from Publication x
	join @import i on i.publication_id = x.publication_id

END
go