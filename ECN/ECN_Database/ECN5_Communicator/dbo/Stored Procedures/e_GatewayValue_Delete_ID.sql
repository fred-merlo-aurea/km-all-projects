CREATE PROCEDURE [dbo].[e_GatewayValue_Delete_ID]
	@GatewayValueID int
AS
	Update GatewayValue
	SET IsDeleted = 1
	where GatewayValueID = @GatewayValueID and IsDeleted = 0
