create procedure e_CustomField_SaveBulkXml
@xml xml
as
BEGIN

	set nocount on  	

	declare @docHandle int
    declare @insertcount int
    
	declare @import table    
	(  
		[field_id] [int] NOT NULL,
		[account_id] [int] NOT NULL,
		[name] [varchar](255) NULL,
		[display_as] [varchar](255) NULL,
		[type] [varchar](50) NULL,
		[allow_other] [bit] NULL,
		[text_value] [varchar](255) NULL,
		[KMResponseGroupID] int null,
		[KMProductCode]	VARCHAR(50) NULL,
		[KMProductId] INT NULL
	)
	exec sp_xml_preparedocument @docHandle output, @xml  
	insert into @import 
	(
		field_id,account_id,name,display_as,type,allow_other,text_value,KMResponseGroupID,KMProductCode,KMProductId
	)  
	
	select field_id,account_id,name,display_as,type,allow_other,text_value,KMResponseGroupID,KMProductCode,KMProductId
	from openxml(@docHandle,N'/XML/CustomField')
	with
	(
		field_id int 'field_id',
		account_id int 'account_id',
		name varchar(255) 'name',
		display_as varchar(255) 'display_as',
		type varchar(50) 'type',
		allow_other bit 'allow_other',
		text_value varchar(255) 'text_value',
		KMResponseGroupID int 'KMResponseGroupID',
		KMProductCode	varchar(50) 'KMProductCode',
		KMProductId int 'KMProductId'
	)
	
	exec sp_xml_removedocument @docHandle

	update @import
	set name = dbo.RevertXmlFormatting(name)

	update @import
	set display_as = dbo.RevertXmlFormatting(display_as)

	update @import
	set type = dbo.RevertXmlFormatting(type)

	update @import
	set text_value = dbo.RevertXmlFormatting(text_value)

	insert into CustomField(field_id,account_id,name,display_as,type,allow_other,text_value,KMResponseGroupID,KMProductCode,KMProductId)
	select i.field_id,i.account_id,i.name,i.display_as,i.type,i.allow_other,i.text_value,i.KMResponseGroupID,i.KMProductCode,i.KMProductId
	from @import i
	left join CustomField x on i.field_id = x.field_id
	where x.field_id is null

	update x
	set x.account_id = i.account_id,
		x.name = i.name,
		x.display_as = i.display_as,
		x.type = i.type,
		x.allow_other = i.allow_other,
		x.text_value = i.text_value,
		x.KMResponseGroupID = case when i.KMResponseGroupID > 0 then i.KMResponseGroupID else x.KMResponseGroupID end,
		x.KMProductCode = case when len(i.KMProductCode) > 0 then i.KMProductCode else x.KMProductCode end,
		x.KMProductId = case when i.KMProductId > 0 then i.KMProductId else x.KMProductCode end
	from CustomField x
	join @import i on i.field_id = x.field_id


	--------------ValueOption
	declare @importVO table    
	(  
		[option_id] [int] NOT NULL,
		[field_id] [int] NULL,
		[account_id] [int] NOT NULL,
		[value] [varchar](255) NULL,
		[display_as] [varchar](255) NULL,
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
	from openxml(@docHandle,N'/XML/CustomField/value_options/ValueOption')
	with
	(
		option_id int 'option_id',
		field_id int 'field_id',
		account_id int 'account_id',
		value varchar(255) 'value',
		display_as varchar(255) 'display_as',
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
		x.KMProductId = case when i.KMProductId > 0 then i.KMProductId else x.KMProductId end
	from ValueOption x
	join @importVO i on i.option_id = x.option_id

END
go