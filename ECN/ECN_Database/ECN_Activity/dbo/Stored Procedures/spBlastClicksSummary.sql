-- =============================================
-- Author:		<Author,,Name>
-- Create date: <Create Date,,>
-- Description:	<Description,,>
-- =============================================
CREATE PROCEDURE [dbo].[spBlastClicksSummary]
	@BlastID int
AS
BEGIN
	Declare @BounceCount int = (SELECT COUNT(Distinct Bounce.EmailID) from BlastActivityBounces Bounce with(nolock) where Bounce.BlastID = @BlastID)
	Declare @SendCount int = (SELECT SendTotal FROM ecn5_communicator..Blast with(nolock) where BlastID = @BlastID)
	Declare @CampaignName varchar(50) = (SELECT ci.CampaignItemName from ecn5_communicator..CampaignItemBlast cib 
											join ecn5_communicator..CampaignItem ci on cib.CampaignItemID = ci.CampaignItemID where cib.BlastID = @BlastID)
	Declare @Open int = (SELECT COUNT(EmailID) from BlastActivityOpens with(nolock) where BlastID = @BlastID)
	Declare @SendDate varchar(20) = (SELECT CONVERT(varchar(10),Sendtime,101) from ecn5_communicator..Blast where BlastID = @BlastID)
	Declare @CampaignClicks int = (SELECT COUNT(EmailID) from BlastActivityClicks with(nolock) where BlastID = @BlastID)
	
	SELECT @CampaignName as 'CampaignItemName', @SendDate as 'IssueDate', @SendCount as 'TotalSent', @SendCount - @BounceCount as 'TotalDelivered', @Open as 'Open', @CampaignClicks as 'TotalCampaignClicks',bac.URL,	COUNT(bac.EmailID) as 'TotalClicks', COUNT(Distinct bac.EmailID) as 'UniqueClicks'
	FROM BlastActivityClicks bac with(nolock)
	WHERE bac.BlastID = @BlastID
	group by bac.URL
	
END