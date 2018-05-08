-- 2014-10-24 MK Added  WITH (NOLOCK) hints

CREATE proc [dbo].[e_EmailGroup_Update_SubscriptionManagement]
(  
 @XMLProfiles varchar(8000),  
 @UserID int,
 @source varchar(200) = 'ActivityEngine.SubscriptionManagement'
)  
as  
Begin 

set nocount on

--for testing in .241-----------------------------------------------
--declare @UserID int
--set @UserID = 4496
--declare @XMLProfiles varchar(4000)
--set @XMLProfiles = '<root>
--<subscriptionManagement id="1" emailAddress="bill.hipps@teamkm.com">
--  <group id="49195" customer="1" subscribeTypeCode="U">
--    <udf id="254724" value="test data"/>
--    <udf id="254725" value="test data 2"/>
--    <udf id="74152" value="my topic"/>
--    <udf id="254723" value="my advertiser"/>
--    <udf id="512737" value="my udf1"/>
--    <udf id="512738" value="my udf2"/>
--  </group>
--  <group id="176057" customer="1" subscribeTypeCode="S">
--    <udf id="512985" value="test data 1.2"/>
--    <udf id="512986" value="test data 1.4"/>
--  </group>
--  <group id="66511" customer="3053" subscribeTypeCode="S" />
--  <group id="161727" customer="3567" subscribeTypeCode="S" />
--</subscriptionManagement></root>'
--end for testing in .241-------------------------------------------

	--temp table for profiles
	declare @docHandle int

	--record all changes with the same date/time
	declare @CurrentDate datetime
	set @CurrentDate = GETDATE()

	create TABLE #emails
	(  
		  SMID int, EmailID int, Emailaddress varchar(255), EmailgroupID int, GroupID int, CustomerID int, SubscribeTypeCode varchar(1), Reason varchar(100), Skipped bit
	) 

	--insert all profiles into temp table
	EXEC sp_xml_preparedocument @docHandle OUTPUT, @XMLProfiles
	insert into #emails
	(
		  SMID, Emailaddress, GroupID, CustomerID, SubscribeTypeCode, Reason,Skipped
	) 
	SELECT [smid], [emailaddress], [groupid], [customerid], [subscribetypecode],[reason],0
	FROM OPENXML(@docHandle, N'/root/subscriptionManagement/group')
	WITH 
	(
	[smid] int '../@id',
	[emailaddress][varchar](255) '../@emailAddress',
	[groupid] int '@id',
	[customerid] int '@customer',
	[subscribetypecode] [varchar](1) '@subscribeTypeCode',
	[reason] [varchar](100) '@reason')
	EXEC sp_xml_removedocument @docHandle
	
	update #emails
	set Skipped = 1
	where dbo.fn_ValidateEmailAddress(Emailaddress) = 0 

	update #emails
	set Reason = Replace(Reason,'&amp;','&')

	--insert the email record for the customer if it doesn't exist
	insert into Emails (
		EmailAddress, 
		CustomerID, 
		DateAdded)
	select 
		distinct el.emailaddress, 
		el.customerid, 
		@CurrentDate 
	from 
		#emails el 
		left outer join Emails e WITH (NOLOCK) on el.emailaddress = e.emailaddress and el.CustomerID = e.CustomerID 
	where 
		e.EmailID is null and el.Skipped = 0

	--update the profiles temp table with emailids
	update 
		#emails  
	set 
		EmailID = e.EmailID
	from   
		#emails el with (NOLOCK) 
		join [Emails] e with (NOLOCK) on el.emailaddress = e.emailaddress and el.CustomerID = e.CustomerID
	where 
		el.Skipped = 0
	--update the profiles temp table with emailids
	update 
		#emails  
	set 
		EmailgroupID = eg.emailgroupID
	from   
		#emails el with (NOLOCK) 
		join EmailGroups eg  WITH (NOLOCK) on el.EmailID = eg.EmailID and el.GroupID = eg.groupID
	where
		el.Skipped = 0
	--insert all udfs into temp table
	EXEC sp_xml_preparedocument @docHandle OUTPUT, @XMLProfiles

	--temp table for udfs
	create TABLE #udfs
	(  
		  EmailID int, GroupID int, UDFID int, UDFValue varchar(500), DataFieldSetID int, RankID int, EntryID uniqueidentifier,Skipped bit
	)

	insert into #udfs 
	(
		  EmailID, GroupID, UDFID, UDFValue, DataFieldSetID, Skipped  --CustomerID, SubscribeTypeCode, 
	) 
	select e.emailID, inn.groupID, inn.udfID, inn.udfvalue, gdf.DatafieldSetID,0
	from
	#emails e join
	(
		  SELECT [EmailAddress], [groupid], [udfid], [udfvalue] 
		  FROM OPENXML(@docHandle, N'/root/subscriptionManagement/group/udf')
		  WITH 
		  (
				[emailaddress][varchar](255) '../../@emailAddress',
				[groupid] int '../@id',
				[customerid] int '../@customer',
				--[subscribetypecode] [varchar](1) '../@subscribeTypeCode',
				[udfid] int '@id',
				[udfvalue] [varchar](500) 'text()'
		  )
	)
	inn on inn.groupID  = e.groupID and inn.emailaddress = e.Emailaddress 
	left outer join GroupDatafields gdf  WITH (NOLOCK) on gdf.GroupDatafieldsID = inn.udfID
	


	EXEC sp_xml_removedocument @docHandle;

	--COMMENTING OUT BECAUSE STANDALONE WEREN'T PART OF INITIAL REQUEST
	--This is for doing standalone udf default values
			--insert into #udfs (EmailID, GroupID, UDFID,UDFValue,DataFieldsetID)  
			--select distinct e.EmailID,udf.GroupID, gdf2.GroupDatafieldsID, dbo.fn_GetGDFDValue(gdfd.DataValue, gdfd.SystemValue),gdf2.DatafieldSetID
			--from #udfs udf
			--join #emails e on udf.EmailID = e.EmailID
			--join GroupDatafields gdf2 with(nolock) on gdf2.GroupID = e.GroupID
			--join GroupDataFieldsDefault gdfd with(nolock) on gdf2.GroupDatafieldsID = gdfd.GDFID
			--where gdfd.GDFID is not null 
			--	and gdf2.GroupDatafieldsID not in (Select UDFID from #udfs) 
			--	and gdf2.DatafieldSetID is null 
			--	and gdf2.IsDeleted = 0
			--  and ISNULL(udf.Datavalue,'') <> ''
			
			--This is for doing transactional udf default values
			insert into #udfs (EmailID, GroupID, UDFID,UDFValue,DataFieldsetID)  
			select distinct e.EmailID,udf.GroupID, gdf2.GroupDatafieldsID, dbo.fn_GetGDFDValue(gdfd.DataValue, gdfd.SystemValue),gdf2.DatafieldSetID
			from #udfs udf
			join #emails e on udf.EmailID = e.EmailID
			join GroupDatafields gdf2 with(nolock) on gdf2.GroupID = e.GroupID 
			join GroupDataFieldsDefault gdfd with(nolock) on gdf2.GroupDatafieldsID = gdfd.GDFID
			where gdfd.GDFID is not null 
				  and  gdf2.GroupDatafieldsID not in (Select UDFID from #udfs) 
				  and  gdf2.DatafieldSetID is not null 
				  and  gdf2.IsDeleted = 0
				  and gdf2.DatafieldSetID in (select distinct DataFieldSetID from #udfs)
				  and ISNULL(udf.UDFValue,'') <> '';


	--get the entry ids
	WITH CTE (DataFieldSetID, RankID)
	AS (
		  SELECT DataFieldSetID, RANK() OVER  (PARTITION BY 1 order by DataFieldSetID) AS 'RankID'
		  FROM #udfs
		  WHERE Skipped = 0
		  )
	UPDATE t
		SET RankID = cte.RankID
	FROM
		#udfs t 
		INNER JOIN cte on t.DataFieldSetID = cte.DataFieldSetID;
	      
	SELECT DISTINCT
		RankID,
		(SELECT NEWID()) AS EntryID
	INTO 
		#NewId
	FROM
		#udfs
	WHERE 
		RankID IS NOT NULL
	   
	UPDATE t
		SET EntryID = #NewId.EntryID
	FROM
		#udfs t 
		INNER JOIN #NewId on t.RankID = #NewId.RankId;      

	DROP TABLE #NewId

	--testing-------------------------------------------------------
	--select * from #emails
	--select * from #udfs
	--add activty for unsubscribes
	--select EmailID, 0, 'subscribe', SubscribeTypeCode, 'UNSUBSCRIBED THRU Subscription Management ID: ' + Convert(varchar(20),SMID) + ' for Group ID: ' + Convert(varchar(20),GroupID), 'n', @CurrentDate
	--from #emails
	--where SubscribeTypeCode = 'U'

	----update emailgroups
	--select #emails.SubscribeTypeCode, @CurrentDate
	--from EmailGroups join #emails on EmailGroups.EmailGroupID = #emails.EmailgroupID

	----insert emailgroups
	--select EmailID, GroupID, @CurrentDate, 'HTML', SubscribeTypeCode
	--from #emails
	--where EmailgroupID is null

	----insert transactional
	--select EmailID, UDFID, UDFValue, @CurrentDate, EntryID, @UserID
	--from #udfs
	--where DataFieldSetID is not null

	----update non-transactional
	--select u1.UDFValue, @CurrentDate, @UserID
	--from #udfs u1 join EmailDataValues edv on u1.UDFID = edv.GroupDatafieldsID and u1.EmailID = edv.EmailID and u1.DataFieldSetID is null

	----insert non-transactional
	--select u1.EmailID, UDFID, UDFValue, @CurrentDate, null, @UserID
	--from #udfs u1 left outer join EmailDataValues edv on u1.UDFID = edv.GroupDatafieldsID and u1.EmailID = edv.EmailID and edv.EmailDataValuesID is null
	--where u1.DataFieldSetID is null
	--end testing---------------------------------------------------


	--add activty for unsubscribes
	insert into EmailActivityLog(
		EmailID, 
		BlastID, 
		ActionTypeCode, 
		ActionValue, 
		ActionNotes, 
		Processed, 
		ActionDate)
	select 
		EmailID, 
		0, 
		'subscribe', 
		SubscribeTypeCode, 
		'UNSUBSCRIBED THRU Subscription Management ID: ' + Convert(varchar(20),SMID) + ' for Group ID: ' + Convert(varchar(20),GroupID) + ISNULL(Reason, ''), 
		'n', 
		@CurrentDate
	from 
		#emails
	where
		SubscribeTypeCode = 'U'
		and Skipped = 0

	--update emailgroups
	update
		EmailGroups
	set 
		EmailGroups.SubscribeTypeCode = #emails.SubscribeTypeCode, 
		EmailGroups.LastChanged = @CurrentDate, LastChangedSource = @source
	--select #emails.SubscribeTypeCode, @CurrentDate
	from 
		EmailGroups WITH (NOLOCK)  
		join #emails on EmailGroups.EmailGroupID = #emails.EmailgroupID
	WHERE Skipped = 0

	--insert emailgroups
	insert into EmailGroups(
		EmailID, 
		GroupID, 
		CreatedOn, 
		FormatTypeCode, 
		SubscribeTypeCode,
		CreatedSource)
	select
		EmailID, 
		GroupID, 
		@CurrentDate, 
		'HTML', 
		SubscribeTypeCode, 
		@source
	from 
		#emails
	where
		EmailgroupID is null and Skipped = 0

	--insert transactional
	Insert into EmailDataValues (
		EmailID, 
		GroupDatafieldsID, 
		DataValue, 
		CreatedDate, 
		EntryID, 
		CreatedUserID)
	SELECT DISTINCT
		u.EmailID, 
		UDFID, 
		UDFValue, 
		@CurrentDate, 
		EntryId,
		@UserID
	from 
		#udfs u
		join #emails e on u.EmailID = e.EmailID
	where
		DataFieldSetID is not null 
		and e.SubscribeTypeCode = 'S' 
		and e.Skipped = 0

	--update non-transactional
	update 
		EmailDataValues
	set 
		DataValue = u1.UDFValue, 
		ModifiedDate = @CurrentDate, 
		UpdatedUserID = @UserID
	--select u1.UDFValue, @CurrentDate, @UserID
	from 
		#udfs u1 
		join EmailDataValues edv  WITH (NOLOCK) on u1.UDFID = edv.GroupDatafieldsID and u1.EmailID = edv.EmailID and u1.DataFieldSetID is null
		join #emails e on u1.EmailID = e.EmailID
	where 
		e.SubscribeTypeCode = 'S'
		and e.Skipped = 0

	--insert non-transactional
	insert into EmailDataValues(
		EmailID, 
		GroupDatafieldsID, 
		DataValue, 
		CreatedDate, 
		EntryID, 
		CreatedUserID)
	SELECT DISTINCT
		u1.EmailID, 
		UDFID, 
		UDFValue, 
		@CurrentDate, 
		null, 
		@UserID
	from 
		#udfs u1 
		join #emails e on u1.EmailID = e.EmailID 
		left outer join EmailDataValues edv  WITH (NOLOCK) on u1.UDFID = edv.GroupDatafieldsID and u1.EmailID = edv.EmailID
	where
		u1.DataFieldSetID is null 
		and e.SubscribeTypeCode = 'S'
		and edv.EmailDataValuesID is null
		and e.Skipped = 0
		and u1.Skipped = 0

	drop table #udfs
	drop table #emails

END