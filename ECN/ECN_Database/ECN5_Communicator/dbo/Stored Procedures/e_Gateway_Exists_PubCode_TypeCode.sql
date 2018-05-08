CREATE PROCEDURE [dbo].[e_Gateway_Exists_PubCode_TypeCode]
	@PubCode varchar(20),
	@TypeCode varchar(20)
AS
	if exists(Select top 1 GatewayID from Gateway g with(nolock) where g.PubCode = @PubCode and g.TypeCode = @TypeCode and g.IsDeleted = 0)
		select 1
	else
		select 0

