create procedure e_ValueOption_SaveBulkXml
@xml xml
as
BEGIN
	set nocount on  	

	declare @docHandle int
    declare @insertcount int
    
	declare @importVO table    
	(  
		[option_id] [int] NOT NULL,
		[field_id] [int] NULL,
		[account_id] [int] NOT NULL,
		[value] [varchar](50) NULL,
		[display_as] [varchar](50) NULL,
		[disqualifier] [bit] NULL,
		[active] [bit] NULL,
		[order] [int] NULL,
		[KMCodeSheetID] int null,
		[KMProductCode]	VARCHAR(50) NULL,
		[KMProductId] INT NULL
	)
	exec sp_xml_preparedocument @docHandle output, @xml  
	insert into @importVO 
	(
		option_id,field_id,account_id,value,display_as,disqualifier,active,[order],KMCodeSheetID,KMProductCode,KMProductId
	)  
	
	select option_id,field_id,account_id,value,display_as,disqualifier,active,[order],KMCodeSheetID,KMProductCode,KMProductId
	from openxml(@docHandle,N'/XML/ValueOption')
	with
	(
		option_id int 'option_id',
		field_id int 'field_id',
		account_id int 'account_id',
		value varchar(50) 'value',
		display_as varchar(50) 'display_as',
		disqualifier bit 'disqualifier',
		active bit 'active',
		[order] int 'order',
		KMCodeSheetID int 'KMCodeSheetID',
		KMProductCode	varchar(50) 'KMProductCode',
		KMProductId int 'KMProductId'
	)
	
	exec sp_xml_removedocument @docHandle

	insert into ValueOption(option_id,field_id,account_id,value,display_as,disqualifier,active,[order],KMCodeSheetID,KMProductCode,KMProductId)
	select i.option_id,i.field_id,i.account_id,i.value,i.display_as,i.disqualifier,i.active,i.[order],i.KMCodeSheetID,i.KMProductCode,i.KMProductId
	from @importVO i
	left join ValueOption x on i.option_id = x.option_id
	where x.option_id is null

	update x
	set x.field_id = i.field_id,
		x.account_id = i.account_id,
		x.value = i.value,
		x.display_as = i.display_as,
		x.disqualifier = i.disqualifier,
		x.active = i.active,
		x.[order] = i.[order],
		x.KMCodeSheetID = case when i.KMCodeSheetID > 0 then i.KMCodeSheetID else x.KMCodeSheetID end,
		x.KMProductCode = case when len(i.KMProductCode) > 0 then i.KMProductCode else x.KMProductCode end,
		x.KMProductId = case when i.KMProductId > 0 then i.KMProductId else x.KMProductCode end
	from ValueOption x
	join @importVO i on i.option_id = x.option_id

END
go