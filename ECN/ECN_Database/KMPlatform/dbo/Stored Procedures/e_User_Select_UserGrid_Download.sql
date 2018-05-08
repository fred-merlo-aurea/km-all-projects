CREATE PROC [dbo].[e_User_Select_UserGrid_Download]
	@ClientGroupID int = null,
	@ClientID int,
	@IncludePlatformAdmins bit,
	@UserIsCAdmin bit,
	@IncludeAllClients bit,
	@IncludeBCAdmins bit,
	@IsKMStaff bit = false,
	@searchText varchar(50) = '',
	@ShowDisabledUsers bit,
	@ShowDisabledUserRoles bit
AS
	declare @FinalUsers table(UserID int, UserName varchar(50), UserStatus varchar(20),  FirstName varchar(50), 
						LastName varchar(50),IsActive bit,  
						IsPlatformAdministrator bit, DateCreated datetime, DateUpdated datetime, ClientName varchar(100),SecurityGroupName varchar(100),
						Status varchar(20),RoleCreatedDate datetime, RoleUpdatedDate datetime)

	declare @SecurityGroups table (SecurityGroupID int,SecurityGroupName varchar(100))
	declare @ClientIDs table (ClientID int)
	declare @ClientGroupName varchar(400)
	Select @ClientGroupName = ClientGroupName from ClientGroup cg where cg.ClientGroupID = @ClientGroupID
	--determine if we want all clients for a clientgroup or just one client
	if @IncludeAllClients = 1 and @ClientGroupID is not null and @ClientID < 0
	BEGIN
		insert into @ClientIDs(ClientID)
		Select c.ClientID 
		from Client c with(nolock)
		join ClientGroupClientMap cgcm with(nolock) on c.ClientID = cgcm.ClientID
		where cgcm.ClientGroupID = @ClientGroupID and c.IsActive = 1 and cgcm.IsActive = 1
	END
	ELSE
	BEGIN
		insert into @ClientIDs(ClientID)
		VALUES(@ClientID)
	END

	--is user customer admin? if so show other customer admins else remove customer admins
	if @UserIsCAdmin = 1
	BEGIN
		INSERT INTO @SecurityGroups(SecurityGroupID, SecurityGroupName)
		SELECT SecurityGroupID, sg.SecurityGroupName
		from SecurityGroup sg with(nolock)
		join @ClientIDs c on sg.ClientID = c.ClientID
	END
	ELSE
	BEGIN
		INSERT INTO @SecurityGroups(SecurityGroupID, SecurityGroupName)
		Select SecurityGroupID, sg.SecurityGroupName
		FROM SecurityGroup sg with(nolock)
		join @ClientIDs c on sg.ClientID = c.ClientID
		where sg.SecurityGroupName <> 'administrator'
	END	

	--Channel admins
	if @IncludeBCAdmins = 1 and @ClientGroupID is not null
	BEGIN
		INSERT INTO @FinalUsers(UserID, UserName,UserStatus, FirstName, LastName, IsActive
								,IsPlatformAdministrator, DateCreated, DateUpdated, ClientName, SecurityGroupName, Status,RoleCreatedDate, RoleUpdatedDate)
		select u.UserID, UserName, u.Status, FirstName, LastName, u.IsActive
								,IsPlatformAdministrator, u.DateCreated, u.DateUpdated, '',case when sg.AdministrativeLevel = 'ChannelAdministrator' then 'Basechannel Admin' else sg.SecurityGroupName end, case when ucsgm.IsActive = 1 then 'Active' else ucsgm.InactiveReason end,ucsgm.DateCreated, ucsgm.DateUpdated
		from [User] u with(nolock)
			join UserClientSecurityGroupMap ucsgm with(nolock) on u.UserID = ucsgm.UserID
			join Client c with(nolock) on ucsgm.ClientID = c.ClientID
			join SecurityGroup sg with(nolock) on ucsgm.SecurityGroupID = sg.SecurityGroupID
			left outer join SecurityGroupOptIn sgoi with(nolock) on sg.SecurityGroupID = sgoi.SecurityGroupID and u.UserID = sgoi.UserID and sgoi.HasAccepted = 0 and sgoi.IsDeleted = 0
		where ISNULL(sg.ClientGroupID,0) = @ClientGroupID
			and sg.IsActive = 1
			and ISNULL(u.IsPlatformAdministrator, 0) = 0
			and u.IsActive = case when @ShowDisabledUsers = 1 then u.IsActive else 1 end
			and  ((@IsKMStaff = 1) or (@IsKMStaff = 0 and u.IsKMStaff = 0))
			and ucsgm.IsActive = case when @ShowDisabledUserRoles = 1 then ucsgm.IsActive else 1 end
			and ((len(@searchText) = 0) or (len(@searchText) > 0 and (u.UserName like '%' + @searchText + '%' or u.EmailAddress like '%' + @searchText + '%' or  u.FirstName like '%' + @searchText + '%' or u.LastName like '%' + @searchText + '%')))
	END
	-- Other users
	INSERT INTO @FinalUsers(UserID, UserName,UserStatus , FirstName, LastName, IsActive
							,IsPlatformAdministrator, DateCreated, DateUpdated,ClientName, SecurityGroupName, Status,RoleCreatedDate, RoleUpdatedDate)
	select u.UserID, UserName,u.Status, FirstName, LastName, u.IsActive
							,IsPlatformAdministrator, u.DateCreated, u.DateUpdated,c.ClientName, case when sg.AdministrativeLevel = 'Administrator' then 'Customer Admin' else sg.SecurityGroupName end,case when ucsgm.IsActive = 1 then 'Active' else ucsgm.InactiveReason end, ucsgm.DateCreated, ucsgm.DateUpdated
	from [User] u with(nolock)
		join UserClientSecurityGroupMap ucsgm with(nolock) on u.UserID = ucsgm.UserID
		join Client c with(nolock) on ucsgm.ClientID = c.ClientID
		join SecurityGroup sg with(nolock) on ucsgm.SecurityGroupID = sg.SecurityGroupID
		join @SecurityGroups sg2 on sg.SecurityGroupID = sg2.SecurityGroupID
		left outer join SecurityGroupOptIn sgoi with(nolock) on sg.SecurityGroupID = sgoi.SecurityGroupID and u.UserID = sgoi.UserID and sgoi.HasAccepted = 0 and sgoi.IsDeleted = 0
	where sg.IsActive = 1
		and ISNULL(u.IsPlatformAdministrator, 0) = 0 
		and u.IsActive = case when @ShowDisabledUsers = 1 then u.IsActive else 1 end
		and ucsgm.IsActive = case when @ShowDisabledUserRoles = 1 then ucsgm.IsActive else 1 end
		and  ((@IsKMStaff = 1) or (@IsKMStaff = 0 and u.IsKMStaff = 0))
		and ((len(@searchText) = 0) or (len(@searchText) > 0 and (u.UserName like '%' + @searchText + '%' or u.EmailAddress like '%' + @searchText + '%' or  u.FirstName like '%' + @searchText + '%' or u.LastName like '%' + @searchText + '%')))
	--Platform Admins
	if @IncludePlatformAdmins = 1
	BEGIN
		INSERT INTO @FinalUsers(UserID, UserName,UserStatus ,FirstName, LastName,IsActive
							,IsPlatformAdministrator, DateCreated, DateUpdated,ClientName, SecurityGroupName,Status,RoleCreatedDate, RoleUpdatedDate)
		SELECT u.UserID, UserName,u.Status, FirstName, LastName, u.IsActive
							,IsPlatformAdministrator, u.DateCreated, u.DateUpdated,'','System Administrator','', u.DateCreated, u.DateUpdated
		FROM [User] u with(nolock)
		where ISNULL(u.IsPlatformAdministrator,0) = 1
		and u.IsActive = case when @ShowDisabledUsers = 1 then u.IsActive else 1 end
		and  ((@IsKMStaff = 1) or (@IsKMStaff = 0 and u.IsKMStaff = 0))
		and ((len(@searchText) = 0) or (len(@searchText) > 0 and (u.UserName like '%' + @searchText + '%' or u.EmailAddress like '%' + @searchText + '%' or  u.FirstName like '%' + @searchText + '%' or u.LastName like '%' + @searchText + '%')))
	END




	SELECT @ClientGroupName as 'Channel', UserID, UserName,FirstName + ' ' + LastName as 'Name',UserStatus as 'User Status',DateCreated as 'User Date Added', DateUpdated as 'User Date Updated',
			ClientName as 'Customer Name',SecurityGroupName as 'Role',Status as 'Role Status', MAX(RoleCreatedDate) as 'Role Date Added', MAX(RoleUpdatedDate) as 'Role Date Updated'
							
			FROM @FinalUsers
			GROUP BY UserID,UserName, FirstName + ' ' + LastName,UserStatus, DateCreated, DateUpdated,ClientName, SecurityGroupName,Status

		ORDER BY FirstName + ' ' + LastName