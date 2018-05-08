CREATE PROCEDURE [dbo].[e_SecurityGroupBrandMap_Select_securityGroupID]
@SecurityGroupID int
AS
BEGIN

	SET NOCOUNT ON

	select *
	from SecurityGroupBrandMap with(nolock)
	where SecurityGroupID = @SecurityGroupID

END
GO