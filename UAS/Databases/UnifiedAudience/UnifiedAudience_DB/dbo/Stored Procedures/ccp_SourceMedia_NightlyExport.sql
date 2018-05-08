
create proc ccp_SourceMedia_NightlyExport
as
select  
  ISNULL(ps.IMBSEQ, '') as IMBSeq  --empty
, isnull(ps.PubCategoryID,'') as PubCategoryID 
, isnull(ps.PubTransactionID,'') as PubTransactionID 
, isnull(s.QDate,'') as Qualificationdate 
, ISNULL(sp.ExpireIssueDate, '') as 'ExpireDate' -- empty  
, isnull(s.FNAME,'') as FirstName 
, isnull(s.LNAME,'') as LastName 
, isnull(s.TITLE,'') as Title 
, isnull(s.COMPANY,'') as Company 
, isnull(ps.Copies,'') as Copies 
, isnull(s.ADDRESS,'') as Address1 
, isnull(s.MAILSTOP,'') as Address2 
, isnull(s.ADDRESS3,'') as Address3 --empty
, isnull(s.CITY,'') as CITY 
, isnull(ps.RegionCode,'') as REGIONCODE 
, isnull(s.ZIP,'') as ZipCode 
, isnull(s.PLUS4,'') as Plus4 
, isnull(s.COUNTRY,'') as Country 
, isnull(s.SEQUENCE,'') as SequenceID 
, isnull(ps.ReqFlag,'') as Req_Flag 
, ISNULL(ami.AcsCode, '') as ACSCODE  
, isnull(p.PubCode,'') as ProductCode 
, isnull(ps.PubQSourceID,'') as PubQSource 
, '' as Keyline ---generate in C# 
, ISNULL(ami.MailerID,'') as mailerid  
, '' as split -- generated
, '' as keycode --leave empty
, isnull(p.PubCode,'') as acronym 
, 'P' as pstclass  
, 'UPS' as delivery_code   
, isnull(ps.CarrierRoute,'') as route_code  
, isnull(ps.SequenceID,'') as sequence_number  
, '' as floor_number  
from Subscriptions s with(nolock) 
join PubSubscriptions ps with(nolock) on s.subscriptionid = ps.SubscriptionID 
join Pubs p with(Nolock) on ps.PubID = p.PubID 
left join SubscriptionPaid sp with(nolock) on ps.PubSubscriptionID = sp.PubSubscriptionID 
left join AcsMailerInfo ami with(nolock) on p.AcsMailerInfoId = ami.AcsMailerInfoId 
where p.PubCode = 'BBC' and ps.IsActive = 1