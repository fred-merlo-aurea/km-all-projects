
CREATE proc [dbo].[spAdvanstarEngagementFilter_LastClickDate]
as
Begin

	declare @dt datetime,
			@groupID int,
			@EmailID int,
			@clickTime varchar(10),
			@lastclickdateUDFID int

	
	set @dt =  GETDATE() 

	set nocount on
	
	DECLARE c_cursor CURSOR FOR 
		select 
			EmailID, 
			GroupID, 
			convert(varchar(10), max(bac.ClickTime), 101) 
		from 
			ECN_ACTIVITY..BlastActivityClicks bac WITH (NOLOCK) 
			join [BLAST] b WITH (NOLOCK) on b.BlastID = bac.BlastID
		where 
			CustomerID in (select customerID from [ECN5_ACCOUNTS].[DBO].[CUSTOMER]  where BaseChannelID in (65, 77)) 
			and TestBlast = 'n' 
			and Blasttype <> 'layout'
			and	ClickTime >=  convert(date, GETDATE()-1)
		group by EmailID, GroupID

	OPEN c_cursor  

	FETCH NEXT FROM c_cursor INTO @EmailID, @groupID, @clickTime

	WHILE @@FETCH_STATUS = 0  
	BEGIN  
		set @lastclickdateUDFID = 0
		
		select @lastclickdateUDFID = isnull(GroupDatafieldsID,0) from GroupDatafields WITH (NOLOCK)  where GroupID = @groupID and ShortName = 'lastclickdate'
		
		if @lastclickdateUDFID= 0
		Begin
			insert into ecn5_communicator.dbo.GroupDatafields (GroupID,ShortName,LongName,SurveyID,DatafieldSetID,IsPublic,IsPrimaryKey)
			values (@groupID, 'lastclickdate','lastclickdate', null, null, 'N', 0)
			
			set @lastclickdateUDFID = @@IDENTITY
		End
		
		MERGE ECN5_COMMUNICATOR.DBO.EMAILDATAVALUES AS target
		USING (SELECT @EmailID, @lastclickdateUDFID) AS source (EmailID, GroupdatafieldsID) 
		ON TARGET.EmailID = SOURCE.EmailID and TARGET.GroupdatafieldsID = SOURCE.GroupdatafieldsID
		WHEN NOT MATCHED THEN
			INSERT (EmailID,GroupDatafieldsID,DataValue,ModifiedDate,SurveyGridID,EntryID)
			values 
					(Source.EmailID, Source.GroupdatafieldsID, @clickTime, @dt, null, null)
		WHEN MATCHED THEN
			UPDATE SET DATAVALUE = @clickTime, ModifiedDate = @DT;
		
		FETCH NEXT FROM c_cursor INTO @EmailID, @groupID, @clickTime
	END

	CLOSE c_cursor  
	DEALLOCATE c_cursor  
	
End