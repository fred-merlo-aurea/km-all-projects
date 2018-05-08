CREATE PROCEDURE [dbo].[e_SecurityGroup_Select_ClientGroupID_AdministrativeLevel]
@ClientGroupID int,
@AdministrativeLevel varchar(50)
AS
	select sg.*
	from SecurityGroup sg with(nolock)
	where isnull(sg.ClientGroupID,0) = @ClientGroupID
	  AND ISNULL(sg.AdministrativeLevel,'') = @AdministrativeLevel
	order by 1