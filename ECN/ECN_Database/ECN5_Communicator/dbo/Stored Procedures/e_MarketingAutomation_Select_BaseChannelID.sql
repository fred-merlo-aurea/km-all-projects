CREATE PROCEDURE [dbo].[e_MarketingAutomation_Select_BaseChannelID]
	@BaseChannelID int
AS
	Select ma.CreatedDate, ma.CreatedUserID, ma.CustomerID, ma.EndDate, ma.Goal, ma.IsDeleted, ma.JSONDiagram, ma.MarketingAutomationID, ma.Name, ma.StartDate, ma.State, ma.UpdatedDate, ma.UpdatedUserID, MAX(mah.HistoryDate) as 'LastPublishedDate'
	from MarketingAutomation ma with(nolocK)
	join ECn5_Accounts..Customer c with(nolock) on ma.CustomerID = c.CustomerID
	left outer join MarketingAutomationHistory mah with(nolock) on ma.MarketingAutomationID = mah.MarketingAutomationID and mah.Action = 'Publish'
	where c.BaseChannelID = @BaseChannelID and ISNULL(ma.IsDeleted,0) = 0
	group by ma.CreatedDate, ma.CreatedUserID, ma.CustomerID, ma.EndDate, ma.Goal, ma.IsDeleted, ma.JSONDiagram, ma.MarketingAutomationID, ma.Name, ma.StartDate, ma.State, ma.UpdatedDate, ma.UpdatedUserID
