CREATE PROCEDURE [dbo].[e_Adhoc_Select_CategoryID]
	@CategoryID int,
	@BrandID int,
	@PubID int
AS
BEGIN

	set nocount on

	if @CategoryID = 0
		begin	

			if(@PubID != 0)
				Begin
					--standardfields, SubscriptionsExtensionMapper
					select 
						case when t.name like '%date%' then 'd|[' +  upper(c.name) + ']' 
						when t.name like '%bit%' then 'b|[' +  upper(c.name) + ']'  
						when t.name like '%int%' then 'i|[' +  upper(c.name) + ']' 
						when t.name like '%float%' then 'f|[' +  upper(c.name) + ']' else '[' +  upper(c.name) + ']' end  as ColumnValue , 
						upper(c.name) as DisplayName, 
						t.name as ColumnType, 
						c.name as ColumnName
					from sysobjects s  WITH (NOLOCK)
						join syscolumns c WITH (NOLOCK) on s.ID = c.ID  
						inner join systypes t WITH (NOLOCK) on c.xtype = t.xtype 
					where s.name = 'subscriptions' and (t.name = 'varchar' or t.name like '%datetime%' or (t.name like '%int%' and c.name in ('score'))  or t.name like '%float%') and c.name not in ('DEMO7','VERIFIED','MAILSTOP','PAR3C','EMPLOY','PLUS4', 'FORZIP', 'PUBIDs', 'NOTES','IGRP_RANK','CGRP_RANK','PRIORITY') and 
						c.name not in (select AdhocName from Adhoc WITH (NOLOCK))  
					union
						select 'e|' + StandardField  + '|' +
							case when CustomFieldDataType like '%date%' then 'd' 
								 when CustomFieldDataType like '%bit%' then 'b' 
								 when CustomFieldDataType like '%int%' then 'i' 
								 when CustomFieldDataType like '%float%' then 'f'
								 else '' 
							end
							as ColumnValue, 
							upper(CustomField) as displayName,
							CustomFieldDataType as columnType,
							StandardField as columnName  
						from SubscriptionsExtensionMapper map  WITH (NOLOCK)
						where map.Active = 1 and map.CustomField not in (select AdhocName from Adhoc WITH (NOLOCK))  
					order by columnName

				End
			else
				Begin
					--standardfields, SubscriptionsExtensionMapper, MasterGroups
				   if @BrandID = 0
					   begin
 							select case when t.name like '%date%' then 'd|[' +  upper(c.name) + ']' 
								when t.name like '%bit%' then 'b|[' +  upper(c.name) + ']'  
								when t.name like '%int%' then 'i|[' +  upper(c.name) + ']' 
								when t.name like '%float%' then 'f|[' +  upper(c.name) + ']' else '[' +  upper(c.name) + ']' end  as ColumnValue , 
								upper(c.name) as DisplayName, t.name as ColumnType, c.name as ColumnName
							from sysobjects s WITH (NOLOCK)
								join syscolumns c WITH (NOLOCK) on s.ID = c.ID 
								inner join systypes t WITH (NOLOCK) on c.xtype = t.xtype 
							where s.name = 'subscriptions' and (t.name = 'varchar' or t.name like '%datetime%' or (t.name like '%int%' and c.name in ('score'))  or t.name like '%float%') and c.name not in ('DEMO7','VERIFIED','MAILSTOP','PAR3C','EMPLOY','PLUS4', 'FORZIP', 'PUBIDs', 'NOTES','IGRP_RANK','CGRP_RANK','PRIORITY') and 
								c.name not in (select AdhocName from Adhoc WITH (NOLOCK))  
							union
								select 'm|' + CONVERT(varchar(3),m.MasterGroupID)  as columnValue ,
									upper(m.Displayname) as displayName, 'varchar' as columnType, ColumnReference as columnName  
								from MasterGroups m  WITH (NOLOCK)
								where m.EnableAdhocSearch = 1 and m.Displayname not in (select AdhocName from Adhoc WITH (NOLOCK))   
							union
								select 'e|' + StandardField  + '|' +
									case when CustomFieldDataType like '%date%' then 'd' 
										 when CustomFieldDataType like '%bit%' then 'b' 
										 when CustomFieldDataType like '%int%' then 'i' 
										 when CustomFieldDataType like '%float%' then 'f'
										 else '' 
									end
									as ColumnValue, 
									upper(CustomField) as displayName,
									CustomFieldDataType as columnType,
									StandardField as columnName  
								from SubscriptionsExtensionMapper map  WITH (NOLOCK)
								where map.Active = 1 and map.CustomField not in (select AdhocName from Adhoc  WITH (NOLOCK)) 
							union
								select top 1 'i|[PRODUCT COUNT]', 'PRODUCT COUNT', 'int', 'PRODUCT COUNT' 
								where not exists (select adhocname from Adhoc WITH (NOLOCK) where AdhocName = 'PRODUCT COUNT') 					 
							order by 
								columnName
						end
					else
						begin
							select case when t.name like '%date%' then 'd|[' +  upper(c.name) + ']' 
								when t.name like '%bit%' then 'b|[' +  upper(c.name) + ']'  
								when t.name like '%int%' then 'i|[' +  upper(c.name) + ']' 
								when t.name like '%float%' then 'f|[' +  upper(c.name) + ']' else '[' +  upper(c.name) + ']' end  as ColumnValue , 
								upper(c.name) as DisplayName, t.name as ColumnType, c.name as ColumnName
							from sysobjects s  WITH (NOLOCK)
								join syscolumns c WITH (NOLOCK) on s.ID = c.ID
								inner join systypes t WITH (NOLOCK) on c.xtype = t.xtype
							where s.name = 'subscriptions' and (t.name = 'varchar' or t.name like '%datetime%' or (t.name like '%int%' and c.name in ('score'))  or t.name like '%float%') and c.name not in ('DEMO7','VERIFIED','MAILSTOP','PAR3C','EMPLOY','PLUS4', 'FORZIP', 'PUBIDs', 'NOTES','IGRP_RANK','CGRP_RANK','PRIORITY') and 
								c.name not in (select AdhocName from Adhoc WITH (NOLOCK))  
							union
								SELECT distinct 'm|' + CONVERT(varchar(3),mg.MasterGroupID)  as columnValue,
										upper(mg.Displayname) as displayName, 'varchar' as columnType, 
										mg.ColumnReference as columnName  
								From vw_Mapping v WITH (NOLOCK)
									join MasterGroups mg WITH (NOLOCK) on mg.MasterGroupID = v.MasterGroupID 
									join branddetails bd WITH (NOLOCK) on bd.pubid = v.pubid  
									join Brand b WITH (NOLOCK) on b.BrandID = bd.BrandID
								WHERE mg.EnableAdhocSearch = 1 and mg.Displayname not in (select AdhocName from Adhoc  WITH (NOLOCK))and bd.brandid = @BrandID and b.IsDeleted = 0
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
								from SubscriptionsExtensionMapper map  WITH (NOLOCK)
								where map.Active = 1
									and map.CustomField not in (select AdhocName from Adhoc WITH (NOLOCK))  
							union
								select top 1 'i|[PRODUCT COUNT]', 'PRODUCT COUNT', 'int', 'PRODUCT COUNT' 
								where  not exists (select adhocname from Adhoc WITH (NOLOCK) where AdhocName = 'PRODUCT COUNT')				
							order by columnName	
						end
				end
		end
	else
		begin
			if(@PubID != 0)
				Begin
					--standardfields, SubscriptionsExtensionMapper
					select case when t.name like '%date%' then 'd|[' +  upper(c.name) + ']' 
						when t.name like '%bit%' then 'b|[' +  upper(c.name) + ']'  
						when t.name like '%int%' then 'i|[' +  upper(c.name) + ']' 
						when t.name like '%float%' then 'f|[' +  upper(c.name) + ']' else '[' +  upper(c.name) + ']' end  as ColumnValue , 
						upper(c.name) as DisplayName, t.name as ColumnType, c.name as ColumnName, a.AdhocName,  a.SortOrder 
					from Adhoc a WITH (NOLOCK)
						left outer join syscolumns c WITH (NOLOCK) on  a.AdhocName = c.name
						join sysobjects s WITH (NOLOCK) on s.ID = c.ID 
						inner join systypes t WITH (NOLOCK) on c.xtype = t.xtype 
					where s.name = 'subscriptions' and (t.name = 'varchar' or t.name like '%datetime%' or (t.name like '%int%' and c.name in ('score'))  or t.name like '%float%') and c.name not in ('DEMO7','VERIFIED','MAILSTOP','PAR3C','EMPLOY','PLUS4', 'FORZIP', 'PUBIDs', 'NOTES','IGRP_RANK','CGRP_RANK','PRIORITY') and 
					a.CategoryID = @CategoryID
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
							,a.AdhocName, a.SortOrder
						from Adhoc a WITH (NOLOCK)
							join SubscriptionsExtensionMapper sem WITH (NOLOCK) on a.AdhocName = sem.CustomField and sem.Active = 1
						where a.CategoryID = @CategoryID   
					order by SortOrder		
				end
			else
				begin
					--standardfields, SubscriptionsExtensionMapper, MasterGroups
					if @BrandID = 0
						begin
							select case when t.name like '%date%' then 'd|[' +  upper(c.name) + ']' 
								when t.name like '%bit%' then 'b|[' +  upper(c.name) + ']'  
								when t.name like '%int%' then 'i|[' +  upper(c.name) + ']' 
								when t.name like '%float%' then 'f|[' +  upper(c.name) + ']' else '[' +  upper(c.name) + ']' end  as ColumnValue , 
								upper(c.name) as DisplayName, t.name as ColumnType, c.name as ColumnName, a.AdhocName,  a.SortOrder 
							from Adhoc a WITH (NOLOCK)
								left outer join syscolumns c WITH (NOLOCK) on a.AdhocName = c.name
								join sysobjects s WITH (NOLOCK) on s.ID = c.ID 
								inner join systypes t WITH (NOLOCK) on c.xtype = t.xtype 
							where s.name = 'subscriptions' and (t.name = 'varchar' or t.name like '%datetime%' or (t.name like '%int%' and c.name in ('score'))  or t.name like '%float%') and c.name not in ('DEMO7','VERIFIED','MAILSTOP','PAR3C','EMPLOY','PLUS4', 'FORZIP', 'PUBIDs', 'NOTES','IGRP_RANK','CGRP_RANK','PRIORITY') and 
							a.CategoryID = @CategoryID
							union
								select 'm|' + CONVERT(varchar(3),m.MasterGroupID)  as columnValue ,
								upper(m.Displayname) as displayName, 'varchar' as columnType, ColumnReference as columnName, a.AdhocName, a.SortOrder  
								from Adhoc a WITH (NOLOCK)
									left outer join MasterGroups m WITH (NOLOCK) on a.AdhocName = m.Displayname
								where m.EnableAdhocSearch = 1  and a.CategoryID = @CategoryID
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
									,a.AdhocName, a.SortOrder
								from Adhoc a  WITH (NOLOCK)
									join SubscriptionsExtensionMapper sem WITH (NOLOCK) on a.AdhocName = sem.CustomField and sem.Active = 1
								where a.CategoryID = @CategoryID 
							union
								select 'i|[PRODUCT COUNT]', 'PRODUCT COUNT', 'int', 'PRODUCT COUNT', AdhocName, SortOrder 
								from Adhoc  WITH (NOLOCK)
								where AdhocName = 'PRODUCT COUNT' and CategoryID = @CategoryID
							order by SortOrder		
						end
					else
						begin
							select case when t.name like '%date%' then 'd|[' +  upper(c.name) + ']' 
								when t.name like '%bit%' then 'b|[' +  upper(c.name) + ']'  
								when t.name like '%int%' then 'i|[' +  upper(c.name) + ']' 
								when t.name like '%float%' then 'f|[' +  upper(c.name) + ']' else '[' +  upper(c.name) + ']' end  as ColumnValue , 
								upper(c.name) as DisplayName, t.name as ColumnType, c.name as ColumnName, a.AdhocName,  a.SortOrder 
							from Adhoc a WITH (NOLOCK)
								left outer join syscolumns c WITH (NOLOCK) on a.AdhocName = c.name
								join sysobjects s WITH (NOLOCK) on s.ID = c.ID 
								inner join systypes t WITH (NOLOCK) on c.xtype = t.xtype 
							where s.name = 'subscriptions' and (t.name = 'varchar' or t.name like '%datetime%' or (t.name like '%int%' and c.name in ('score'))  or t.name like '%float%') and c.name not in ('DEMO7','VERIFIED','MAILSTOP','PAR3C','EMPLOY','PLUS4', 'FORZIP', 'PUBIDs', 'NOTES','IGRP_RANK','CGRP_RANK','PRIORITY')
								and a.CategoryID = @CategoryID
							union
								select distinct 'm|' + CONVERT(varchar(3), mg.MasterGroupID)  as columnValue ,
									upper(mg.Displayname) as displayName, 'varchar' as columnType, 
									mg.ColumnReference as columnName, a.AdhocName, a.SortOrder  
								From vw_Mapping v WITH (NOLOCK) 
									join MasterGroups mg WITH (NOLOCK) on mg.MasterGroupID = v.MasterGroupID 
									join branddetails bd WITH (NOLOCK) on bd.pubid = v.pubid  
									join Brand b WITH (NOLOCK) on b.BrandID = bd.BrandID 
									join Adhoc a WITH (NOLOCK)  on a.AdhocName = mg.Displayname
								where mg.EnableAdhocSearch = 1  and a.CategoryID = @CategoryID and bd.BrandID = @BrandID and b.Isdeleted = 0
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
									,a.AdhocName, a.SortOrder
								from Adhoc a  WITH (NOLOCK) 
									join SubscriptionsExtensionMapper sem WITH (NOLOCK) on a.AdhocName = sem.CustomField and sem.Active = 1
								where a.CategoryID = @CategoryID 
							union
								select 'i|[PRODUCT COUNT]', 'PRODUCT COUNT', 'int', 'PRODUCT COUNT', AdhocName, SortOrder 
								from Adhoc  WITH (NOLOCK)
								where AdhocName = 'PRODUCT COUNT' and CategoryID = @CategoryID	  
							order by SortOrder	
						end
				end
		end			
End