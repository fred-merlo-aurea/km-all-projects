CREATE proc [dbo].[sp_Watt_MasterGroup_to_eNewsletters_Update]
(
	@CustomerID int,        	
	@MasterGroupID int,
	@copytoGroupID int,
	@UDF varchar(10)
)
as

BEGIN        
	set NOCOUNT ON        
        
	declare         
		@SqlString  varchar(8000),         
		@EmailString  varchar(8000),        
		@Col1 varchar(8000),        
		@Col2 varchar(8000),
		@dt datetime

 	set @SqlString = ''        

	set @Col1  = ''        
	set @Col2  = '' 
	Set @dt = GETDATE()       


	--select * from groups where groupID in(32532,32533,34367,34686,35421,35473,35474,37021,37022,37023,45851,46592,48976)
	--select * from GroupDatafields where GroupID = 48140 and ShortName not like '%source%'

 	declare @gdf table(GID int, ShortName varchar(50))        
	
	insert into @gdf
	select GroupDatafieldsID, ShortName from GroupDatafields where GroupID = @MasterGroupID and ShortName like '%' + @UDF + '%' order by Shortname
	  	
 	create table #E(EmailID int, GID int, DataValue varchar(500))   	
  	
	insert into #E         
	select EmailDataValues.EmailID, EmailDataValues.GroupDataFieldsID, DataValue
	from EmailDataValues join @gdf g on g.GID = EmailDataValues.GroupDataFieldsID  -- and isnull(datavalue,'') <> ''
	        
	select  	@Col1 = @Col1 + coalesce('max([' + case when RTRIM(ShortName) like '%source%' then 'source' else 'subscribed' end + ']) as ''' + RTRIM(case when RTRIM(ShortName) like '%source%' then 'source' else 'subscribed' end)  + ''',',''),        
			@Col2 = @Col2 + coalesce('Case when E.GID=' + convert(varchar(10),g.GID) + ' then E.DataValue end [' + RTRIM(case when RTRIM(ShortName) like '%source%' then 'source' else 'subscribed' end)  + '],', '')        
	from   @gdf g     
	   
         
	set @Col1 = substring(@Col1, 0, len(@Col1))         
	set @Col2 = substring(@Col2 , 0, len(@Col2))        

	--select @Col1, @col2
	
	create table #mastgroup (EmailID int, IsSubscribed char(1), RecordSource varchar(100))
	
	exec ( ' Insert into #mastgroup (emailID, IsSubscribed, RecordSource)
		select InnerTable2.*' +      
		' from Emails left outer join ' +       
			 '(select InnerTable1.EmailID as tmp_EmailID, ' + @Col1 + ' from  ' +         
				   '(select e.EmailID, ' + @Col2 + ' from #E E) as InnerTable1 ' +         
			  ' Group by InnerTable1.EmailID ' +         
			 ') as InnerTable2 on Emails.EmailID = InnerTable2.tmp_EmailID join ' +        
		' EmailGroups  on EmailGroups.EmailID = Emails.EmailID' +        
		' where Emails.CustomerID = ' + @CustomerID + ' and EmailGroups.GroupID = ' + @MasterGroupID + 
		--' and isnull(lastchanged, createdon) >= convert(datetime, convert(varchar(10),GETDATE()-1,101)) ' + 
		' and isnull(Subscribed,'''') <> '''' and source like ''%webservice%''' + 
		' order by emails.emailaddress;')  
		
		

	--and EmailGroups.SubscribeTypeCode = ''S'' 
	
	if exists (select top 1 * from #mastgroup )
	Begin
		MERGE INTO emailgroups AS Target
			USING (select EmailID , IsSubscribed from #mastgroup) AS Source (EmailID, IsSubscribed)
			ON Target.emailID = Source.emailID and Target.groupID = @copytoGroupID
			WHEN MATCHED THEN
				UPDATE set SubscribeTypeCode = (case when Source.IsSubscribed='Y' then 'S' when Source.IsSubscribed='U' then 'U' when Source.IsSubscribed='N' then 'U' end), 
							LastChanged = @dt
			WHEN NOT MATCHED BY TARGET THEN
				INSERT (EmailID,GroupID,FormatTypeCode,SubscribeTypeCode,CreatedOn,LastChanged)
				values 
				(	Source.EmailID, @copytoGroupID, 'html', (case when Source.IsSubscribed='Y' then 'S' when Source.IsSubscribed='U' then 'U'  when Source.IsSubscribed='N' then 'U' end), 
					@dt, @dt
				);
	End
			
	drop table #E    
	drop table #mastgroup   
END

--select * from groups where CustomerID = 2807
--select * from EmailGroups where groupID = 45851

--select * from emailgroups where emailID = 89882242 and GroupID = 45851
--select * from emaildatavalues edv join groupdatafields gdf on edv.GroupDatafieldsID = gdf.GroupDatafieldsID where emailID = 89882242 and GroupID = 48140

--update EmailDataValues set DataValue = 'Y' where EmailDataValuesID = 533120496
