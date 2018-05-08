﻿CREATE PROCEDURE [dbo].[e_Client_Select_UserID_ClientGroupID]
@UserID int,
@ClientGroupID int
AS
Begin
	SELECT c.*
	FROM 
			[User] u With(NoLock)
			join [UserClientSecurityGroupMap] ucsgm With(NoLock) on u.UserID = ucsgm.UserID
			join [ClientGroupClientMap] cgcm with (nolock) on cgcm.ClientID = ucsgm.ClientID
			join [ClientGroup] cg with (nolock) on cg.ClientGroupID = cgcm.ClientGroupID
			join [Client] c with (nolock) on c.ClientID = cgcm.ClientID
	WHERE u.UserID = @UserID and cg.ClientGroupID = @clientgroupID and u.IsActive = 1 and cg.IsActive = 1 and c.IsActive = 1
	
End