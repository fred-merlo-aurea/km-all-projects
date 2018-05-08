CREATE proc [dbo].[rpt_InternetRetailer_KMLogoClickReport]
(
	@fromdt varchar(10),
	@todt varchar(10),
	@showdetails int
)
as
Begin

	if len(rtrim(ltrim(@fromdt))) = 0
		set @fromdt = '1/1/2000'

	if len(rtrim(ltrim(@todt))) = 0
		set @todt = convert(varchar(10), getdate(), 101)

	if @showdetails = 0
	Begin
		select	CustomerName, 
				b.BlastID, 
				EmailSubject, 
				Sendtime, 
				SendTotal,
				'' firstname,
				'' lastname,
				'' emailaddress,
				'' voice,
				'' mobile,
				count(eal.eaid) as logo_clicked
		from	
				[ECN5_COMMUNICATOR].[DBO].[BLAST] b join 
				ecn5_accounts..[Customer] c on b.customerID = c.customerID left outer join 
				[ECN5_COMMUNICATOR].[DBO].EmailActivityLog eal on eal.blastID = b.blastID and actiontypecode='click' and (actionvalue like '%awaredemo.aspx%' or actionvalue like '%www.knowledgemarketing.com%') 
		where 
				basechannelID = 45 and 
				testblast <> 'y' and
				b.statuscode ='sent' and
				b.sendtime between @fromdt and dateadd(dd,1,@todt)
		group by 
				CustomerName, b.BlastID, EmailSubject, Sendtime, SendTotal  
	end
	else
	Begin
		select	CustomerName, 
				b.BlastID, 
				EmailSubject, 
				Sendtime, 
				SendTotal,
				e.firstname,
				e.lastname,
				e.emailaddress,
				e.voice,
				e.mobile,
				count(eal.eaid) as logo_clicked
		from	
				[ECN5_COMMUNICATOR].[DBO].[BLAST] b join 
				ecn5_accounts..[Customer] c on b.customerID = c.customerID left outer join 
				[ECN5_COMMUNICATOR].[DBO].EmailActivityLog eal on eal.blastID = b.blastID and actiontypecode='click' and (actionvalue like '%awaredemo.aspx%' or actionvalue like '%www.knowledgemarketing.com%') left outer join
				[ECN5_COMMUNICATOR].[DBO].emails e on e.emailID = eal.emailID
		where 
				basechannelID = 45 and 
				testblast <> 'y' and
				b.statuscode ='sent' and
				b.sendtime between @fromdt and dateadd(dd,1,@todt)
		group by 
				CustomerName, b.BlastID, EmailSubject, Sendtime, SendTotal, e.emailID, e.emailaddress,e.firstname,e.lastname, e.voice, e.mobile 
	end
end
