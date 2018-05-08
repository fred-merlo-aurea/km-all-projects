-- =============================================
-- Author:		<Author,,Name>
-- Create date: <Create Date,,>
-- Description:	<Description,,>
-- =============================================
CREATE PROCEDURE [dbo].[spBlastClicksDetail]
	@BlastID int
AS
BEGIN
	DECLARE @Campaign varchar(100) = (SELECT ci.CampaignItemName FROM ecn5_communicator..CampaignItem ci with(nolock)
										 join ecn5_communicator..CampaignItemBlast cib with(nolock) on ci.CampaignItemID = cib.CampaignItemID
										 WHERE cib.BlastID = @BlastID)
	
	SET NOCOUNT ON;
	SELECT @Campaign as 'Campaign', bac.URL as 'Link', e.FirstName, e.LastName, e.Title, e.Company, (isnull(e.Address,'') + ' ' + isnull(e.Address2,'')) as 'Address', e.City, e.State as 'State',
		e.Zip as 'PostalCode', e.Country, e.Voice as 'Phone', e.EmailAddress as 'Email'
	FROM 
	BlastActivityClicks bac with(nolock)
	join ecn5_communicator..Emails e with(nolock) on bac.EmailID = e.EmailID
	where bac.BlastID = @BlastID 
	group by bac.URL, e.FirstName, e.LastName, e.Title, e.Company,e.Address, e.Address2, e.City, e.State, e.Zip, e.Country, e.Voice, e.EmailAddress, e.EmailID
	
END