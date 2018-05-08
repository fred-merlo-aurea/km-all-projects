CREATE PROCEDURE [dbo].[sp_Import_MasterGroup]
	@importXML TEXT,
	@userID int
AS
BEGIN	
		
	SET NOCOUNT ON
	
 --	exec sp_Import_MasterGroup 
--'<mastergrouplist>
--  <mastergroup>
--    <name>MGTest</name>
--    <displayname>MGTest</displayname>
--    <isactive>1</isactive>
--    <enablesubreporting>1</enablesubreporting>
--    <enablesearching>1</enablesearching>
--    <enableadhocsearch></enableadhocsearch>
--  </mastergroup>
--</mastergrouplist>', 521
 
	DECLARE @docHandle INT

	CREATE TABLE #tmpData (
	Name varchar(100) NOT NULL,
	DisplayName varchar(100) NOT NULL,
	IsActive bit NULL,
	EnableSubReporting bit NULL,
	EnableSearching bit NULL,
	EnableAdhocSearch bit NULL
	)

	EXEC sp_xml_preparedocument @docHandle OUTPUT, @importXML  

	INSERT INTO #tmpData (
		Name,
		DisplayName,
		IsActive,
		EnableSubReporting,
		EnableSearching,
		EnableAdhocSearch
		)  
	SELECT 	DISTINCT 
		Name,
		DisplayName,
		IsActive,
		EnableSubReporting,
		EnableSearching,
		EnableAdhocSearch
	FROM 
		OPENXML(@docHandle, N'/mastergrouplist/mastergroup')   
	WITH   
		(  
			Name varchar(100) 'name',
			DisplayName varchar(50) 'displayname',
			IsActive int 'isactive',
			EnableSubReporting int 'enablesubreporting',
			EnableSearching bit 'enablesearching',
			EnableAdhocSearch int 'enableadhocsearch'
		) 

	EXEC sp_xml_removedocument @docHandle  
	
	declare @references table  (Reference varchar(100), ReferenceError varchar(100))
	
	if exists (select top 1 1 from #tmpData td join MasterGroups mg with (nolock) on td.Name = mg.Name)
	Begin
		insert into @references
			select td.Name, 'Name already exists.' from #tmpData td join MasterGroups mg WITH (NOLOCK)on td.Name = mg.Name
	End	
	
	if exists (select top 1 1 from #tmpData td join MasterGroups mg with (nolock) on td.Name = mg.DisplayName)
	Begin
		insert into @references
			select td.Name, 'DisplayName already exists.' from #tmpData td join MasterGroups mg WITH (NOLOCK)on td.Name = mg.DisplayName
	End		
	
	if exists (select top 1 1 from sysobjects so join syscolumns sc on so.id = sc.id join #tmpData td on td.Name = sc.name where (so.name = 'Subscriptions'  or so.name = 'PubSubscriptions'  or so.name = 'QSource' or so.name = 'Transaction' or so.name = 'TransactionGroup' or so.name = 'EmailStatus'))
	Begin
		insert into @references
			select td.Name, 'Name cannot be same as Standard Field Name.' from sysobjects so join syscolumns sc on so.id = sc.id join #tmpData td on td.Name = sc.name where (so.name = 'Subscriptions'  or so.name = 'PubSubscriptions'  or so.name = 'QSource' or so.name = 'Transaction' or so.name = 'TransactionGroup' or so.name = 'EmailStatus') 	End
	
	if exists (select top 1 1 from #tmpData td join SubscriptionsExtensionMapper sem with (nolock) on td.Name = sem.CustomField)
	Begin
		insert into @references
			select td.Name, 'Name cannot be the same as a Pub Custom Field name.' from #tmpData td join SubscriptionsExtensionMapper sem WITH (NOLOCK)on td.Name = sem.CustomField
	End
	
	if exists (select top 1 1 from sysobjects so join syscolumns sc on so.id = sc.id join #tmpData td on td.DisplayName = sc.name where (so.name = 'Subscriptions'  or so.name = 'PubSubscriptions'  or so.name = 'QSource' or so.name = 'Transaction' or so.name = 'TransactionGroup' or so.name = 'EmailStatus'))
	Begin
		insert into @references
			select td.DisplayName, 'DisplayName cannot be same as Standard Field Name.' from sysobjects so join syscolumns sc on so.id = sc.id join #tmpData td on td.DisplayName = sc.name where (so.name = 'Subscriptions'  or so.name = 'PubSubscriptions'  or so.name = 'QSource' or so.name = 'Transaction' or so.name = 'TransactionGroup' or so.name = 'EmailStatus') 
	End
	
	if exists (select top 1 1 from #tmpData td join SubscriptionsExtensionMapper sem with (nolock) on td.DisplayName = sem.CustomField)
	Begin
		insert into @references
			select td.DisplayName, 'DisplayName cannot be the same as a Pub Custom Field name.' from #tmpData td join SubscriptionsExtensionMapper sem WITH (NOLOCK)on td.DisplayName = sem.CustomField
	End
	
	Declare @count int
	select @count = COUNT(*) from @references
	
	if (@count = 0)
	begin
		Insert into MasterGroups 
		(
			Name,
			[Description],
			DisplayName,
			IsActive,
			EnableSubReporting,
			EnableSearching,
			EnableAdhocSearch,
			ColumnReference,
			SortOrder,
			DateCreated,
			CreatedByUserID		
		)
		select distinct 
			t.Name, 
			t.Name, 
			t.DisplayName, 
			t.IsActive,
			t.EnableSubReporting,
			t.EnableSearching,
			t.EnableAdhocSearch,
			'MASTER_' + REPLACE (t.Name, ' ', '_') as ColumnReference, 
			null,
			getdate(), 
			@userID
		from 
				#tmpData t left outer join 
				MasterGroups m on m.Name = t.Name
		where 
				m.MasterGroupID is null
	end

	Select * from @references
	
	--select * from #tmpData
	
	Drop table #tmpData

END


