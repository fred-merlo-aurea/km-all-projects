CREATE PROCEDURE [dbo].[rpt_BlastEngine_Status]

AS
BEGIN

	CREATE table #BlastEngines (BlastEngineID int, Name varchar(50), BlastID int null, StatusCode varchar(10), EmailSubject varchar(255), CustomerID int null, SendTime datetime null, StartTime datetime null, SendTotal int null, LastSendTime datetime null, AlreadySent int)  

	--active

	INSERT INTO #BlastEngines
	SELECT 
		b.BlastEngineID, be.Name, b.BlastID, b.statuscode, EmailSubject, CustomerID, b.sendtime, 
		b.StartTime, sendtotal, MAX(eal.ActionDate) as lastsendtime, COUNT(eal.eaid) as AlreadySent
	FROM 
		Blast b 
		left outer join blastengines be with (NOLOCK) on be.blastengineID = b.blastengineID and be.IsActive = 1 and be.IsPort25 = 1
		left outer join EmailActivityLog eal with (NOLOCK) on eal.BlastID = b.BlastID and eal.ActionTypeCode = 'send'
	WHERE 
		b.StatusCode in ('active','error')
	GROUP BY 
		b.BlastEngineID, be.Name, b.BlastID, b.statuscode, EmailSubject, CustomerID, b.sendtime, b.StartTime, sendtotal

	--pending

	INSERT INTO #BlastEngines
	SELECT 
		b.BlastEngineID, be.Name, b.BlastID, b.statuscode, EmailSubject, CustomerID, b.sendtime, 
		b.StartTime, sendtotal, null as lastsendtime, 0 as AlreadySent
	FROM 
		Blast b 
		left outer join blastengines be with (NOLOCK) on be.blastengineID = b.blastengineID and be.IsActive = 1 and be.IsPort25 = 1
	WHERE 
		( 
			(b.StatusCode = 'pending' and b.SendTime < GETDATE())
			or (b.StatusCode = 'pending' and b.BlastEngineID is null and b.SendTime < GETDATE())
		)
	GROUP BY  
		b.BlastEngineID, be.Name, b.BlastID, b.statuscode, EmailSubject, CustomerID, b.sendtime, b.StartTime, sendtotal

	--waiting

	INSERT INTO #BlastEngines
	SELECT 
		be.BlastEngineID, be.Name, null, 'Waiting', null, null, null, null, null, GETDATE() as lastsendtime, 0 as AlreadySent
	FROM 
		BlastEngines be with (nolock)
	WHERE 
		be.IsActive = 1 and be.IsPort25 = 1 and be.BlastEngineID not in (select BlastEngineID from #BlastEngines)

	--result list

	SELECT 
		BlastEngineID, case when CustomerName is null then '' else CustomerName + ' (' + convert(varchar(10), be.CustomerID) + ')' end as Customer, 
		replace(replace(Name, 'ECNBlastEngine_', ''), '_', ' ') as BlastEngineName, BlastID, statuscode, IsNull(EmailSubject,'') as EmailSubject, 
		SendTime, StartTime, Isnull(SendTotal, 0) as SendTotal , AlreadySent, LastSendTime as LastUpdatedTime,
		case 
			when statuscode = 'Waiting' then 'GREEN'
			when statuscode = 'Active' then case when DateDiff(mi, LastSendTime, GETDATE()) <= 3 then 'GREEN' else 'RED' end
			when StatusCode = 'Pending' then case when DATEDIFF(mi, SendTime, GETDATE()) <= 1 then 'GREEN' else 'RED' end
		end as Status
	FROM 
		#BlastEngines be 
		left outer join ECN5_ACCOUNTS..Customer c with (nolock) on be.CustomerID = c.CustomerID
	order by 
		BlastEngineID, StatusCode

	drop table #BlastEngines

END
