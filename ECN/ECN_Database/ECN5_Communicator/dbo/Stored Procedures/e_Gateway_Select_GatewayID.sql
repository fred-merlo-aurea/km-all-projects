CREATE PROCEDURE [dbo].[e_Gateway_Select_GatewayID]
	@GatewayID int
AS
	Select * 
	FROM Gateway g with(nolock)
	where g.GatewayID = @GatewayID and g.IsDeleted = 0