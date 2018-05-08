CREATE  PROCEDURE [dbo].[sp_Adhoc_Select]   
AS  
BEGIN
	
	SET NOCOUNT ON

	select case when t.name like '%date%' then 'd|[' +  upper(c.name) + ']' 
		when t.name like '%bit%' then 'b|[' +  upper(c.name) + ']'  
		when t.name like '%int%' then 'i|[' +  upper(c.name) + ']' 
		when t.name like '%float%' then 'f|[' +  upper(c.name) + ']' else '[' +  upper(c.name) + ']' end  as columnValue , 
		upper(c.name) as displayName, t.name as columnType, c.name as columnName
	from sysobjects s WITH(NOLOCK)
		join syscolumns c WITH(NOLOCK) on s.ID = c.ID 
		inner join systypes t WITH(NOLOCK) on c.xtype = t.xtype 
	where s.name = 'subscriptions' and (t.name = 'varchar' or t.name like '%datetime%' or (t.name like '%int%' and c.name in ('score')) or t.name like '%float%') and c.name not in ('DEMO7','VERIFIED','MAILSTOP','PAR3C','EMPLOY','PLUS4','STATE', 'COUNTRY','COUNTY', 'FORZIP') and 
		c.name not in (select AdhocName from Adhoc WITH(NOLOCK))  
	union
		select 'm|' + CONVERT(varchar(3),MasterGroupID)  as columnValue ,
			upper(m.Displayname) as displayName, 'varchar' as columnType, ColumnReference as columnName  
		from MasterGroups m  WITH(NOLOCK)
		where m.EnableAdhocSearch = 1 and m.Displayname not in (select AdhocName from Adhoc WITH(NOLOCK))   
	union
		select 'e|' + StandardField  + '|' +
		case when CustomFieldDataType like '%date%' then 'd' 
			 when CustomFieldDataType like '%bit%' then 'b' 
			 when CustomFieldDataType like '%int%' then 'i' 
			 when CustomFieldDataType like '%float%' then 'f'
			 else '' 
		end
		as ColumnValue 
			,upper(CustomField) as displayName
			,CustomFieldDataType as columnType
			,StandardField as columnName  
		from SubscriptionsExtensionMapper map  WITH(NOLOCK)
		where map.Active = 1
			and map.CustomField not in (select AdhocName from Adhoc WITH(NOLOCK))  		
	union
		select 'i|[PRODUCTCOUNT]', 'PRODUCTCOUNT', 'int', 'PRODUCTCOUNT'		
		
	order by columnName

End