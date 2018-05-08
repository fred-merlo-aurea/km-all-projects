
CREATE PROCEDURE e_ClientGroup_Select_User_CustAdmin
	@UserID int
AS
BEGIN
	Select distinct cg.* 
	from ClientGroup cg with(nolock)
		JOIN ClientGroupClientMap cgcm with(nolock) on cg.ClientGroupID = cgcm.ClientGroupID
		join UserClientSecurityGroupMap ucsgm with(nolock) on cgcm.ClientID = ucsgm.ClientID and ucsgm.UserID = @userID
		JOIN SecurityGroup sg with(nolock) on cgcm.clientGroupID = sg.ClientGroupID
	where sg.SecurityGroupName  = 'Channel Administrator' 
		and sg.IsActive = 1
		and ucsgm.IsActive = 1
		and cgcm.IsActive = 1
		and cg.IsActive = 1
UNION 
--Gets customer admin
	SELECT distinct cg.* 
	FROM ClientGroup cg with(nolock)
		join ClientGroupClientMap cgcm with(nolock) on cg.ClientGroupID = cgcm.ClientGroupID
		JOIN UserClientSecurityGroupMap ucsgm with(nolock) on cgcm.ClientID = ucsgm.ClientID and ucsgm.UserID = @userID
		JOIN SecurityGroup sg with(nolock) on ucsgm.SecurityGroupID = sg.SecurityGroupID
	WHERE sg.SecurityGroupName = 'Administrator'
		and sg.IsActive = 1
		and ucsgm.IsActive = 1
		and cgcm.IsActive = 1
		and cg.IsActive = 1
END