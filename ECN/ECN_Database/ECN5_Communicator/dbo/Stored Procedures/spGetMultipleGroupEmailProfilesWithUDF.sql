CREATE PROCEDURE [dbo].[spGetMultipleGroupEmailProfilesWithUDF](
	@GroupID VARCHAR(4000),    
	@FromDate varchar(10),
	@ToDate varchar(10))
AS
BEGIN
	 
--declare
--	@GroupID VARCHAR(4000),    
--	@FromDate varchar(10),
--	@ToDate varchar(10)

--BEGIN
--	set @GroupID = 49449
--	set @FromDate = ''
--	set @ToDate = ''
	 
	 
  set NOCOUNT ON  
   
  declare @SubscribeType varchar(100),
		  @filter varchar(1000) 
--set @GroupID = '52958,52957'
--set @GroupID = '19972,17865,17870,17868,24185,18587,17877,15282,18049,18361,18050,17866,39024,23228,18261,17871,18204,17879,17867,36447,21724,21143,18474,21624,18510,18511,21724,21726,18362,18364,31695,18512,17876,18095,18096,36950,36013,18410,18731,18507,21809,18159,18098,18097,18100,22547,22548,22549,22906,32332,36447,38943,31047'

  set @SubscribeType = '''S'''
  set @filter = ''
  
  if LEN(@FromDate) > 0 and LEN(@ToDate) > 0
	 set @filter = ' and isnull(EmailGroups.LastChanged, EmailGroups.CreatedOn) between ''' + @FromDate + ''' and ''' + @ToDate + ' 23:59:59'''
			   
  declare   
	@Col1 varchar(8000),  
	@Col2 varchar(8000)  
  
	set @Col1  = ''  
	set @Col2  = ''  
 
	select  @Col1 = @Col1 + coalesce(',max([' + RTRIM(ShortName)  + ']) as ''' + RTRIM(ShortName) + '''',''),        
			@Col2 = @Col2 + coalesce(',Case when shortname=''' + convert(varchar(10),inn1.shortname) + ''' then DataValue end [' + RTRIM(ShortName)  + ']', '')        
	from 
	(select distinct shortname from groupdatafields GDF where GDF.groupID in (select items from dbo.fn_Split(@GroupID, ','))) inn1

    
    if (LEN(@col1) > 0)
    Begin
			
		DECLARE @StandAloneUDFs VARCHAR(2000)
		SELECT  @StandAloneUDFs = STUFF(( SELECT DISTINCT '],[' + ShortName FROM groupdatafields WHERE GroupID in (select items from dbo.fn_Split(@GroupID, ',')) AND DatafieldSetID is null ORDER BY '],[' + ShortName FOR XML PATH('') ), 1, 2, '') + ']'
		DECLARE @TransactionalUDFs VARCHAR(2000)
		SELECT  @TransactionalUDFs = STUFF(( SELECT DISTINCT '],[' + ShortName FROM groupdatafields WHERE GroupID in (select items from dbo.fn_Split(@GroupID, ',')) AND DatafieldSetID > 0 ORDER BY '],[' + ShortName FOR XML PATH('') ), 1, 2, '') + ']'       
		
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
						SELECT gdf.groupID as SUDF_GID, edv.emailID as tmp_EmailID,  gdf.ShortName, edv.DataValue
						from	EmailDataValues edv  join  
								Groupdatafields gdf on edv.GroupDatafieldsID = gdf.GroupDatafieldsID
						where 
								gdf.GroupID in (' + @GroupID + ')  and datafieldsetID is null
					 ) u
					 PIVOT
					 (
					 MAX (DataValue)
					 FOR ShortName in (' + @StandAloneUDFs + ')) as pvt 			
				) 
				SAUDFs on Emails.emailID = SAUDFs.tmp_EmailID  and emailgroups.groupID = SAUDFs.SUDF_GID '
		End
		if LEN(@TransactionalUDFs) > 0
		Begin

			set @tColumns = ', TUDFs.* '
			set @TransactionalQuery= '  left outer join
			(
				SELECT *
				 FROM
				 (
					SELECT gdf.groupID As TUDF_GID, edv.emailID as tmp_EmailID1, edv.entryID, gdf.ShortName, edv.DataValue
					from	EmailDataValues edv  join  
							Groupdatafields gdf on edv.GroupDatafieldsID = gdf.GroupDatafieldsID
					where 
							gdf.GroupID in (' + @GroupID + ')  and datafieldsetID > 0
				 ) u
				 PIVOT
				 (
				 MAX (DataValue)
				 FOR ShortName in (' + @TransactionalUDFs + ')) as pvt 			
			) 
			TUDFs on Emails.emailID = TUDFs.tmp_EmailID1  and emailgroups.groupID = TUDFs.TUDF_GID '
		End
	
	EXEC (' select Emails.EmailID, EmailAddress, Title, FirstName, LastName, FullName, Company, Occupation, Address, Address2, ' +  
			' City, State, Zip, Country, Voice, Mobile, Fax, Website, Age, Income, Gender, ' +  
			' User1, User2, User3, User4, User5, User6, Birthdate, UserEvent1, UserEvent1Date, UserEvent2, UserEvent2Date, ' +  
			' Convert(varchar,Notes) as Notes, CreatedOn, LastChanged, FormatTypeCode, SubscribeTypeCode, Emailgroups.GroupId ' + @sColumns + @tColumns +  
			' from Emails with (NOLOCK) join EmailGroups with (NOLOCK) on EmailGroups.EmailID = Emails.EmailID ' + @standAloneQuery + @TransactionalQuery +  
			' where EmailGroups.SubscribeTypeCode = ''S'''+  
			' and EmailGroups.GroupID IN (' + @GroupID + ')' + @Filter)
						

	
END

END
