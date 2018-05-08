CREATE proc [dbo].[sp_importEmailsToCS]
(  
 @BaseChannelID int,  
 @xml text
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

	--Validate email address and mark as Skipped for invalid emails

	Update #emails
	set SkippedRecord = 1
	where 
		NOT  (   
				CHARINDEX ( ' ',Emailaddress) = 0  AND  -- No embedded spaces
				CHARINDEX ( '''',Emailaddress) = 0  AND  -- No single quotes  
				CHARINDEX ( '"',Emailaddress) = 0  AND  -- No single quotes  
				CHARINDEX ( '(',Emailaddress) = 0  AND  -- No ( brackets
				CHARINDEX ( ')',Emailaddress) = 0  AND  -- No ) brackets
				CHARINDEX ( '.',Emailaddress,CHARINDEX ( '@',Emailaddress)) - CHARINDEX ( '@',Emailaddress) > 1 AND  -- There must be a '.' after '@'
				LEFT(Emailaddress,1) <> '@' AND -- '@' can't be the first character of an email address  
				RIGHT(Emailaddress,1) <> '.' AND -- '.' can't be the last character of an email address
				LEN(Emailaddress) - LEN(REPLACE(Emailaddress,'@','')) = 1 AND   -- Only one '@' sign is allowed
				CHARINDEX ( '.',REVERSE(Emailaddress)) >= 3 AND    -- Domain name should end with at least 2 character extension
				CHARINDEX ('.@',Emailaddress) = 0 AND CHARINDEX ( '..',Emailaddress) = 0 -- can't have patterns like '.@' and '..'
			)  

	insert into @result
	select 'S', @@ROWCOUNT

	update #emails
	set alreadyexists = 1
	from #emails e join ChannelMasterSuppressionList cms on e.emailaddress = cms.emailaddress 
	where basechannelID = @basechannelID

	insert into @result values ('U',@@ROWCOUNT)
	
	declare @dt datetime
	set @dt = getdate()

	insert into ChannelMasterSuppressionList (BasechannelID, Emailaddress, CreatedDate)  
	SELECT @basechannelID, el.EmailAddress, @dt
	from #emails el
	Where SkippedRecord = 0  and alreadyexists = 0

	insert into @result values ('I',@@ROWCOUNT)
	 
	drop TABLE #emails
	select * from @result  
END
