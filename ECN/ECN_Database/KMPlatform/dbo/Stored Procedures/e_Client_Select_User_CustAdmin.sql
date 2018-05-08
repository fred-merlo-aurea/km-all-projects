CREATE PROCEDURE e_Client_Select_User_CustAdmin
	@ClientGroupID int,
	@UserID int
AS
BEGIN
	SELECT distinct c.* 
FROM Client c with(nolock)
	join ClientGroupClientMap cgcm with(nolock) on c.ClientID = cgcm.ClientID and cgcm.ClientGroupID = @ClientGroupID
	JOIN UserClientSecurityGroupMap ucsgm with(nolock) on cgcm.ClientID = ucsgm.ClientID and ucsgm.UserID = @UserID
	JOIN SecurityGroup sg with(nolock) on ucsgm.SecurityGroupID = sg.SecurityGroupID
WHERE sg.SecurityGroupName = 'Administrator'
	and sg.IsActive = 1
	and ucsgm.IsActive = 1
	and cgcm.IsActive = 1
	and c.IsActive = 1
END