CREATE proc [dbo].[rpt_KMLogoClickReport]
(
	@fromdt varchar(10),
	@todt varchar(10)
)
as
Begin

	if len(rtrim(ltrim(@fromdt))) = 0
		set @fromdt = convert(varchar(10),  dateadd(dd, -7, getdate()), 101)

	if len(rtrim(ltrim(@todt))) = 0
		set @todt = convert(varchar(10), getdate(), 101)

	select	distinct 
		bc.BaseChannelName,
		c.CustomerName, 
		EmailSubject, 
		Sendtime, 
		e.firstname,
		e.lastname,
		e.emailaddress,
		e.voice
	from	
			[ECN5_COMMUNICATOR].[DBO].[BLAST] b join 
			ecn5_accounts..[Customer] c on b.customerID = c.customerID join 
			[ECN5_COMMUNICATOR].[DBO].EmailActivityLog eal on eal.blastID = b.blastID join
			[ECN5_COMMUNICATOR].[DBO].emails e on e.emailID = eal.emailID join
			ecn5_accounts..[BaseChannel] bc on bc.basechannelID = c.basechannelID
	where 
			actiontypecode='click' and (actionvalue like '%awaredemo.aspx%' OR actionvalue like 'http://www.knowledgemarketing.com%') and 
			testblast <> 'y' and
			b.statuscode ='sent' and
			b.sendtime between @fromdt and @todt and
			substring(emailaddress,charindex('@',emailaddress) , charindex('.', substring(emailaddress,charindex('@',emailaddress) + 1,len(emailaddress)))) not in 
				('@yahoo','@hotmail','@aol','@gmail','@knowledgemarketing','@msn','@comcast','@earthlink', '@verizon', '@bellsouth', '@juno', '@sbcglobal','@webtv','@xcorp5','@netscape','@teckman', '@mailinator', '@att', '@charter')
			and emailaddress not like '%.rr.com'
	order by sendtime desc
end
