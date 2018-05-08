CREATE procedure [dbo].[sp_ChannelLook_details]
(     
 @CustomerID int,    
 @fromdt varchar(10),    
 @todt varchar(10),
 @testblast varchar(1)    
)    
as    
Begin    
 Set nocount on    
    




if @testblast = 'N'
BEGIN

	declare @TrigBlasts table(BlastID int, Sends int)
    INSERT INTO @TrigBlasts
    Select bas.BlastID, COUNT(bas.SendID)
    FROM ECN5_COMMUNICATOR..Blast b with(nolock)
    JOIN ECN_Activity..BlastActivitySends bas with(nolock) on b.BlastID = bas.BlastID
    WHERE b.BlastType in ('Layout','NoOpen') 
		and b.StatusCode in ('Sent', 'Deleted') 
		and cast(bas.SendTime as date) between @fromdt and @todt
		and b.CustomerID = @CustomerID
	Group by bas.BlastID

	select	b.BlastID,
			c.campaignName,
			b.EmailSubject,
			b.EmailFrom,
			EmailFromName,
			b.Sendtime,
			convert(int,sendtotal) as 'SuccessTotal',
			codedisplay as category,
			bf.field1,
			g.GroupName,
			isnull((select FolderName from [ECN5_COMMUNICATOR].[dbo].Folder f where f.FolderID = g.FolderID and f.CustomerID = b.CustomerID),'root') as 'GroupFolderName'
		
	From     
	   [ECN5_COMMUNICATOR].[DBO].[BLAST] b  left outer join [ECN5_COMMUNICATOR].[DBO].Code on code.codeID = b.codeID left outer join
	   [ECN5_COMMUNICATOR].[DBO].Blastfields bf on bf.blastID = b.blastID
	   join [ECN5_COMMUNICATOR].[dbo].Groups g on b.GroupID = g.GroupID
	   join [ecn5_COMMUNICATOR].[dbo].CampaignItemBlast cib on b.BlastID = cib.BlastID
	   join [ECN5_COMMUNICATOR].[dbo].CampaignItem ci on cib.CampaignItemID = ci.CampaignItemID
	   join [ecn5_communicator].dbo.Campaign c on ci.CampaignID = c.CampaignID

	where
		b.CustomerID = @CustomerID and   
		convert(smalldatetime, convert(varchar(10),b.sendtime,101)) between @fromdt and @todt and b.BlastType not in ('Layout','NoOpen')
		and statuscode in ('sent','deleted') and sendtotal > 0
UNION ALL
	select	b.BlastID,
			c.campaignName,
			b.EmailSubject,
			b.EmailFrom,
			EmailFromName,
			b.Sendtime,
			convert(int,t.Sends) as 'SuccessTotal',
			codedisplay as category,
			bf.field1,
			'' as GroupName,
			'' as 'GroupFolderName'
		
	From     
	   [ECN5_COMMUNICATOR].[DBO].[BLAST] b  
	   left outer join [ECN5_COMMUNICATOR].[DBO].Code on code.codeID = b.codeID 
	   left outer join [ECN5_COMMUNICATOR].[DBO].Blastfields bf on bf.blastID = b.blastID
	   join [ecn5_COMMUNICATOR].[dbo].CampaignItemBlast cib on b.BlastID = cib.BlastID
	   join [ECN5_COMMUNICATOR].[dbo].CampaignItem ci on cib.CampaignItemID = ci.CampaignItemID
	   join [ecn5_communicator].dbo.Campaign c on ci.CampaignID = c.CampaignID
	   JOIN @TrigBlasts t on b.blastid = t.BlastID
	   WHERE b.TestBlast = 'N'

END
ELSE IF @testblast = 'Y'
BEGIN
	select	b.BlastID,
		c.campaignName,
		b.EmailSubject,
		b.EmailFrom,
		EmailFromName,
		b.Sendtime,
		convert(int,sendtotal) as 'SuccessTotal',
		codedisplay as category,
		bf.field1,
		g.GroupName,
		isnull((select FolderName from [ECN5_COMMUNICATOR].[dbo].Folder f where f.FolderID = g.FolderID and f.CustomerID = b.CustomerID),'root') as 'GroupFolderName'
		
	From     
	   [ECN5_COMMUNICATOR].[DBO].[BLAST] b  left outer join [ECN5_COMMUNICATOR].[DBO].Code on code.codeID = b.codeID left outer join
	   [ECN5_COMMUNICATOR].[DBO].Blastfields bf on bf.blastID = b.blastID
	   join [ECN5_COMMUNICATOR].[dbo].Groups g on b.GroupID = g.GroupID
	   join [ecn5_COMMUNICATOR].[dbo].CampaignItemTestBlast cib on b.BlastID = cib.BlastID
	   join [ECN5_COMMUNICATOR].[dbo].CampaignItem ci on cib.CampaignItemID = ci.CampaignItemID
	   join [ecn5_communicator].dbo.Campaign c on ci.CampaignID = c.CampaignID

	where
		b.CustomerID = @CustomerID and   
		convert(smalldatetime, convert(varchar(10),b.sendtime,101)) between @fromdt and @todt and b.BlastType not in ('Layout','NoOpen') and
		testblast = 'Y' and statuscode in ('sent','deleted') and sendtotal > 0
END
End