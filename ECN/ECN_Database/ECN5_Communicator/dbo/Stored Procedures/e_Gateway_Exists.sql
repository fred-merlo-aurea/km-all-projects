CREATE PROCEDURE [dbo].[e_Gateway_Exists]
	@GatewayID int,
	@CustomerID int
AS
	if exists(Select top 1 GatewayID from Gateway g where g.GatewayID = @GatewayID and g.CustomerID = @CustomerID and IsDeleted = 0)
		select 1
	else
		select 0

