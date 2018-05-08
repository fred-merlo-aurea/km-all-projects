create procedure e_AdHocDimensionGroup_Select
AS
BEGIN

	set nocount on

	select *
	from AdHocDimensionGroup with(nolock)
	order by ClientID, AdHocDimensionGroupName

END
GO