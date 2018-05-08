CREATE PROCEDURE [dbo].[e_MarketingAutomation_Select_BaseChannelID]
	@BaseChannelID int
AS
	Select * from MarketingAutomation ma with(nolocK)
	join ECn5_Accounts..Customer c with(nolock) on ma.CustomerID = c.CustomerID
	where c.BaseChannelID = @BaseChannelID and ISNULL(ma.IsDeleted,0) = 0
