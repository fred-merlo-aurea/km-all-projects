create procedure rpt_AcsImportCount
@StartDate date,
@EndDate date
as
	select c.ClientID,c.ClientName,p.PublicationCode, 
		(select count(TransactionCodeValue) 
		 from AcsFileDetail x21
		 where TransactionCodeValue = 21
		 and x21.ProductCode = d.ProductCode) as 'xact21',
		 (select count(TransactionCodeValue) 
		 from AcsFileDetail x31
		 where TransactionCodeValue = 31
		 and x31.ProductCode = d.ProductCode) as 'xact31',
		  (select count(TransactionCodeValue) 
		 from AcsFileDetail x32
		 where TransactionCodeValue = 32
		 and x32.ProductCode = d.ProductCode) as 'xact32',
		 (select count(TransactionCodeValue) 
		 from AcsFileDetail t
		 where t.ProductCode = d.ProductCode) as 'Total'
	from AcsFileDetail d with(nolock)
	join AcsMailerInfo a with(nolock) on d.AcsMailerId = a.AcsMailerId
	join Publication p with(nolock) on a.AcsMailerInfoId = p.AcsMailerInfoId
	join Publisher pub with(nolock) on p.PublisherID = pub.PublisherID
	join uas..Client c with(nolock) on pub.ClientID = c.ClientID 
	where d.CreatedDate between @StartDate and @EndDate
	group by c.ClientID,c.ClientName,p.PublicationCode,d.ProductCode
	order by p.PublicationCode
go
