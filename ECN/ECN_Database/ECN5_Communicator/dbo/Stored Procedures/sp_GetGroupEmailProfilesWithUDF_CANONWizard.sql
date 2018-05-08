CREATE proc [dbo].[sp_GetGroupEmailProfilesWithUDF_CANONWizard] 
(    
	@folderID int,    
	@CustomerID int,  	
	@SubscribeType varchar(100),
	@shortName varchar(100) 
)    
as    
BEGIN  
  set NOCOUNT ON  
   
    declare @groupFilter varchar(20)
    
    set @groupFilter =  '%adhoc%' 
    
	declare @gdf table(groupID int, GID int, ShortName varchar(50)) 
  
	Create table #E(EmailID int, GID int, DataValue varchar(500), EntryID uniqueidentifier) 
	CREATE UNIQUE CLUSTERED INDEX E_ind on  #E(EmailID,EntryID,GID) with ignore_dup_key
 
	insert into @gdf   
	select g.groupID, gdf.GroupDatafieldsID, gdf.ShortName 
	from Groups g left outer join groupdatafields gdf on g.GroupID = gdf.groupID
	where folderID = @folderID and ShortName = @shortName
    
	if not exists(select GID from @gdf)  
	Begin  
		SELECT distinct e.EmailAddress, eg.SubscribeTypeCode 
		from	Groups g join emailgroups eg on g.groupID = eg.groupID join Emails e on e.EmailID = eg.emailID 
		where	FolderID =  @folderID  and 
				GroupName not like @groupFilter and 
				eg.SubscribeTypeCode = @SubscribeType  
	End  
	Else --if UDF's exists  
	Begin  

		--select * from @gdf
		--select * from #e
  
		 select EmailAddress, SubscribeTypeCode, case when isnull(AcctID,'') = '' then '' else AcctID end as AcctID
		 from Emails left outer join  
		(select EmailID,  max(DataValue) as AcctID from EmailDataValues join @gdf g on g.GID = EmailDataValues.GroupDataFieldsID group by EmailID) 
		 as inn1 on inn1.EmailID = Emails.emailID
		 join EmailGroups on EmailGroups.EmailID = Emails.EmailID join Groups on Groups.GroupID = EmailGroups.GroupID 
		 where Emails.CustomerID = @CustomerID and EmailGroups.SubscribeTypeCode = @SubscribeType and Groups.FolderID = @FolderID   
	END   

	drop table #E  

END


 --' select Emails.EmailID, E.EntryID, ' + @Col2 +  
 --      ' from Groups join EmailGroups on Groups.groupID = EmailGroups.groupID join  Emails on Emails.EmailID = EmailGroups.EmailID join ' +   
 --      ' #E E on Emails.EmailID = E.EmailID join ' +   
 --      ' #g G on E.groupDataFieldsID = G.GID AND G.GroupID = Groups.GroupID ' +   
 --      ' where Groups.groupID = ' + @GroupID + ' and Groups.CustomerID = ' + @CustomerID + ' and EmailGroups.SubscribeTypeCode IN ('+@SubscribeType+') ) as InnerTable1 ' +   
 --      ' Group by InnerTable1.EmailID, InnerTable1.EntryID' +   
 --   ')
