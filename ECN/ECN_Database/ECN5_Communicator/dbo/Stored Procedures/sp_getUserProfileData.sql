CREATE  PROCEDURE dbo.sp_getUserProfileData(   
	@GroupID int,        
	@EmailID int)
	
	--set @GroupID = 52958
	--set @EmailID = 111011784
    
AS
BEGIN 
      
	set NOCOUNT ON      
      
	declare @SqlString  varchar(8000)
     
	set @SqlString = ''    
	  
	create table #g
	(      
		GID int,      
		GroupID int,      
		ShortName varchar(50),      
		LongName varchar(255),      
		SurveyID int,      
		DatafieldSetID int    
	)      
	  
   
	  
	  
	insert	into #g       
	select	GroupDatafieldsID, GroupID, ShortName, LongName, SurveyID, DatafieldSetID     
	from	groupdatafields      
	where	GroupDatafields.groupID = @GroupID and IsDeleted = 0    
      
	if not exists(select * from #g)
		select e.* from [Emails] e join [EmailGroups] eg on e.EmailID = eg.EmailID where eg.GroupID = @GroupID and eg.emailID = @EmailID
	Else --if UDF's exists      
	Begin      
		DECLARE @StandAloneUDFs VARCHAR(2000)
		SELECT  @StandAloneUDFs = STUFF(( SELECT DISTINCT '],[' + ShortName FROM groupdatafields WHERE GroupID = @GroupID and IsDeleted = 0  AND DatafieldSetID is null ORDER BY '],[' + ShortName FOR XML PATH('') ), 1, 2, '') + ']'
		DECLARE @TransactionalUDFs VARCHAR(2000)
		SELECT  @TransactionalUDFs = STUFF(( SELECT DISTINCT '],[' + ShortName FROM groupdatafields WHERE GroupID = @GroupID and IsDeleted = 0  AND DatafieldSetID > 0 ORDER BY '],[' + ShortName FOR XML PATH('') ), 1, 2, '') + ']'       
		
		declare @sColumns varchar(200),
				@tColumns varchar(200),
				@standAloneQuery varchar(4000),
				@TransactionalQuery varchar(4000)
				
		if LEN(@standaloneUDFs) > 0
		Begin
			set @sColumns = ', SAUDFs.* '
			set @standAloneQuery= ' left outer join			
				(
					SELECT *
					 FROM
					 (
						SELECT edv.emailID as tmp_EmailID,  gdf.ShortName, edv.DataValue
						from	[EMAILDATAVALUES] edv  join  
								Groupdatafields gdf on edv.GroupDatafieldsID = gdf.GroupDatafieldsID
						where 
								gdf.GroupID = ' + convert(varchar(15), @GroupID) + ' and datafieldsetID is null and gdf.IsDeleted = 0  and edv.emailID = ' + convert(varchar(15), @EmailID) + '
					 ) u
					 PIVOT
					 (
					 MAX (DataValue)
					 FOR ShortName in (' + @StandAloneUDFs + ')) as pvt 			
				) 
				SAUDFs on [Emails].emailID = SAUDFs.tmp_EmailID'
		End
		if LEN(@TransactionalUDFs) > 0
		Begin

			set @tColumns = ', TUDFs.* '
			set @TransactionalQuery= '  left outer join
			(
				SELECT *
				 FROM
				 (
					SELECT edv.emailID as tmp_EmailID1, edv.entryID, gdf.ShortName, edv.DataValue
					from	[EMAILDATAVALUES] edv  join  
							Groupdatafields gdf on edv.GroupDatafieldsID = gdf.GroupDatafieldsID
					where 
							gdf.GroupID = ' + convert(varchar(15), @GroupID) + ' and datafieldsetID > 0 and gdf.IsDeleted = 0  and edv.emailID = ' + convert(varchar(15), @EmailID) + '
				 ) u
				 PIVOT
				 (
				 MAX (DataValue)
				 FOR ShortName in (' + @TransactionalUDFs + ')) as pvt 			
			) 
			TUDFs on [Emails].emailID = TUDFs.tmp_EmailID1 '
		End
	    

     
  exec (	' select top 1 [Emails].* ' + @sColumns + @tColumns +     
			' from [Emails] 
				join ' +      
			' [EmailGroups] on [EmailGroups].EmailID = [Emails].EmailID ' + @standAloneQuery + @TransactionalQuery +    
			' where [Emails].emailID = ' + @emailID + ' and [EmailGroups].GroupID = ' + @GroupID)      
      
      
  END      
         
 drop table #G

END