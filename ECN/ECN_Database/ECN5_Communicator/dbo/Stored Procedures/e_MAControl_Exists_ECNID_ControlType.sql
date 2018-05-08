CREATE PROCEDURE [dbo].[e_MAControl_Exists_ECNID_ControlType]
	@ECNID int,
	@ControlType varchar(50)
AS
	if(@ControlType = 'CampaignItem')
	BEGIN
		SELECT * FROM MarketingAutomation ma with(nolock)
		join MAControl mc with(nolock) on ma.MarketingAutomationID = mc.MarketingAutomationID
		where mc.ControlType not in('Group','Wait','Start','End', 'Direct_Click','Direct_Open','Direct_NoOpen', 'Subscribe','Unsubscribe') and mc.ECNID = @ECNID
	END
	ELSE
	BEGIN
		SELECT * FROM MarketingAutomation ma with(nolock)
		join MAControl mc with(nolock) on ma.MarketingAutomationID = mc.MarketingAutomationID
		where mc.ControlType = @ControlType and mc.ECNID = @ECNID
	END