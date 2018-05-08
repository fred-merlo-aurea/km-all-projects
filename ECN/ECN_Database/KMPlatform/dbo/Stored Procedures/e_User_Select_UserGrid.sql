CREATE PROC [dbo].[e_User_Select_UserGrid]
	@PageSize int,
    @PageIndex int,
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
 
--declare @PageSize int = 20,
-- @PageIndex int =1,
-- @ClientGroupID int = 101,
--@ClientID int =101,
--@IncludePlatformAdmins bit = 0,
--@UserIsCAdmin bit = 1,
--@IncludeAllClients bit = 0,
--@IncludeBCAdmins bit = 1
declare @FinalUsers table(UserID int, UserName varchar(50), UserStatus varchar(20), Password varchar(250), DefaultClientGroupID int, DefaultClientID int,FirstName varchar(50), 
						LastName varchar(50),Salt varchar(250), EmailAddress varchar(250), IsActive bit, AccessKey uniqueidentifier, IsAccessKeyValid bit, 
						IsKMStaff bit, IsPlatformAdministrator bit, DateCreated datetime, DateUpdated datetime, CreatedByUserID int, UpdatedByUserID int, ClientName varchar(100),SecurityGroupName varchar(100),
						Status bit,InactiveReason varchar(50),SecurityGroupID int, SetID varchar(100))

declare @SecurityGroups table (SecurityGroupID int,SecurityGroupName varchar(100))
declare @ClientIDs table (ClientID int)

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
	INSERT INTO @FinalUsers(UserID, UserName,UserStatus, Password, DefaultClientGroupID, DefaultClientID, FirstName, LastName, Salt, EmailAddress, IsActive, AccessKey, IsAccessKeyValid,IsKMStaff
							,IsPlatformAdministrator, DateCreated, DateUpdated, CreatedByUserID, UpdatedByUserID,ClientName, SecurityGroupName, Status,InactiveReason,SecurityGroupID,SetID)
	select u.UserID, UserName, u.Status, Password, DefaultClientGroupID, DefaultClientID, FirstName, LastName, Salt, EmailAddress, u.IsActive, AccessKey, IsAccessKeyValid,IsKMStaff
							,IsPlatformAdministrator, u.DateCreated, u.DateUpdated, u.CreatedByUserID, u.UpdatedByUserID ,'',sg.SecurityGroupName, ucsgm.IsActive,ISNULL(ucsgm.InactiveReason,''),sg.SecurityGroupID, case when ucsgm.InactiveReason = 'Pending' then ISNULL(convert
(varchar(100),sgoi.SetID),'') else '' end
	from [User] u with(nolock)
		join UserClientSecurityGroupMap ucsgm with(nolock) on u.UserID = ucsgm.UserID
		join SecurityGroup sg with(nolock) on ucsgm.SecurityGroupID = sg.SecurityGroupID
		left outer join SecurityGroupOptIn sgoi with(nolock) on sg.SecurityGroupID = sgoi.SecurityGroupID and u.UserID = sgoi.UserID and sgoi.HasAccepted = 0 and sgoi.IsDeleted = 0
	where ISNULL(sg.ClientGroupID,0) = @ClientGroupID
		and sg.IsActive = 1
		and ISNULL(u.IsPlatformAdministrator, 0) = 0
		and u.IsActive = case when @ShowDisabledUsers = 1 then u.IsActive else 1 end
		and ucsgm.IsActive = case when @ShowDisabledUserRoles = 1 then ucsgm.IsActive else 1 end
		and  ((@IsKMStaff = 1) or (@IsKMStaff = 0 and u.IsKMStaff = 0))
		and ((len(@searchText) = 0) or (len(@searchText) > 0 and (u.UserName like '%' + @searchText + '%' or u.EmailAddress like '%' + @searchText + '%' or  u.FirstName like '%' + @searchText + '%' or u.LastName like '%' + @searchText + '%')))
END
-- Other users
INSERT INTO @FinalUsers(UserID, UserName,UserStatus ,Password, DefaultClientGroupID, DefaultClientID, FirstName, LastName, Salt, EmailAddress, IsActive, AccessKey, IsAccessKeyValid,IsKMStaff
						,IsPlatformAdministrator, DateCreated, DateUpdated, CreatedByUserID, UpdatedByUserID,ClientName, SecurityGroupName, Status, InactiveReason,SecurityGroupID,SetID)
select u.UserID, UserName,u.Status, Password, DefaultClientGroupID, DefaultClientID, FirstName, LastName, Salt, EmailAddress, u.IsActive, u.AccessKey, IsAccessKeyValid,IsKMStaff
						,IsPlatformAdministrator, u.DateCreated, u.DateUpdated, u.CreatedByUserID, u.UpdatedByUserID ,c.ClientName, sg2.SecurityGroupName,ucsgm.IsActive, ISNULL(ucsgm.InactiveReason,''),sg.SecurityGroupID,case when ucsgm.InactiveReason = 'Pending' then ISNULL(convert(varchar(100),sgoi.SetID),'') else '' end
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
	INSERT INTO @FinalUsers(UserID, UserName,UserStatus ,Password, DefaultClientGroupID, DefaultClientID, FirstName, LastName, Salt, EmailAddress, IsActive, AccessKey, IsAccessKeyValid,IsKMStaff
						,IsPlatformAdministrator, DateCreated, DateUpdated, CreatedByUserID, UpdatedByUserID,ClientName, SecurityGroupName,Status,InactiveReason,SecurityGroupID,SetID)
	SELECT u.UserID, UserName,u.Status, Password, DefaultClientGroupID, DefaultClientID, FirstName, LastName, Salt, EmailAddress, u.IsActive, AccessKey, IsAccessKeyValid,IsKMStaff
						,IsPlatformAdministrator, u.DateCreated, u.DateUpdated, u.CreatedByUserID, u.UpdatedByUserID ,'','',0,'',-1,''
	FROM [User] u with(nolock)
	where ISNULL(u.IsPlatformAdministrator,0) = 1
		and u.IsActive = case when @ShowDisabledUsers = 1 then u.IsActive else 1 end
		and  ((@IsKMStaff = 1) or (@IsKMStaff = 0 and u.IsKMStaff = 0))
		and ((len(@searchText) = 0) or (len(@searchText) > 0 and (u.UserName like '%' + @searchText + '%' or u.EmailAddress like '%' + @searchText + '%' or  u.FirstName like '%' + @searchText + '%' or u.LastName like '%' + @searchText + '%')))
END

declare @TotalCount int

select  @TotalCount = COUNT(*) OVER () 
from @FinalUsers
group by UserID,UserName,UserStatus,Password, DefaultClientGroupID, DefaultClientID, FirstName, LastName, Salt, EmailAddress, IsActive, AccessKey, IsAccessKeyValid,IsKMStaff
						,IsPlatformAdministrator, DateCreated, DateUpdated, CreatedByUserID, UpdatedByUserID,ClientName,SecurityGroupName,Status,InactiveReason,SecurityGroupID,SetID
;



--Do the final select with paging
WITH Results
	AS (SELECT ROW_NUMBER() OVER (ORDER BY FirstName,LastName) 
	AS ROWNUM,UserID, UserName,UserStatus,Password, DefaultClientGroupID, DefaultClientID, FirstName, LastName, Salt, EmailAddress, IsActive, AccessKey, IsAccessKeyValid,IsKMStaff
						,IsPlatformAdministrator, DateCreated, DateUpdated, CreatedByUserID, UpdatedByUserID,ClientName,SecurityGroupName,Status,InactiveReason,SecurityGroupID,SetID
		FROM @FinalUsers
		GROUP BY UserID,UserName,UserStatus,Password, DefaultClientGroupID, DefaultClientID, FirstName, LastName, Salt, EmailAddress, IsActive, AccessKey, IsAccessKeyValid,IsKMStaff
						,IsPlatformAdministrator, DateCreated, DateUpdated, CreatedByUserID, UpdatedByUserID,ClientName,SecurityGroupName,Status,InactiveReason,SecurityGroupID,SetID )
	SELECT UserID, UserName,UserStatus,Password, DefaultClientGroupID, DefaultClientID, FirstName, LastName, Salt, EmailAddress, IsActive, AccessKey, IsAccessKeyValid,IsKMStaff
						,IsPlatformAdministrator, DateCreated, DateUpdated, CreatedByUserID, UpdatedByUserID,ClientName,@TotalCount as TotalCount, SecurityGroupName,Status,InactiveReason,SecurityGroupID,SetID
	FROM Results
	WHERE ROWNUM between ((@PageIndex - 1) * @PageSize + 1) and (@PageIndex * @PageSize)
	ORDER BY FirstName, LastName