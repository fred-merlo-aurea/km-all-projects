CREATE PROCEDURE [dbo].[rpt_CanonTradeShow]
	@from datetime,
	@to datetime		
AS
BEGIN
	set @from = @from + ' 00:00:00 '    
	set @to = @to + '  23:59:59'      
	declare @ClientID int
	Select @ClientID = PlatformClientID 
	FROM ECN5_Accounts..Customer c with(nolock)
	where c.customerid = 1794
--Report doesn't actually pull from Wizard table but i'm making the column names match as if it were JWelter 11/14/2013
select ci.campaignItemID as 'WizardID', ci.CampaignItemNameOriginal as 'WizardName', cib.EmailSubject, SUM(b.SendTotal) as 'SendTotal', CONVERT(varchar(19), b.SendTime,120) as 'SendTime' ,ci.CampaignItemName as 'CustomerReference',bf.Field2 as 'BillingCode',cit.TemplateName as 'ShowName'
from Blast b with(nolock)
	join BlastFields bf with(nolock) on b.BlastID = bf.BlastID
	join CampaignItemBlast cib with(nolock) on b.BlastID = cib.BlastID
	join CampaignItem ci with(nolock) on cib.CampaignItemID = ci.CampaignItemID
	left join CampaignItemTemplates cit with(nolock) on ci.CampaignItemTemplateID = cit.CampaignItemTemplateID
where ci.CreatedUserID in (Select UserID from KMPlatform.dbo.UserClientSecurityGroupMap u where u.ClientID = @ClientID) 
	and cib.CreatedUserID in (Select UserID from KMPlatform.dbo.UserClientSecurityGroupMap u where u.ClientID = @ClientID)
	and b.TestBlast = 'N' and b.SendTime between @from and @to
	and b.CustomerID = 1794
group by ci.CampaignItemID,ci.CampaignItemNameOriginal, ci.CampaignItemName,bf.Field2, cib.EmailSubject,cit.TemplateName,b.SendTime

order by b.SendTime, cit.TemplateName
END
