
CREATE PROCEDURE [dbo].[rpt_AdvanstarBilling] 
	@From Date,
	@To Date,
	@BaseChannelID int
AS
BEGIN

select	ct.customerID, 
		ct.customername,
		b.BlastID,
		b.EmailSubject,
		b.EmailFrom,
		b.EmailFromName,
		b.Sendtime,
		convert(int,sendtotal) as 'SuccessTotal',
		--codedisplay as category,
		c.CampaignName as campaignName,
		bf.field1, bf.Field2, bf.Field3, bf.Field4, bf.Field5
From     
   ECN5_Communicator..Blast b  --left outer join ECN5_Communicator..code c on c.codeID = b.codeID
   JOIN ecn5_communicator..campaignitemblast cib on b.BlastID = cib.BlastID
   join ECN5_COMMUNICATOR..CampaignItem ci on ci.CampaignItemID = cib.CampaignItemID
   join ECN5_COMMUNICATOR..Campaign c on c.CampaignID = ci.CampaignID
    left outer join ECN5_Communicator..Blastfields bf on bf.blastID = b.blastID join
   ECN5_ACCOUNTS..Customer ct on b.customerID = ct.customerID
where
	BaseChannelID = @BaseChannelID and
	convert(date, b.sendtime) >= @From and convert(date, b.sendtime) <= @To and
	testblast = 'n' and statuscode in ('sent','deleted') and sendtotal > 0
END
