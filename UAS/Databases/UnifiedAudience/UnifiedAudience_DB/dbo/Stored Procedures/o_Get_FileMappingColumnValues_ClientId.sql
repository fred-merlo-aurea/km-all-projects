create procedure o_Get_FileMappingColumnValues_ClientId
@clientId int = 0
as
	begin
		set nocount on
		declare @ruleField table
		(
			rfId int,
			DataTable varchar(75),
			TablePrefix varchar(5),
			ColumnName varchar(50),
			DataType varchar(50),
			IsDemographic bit,
			IsDemographicOther bit,
			IsMultiSelect bit,
			ClientId int,
			UxControl varchar(50)
		)
		insert into @ruleField (rfId,DataTable,TablePrefix,ColumnName,DataType,IsDemographic,IsDemographicOther,IsMultiSelect,ClientId,UxControl) exec o_Get_FileMappingColumns_ClientId @clientId
		
		select
			x.rfId as 'Id',
			p.PubCode as 'ItemText',
			cast(p.PubID as varchar(50)) as 'ItemValue',
		case when p.SortOrder = 0 or p.SortOrder is null then row_number()over(ORDER BY p.PubTypeID,p.PubCode) else p.SortOrder end as 'ItemOrder'
		from Pubs p
		join @ruleField x on p.ClientID = x.ClientId
		where x.DataTable='PubSubscriptions' and x.ColumnName='PUBCODE'
		union
		--****** ResponseGroups *********
		select distinct 
					x.rfId as 'Id',
					cs.Responsedesc as 'ItemText',
					cs.Responsedesc as 'ItemValue',
		 isnull(cs.DisplayOrder,0) as 'ItemOrder'
		from ResponseGroups rg with(nolock)
		join CodeSheet cs with(nolock) on rg.ResponseGroupId = cs.ResponseGroupId 
		join @ruleField x on rg.ResponseGroupName = x.ColumnName
		where x.DataTable='ResponseGroups' 
	end
go

