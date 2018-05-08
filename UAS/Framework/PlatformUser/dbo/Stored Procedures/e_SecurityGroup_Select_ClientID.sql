CREATE PROCEDURE [dbo].[e_SecurityGroup_Select_ClientID]
@ClientID int
AS
	select distinct sg.*
	from SecurityGroup sg with(nolock)
	where isnull(sg.ClientID,0) = @ClientID
	order by sg.SecurityGroupName
