CREATE  PROCEDURE [dbo].[spGreatClipsWarmProspectImport](
	@CustomerID int,        	
	@GroupID int)
AS
BEGIN 
--set		@CustomerID = 2519
--set 	@GroupID = 26547

      
	set NOCOUNT ON        
        
	declare         
		@SqlString  varchar(8000),         
		@EmailString  varchar(8000),     
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
	set @emailcolumns = ''
	set @Filter = ''

				DECLARE @StandAloneUDFs VARCHAR(2000)
		SELECT  @StandAloneUDFs = STUFF(( SELECT DISTINCT '],[' + ShortName FROM groupdatafields WHERE GroupID = @GroupID AND DatafieldSetID is null ORDER BY '],[' + ShortName FOR XML PATH('') ), 1, 2, '') + ']'
		DECLARE @TransactionalUDFs VARCHAR(2000)
		SELECT  @TransactionalUDFs = STUFF(( SELECT DISTINCT '],[' + ShortName FROM groupdatafields WHERE GroupID = @GroupID AND DatafieldSetID > 0 ORDER BY '],[' + ShortName FOR XML PATH('') ), 1, 2, '') + ']'       
		
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
						from	EmailDataValues edv  join  
								Groupdatafields gdf on edv.GroupDatafieldsID = gdf.GroupDatafieldsID
						where 
								gdf.GroupID = ' + convert(varchar(15), @GroupID) + ' and datafieldsetID is null
					 ) u
					 PIVOT
					 (
					 MAX (DataValue)
					 FOR ShortName in (' + @StandAloneUDFs + ')) as pvt 			
				) 
				SAUDFs on Emails.emailID = SAUDFs.tmp_EmailID'
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
					from	EmailDataValues edv  join  
							Groupdatafields gdf on edv.GroupDatafieldsID = gdf.GroupDatafieldsID
					where 
							gdf.GroupID = ' + convert(varchar(15), @GroupID) + ' and datafieldsetID > 0
				 ) u
				 PIVOT
				 (
				 MAX (DataValue)
				 FOR ShortName in (' + @TransactionalUDFs + ')) as pvt 			
			) 
			TUDFs on Emails.emailID = TUDFs.tmp_EmailID1 '
		End

 	        


				exec ( ' 
					select Emails.*, FormatTypeCode, SubscribeTypeCode '  + @sColumns + @tColumns + ' into #tmpWarmProspects' +      
					' from Emails join ' +       
					' EmailGroups  on EmailGroups.EmailID = Emails.EmailID ' + @standAloneQuery + @TransactionalQuery +        
					' where Emails.CustomerID = ' + @CustomerID + ' and EmailGroups.GroupID = ' + @GroupID + ' 
					and convert(varchar(10),isnull(LastChanged, CreatedOn),101) = convert(varchar(10), getdate()-1, 101);
					
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
				










END
