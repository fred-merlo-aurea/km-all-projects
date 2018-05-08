CREATE PROCEDURE [dbo].[e_MAConnector_Delete]
	@MAConnectorID int
AS
	if exists(select top 1 * from MAConnector m with(nolock) where m.ConnectorID = @MAConnectorID)
	BEGIN
		Delete from MAConnector
		where ConnectorID = @MAConnectorID
	END