-- =============================================
-- Author:		Rohit Pooserla
-- =============================================
CREATE PROCEDURE [dbo].[sp_GetBlastID_fromCampaignItem]
	-- Add the parameters for the stored procedure here
	@CampaignItemID int
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;
	select BlastID from ecn5_communicator..CampaignItemBlast where IsDeleted=0 and CampaignItemID=@CampaignItemID
	
END