CREATE PROCEDURE [dbo].[e_UserClientSecurityGroupMap_Select_ClientID]
@ClientID int
AS
	select * 
	from UserClientSecurityGroupMap with(nolock)
	where ClientID = @ClientID
