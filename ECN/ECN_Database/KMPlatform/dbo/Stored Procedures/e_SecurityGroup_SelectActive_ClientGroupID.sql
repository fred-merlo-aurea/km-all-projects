CREATE PROCEDURE [dbo].[e_SecurityGroup_SelectActive_ClientGroupID]
@ClientGroupID int
AS
	select distinct sg.*
	from 
			SecurityGroup sg with(nolock)
	where 
			isnull(sg.ClientGroupID,0) = @ClientGroupID and sg.IsActive = 'true'
	order by 
			sg.SecurityGroupName