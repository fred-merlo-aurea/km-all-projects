CREATE PROCEDURE [dbo].[spGreatClipsSelectWarmProspects]
AS
declare
	@CustomerID int,        	
	@GroupID int

set		@CustomerID = 2519
set 	@GroupID = 26547

BEGIN        
	set NOCOUNT ON        
        
	declare         
		@SqlString  varchar(8000),         
		@EmailString  varchar(8000),        
		@Col1 varchar(8000),        
		@Col2 varchar(8000),      
		@topcount varchar(10),
		@emailcolumns varchar(2000),
		@emailsubject varchar(1000),
		@Filter varchar(8000),
		@layoutID int,
		@TestBlast  varchar(5)  ,
		@selectslotstr varchar(8000),
		@BasechannelID int,
		@DynamicFromName	varchar(100),       
		@DynamicFromEmail	varchar(100),      
		@DynamicReplyToEmail  varchar(100),
		@CurrDate varchar(10),
		@LastVisitDate datetime    


 	set @SqlString = ''        
	set @EmailString  = ''        
	set @Col1  = ''        
	set @Col2  = ''        
	set @emailcolumns = ''
	set @Filter = ''


 	declare @gdf table(GID int, ShortName varchar(50))        
	
	insert into @gdf
	select GroupDatafieldsID, ShortName from GroupDatafields where GroupID = @GroupID 

 	create table #E(EmailID int, GID int, DataValue varchar(500), EntryID uniqueidentifier)        
  	
	  insert into #E        
	  select EmailDataValues.EmailID, EmailDataValues.GroupDataFieldsID, DataValue, EntryID
	  from EmailDataValues join @gdf g on g.GID = EmailDataValues.GroupDataFieldsID  -- and isnull(datavalue,'') <> ''
	        
	  select   	@Col1 = @Col1 + coalesce('max([' + RTRIM(ShortName)  + ']) as ''' + RTRIM(ShortName)  + ''',',''),        
	    		@Col2 = @Col2 + coalesce('Case when E.GID=' + convert(varchar(10),g.GID) + ' then E.DataValue end [' + RTRIM(ShortName)  + '],', '')
	    		     
	  from   @gdf g        
         
	  set @Col1 = substring(@Col1, 0, len(@Col1))         
	  set @Col2 = substring(@Col2 , 0, len(@Col2))        

	--select @Col1, @col2

				exec ( ' 
					select Emails.*, FormatTypeCode, SubscribeTypeCode, InnerTable2.* into #tmpWarmProspects' +      
					' from Emails left outer join ' +       
						 '(select InnerTable1.EmailID as tmp_EmailID, InnerTable1.EntryID, ' + @Col1 + ' from  ' +         
							   '(select e.EmailID, E.EntryID, ' + @Col2 + ' from #E E) as InnerTable1 ' +         
						  ' Group by InnerTable1.EmailID, InnerTable1.EntryID ' +         
						 ') as InnerTable2 on Emails.EmailID = InnerTable2.tmp_EmailID join ' +        
					' EmailGroups  on EmailGroups.EmailID = Emails.EmailID' +        
					' where Emails.CustomerID = ' + @CustomerID + ' and EmailGroups.GroupID = ' + @GroupID + ' 
					--and convert(varchar(10),isnull(LastChanged, CreatedOn),101) = convert(varchar(10), getdate()-1, 101);
					
					select EmailID as EcnEmailID
					      ,EmailAddress
					      ,FirstName
					      ,LastName
					      ,Address as Address1
					      ,Address2
					      ,City
					      ,State
					      ,Zip
					      ,DateAdded as EcnEmailAdded
					      ,DateUpdated as EcnEmailUpdated
					      ,VISITS as NumberOfVisits
					      ,SalonNumber
					      ,CASE WHEN SubscribeTypeCode = ''S'' THEN 1 ELSE 0 END as IsActive
					      
					
					
					
					 from #tmpWarmProspects order by emailaddress
					
					drop table #tmpWarmProspects
					
					
					')  
				
	drop table #E        

END
