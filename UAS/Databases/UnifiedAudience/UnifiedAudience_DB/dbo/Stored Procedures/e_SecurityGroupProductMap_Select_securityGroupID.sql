CREATE PROCEDURE [dbo].[e_SecurityGroupProductMap_Select_securityGroupID]
@SecurityGroupID int
AS
BEGIN

	SET NOCOUNT ON

	select *
	from SecurityGroupProductMap with(nolock)
	where SecurityGroupID = @SecurityGroupID

END
GO