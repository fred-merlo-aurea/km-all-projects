create procedure job_RuleFieldPredefinedValue_Setup
@xml xml,
@clientId int
as
	begin
		set nocount on
		declare @docHandle int
		create table #rfXML
		(
			RuleFieldId int,
			ClientId int,
			DataTable varchar(75),
			Field varchar(50)
		)
		create index EA_1 ON #rfXML (RuleFieldId)

		exec sp_xml_preparedocument @docHandle OUTPUT, @xml  

		insert into #rfXML (RuleFieldId,ClientId,DataTable,Field)  
		select  RuleFieldId,ClientId,DataTable,Field
		from openxml(@docHandle, N'/XML/RuleFieldNeedValue')   
		with   
		( 
			RuleFieldId int 'RuleFieldId',
			ClientId int 'ClientId', 
			DataTable varchar(75) 'DataTable',
			Field varchar(50) 'Field'
		)
		where ClientId = @clientId

		exec sp_xml_removedocument @docHandle  

		--CLIENT DBs need sproc for inserting values for RespoonseGroups
		--****** PUBCODE	VARCHAR  *********
		update Pubs
		set ClientID = @clientId
		where ClientId is null or ClientId = 0

		select 0 as 'RuleFieldPredefinedValueId',
			x.RuleFieldId as 'RuleFieldId',
			p.PubCode as 'ItemText',
			cast(p.PubID as varchar(50)) as 'ItemValue',
		case when p.SortOrder = 0 or p.SortOrder is null then row_number()over(ORDER BY p.PubTypeID,p.PubCode) else p.SortOrder end as 'ItemOrder',
		'true' as 'IsActive',
		getdate() as 'CreatedDate',
		1 as 'CreatedByUserId',
		1 as 'UpdatedByUserId'
		from Pubs p
		join #rfXML x on p.ClientID = x.ClientId
		where x.DataTable='PubSubscriptions' and x.Field='PUBCODE'

		union
		--****** ResponseGroups *********
		select distinct 0 as 'RuleFieldPredefinedValueId',
					x.RuleFieldId as 'RuleFieldId',
					cs.Responsedesc as 'ItemText',
					cs.Responsedesc as 'ItemValue',
		 0 as 'ItemOrder',
		'true' as 'IsActive',getdate() as 'CreatedDate',1 as 'CreatedByUserId',1 as 'UpdatedByUserId'
		from ResponseGroups rg with(nolock)
		join CodeSheet cs with(nolock) on rg.ResponseGroupId = cs.ResponseGroupId 
		join #rfXML x on rg.ResponseGroupName = x.Field
		where x.DataTable='ResponseGroups' 

		drop table #rfXML
		--will return back a datatable for inserting to UAS..RuleFieldPredefinedValue
	end
go

