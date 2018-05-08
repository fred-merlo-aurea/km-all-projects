CREATE PROCEDURE [dbo].[e_SecurityGroup_Select_ClientGroupID]
@ClientGroupID int
AS
	select distinct sg.*
	from SecurityGroup sg with(nolock)
	where isnull(sg.ClientGroupID,0) = @ClientGroupID
	order by sg.SecurityGroupName
GO

