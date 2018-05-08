CREATE proc [dbo].[spNEEBODailySummary]
as
BEGIN

declare @Currentdt date
set @Currentdt = CONVERT(date, getdate())

declare @blacklist Table (EmailID int)  

insert into @blacklist        
SELECT	
		eg.EmailID 
FROM	
		EmailGroups eg   WITH (NOLOCK) JOIN 
		Groups g   WITH (NOLOCK) ON eg.groupID = g.groupID        
WHERE 	
		g.CustomerID = 1041 AND 
		g.MasterSupression=1        


select		StoreNumber, 
			CONVERT(varchar,RETURNDATE,101) as DueDate,
			--count(distinct Emails.EmailID) as TotalBeforeSuppressed,
			count(distinct Case when Emails.EmailAddress like '%@%.%' and Emails.emailAddress not like '%@%@%'  and Emails.emailAddress not like '@%.%' and --initial reminder
				SubscribeTypeCode='S' and cms.emailaddress is null and b.emailID is null and dl.Domain is null and BOOK_RETURNED <> 1 and 
				RETURNDATE = dateadd(day,10,@Currentdt) then Emails.EmailID end) as InitialReminder,
			count(distinct Case when Emails.EmailAddress like '%@%.%' and Emails.emailAddress not like '%@%@%'  and Emails.emailAddress not like '@%.%' and --second reminder
				SubscribeTypeCode='S' and cms.emailaddress is null and b.emailID is null and dl.Domain is null and BOOK_RETURNED <> 1 and 
				RETURNDATE = dateadd(day,5,@Currentdt) then Emails.EmailID end) as SecondReminder,
			count(distinct Case when Emails.EmailAddress like '%@%.%' and Emails.emailAddress not like '%@%@%'  and Emails.emailAddress not like '@%.%' and --final reminder
				SubscribeTypeCode='S' and cms.emailaddress is null and b.emailID is null and dl.Domain is null and BOOK_RETURNED <> 1 and 
				RETURNDATE = dateadd(day,1,@Currentdt) then Emails.EmailID end) as FinalReminder,
			count(distinct Case when Emails.EmailAddress like '%@%.%' and Emails.emailAddress not like '%@%@%'  and Emails.emailAddress not like '@%.%' and --previously returned
				SubscribeTypeCode='S' and cms.emailaddress is null and b.emailID is null and dl.Domain is null and BOOK_RETURNED = 1 and 
				BOOK_RETURNEDDATE < @Currentdt then Emails.EmailID end) as BookPreviouslyReturned ,
			count(distinct Case when Emails.EmailAddress like '%@%.%' and Emails.emailAddress not like '%@%@%'  and Emails.emailAddress not like '@%.%' and --currently returned
				SubscribeTypeCode='S' and cms.emailaddress is null and b.emailID is null and dl.Domain is null and BOOK_RETURNED = 1 and 
				BOOK_RETURNEDDATE = @Currentdt then Emails.EmailID end) as BookReturned ,
			count(distinct Case when Emails.EmailAddress like '%@%.%' and Emails.emailAddress not like '%@%@%'  and Emails.emailAddress not like '@%.%' and --charged
				SubscribeTypeCode='S' and cms.emailaddress is null and b.emailID is null and dl.Domain is null and BOOK_RETURNED <> 1 and  
				CC_CHARGED = 1   and CC_CHARGED_DATE = @Currentdt then Emails.EmailID end) CCCharged,
			count(distinct Case when Emails.EmailAddress like '%@%.%' and Emails.emailAddress not like '%@%@%'  and Emails.emailAddress not like '@%.%' and --charge failed
				SubscribeTypeCode='S' and cms.emailaddress is null and b.emailID is null and dl.Domain is null and BOOK_RETURNED <> 1 and  
				CC_FAILED  = 1   and CC_FAILED_DATE = @Currentdt then Emails.EmailID end) CCFailed,
			count(distinct Case when Emails.EmailAddress not like '%@%.%' or Emails.emailAddress like '%@%@%'  or Emails.emailAddress like '@%.%' then Emails.EmailID end) as InvalidEmails, --invalid
			count(distinct Case when Emails.EmailAddress like '%@%.%' and Emails.emailAddress not like '%@%@%'  and Emails.emailAddress not like '@%.%' and --unsub supp
				SubscribeTypeCode='U' then Emails.EmailID end) as UnsubSuppressed,
			count(distinct Case when Emails.EmailAddress like '%@%.%' and Emails.emailAddress not like '%@%@%'  and Emails.emailAddress not like '@%.%' and --master supp
				SubscribeTypeCode<>'U' and b.EmailID is not null then Emails.EmailID end) as MasterSuppressed,
			count(distinct Case when Emails.EmailAddress like '%@%.%' and Emails.emailAddress not like '%@%@%'  and Emails.emailAddress not like '@%.%' and --channel supp
				SubscribeTypeCode<>'U' and b.EmailID is null  and cms.emailaddress is not null  then Emails.EmailID end) as ChannelSuppressed,
			count(distinct Case when Emails.EmailAddress like '%@%.%' and Emails.emailAddress not like '%@%@%'  and Emails.emailAddress not like '@%.%' and --domain supp
				dl.Domain is not null then Emails.EmailID end) as DomainSuppressed --add blacklist Suppressed (Customer master suppress)
from
		Emails  join 
		EmailGroups  on EmailGroups.EmailID = Emails.EmailID  left outer join
		(
			SELECT	tmp_EmailID1, EntryID, [STORENUMBER],
					Convert(date,[RETURNDATE]) as 'RETURNDATE',
					Convert(int,isnull([BOOK_RETURNED],0)) as 'BOOK_RETURNED',
					convert(date,[BOOK_RETURNEDDATE]) AS 'BOOK_RETURNEDDATE',
					Convert(int,isnull([CC_CHARGED],0)) as 'CC_CHARGED',
					convert(date,[CC_CHARGED_DATE]) AS 'CC_CHARGED_DATE',
					Convert(int,isnull([CC_FAILED],0)) as 'CC_FAILED',
					convert(date,[CC_FAILED_DATE]) AS 'CC_FAILED_DATE'
			 FROM
			 (
				SELECT edv.emailID as tmp_EmailID1, edv.entryID, gdf.ShortName, edv.DataValue
				from	EmailDataValues edv  join groupdatafields gdf on edv.groupdatafieldsID = gdf.groupdatafieldsID
				where 
						gdf.groupdatafieldsID in (38567,38575,40181,40182,40183,40184,40185,40186)
			 ) u
			 PIVOT
			 (
			 MAX (DataValue)
			 FOR ShortName in ([STORENUMBER],[RETURNDATE],[BOOK_RETURNED],[BOOK_RETURNEDDATE],[CC_CHARGED],[CC_CHARGED_DATE],[CC_FAILED],[CC_FAILED_DATE],[ISSUE_SENTDATE],[ISSUE_SENT])) as pvt 			
		) 
		TUDFs on Emails.emailID = TUDFs.tmp_EmailID1  left outer join 
		ChannelMasterSuppressionList cms on emails.emailaddress = cms.emailaddress and cms.basechannelID = 25 left outer join 
		@blacklist b on b.EmailID = emails.emailID left outer join 
		( SELECT Domain FROM DomainSuppression WHERE CustomerID = 1041 or BaseChannelID = 25 )  dl on RIGHT(Emails.EmailAddress, LEN(Emails.EmailAddress) - CHARINDEX('@', Emails.EmailAddress)) = dl.Domain  
where 
		Emails.CustomerID = 1041 and 
		EmailGroups.GroupID = 35894 and 
		(
			(
				RETURNDATE = dateadd(day,10,@Currentdt) -- Reminder 1
			)
			or
			(
				RETURNDATE = dateadd(day,5,@Currentdt) -- Reminder 2
			)
			or 
			(
				RETURNDATE = dateadd(day,1,@Currentdt) -- Reminder 3
			)
			or 
			(
				BOOK_RETURNEDDATE = @Currentdt --Book returned 
			)
			or 
			(
				CC_CHARGED = 1   and CC_CHARGED_DATE = @Currentdt --Book issue - CC charged
			)
			or 
			(
				CC_FAILED = 1  and CC_FAILED_DATE = @Currentdt --Book issue - CC failed
			)
		)	
group by StoreNumber, RETURNDATE order by 2,1 

END
