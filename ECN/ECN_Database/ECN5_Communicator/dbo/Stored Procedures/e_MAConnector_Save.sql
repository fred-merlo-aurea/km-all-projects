CREATE PROCEDURE [dbo].[e_MAConnector_Save]
	@MAConnectorID int,
	@From varchar(255),
	@To varchar(255),
	@MarketingAutomationID int,
	@ControlID varchar(255)
AS
	if @MAConnectorID > 0--update
	BEGIN
		UPDATE MAConnector
		set [From] = @From,
			[To] = @To,
			MarketingAutomationID = @MarketingAutomationID,
			ControlID = @ControlID
		WHERE ConnectorID = @MAConnectorID
		select @MAConnectorID
	END
	Else if @MAConnectorID <= 0--insert
	BEGIN
		INSERT INTO MAConnector([From],[To], MarketingAutomationID, ControlID)
		VALUES(@From, @To, @MarketingAutomationID, @ControlID)
		Select @@IDENTITY;
	END
