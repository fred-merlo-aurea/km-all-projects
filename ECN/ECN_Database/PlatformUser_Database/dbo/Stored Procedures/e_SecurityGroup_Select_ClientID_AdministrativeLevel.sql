CREATE PROCEDURE [dbo].[e_SecurityGroup_Select_ClientID_AdministrativeLevel]
@ClientID int,
@AdministrativeLevel varchar(50)
AS
	select sg.*
	from SecurityGroup sg with(nolock)
	where isnull(sg.ClientID,0) = @ClientID
	  AND ISNULL(sg.AdministrativeLevel,'') = @AdministrativeLevel
	order by 1