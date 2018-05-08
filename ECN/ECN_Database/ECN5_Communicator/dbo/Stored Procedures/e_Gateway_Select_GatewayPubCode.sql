CREATE PROCEDURE [dbo].[e_Gateway_Select_GatewayPubCode]
	@PubCode varchar(50),
	@TypeCode varchar(50)
AS
	Select * 
	FROM Gateway g with(nolock)
	where g.PubCode = @PubCode and g.TypeCode = @TypeCode