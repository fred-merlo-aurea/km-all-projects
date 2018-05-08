
CREATE proc [dbo].[spAdvanstarEngagementFilter_LastOpenDate]
as
Begin

	declare @dt datetime,
			@groupID int,
			@EmailID int,
			@openTime varchar(10),
			@lastopendateUDFID int

	
	set @dt =  GETDATE() 

	set nocount on
	
	DECLARE c_cursor CURSOR FOR 
		select 
			EmailID, 
			GroupID, 
			convert(varchar(10), max(bao.OpenTime), 101) 
		from 
			ECN_ACTIVITY..BlastActivityOpens bao WITH (NOLOCK) 
			join [BLAST] b WITH (NOLOCK) on b.BlastID = bao.BlastID
		where 
			CustomerID in (select customerID from [ECN5_ACCOUNTS].[DBO].[CUSTOMER]  where BaseChannelID in (65, 77)) 
			and TestBlast = 'n' 
			and Blasttype <> 'layout'
			and	OpenTime >=  convert(date, GETDATE()-1)
		group by EmailID, GroupID

	OPEN c_cursor  

	FETCH NEXT FROM c_cursor INTO @EmailID, @groupID, @openTime

	WHILE @@FETCH_STATUS = 0  
	BEGIN  
		set @lastopendateUDFID = 0
		
		select @lastopendateUDFID = isnull(GroupDatafieldsID,0) from GroupDatafields WITH (NOLOCK)  where GroupID = @groupID and ShortName = 'lastopendate'
		
		if @lastopendateUDFID= 0
		Begin
			insert into ecn5_communicator.dbo.GroupDatafields (GroupID,ShortName,LongName,SurveyID,DatafieldSetID,IsPublic,IsPrimaryKey)
			values (@groupID, 'lastopendate','lastopendate', null, null, 'N', 0)
			
			set @lastopendateUDFID = @@IDENTITY
		End
		
		MERGE ECN5_COMMUNICATOR.DBO.EMAILDATAVALUES AS target
		USING (SELECT @EmailID, @lastopendateUDFID) AS source (EmailID, GroupdatafieldsID) 
		ON TARGET.EmailID = SOURCE.EmailID and TARGET.GroupdatafieldsID = SOURCE.GroupdatafieldsID
		WHEN NOT MATCHED THEN
			INSERT (EmailID,GroupDatafieldsID,DataValue,ModifiedDate,SurveyGridID,EntryID)
			values 
					(Source.EmailID, Source.GroupdatafieldsID, @openTime, @dt, null, null)
		WHEN MATCHED THEN
			UPDATE SET DATAVALUE = @openTime, ModifiedDate = @DT;
		
		FETCH NEXT FROM c_cursor INTO @EmailID, @groupID, @openTime
	END

	CLOSE c_cursor  
	DEALLOCATE c_cursor  
	
End