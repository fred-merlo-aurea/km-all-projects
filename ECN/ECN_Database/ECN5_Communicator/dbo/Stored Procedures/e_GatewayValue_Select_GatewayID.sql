CREATE PROCEDURE [dbo].[e_GatewayValue_Select_GatewayID]
	@GatewayID int
AS
	Select * 
	FROM GatewayValue gv with(nolock)
	WHERE gv.GatewayID = @GatewayID and gv.IsDeleted = 0
