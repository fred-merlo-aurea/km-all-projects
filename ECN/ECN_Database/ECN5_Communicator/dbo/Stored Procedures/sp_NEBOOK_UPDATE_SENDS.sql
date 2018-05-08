CREATE proc [dbo].[sp_NEBOOK_UPDATE_SENDS]
(
	@CustomerID int,        	
	@GroupID int
)
AS

BEGIN        
	set NOCOUNT ON        
        
	declare @Col1 varchar(8000),        
			@Col2 varchar(8000),      
			@Filter varchar(8000),
			@today varchar(10),
			@gdfID int
			
     
	set @Col1  = ''        
	set @Col2  = ''        
	set @Filter = ''
	set @today = convert(varchar(10), getdate(), 101)
	set @gdfID = 0
	
 	declare @gdf table(GID int, ShortName varchar(50))        
	
	insert into @gdf
	select	GroupDatafieldsID, ShortName
	from	GroupDatafields 
	where	GroupID = @GroupID and 
			DatafieldSetID is not null and 
			ShortName in ('RECORDKEY','RETURNDATE','REMINDER1_SENT','REMINDER2_SENT','REMINDER3_SENT','BOOK_RETURNED','CC_CHARGED','CC_CHARGED_DATE','CHARGE_SENT','CC_FAILED','CC_FAILED_DATE','ISSUE_SENT')
	
	
	create table #E(EmailID int, GID int, DataValue varchar(500), EntryID uniqueidentifier)        
	CREATE UNIQUE CLUSTERED INDEX E_ind on  #E(EmailID,EntryID,GID) with ignore_dup_key
	
	create table #T
	(	EmailID int, EmailAddress varchar(100), tmp_EmailID int, EntryID varchar(255),
		RETURNDATE date, BOOK_RETURNED bit,CC_CHARGED bit,CC_CHARGED_DATE date,CC_FAILED bit,CC_FAILED_DATE date,
		REMINDER1_SENT bit,REMINDER2_SENT bit,REMINDER3_SENT bit,
		CHARGE_SENT bit,RECORDKEY varchar(255),ISSUE_SENT bit
	)
	CREATE UNIQUE CLUSTERED INDEX T_ind on  #T(EmailID, EntryID,RECORDKEY) with ignore_dup_key
  	
	insert into #E         
	select EmailDataValues.EmailID, EmailDataValues.GroupDataFieldsID, DataValue, EntryID
	from EmailDataValues join @gdf g on g.GID = EmailDataValues.GroupDataFieldsID  -- and isnull(datavalue,'') <> ''

	select  @Col1 = @Col1 + coalesce('max([' + RTRIM(ShortName)  + ']) as ''' + RTRIM(ShortName)  + ''',',''),        
			@Col2 = @Col2 + coalesce('Case when E.GID=' + convert(varchar(10),g.GID) + ' then E.DataValue end [' + RTRIM(ShortName)  + '],', '')        
	from   @gdf g        

	set @Col1 = substring(@Col1, 0, len(@Col1))         
	set @Col2 = substring(@Col2 , 0, len(@Col2))        

	--select @Col1, @col2

	exec ( ' insert into #t
		select Emails.EmailID, Emails.EmailAddress, InnerTable2.* ' +      
		' from Emails join ' +       
			 '(select InnerTable1.EmailID as tmp_EmailID, InnerTable1.EntryID, ' + @Col1 + ' from  ' +         
				   '(select e.EmailID, E.EntryID, ' + @Col2 + ' from #E E) as InnerTable1 ' +         
			  ' Group by InnerTable1.EmailID, InnerTable1.EntryID ' +         
			 ') as InnerTable2 on Emails.EmailID = InnerTable2.tmp_EmailID join ' +        
		' EmailGroups  on EmailGroups.EmailID = Emails.EmailID' +        
		' left outer join ChannelMasterSuppressionList cms on emails.emailaddress = cms.emailaddress and cms.basechannelID = 25' + 
		' where Emails.CustomerID = ' + @CustomerID + ' and EmailGroups.GroupID = ' + @GroupID + ' and EmailGroups.SubscribeTypeCode = ''S'' and cms.emailaddress is null and entryID is not null order by emails.emailaddress;')  
		
		--select * from #T
	
	--select * from [FILTER] where GroupID = 35894
	--select * from GroupDatafields where GroupID = 35894
	--select top 1 * from emaildatavalues
	create table #tmpSends (EmailID int,EntryID varchar(50)) 
	
	--1st Reminder
	
	insert into #tmpSends 
	select emailID, EntryID from #T where ISNULL(CONVERT (DATE, [RETURNDATE]),'') BETWEEN CONVERT (DATE, getDate() +(+6)) AND CONVERT (DATE, getDate() +(+10)) AND ISNULL([REMINDER1_SENT],'') <> '1'  AND ISNULL([BOOK_RETURNED],'') <> '1' 
	
	if exists(select top 1 * from #tmpSends) 
	Begin
		select @gdfID = GroupDatafieldsID from GroupDatafields where GroupID = @GroupID and shortname = 'REMINDER1_SENT'
		
		insert into EmailDataValues
		select EmailID, @gdfID, '1', GETDATE(), -1, EntryID from #tmpSends
		
		select @gdfID = GroupDatafieldsID from GroupDatafields where GroupID = @GroupID and shortname = 'REMINDER1_SENTDATE'

		insert into EmailDataValues
		select EmailID, @gdfID, @today, GETDATE(), -1, EntryID from #tmpSends
			
		delete from #tmpSends
	end
			
	--2nd Reminder
	
	insert into #tmpSends 
	select emailID, EntryID from #T where ISNULL(CONVERT (DATE, [RETURNDATE]),'') BETWEEN CONVERT (DATE, getDate() +(+2)) AND CONVERT (DATE, getDate() +(+5)) AND ISNULL([REMINDER2_SENT],'') <> '1'  AND ISNULL([BOOK_RETURNED],'') <> '1' 
	
	if exists(select top 1 * from #tmpSends) 
	Begin
		select @gdfID = GroupDatafieldsID from GroupDatafields where GroupID = @GroupID and shortname = 'REMINDER2_SENT'
		insert into EmailDataValues
		select EmailID, @gdfID, '1', GETDATE(), -1, EntryID from #tmpSends
		
		select @gdfID = GroupDatafieldsID from GroupDatafields where GroupID = @GroupID and shortname = 'REMINDER2_SENTDATE'
		insert into EmailDataValues
		select EmailID, @gdfID, @today, GETDATE(), -1, EntryID from #tmpSends

		delete from #tmpSends
	end
	--3rd & final Reminder

	insert into #tmpSends 
	select emailID, EntryID from #T where CONVERT(varchar(10), ISNULL(CONVERT (DATE, [RETURNDATE]),''), 101)  =  CONVERT(varchar(10), CONVERT (DATE, getDate() +(+1)), 101) AND ISNULL([REMINDER3_SENT],'') <> '1'  AND ISNULL([BOOK_RETURNED],'') <> '1' 
	
	if exists(select top 1 * from #tmpSends) 
	Begin
		select @gdfID = GroupDatafieldsID from GroupDatafields where GroupID = @GroupID and shortname = 'REMINDER3_SENT'
		insert into EmailDataValues
		select EmailID, @gdfID, '1', GETDATE(), -1, EntryID from #tmpSends
		
		select @gdfID = GroupDatafieldsID from GroupDatafields where GroupID = @GroupID and shortname = 'REMINDER3_SENTDATE'
		insert into EmailDataValues
		select EmailID, @gdfID, @today, GETDATE(), -1, EntryID from #tmpSends
		
		delete from #tmpSends
	end
	--CC Charged 
	insert into #tmpSends 
	select emailID, EntryID from #T where CONVERT(varchar(10), ISNULL(CONVERT (DATE, [CC_CHARGED_DATE]),''), 101)  =  CONVERT(varchar(10), CONVERT (DATE, getDate()), 101) AND ISNULL([CHARGE_SENT],'') <> '1' 
	
	if exists(select top 1 * from #tmpSends) 
	Begin
		select @gdfID = GroupDatafieldsID from GroupDatafields where GroupID = @GroupID and shortname = 'CHARGE_SENT'
		insert into EmailDataValues
		select EmailID, @gdfID, '1', GETDATE(), -1, EntryID from #tmpSends
		
		select @gdfID = GroupDatafieldsID from GroupDatafields where GroupID = @GroupID and shortname = 'CHARGE_SENTDATE'
		insert into EmailDataValues
		select EmailID, @gdfID, @today, GETDATE(), -1, EntryID from #tmpSends

		delete from #tmpSends
	end
	--CC Failed
	
	insert into #tmpSends 
	select emailID, EntryID from #T where CONVERT(varchar(10), ISNULL(CONVERT (DATE, [CC_FAILED_DATE]),''), 101)  =  CONVERT(varchar(10), CONVERT (DATE, getDate()), 101) AND ISNULL([ISSUE_SENT],'') <> '1' 

	if exists(select top 1 * from #tmpSends) 
	Begin
		select @gdfID = GroupDatafieldsID from GroupDatafields where GroupID = @GroupID and shortname = 'ISSUE_SENT'
		insert into EmailDataValues
		select EmailID, @gdfID, '1', GETDATE(), -1, EntryID from #tmpSends
		
		select @gdfID = GroupDatafieldsID from GroupDatafields where GroupID = @GroupID and shortname = 'ISSUE_SENTDATE'
		insert into EmailDataValues
		select EmailID, @gdfID, @today, GETDATE(), -1, EntryID from #tmpSends
		
	end
	
	drop table #E     
	drop table #T  
	drop table #tmpSends       	   

END
