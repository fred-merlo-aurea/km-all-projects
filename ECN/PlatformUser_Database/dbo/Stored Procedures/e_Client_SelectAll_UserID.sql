CREATE PROCEDURE [dbo].[e_Client_SelectAll_UserID]
	@UserID int
AS
		DECLARE @clientIDSecurityGroupID table (clientID int, clientGroupID int, SecurityGroupID int, AdminLevel varchar(50))

	INSERT INTO @clientIDSecurityGroupID(clientID, clientGroupID, SecurityGroupID, AdminLevel)
	Select ucsgm.ClientID,sg.ClientGroupID, ucsgm.SecurityGroupID, sg.AdministrativeLevel 
	from UserClientSecurityGroupMap ucsgm with(nolock)
	join SecurityGroup sg with(nolock) on ucsgm.SecurityGroupID = sg.SecurityGroupID
	WHERE ucsgm.UserID = @UserID and ucsgm.IsActive = 1 and sg.IsActive = 1

	Declare @ClientIDsToTake table(clientID int)

	insert into @ClientIDsToTake(clientID)
	Select cgcm.ClientID
	FROM ClientGroupClientMap cgcm with(nolock)
	join @clientIDSecurityGroupID c on cgcm.ClientGroupID = c.clientGroupID
	join Client cl with(nolock) on cgcm.ClientID = cl.ClientID
	join ClientGroup cg with(nolock) on cgcm.ClientGroupID = cg.ClientGroupID
	where c.clientGroupID is not null and cgcm.IsActive = 1 and cg.IsActive = 1 and cl.IsActive = 1
	UNION 
	SELECT c.clientID
	FROM @clientIDSecurityGroupID c
	join Client cl with(nolock) on c.clientID = cl.ClientID
	join ClientGroupClientMap cgcm with(nolock) on cl.ClientID = cgcm.ClientID
	join ClientGroup cg with(nolock) on cgcm.ClientGroupID = cg.ClientGroupID
	where c.clientGroupID is null and cl.IsActive = 1 and cg.IsActive = 1


	SELECT distinct * FROM Client c with(nolock)
	join @ClientIDsToTake cl on c.ClientID = cl.clientID