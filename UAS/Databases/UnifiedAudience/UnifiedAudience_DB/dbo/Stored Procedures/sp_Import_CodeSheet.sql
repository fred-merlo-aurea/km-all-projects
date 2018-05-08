CREATE PROCEDURE [dbo].[sp_Import_CodeSheet]
	@importXML TEXT,
	@userID int
AS
BEGIN	
		
	SET NOCOUNT ON
	
 --	exec sp_Import_CodeSheet 
 --'<codesheetlist>
 --     <codesheet>
 --        <pubcode>PHAR</pubcode>
 --        <responsegroupname>AOI1</responsegroupname>
 --        <responsegroup_displayname>AOI1</responsegroup_displayname>
 --        <responsevalue>10</responsevalue>
 --        <responsedesc>Environmental Control Products</responsedesc>
 --        <mastervalue>229</mastervalue>
 --        <masterdesc>Environmental Control Products</masterdesc>
 --        <mastergroupname>AOI</mastergroupname>
 --     </codesheet>
 --  </codesheetlist>'
 
	DECLARE @docHandle INT,
			@dt datetime = getdate(),
			@UADCIRResponseGroupTypeID int		
			
	select @UADCIRResponseGroupTypeID = CodeID from UAD_Lookup..Code c join UAD_Lookup..CodeType ct on c.CodeId = ct.CodeTypeId where ct.CodeTypeName = 'Response Group' and c.CodeName = 'Circ and UAD'
	
	CREATE TABLE #tmpData (
		PubID int null,
		pubcode varchar(50) NOT NULL, 
		ResponseGroupID int null,
		responsegroupname VARCHAR(100) NOT NULL,
		responsegroup_displayname  VARCHAR(100) NOT NULL,
		CodeSheetID int null, 
		Responsevalue VARCHAR(255) NOT NULL,
		Responsedesc VARCHAR(255) NOT NULL,
		MasterID int NULL,
		mastervalue VARCHAR(100) NOT NULL,
		masterdesc VARCHAR(255) NOT NULL, 
		MasterGroupID int null,
		mastergroupname  VARCHAR(100)
	)

	EXEC sp_xml_preparedocument @docHandle OUTPUT, @importXML  

	INSERT INTO #tmpData (
		pubcode, 
		responsegroupname,
		responsegroup_displayname,
		Responsevalue,
		Responsedesc,
		mastervalue,
		masterdesc, 
		mastergroupname
		)  
	SELECT 	DISTINCT 
		pubcode, 
		responsegroupname,
		responsegroup_displayname,
		Responsevalue,
		Responsedesc,
		mastervalue,
		masterdesc, 
		mastergroupname
	FROM 
		OPENXML(@docHandle, N'/codesheetlist/codesheet')   
	WITH   
		(  
			pubcode varchar(50) 'pubcode', 
			responsegroupname VARCHAR(100) 'responsegroupname',
			responsegroup_displayname  VARCHAR(100) 'responsegroup_displayname',
			Responsevalue VARCHAR(255) 'responsevalue',
			Responsedesc VARCHAR(255) 'responsedesc',
			mastervalue VARCHAR(100) 'mastervalue',
			masterdesc VARCHAR(255) 'masterdesc', 
			mastergroupname  VARCHAR(100) 'mastergroupname'
		) 

	EXEC sp_xml_removedocument @docHandle  

	update t
	set t.PubID = p.pubID
	from #tmpData t join pubs p on t.pubcode = p.pubcode
	
	update t
	set t.MasterGroupID = mg.MasterGroupID
	from #tmpData t join MasterGroups mg on t.mastergroupname = mg.Name
	
	declare @references table  (Reference varchar(100), ReferenceError varchar(100))
	
	--if exists (select top 1 1 from #tmpData td join ResponseGroups rg with (nolock) on td.responsegroupname = rg.ResponseGroupName and td.PubID = rg.PubID)
	--Begin
	--	insert into @references
	--		select td.responsegroupname, 'ResponseGroupName already exists.' 
	--		from #tmpData td join ResponseGroups rg WITH (NOLOCK)on td.responsegroupname = rg.ResponseGroupName  and td.PubID = rg.PubID
	--End	
	
	if exists (select top 1 1 from #tmpData where PubID is null)
	Begin
		insert into @references 
		select distinct 'Invalid Pubcode', 'Invalid Pubcodes in file.('  + pubcode + ')'
		 from #tmpData where PubID is null
	End
	
	if exists (select top 1 1 from #tmpData where MasterGroupID is null)
	Begin
		insert into @references 
		select distinct 'Invalid Master Group', 'Invalid Master Groups in file.('  + mastergroupname + ')'
		from #tmpData where MasterGroupID is null
	End
	
	if exists (select top 1 1 from #tmpData td join ResponseGroups rg with (nolock) on td.responsegroup_displayname = rg.DisplayName and td.PubID = rg.PubID)
	Begin
		insert into @references
			select distinct td.responsegroup_displayname, 'ResponseGroup DisplayName already exists. ('  + pubcode + ' - ' + td.responsegroupname + ')'
			from #tmpData td join ResponseGroups rg WITH (NOLOCK)on td.responsegroupname <> rg.ResponseGroupName and  td.responsegroup_displayname = rg.DisplayName and td.PubID = rg.PubID
	End
	
	if exists (select top 1 1 from sysobjects so join syscolumns sc on so.id = sc.id join #tmpData td on td.responsegroupname = sc.name where (so.name = 'Subscriptions'  or so.name = 'PubSubscriptions'  or so.name = 'QSource' or so.name = 'Transaction' or so.name = 'TransactionGroup' or so.name = 'EmailStatus'))
	Begin
		insert into @references
			select distinct td.responsegroupname, 'ResponseGroupName cannot be same as Standard Field Name.' from sysobjects so join syscolumns sc on so.id = sc.id join #tmpData td on td.responsegroupname = sc.name where (so.name = 'Subscriptions'  or so.name = 'PubSubscriptions'  or so.name = 'QSource' or so.name = 'Transaction' or so.name = 'TransactionGroup' or so.name = 'EmailStatus') 
	End
	
	if exists (select top 1 1 from #tmpData td join PubSubscriptionsExtensionMapper sem WITH (NOLOCK)on td.responsegroupname = sem.CustomField)
	Begin
		insert into @references
			select distinct td.responsegroupname, 'ResponseGroupName cannot be the same as a Adhoc name.' from #tmpData td join PubSubscriptionsExtensionMapper sem WITH (NOLOCK)on td.responsegroupname = sem.CustomField
	End
	
	if exists (select top 1 1 from sysobjects so join syscolumns sc on so.id = sc.id join #tmpData td on td.responsegroup_displayname = sc.name where (so.name = 'Subscriptions'  or so.name = 'PubSubscriptions'  or so.name = 'QSource' or so.name = 'Transaction' or so.name = 'TransactionGroup' or so.name = 'EmailStatus'))
	Begin
		insert into @references
			select distinct td.responsegroup_displayname, 'ResponseGroup_DisplayName cannot be same as Standard Field Name.' from sysobjects so join syscolumns sc on so.id = sc.id join #tmpData td on td.responsegroup_displayname = sc.name where (so.name = 'Subscriptions'  or so.name = 'PubSubscriptions'  or so.name = 'QSource' or so.name = 'Transaction' or so.name = 'TransactionGroup' or so.name = 'EmailStatus') 
	End
	
	if exists (select top 1 1 from #tmpData td join PubSubscriptionsExtensionMapper sem WITH (NOLOCK)on td.responsegroup_displayname = sem.CustomField)
	Begin
		insert into @references
			select distinct td.responsegroup_displayname, 'ResponseGroup_DisplayName cannot be the same as a Adhoc name.' from #tmpData td join PubSubscriptionsExtensionMapper sem WITH (NOLOCK)on td.responsegroup_displayname = sem.CustomField
	End
	
	if exists(select top 1 1 from #tmpData group by pubcode, responsegroupname, Responsevalue having count(distinct Responsedesc) > 1)
	Begin
		insert into @references 
		select 'Duplicate', 'File contains duplicate response values. Please remove duplicate values. ( ' + pubcode + ' - ' + responsegroupname + ' - ' + Responsevalue + ' )'
		from #tmpData group by pubcode, responsegroupname, Responsevalue having count(distinct Responsedesc) > 1
	End
	
	if exists(select top 1 1 from #tmpData group by mastergroupname, mastervalue having count(distinct masterdesc) > 1)
	Begin
		insert into @references 
		select 'Duplicate', 'File contains duplicate master values. Please remove duplicate values. ( ' + mastergroupname + ' - ' + mastervalue + ' )'
		from #tmpData group by mastergroupname, mastervalue having count(distinct masterdesc) > 1
	End

	Declare @count int
	select @count = COUNT(*) from @references
	
	if (@count = 0)
	begin
		Insert into ResponseGroups 
		(
			PubID,
			ResponseGroupName,
			DisplayName,
			DateCreated,
			CreatedByUserID,
			IsMultipleValue,
			IsRequired,
			IsActive,
			DisplayOrder,
			ResponseGroupTypeId
		)
		select distinct t.PubID, t.responsegroupname, t.responsegroup_displayname, @dt, @userID, 0, 0, 1, null, @UADCIRResponseGroupTypeID
		from 
				#tmpData t left outer join 
				ResponseGroups rg on t.PubID = rg.PubID and t.responsegroupname = rg.ResponseGroupName
		where 
				rg.ResponseGroupID is null
			 
		update t
		set t.ResponseGroupID = rg.ResponseGroupID
		from 
				#tmpData t join 
				ResponseGroups rg on t.PubID = rg.PubID and t.responsegroupname = rg.ResponseGroupName
		
		Insert into CodeSheet 
		(
			PubID,
			ResponseGroup,
			Responsevalue,
			Responsedesc,
			ResponseGroupID,
			DateCreated,
			CreatedByUserID,
			DisplayOrder,
			IsActive,
			IsOther
		)
		select distinct t.PubID, t.responsegroupname, t.Responsevalue, t.Responsedesc, t.ResponseGroupID, @dt, @userID, null, 1, 0
		from 
				#tmpData t left outer join 
				CodeSheet c on  t.ResponseGroupID = c.ResponseGroupID and t.Responsevalue = c.Responsevalue
		where 
				c.CodeSheetID is null
			 
		update t
		set t.CodeSheetID = c.CodeSheetID
		from 
				#tmpData t join 
				CodeSheet c on  t.ResponseGroupID = c.ResponseGroupID and t.Responsevalue = c.Responsevalue
		
		Insert into Mastercodesheet 
		(
			MasterValue,
			MasterDesc,
			MasterGroupID,
			MasterDesc1,
			EnableSearching,
			SortOrder,
			DateCreated,
			CreatedByUserID
		)
		select distinct t.mastervalue, t.masterdesc, t.MasterGroupID, null, 1, null, @dt, @userID
		from 
				#tmpData t left outer join 
				Mastercodesheet mc on  t.MasterGroupID = mc.MasterGroupID and t.mastervalue = mc.MasterValue
		where 
				mc.MasterID is null
			 
		update t
		set t.MasterID = mc.MasterID
		from 
				#tmpData t join 
				Mastercodesheet mc on  t.MasterGroupID = mc.MasterGroupID and t.mastervalue = mc.MasterValue
		
		insert into CodeSheet_Mastercodesheet_Bridge (CodeSheetID, MasterID)
		select distinct t.codesheetID, t.masterID
		from 
				#tmpData t left outer join CodeSheet_Mastercodesheet_Bridge cb on t.CodeSheetID = cb.CodeSheetID and t.MasterID = cb.MasterID
		where
				cb.MasterID is null
	End 	
	select * from @references	
	--select * from #tmpData
	
	Drop table #tmpData

END