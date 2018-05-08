CREATE  proc [dbo].[sp_Watt_eNewsletters_Group_Update]
as

Begin

	set nocount on

	declare @updatedate datetime
	set @updatedate = convert(varchar(10),getdate()-1,101) + ' 23:59:59'

	declare @dt date

	set @dt = GETDATE() - 1

	declare @temails TABLE (emailID int, groupID int, SubscribeTypeCode varchar(50))
	insert into @temails
	select	distinct eg.emailID, eg.groupID, eg.SubscribeTypeCode 
	from emailgroups eg WITH (NOLOCK) join 
			groupdatafields gdf WITH (NOLOCK)  on eg.groupID = gdf.groupID join
			emaildatavalues edv WITH (NOLOCK)  on eg.emailID = edv.emailID and gdf.groupdatafieldsID = edv.groupdatafieldsID
	where	eg.groupID in (32532,32533,34367,34686,35421,35473,35474,37021,37022,37023,45851,46592,48976,127712) 
			and
			(
				CONVERT(Date,Isnull(LastChanged, CreatedOn)) = @dt or
				CONVERT(Date,edv.ModifiedDate) =  @dt
			)

	print '1. completed'
	
	Create table #Watt_eNewsletters_Group_48140  (EmailID	int, GroupID int, scode varchar(10), GdfID int)

	CREATE INDEX IDX_C_Watt_eNewsletters_Group_48140_EmailID_GdfID ON #Watt_eNewsletters_Group_48140(EmailID, GdfID)

	insert into #Watt_eNewsletters_Group_48140
	select EmailID, groupID, SubscribeTypeCode,
		case	when groupID = 32532 then 57399
				when groupID = 32533 then 57407
				when groupID = 34367 then 57400
				when groupID = 34686 then 57405
				when groupID = 35421 then 57404
				when groupID = 35473 then 57402
				when groupID = 35474 then 57403
				when groupID = 37021 then 57397
				when groupID = 37022 then 57398 
				when groupID = 37023 then 57401
				when groupID = 45851 then 57396
				when groupID = 46592 then 57406
				when groupID = 48976 then 64512
				when groupID = 127712 then 216911
		end
	from @temails
	
	
	--select * from GroupDatafields where GroupID = 48140

	/* INSERT INTO EMAIL GROUPS */
	insert into EmailGroups (EmailID,GroupID,FormatTypeCode,SubscribeTypeCode,CreatedOn,LastChanged)
	select distinct p.EmailID, 48140, 'html', 'S', @updatedate, @updatedate
	from	#Watt_eNewsletters_Group_48140 p left outer join
			emailgroups eg WITH (NOLOCK)  on eg.emailID = p.emailID and eg.groupID = 48140
	where	eg.emailgroupID is null

	print ('Insert Emailgroups : ' + Convert(varchar(100), @@ROWCOUNT))

	update emailgroups
	set LastChanged = @updatedate
	from emailgroups eg join #Watt_eNewsletters_Group_48140 w on eg.emailID = w.emailID and eg.GroupID = 48140
	

	-- Newsletter group to master group -- update
	update emaildatavalues
	set datavalue = case when scode = 'S' then 'Y' when scode = 'U' then 'U' else '' end, ModifiedDate = @updatedate
	from emaildatavalues edv join #Watt_eNewsletters_Group_48140 p on edv.emailID = p.emailID and edv.groupdatafieldsID = p.GdfID

	print ('update EDV : ' + Convert(varchar(100), @@ROWCOUNT))

	-- Newsletter group to master group -- insert

	insert into emaildatavalues (EmailID, GroupDatafieldsID, DataValue, ModifiedDate, SurveyGridID,EntryID)
	select p.emailID, p.gdfID, (case when scode = 'S' then 'Y' when scode = 'U' then 'U' else '' end), @updatedate, NULL, NULL 
	from #Watt_eNewsletters_Group_48140 p left outer join  emaildatavalues edv WITH (NOLOCK)   on edv.emailID = p.emailID and edv.groupdatafieldsID = p.GdfID
	where edv.EmaildatavaluesID is null 

	print ('Insert EDV : ' + Convert(varchar(100), @@ROWCOUNT))
	
	drop table #Watt_eNewsletters_Group_48140
  End
