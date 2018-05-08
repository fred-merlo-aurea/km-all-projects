CREATE proc [dbo].[e_EmailGroup_ImportEmailsToNoThreshold]
(  
 @BaseChannelID int,  
 @xml text,
 @UserID int
)  
as  
Begin  
  
	set nocount on  

	declare @docHandle int  

	declare @result TABLE  (Action varchar(100), Counts int)  

	create TABLE #emails
	(  
		Emailaddress varchar(255),
		SkippedRecord bit, 
		alreadyexists bit  
	)  
  
	EXEC sp_xml_preparedocument @docHandle OUTPUT, @xml  

	insert into #emails (Emailaddress, SkippedRecord, alreadyexists)  
	SELECT LTRIM(RTRIM(Emailaddress)), case when isnull(Emailaddress,'') = '' then 1 else 0 end, 0
	FROM OPENXML(@docHandle, N'/XML/ea')   
	WITH   (Emailaddress varchar(255) '.') 
	
	insert into @result values ('T',@@ROWCOUNT)  
  
	EXEC sp_xml_removedocument @docHandle    

	--Validate email address and mark as Skipped for invalid [Emails]

	Update #emails
	set SkippedRecord = 1
	where dbo.fn_ValidateEmailAddress(Emailaddress) = 0

	insert into @result
	select 'S', @@ROWCOUNT

	update #emails
	set alreadyexists = 1
	from #emails e join ChannelNoThresholdList cms on e.emailaddress = cms.emailaddress 
	where basechannelID = @basechannelID and cms.IsDeleted = 0

	insert into @result values ('U',@@ROWCOUNT)
	
	declare @dt datetime
	set @dt = getdate()

	insert into ChannelNoThresholdList (BasechannelID, Emailaddress, CreatedDate, CreatedUserID, IsDeleted)  
	SELECT @basechannelID, el.EmailAddress, @dt, @UserID, 0
	from #emails el
	Where SkippedRecord = 0  and alreadyexists = 0

	insert into @result values ('I',@@ROWCOUNT)
	 
	drop TABLE #emails
	select * from @result  
END
