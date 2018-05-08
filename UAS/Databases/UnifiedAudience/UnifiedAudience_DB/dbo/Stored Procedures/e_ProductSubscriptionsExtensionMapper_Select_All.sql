create procedure e_ProductSubscriptionsExtensionMapper_Select_All
as
BEGIN

	SET NOCOUNT ON

	select *
	from PubSubscriptionsExtensionMapper with(nolock)

END
go