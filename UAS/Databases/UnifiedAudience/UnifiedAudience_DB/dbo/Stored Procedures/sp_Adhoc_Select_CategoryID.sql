CREATE  PROCEDURE [dbo].[sp_Adhoc_Select_CategoryID]   
@CategoryID int,
@BrandID int,
@PubID int
AS  
BEGIN
	
	SET NOCOUNT ON  

	if @CategoryID = 0
		begin	

			if(@PubID != 0)
				Begin
					--standardfields
					select 
						case when t.name like '%date%' then 'd|[' +  upper(c.name) + ']' 
							when t.name like '%bit%' then 'b|[' +  upper(c.name) + ']'  
							when t.name like '%int%' then 'i|[' +  upper(c.name) + ']' 
							when t.name like '%float%' then 'f|[' +  upper(c.name) + ']' else '[' +  upper(c.name) + ']' end  as ColumnValue , 
						case when c.name like '%Address1%' then upper('Address')
							when c.name like '%RegionCode%' then upper('State')
							when c.name like '%ZipCode%' then upper('Zip')
							when c.name like '%PubTransactionDate%' then upper('TransactionDate') 
							when c.name like '%QualificationDate%' then upper('QDate')  else upper(c.name) end as DisplayName,			
						t.name as ColumnType, 
						c.name as ColumnName
					from sysobjects s 
						join syscolumns c on s.ID = c.ID  
						inner join systypes t on c.xtype = t.xtype 
					where s.name = 'pubsubscriptions' and
						 (t.name = 'varchar' or t.name like '%datetime%' or t.name like '%int%' or t.name like '%float%' or t.name like '%date%' or t.name like '%uniqueidentifier%') and 
						 c.name in ('FIRSTNAME', 'LASTNAME', 'COMPANY', 'TITLE', 'ADDRESS' , 'CITY' ,'REGIONCODE', 'ZIPCODE', 'ORIGSSRC', 'COUNTRY', 'PHONE', 'MOBILE', 'EMAIL', 'FAX', 'ADDRESS1', 'ADDRESS2', 'ADDRESS3', 'COUNTY',  'DATECREATED', 'DATEUPDATED', 'GENDER', 'ISACTIVE',  'PUBTRANSACTIONDATE', 'QUALIFICATIONDATE', 'STATUSUPDATEDDATE', 'EXTERNALKEYID', 'ACCOUNTNUMBER', 'SUBSCRIBERSOURCECODE', 'PLUS4', 'WEBSITE', 'IGRP_NO') and 
						(case when c.name = 'Address1' then 'Address' 
							when c.name like '%RegionCode%' then 'State'
							when c.name like '%ZipCode%' then 'Zip'
							when c.name like '%PubTransactionDate%' then 'TransactionDate' 
							when c.name like '%QualificationDate%' then 'QDate' else c.name end)  not in (select AdhocName from Adhoc with (nolock))  
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
						from PubSubscriptionsExtensionMapper pem with (nolock) 
						where pem.PubID = @PubID and 
							pem.Active = 1 and 
							pem.CustomField not in (select AdhocName from Adhoc with (nolock)) 				
					order by columnName
				End
			else
				Begin
					--standardfields, SubscriptionsExtensionMapper, MasterGroups
					if @BrandID = 0
						begin
 							select 
 								case when t.name like '%date%' then 'd|[' +  upper(c.name) + ']' 
									when t.name like '%bit%' then 'b|[' +  upper(c.name) + ']'  
									when t.name like '%int%' then 'i|[' +  upper(c.name) + ']' 
									when t.name like '%float%' then 'f|[' +  upper(c.name) + ']' 
									else '[' +  upper(c.name) + ']' 
								end  as ColumnValue , 
								case when c.name like '%FName%' then upper('FirstName')
									when c.name like '%LName%' then upper('LastName') 
									else upper(c.name) 
								end as DisplayName,	
								t.name as ColumnType, 
								c.name as ColumnName
							from sysobjects s 
								join syscolumns c on s.ID = c.ID 
								inner join systypes t on c.xtype = t.xtype 
							where 
								s.name = 'subscriptions' and 
								(t.name = 'varchar' or t.name like '%datetime%' or (t.name like '%int%' and c.name in ('score'))  or t.name like '%float%' or t.name like '%uniqueidentifier%') and 
								c.name not in ('DEMO7','VERIFIED','PAR3C','EMPLOY', 'FORZIP', 'PUBIDs', 'NOTES','IGRP_RANK','CGRP_RANK','PRIORITY', 'ADDRESSLASTUPDATEDDATE', 'LATLONMSG', 'ACCOUNTNUMBER', 'SUBSCRIBERSOURCECODE', 'CGRP_NO') and 
								(case when c.name = 'FName' then 'FirstName' 
									when c.name like '%LName%' then upper('LastName')  
									else c.name 
								end)  not in (select AdhocName from Adhoc with (nolock))  
							union
								select 'm|' + CONVERT(varchar(3),m.MasterGroupID)  as columnValue ,
									upper(m.Displayname) as displayName, 
									'varchar' as columnType, 
									ColumnReference as columnName  
								from MasterGroups m with (nolock)
								where m.EnableAdhocSearch = 1 and 
									m.Displayname not in (select AdhocName from Adhoc with (nolock))   
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
								from SubscriptionsExtensionMapper map with (nolock) 
								where map.Active = 1 and 
									map.CustomField not in (select AdhocName from Adhoc with (nolock)) 
							union
								select top 1 'i|[PRODUCT COUNT]', 'PRODUCT COUNT', 'int', 'PRODUCT COUNT' 
								where not exists (select adhocname from Adhoc with (nolock) where AdhocName = 'PRODUCT COUNT') 					 
							order by columnName
						end
					else
						begin
							Create Table #MG1 (mastergroupID int primary key)

							insert into #MG1
							select distinct mc.mastergroupID from Mastercodesheet mc with (NOLOCK)
							WHERE mc.MasterID IN
								(SELECT DISTINCT cmb.MasterID
									FROM CodeSheet_Mastercodesheet_Bridge cmb WITH (NOLOCK)
									WHERE cmb.CodeSheetID IN
										(SELECT DISTINCT c.CodeSheetID
										FROM CodeSheet c WITH (NOLOCK)
										WHERE c.ResponseGroupID IN
											(SELECT DISTINCT rg.ResponseGroupID
											FROM ResponseGroups rg WITH (NOLOCK)
											WHERE rg.PubId IN
												(SELECT DISTINCT bd.Pubid
												FROM BrandDetails bd WITH (NOLOCK)
												WHERE bd.BrandID = @BrandID))))

							select 
								case when t.name like '%date%' then 'd|[' +  upper(c.name) + ']' 
									when t.name like '%bit%' then 'b|[' +  upper(c.name) + ']'  
									when t.name like '%int%' then 'i|[' +  upper(c.name) + ']' 
									when t.name like '%float%' then 'f|[' +  upper(c.name) + ']' 
									else '[' +  upper(c.name) + ']' 
								end  as ColumnValue , 
								case when c.name like '%FName%' then upper('FirstName')
									when c.name like '%LName%' then upper('LastName') 
									else upper(c.name) 
								end as DisplayName,
								t.name as ColumnType, 
								c.name as ColumnName
							from sysobjects s 
								join syscolumns c on s.ID = c.ID 
								inner join systypes t on c.xtype = t.xtype 
							where s.name = 'subscriptions' and (t.name = 'varchar' or t.name like '%datetime%' or (t.name like '%int%' and c.name in ('score'))  or t.name like '%float%' or t.name like '%uniqueidentifier%')  and
								c.name not in ('DEMO7','VERIFIED','PAR3C','EMPLOY', 'FORZIP', 'PUBIDs', 'NOTES','IGRP_RANK','CGRP_RANK','PRIORITY', 'ADDRESSLASTUPDATEDDATE', 'LATLONMSG', 'ACCOUNTNUMBER', 'SUBSCRIBERSOURCECODE', 'CGRP_NO') and 
								(case when c.name = 'FName' then 'FirstName' 
									when c.name like '%LName%' then upper('LastName')  
									else c.name 
								end)  not in (select AdhocName from Adhoc with (nolock)) 
							union
								Select 
									distinct 'm|' + CONVERT(varchar(3),mg.MasterGroupID)  as columnValue,
									upper(mg.Displayname) as displayName, 'varchar' as columnType, 
									mg.ColumnReference as columnName  
								FROM MasterGroups mg WITH (NOLOCK)
								join #MG1 t  on t.mastergroupID = mg.mastergroupID
								and mg.EnableAdhocSearch = 1  and 
									mg.Displayname not in (select AdhocName from Adhoc with (nolock))
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
								from SubscriptionsExtensionMapper map with (nolock)
								where map.Active = 1 and
									map.CustomField not in (select AdhocName from Adhoc with (nolock))  
							union
								select top 1 'i|[PRODUCT COUNT]', 'PRODUCT COUNT', 'int', 'PRODUCT COUNT' 
								where not exists (select adhocname from Adhoc with (nolock) where AdhocName = 'PRODUCT COUNT')				
							order by columnName	
						end
			end
		end
	else
		begin
			if(@PubID != 0)
			Begin
				--standardfields
				Select case when t.name like '%date%' then 'd|[' +  upper(c.name) + ']' 
							when t.name like '%bit%' then 'b|[' +  upper(c.name) + ']'  
							when t.name like '%int%' then 'i|[' +  upper(c.name) + ']' 
							when t.name like '%float%' then 'f|[' +  upper(c.name) + ']' 
							else '[' +  upper(c.name) + ']' 
						end  as ColumnValue , 
						case when c.name like '%FName%' then upper('FirstName')
							when c.name like '%LName%' then upper('LastName')
							when c.name like '%Address1%' then upper('Address')
							when c.name like '%RegionCode%' then upper('State')
							when c.name like '%ZipCode%' then upper('Zip')
							when c.name like '%PubTransactionDate%' then upper('TransactionDate') 
							when c.name like '%QualificationDate%' then upper('QDate')  
							else upper(c.name) 
						end as DisplayName,		
						t.name as ColumnType, 
						c.name as ColumnName, 
						a.AdhocName,  
						a.SortOrder 
				from 
					Adhoc a  with (nolock) left outer join   syscolumns c   on  
					a.AdhocName = (case when c.name = 'Address1' then 'Address' else c.name end) or
					a.AdhocName = (case when c.name = 'RegionCode' then 'State' else c.name end) or
					a.AdhocName = (case when c.name = 'ZipCode' then 'Zip' else c.name end) or
					a.AdhocName = (case when c.name = 'PubTransactionDate' then 'TransactionDate' else c.name end) or
					a.AdhocName = (case when c.name = 'QualificationDate' then 'QDate' else c.name end) 
					join sysobjects s on s.ID = c.ID inner join 
					systypes t on c.xtype = t.xtype 
				where 
					s.name = 'pubsubscriptions' and 
					(t.name = 'varchar' or t.name like '%datetime%' or t.name like '%int%' or t.name like '%float%' or t.name like '%date%' or t.name like '%uniqueidentifier%') and 
					c.name in ('FIRSTNAME', 'LASTNAME', 'COMPANY', 'TITLE', 'ADDRESS' , 'CITY' ,'REGIONCODE', 'ZIPCODE', 'ORIGSSRC', 'COUNTRY', 'PHONE', 'MOBILE', 'EMAIL', 'FAX', 'ADDRESS1', 'ADDRESS2', 'ADDRESS3', 'COUNTY',  'DATECREATED', 'DATEUPDATED', 'GENDER', 'ISACTIVE',  'PUBTRANSACTIONDATE', 'QUALIFICATIONDATE', 'STATUSUPDATEDDATE', 'EXTERNALKEYID', 'ACCOUNTNUMBER', 'SUBSCRIBERSOURCECODE', 'PLUS4', 'WEBSITE', 'IGRP_NO') and 
					a.CategoryID = @CategoryID
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
						StandardField as columnName,  
						a.AdhocName, a.SortOrder
					from Adhoc a  with (nolock)join
						 PubSubscriptionsExtensionMapper pem on a.AdhocName = pem.CustomField and pem.Active = 1
					where a.CategoryID = @CategoryID and PubID = @PubID		
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
									when t.name like '%float%' then 'f|[' +  upper(c.name) + ']' 
									else '[' +  upper(c.name) + ']' 
								end  as ColumnValue , 
							case when c.name like '%FName%' then upper('FirstName')
								when c.name like '%LName%' then upper('LastName') 
								else upper(c.name) 
							end as DisplayName,
							t.name as ColumnType, 
							c.name as ColumnName, 
							a.AdhocName,  
							a.SortOrder 
						from 
							Adhoc a  with (nolock) left outer join   syscolumns c   on  
							a.AdhocName = (case when c.name = 'FName' then 'FirstName' else c.name end) or
							a.AdhocName = (case when c.name = 'LName' then 'LastName' else c.name end) 
							join sysobjects s on s.ID = c.ID inner join 
							systypes t on c.xtype = t.xtype 
						where 
							s.name = 'subscriptions' and 
							(t.name = 'varchar' or t.name like '%datetime%' or (t.name like '%int%' and c.name in ('score'))  or t.name like '%float%' or t.name like '%uniqueidentifier%') and 
							c.name not in ('DEMO7','VERIFIED','PAR3C','EMPLOY', 'FORZIP', 'PUBIDs', 'NOTES','IGRP_RANK','CGRP_RANK','PRIORITY', 'ADDRESSLASTUPDATEDDATE', 'LATLONMSG', 'ACCOUNTNUMBER', 'SUBSCRIBERSOURCECODE', 'CGRP_NO') and 
							a.CategoryID = @CategoryID
						union
							select 'm|' + CONVERT(varchar(3),m.MasterGroupID)  as columnValue ,
							upper(m.Displayname) as displayName, 'varchar' as columnType, ColumnReference as columnName, a.AdhocName, a.SortOrder  
							from Adhoc a  with (nolock) left outer join MasterGroups m with (nolock) on a.AdhocName = m.Displayname
							where m.EnableAdhocSearch = 1  and a.CategoryID = @CategoryID
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
								StandardField as columnName,  
								a.AdhocName, a.SortOrder
							from Adhoc a  with (nolock)join
								 SubscriptionsExtensionMapper sem on a.AdhocName = sem.CustomField and sem.Active = 1
							where a.CategoryID = @CategoryID 
						union
							select 
								'i|[PRODUCT COUNT]', 'PRODUCT COUNT', 'int', 'PRODUCT COUNT', AdhocName, SortOrder 
							from 
								Adhoc with (nolock)
							where 
								AdhocName = 'PRODUCT COUNT' and CategoryID = @CategoryID
						order by SortOrder		
					end
					else
					begin
						Create Table #MG (mastergroupID int primary key)

						insert into #MG
						select distinct mc.mastergroupID from Mastercodesheet mc with (NOLOCK)
								WHERE mc.MasterID IN
									(SELECT DISTINCT cmb.MasterID
									 FROM CodeSheet_Mastercodesheet_Bridge cmb WITH (NOLOCK)
									 WHERE cmb.CodeSheetID IN
										 (SELECT DISTINCT c.CodeSheetID
										  FROM CodeSheet c WITH (NOLOCK)
										  WHERE c.ResponseGroupID IN
											  (SELECT DISTINCT rg.ResponseGroupID
											   FROM ResponseGroups rg WITH (NOLOCK)
											   WHERE rg.PubId IN
												   (SELECT DISTINCT bd.Pubid
													FROM BrandDetails bd WITH (NOLOCK)
													WHERE bd.BrandID = @BrandID))))


						select case when t.name like '%date%' then 'd|[' +  upper(c.name) + ']' 
								when t.name like '%bit%' then 'b|[' +  upper(c.name) + ']'  
								when t.name like '%int%' then 'i|[' +  upper(c.name) + ']' 
								when t.name like '%float%' then 'f|[' +  upper(c.name) + ']' else '[' +  upper(c.name) + ']' end  as ColumnValue , 
							case when c.name like '%FName%' then upper('FirstName')
								when c.name like '%LName%' then upper('LastName') else upper(c.name) end as DisplayName,
							t.name as ColumnType, 
							c.name as ColumnName, 
							a.AdhocName,  
							a.SortOrder 
						from 
							Adhoc a  with (nolock) left outer join   syscolumns c   on  
							a.AdhocName = (case when c.name = 'FName' then 'FirstName' else c.name end) or
							a.AdhocName = (case when c.name = 'LName' then 'LastName' else c.name end) 
							join sysobjects s on s.ID = c.ID inner join 
							systypes t on c.xtype = t.xtype 
						where 
							s.name = 'subscriptions' and 
							(t.name = 'varchar' or t.name like '%datetime%' or (t.name like '%int%' and c.name in ('score'))  or t.name like '%float%' or t.name like '%uniqueidentifier%') and 
							c.name not in ('DEMO7','VERIFIED','PAR3C','EMPLOY', 'FORZIP', 'PUBIDs', 'NOTES','IGRP_RANK','CGRP_RANK','PRIORITY', 'ADDRESSLASTUPDATEDDATE', 'LATLONMSG', 'ACCOUNTNUMBER', 'SUBSCRIBERSOURCECODE', 'CGRP_NO') and
							a.CategoryID = @CategoryID
						union
							select 
								distinct 'm|' + CONVERT(varchar(3), mg.MasterGroupID)  as columnValue ,
								upper(mg.Displayname) as displayName, 'varchar' as columnType, 
								mg.ColumnReference as columnName, a.AdhocName, a.SortOrder 
							FROM MasterGroups mg WITH (NOLOCK)
							 join Adhoc a  with (nolock) on a.AdhocName = mg.Displayname
							 join #MG t  on t.mastergroupID = mg.mastergroupID
							 where mg.EnableAdhocSearch = 1  and 
								a.CategoryID = @CategoryID
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
								StandardField as columnName , 
								a.AdhocName, a.SortOrder
							from 
								Adhoc a with (nolock) join
								SubscriptionsExtensionMapper sem with (nolock) on a.AdhocName = sem.CustomField	and sem.Active = 1
							where a.CategoryID = @CategoryID 
						union
							select 
								'i|[PRODUCT COUNT]', 'PRODUCT COUNT', 'int', 'PRODUCT COUNT', AdhocName, SortOrder 
							from 
								Adhoc with (nolock)
							where 
								AdhocName = 'PRODUCT COUNT' and 
								CategoryID = @CategoryID	  
						order by SortOrder	
				end
				end
		end	

End
