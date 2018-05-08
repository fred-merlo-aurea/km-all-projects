CREATE proc [dbo].[sp_KMPS_FlashReporting] (  
 @pubGroupID int,  
 @CustomerID int,  
 @PromoCode varchar(10),  
 @fromdt varchar(10),  
 @todt varchar(10)   
)  
as   
 
Begin	

	declare @gdfid int  
   
	select @gdfid = GroupDataFieldsID from ECN5_COMMUNICATOR..groupdatafields with(nolock)  where ECN5_COMMUNICATOR..GroupDatafields.groupID = @pubGroupID and ShortName = 'DEMO39'  

	if len(@fromdt) = 0
		set @fromdt = '1/1/2005'

	if len(@todt) = 0
		set @todt = convert(varchar(10), dateadd(d,1,getdate()), 101)
  
	create table #E(EmailID int, DataValue varchar(500), ModifiedDate datetime)  
  
	if @PromoCode = ''  
	Begin  
		insert into #E   
		select	EmailID, DataValue, isNull(ModifiedDate, CreatedDate)   
		from	ECN5_COMMUNICATOR..EmailDataValues with(nolock)
		Where	GroupDataFieldsID = @gdfid  and
				cast(isnull(ModifiedDate, CreatedDate) as date) between @fromdt and @todt
  
 /* 
		insert into #E   
		select	EmailID, '', (select top 1 max(ModifiedDate) from emaildatavalues where emailID = emailgroups.emailID)   
		from	emailgroups 
		where	groupID = @pubGroupID and 
				EmailID NOT IN (select EmailID from #E)
*/

		insert into #E  
		select	eg.EmailID, '', max(isNull(ModifiedDate, CreatedDate))
		from	ECN5_COMMUNICATOR..emailgroups eg with(nolock) left outer join ECN5_COMMUNICATOR..emaildatavalues edv with(nolock) on eg.emailID = edv.emailID   and GroupDatafieldsID in (select GroupDatafieldsID from ECN5_COMMUNICATOR..GroupDatafields with(nolock) where GroupID = @pubGroupID)  
		where	groupID = @pubGroupID and edv.GroupDatafieldsID = @gdfid and
				eg.EmailID NOT IN (select EmailID from #E)
		group by eg.emailID
		
		  
		select	RTRIM(LTrim(DataValue)) as 'PromoCode', 
				count(Emails.EmailID) as TotalEmails,   
				COUNT(CASE WHEN EmailAddress NOT LIKE '%.kmpsgroup.com' THEN Emails.emailID end) as UniqueEmails   
				--(select count(Emails.EmailID) from Emails join #E tempE on Emails.EMailID = tempE.EmailID where EmailAddress NOT LIKE '%.kmpsgroup.com' AND tempE.DataValue = #E.DataValue AND ModifiedDate between @fromdt and @todt) as UniqueEmails   
		from 
				#E e JOIN ECN5_COMMUNICATOR..Emails with(nolock) on Emails.EMailID = e.EmailID   
		where 
				cast(isNull(ModifiedDate, @fromdt) as date) between @fromdt and @todt  
		group by 
				DataValue ORDER BY DataValue  
				
	end  
	else  
	begin  
		insert into #E   
		select EmailID, DataValue, isNull(ModifiedDate, CreatedDate) 
		from ECN5_COMMUNICATOR..EmailDataValues edv with(nolock) Where GroupDataFieldsID = @gdfid AND edv.DataValue = @PromoCode 
		and	cast(ISNULL(ModifiedDate,CreatedDate) as date) between @fromdt and @todt 				
				 
		select	RTRIM(LTrim(@PromoCode)) as 'PromoCode',
				COUNT(E.eMAILid)  as TotalEmails,     
				COUNT(CASE WHEN EmailAddress NOT LIKE '%.kmpsgroup.com' THEN Emails.emailID end) as UniqueEmails   
				
		FROM #E E JOIN ECN5_COMMUNICATOR..Emails with(nolock) on Emails.EMailID = e.EmailID   				
	end  
  
	drop table #E  
  
END
