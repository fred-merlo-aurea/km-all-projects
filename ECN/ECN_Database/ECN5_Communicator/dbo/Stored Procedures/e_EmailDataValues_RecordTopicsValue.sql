CREATE Procedure [dbo].[e_EmailDataValues_RecordTopicsValue](  
 @BlastID int,
 @EmailID int,
 @Values varchar(2000)  
)    
as   
BEGIN   
	set nocount on    
	  
	DECLARE @GDFID int = 0
	DECLARE @Success int = 0
	
	select @GDFID = gdf.GroupDataFieldsID 
	from 
		GroupDatafields gdf with(nolock)
        join Blast b with (nolock) on gdf.GroupID = b.GroupID
    where 
		b.BlastID = @BlastID and
        gdf.ShortName = 'Topics' and gdf.IsDeleted = 0
        
    if @GDFID > 0
    begin
		create table #Topics (Topic varchar(30))
		CREATE UNIQUE CLUSTERED INDEX Topics_ind on #Topics(Topic) with ignore_dup_key
		
		insert into #Topics select items as Topic from fn_Split(@Values, ',')
		
		INSERT INTO EmailDataValues (EmailID, GroupDataFieldsID, DataValue, ModifiedDate, EntryID)
			SELECT @EmailID, @GDFID, Topic, GETDATE(), NEWID() 
			from #Topics

        drop table #Topics
        
        set @Success = 1
    end
    
    select @Success   
END