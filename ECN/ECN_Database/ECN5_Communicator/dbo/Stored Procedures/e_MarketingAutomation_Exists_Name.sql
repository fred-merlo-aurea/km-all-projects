CREATE PROCEDURE [dbo].[e_MarketingAutomation_Exists_Name]
	@BaseChannelID int,
	@Name varchar(500),
	@MAID int = -1
AS
	if exists(select top 1 * 
				from MarketingAutomation ma with(nolock) 
				join ECN5_Accounts..Customer c with(nolock) on ma.CustomerID = c.CustomerID
				where c.BaseChannelID = @BaseChannelID and ma.Name = @Name and ma.MarketingAutomationID <> @MAID and ma.IsDeleted=0)
	BEGIN
		Select 1
	END
	else
	Begin
		select 0
	end