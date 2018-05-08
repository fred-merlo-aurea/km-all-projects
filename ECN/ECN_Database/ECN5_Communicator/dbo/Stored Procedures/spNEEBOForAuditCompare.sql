CREATE proc [dbo].[spNEEBOForAuditCompare]
as
BEGIN

	declare 
		@CustomerID int, 
		@GroupID int

	set	@CustomerID = 1041      
	set	@GroupID = 35894

      
	set NOCOUNT ON        

	declare @ms table (emailaddress varchar(100), UNIQUE CLUSTERED (emailaddress))
	
	insert into @ms
	select EmailAddress from ECN5_COMMUNICATOR..Emails e with (NOLOCK) join ECN5_COMMUNICATOR..EmailGroups eg with (NOLOCK) on e.EmailID = eg.EmailID where groupID = 2000
	union 
	select EmailAddress from ECN5_COMMUNICATOR.dbo.ChannelMasterSuppressionList with (NOLOCK) where BaseChannelID = 25

    declare @gdf table(GID int, ShortName varchar(50))        
      
    insert into @gdf
      select GroupDatafieldsID, ShortName from GroupDatafields where GroupID = @GroupID and DatafieldSetID is not null    
                        
	declare @trans table (EmailID int, EntryID varchar(36), Storenumber int, ReturnDate date, Book_returned bit)
	
	insert into @trans
	SELECT EmailID, EntryID, CONVERT(int,storenumber), CONVERT(date, RETURNDATE), ISNULL(Book_returned,0)
	 FROM
	 (
		SELECT edv.emailID as EmailID, edv.entryID, gdf.ShortName, edv.DataValue
		from	EmailDataValues edv  join groupdatafields gdf on edv.groupdatafieldsID = gdf.groupdatafieldsID
		where 
				gdf.groupdatafieldsID in (38567,38575,40181)
	 ) u
	 PIVOT
	 (
		 MAX (DataValue)
		 FOR ShortName in (STORENUMBER,RETURNDATE,BOOK_RETURNED)
	 ) as pvt 	             
	
	declare @neMS table(Storenumber int, ReturnDate date, mscount int, UNIQUE CLUSTERED (Storenumber, ReturnDate))   
	 
	insert into @neMS
	select convert(int, Storenumber), CONVERT(date, returndate), COUNT(*)
	from ecn_TEmp..DataSet_OLD
	where IsMasterSuppressed = 1
	group by   convert(int, Storenumber), CONVERT(date, returndate);
	 
	With rptAudit (Storenumber, ReturnDate, Counts, EmailCounts, MsCount)
	as
	(
		select	t.StoreNumber, 
				Convert(date,t.RETURNDATE) as returndate,  
				Count(distinct case when ms.emailaddress IS null then EntryID end ) as counts, 
				Count(distinct case when ms.emailaddress IS null then emails.EmailID end ) as Emailcounts, 
				isnull(ne.mscount,0) as MScount
		from 
				Emails join 
				EmailGroups  on EmailGroups.EmailID = Emails.EmailID left outer join 
				@trans t on Emails.emailID = t.EmailID left outer join
				@ms ms on ms.emailaddress = emails.EmailAddress left outer join
				@neMS ne on ne.Storenumber = t.Storenumber and ne.ReturnDate = t.ReturnDate
		where Emails.CustomerID = @CustomerID and EmailGroups.GroupID = @GroupID  and t.storenumber > 0 and BOOK_RETURNED <> 1
		group by t.StoreNumber, t.ReturnDate, isnull(ne.mscount,0)
	)
	select * from rptAudit
	union all
	select n.Storenumber, n.returndate, 0, 0, n.mscount
	from @neMS n left outer join rptAudit r on n.Storenumber = r.Storenumber and n.ReturnDate = r.ReturnDate
	where r.Storenumber is null
	order by 1, 2
                        
	--select	StoreNumber, 
	--		Convert(date,RETURNDATE) as returndate,  
	--		Count(distinct case when ms.emailaddress IS null then EntryID end ) as counts, 
	--		Count(distinct case when ms.emailaddress IS null then emails.EmailID end ) as Emailcounts, 
	--		Count(case when ms.emailaddress IS Not null then 1 end) as MScount,
	--		Count(case when ne.emailaddress IS Not null and ne.IsMasterSuppressed = 1 then 1 end) as MSnotinECNcount,
	--from 
	--		Emails join 
	--		EmailGroups  on EmailGroups.EmailID = Emails.EmailID left outer join 
	--		@trans t on Emails.emailID = t.EmailID left outer join
	--		@ms ms on ms.emailaddress = emails.EmailAddress left outer join
	--		ecn_temp..dataset_OLD ne on ne.EmailAddress = emails.EmailAddress
	--where Emails.CustomerID = @CustomerID and EmailGroups.GroupID = @GroupID  and storenumber > 0 and BOOK_RETURNED <> 1 and Convert(date,RETURNDATE) > GETDATE()
	--group by StoreNumber, ReturnDate order by 1,2

END
