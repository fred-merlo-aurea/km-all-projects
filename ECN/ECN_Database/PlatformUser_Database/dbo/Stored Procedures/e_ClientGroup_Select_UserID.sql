CREATE PROCEDURE [dbo].[e_ClientGroup_Select_UserID]  
@UserID int  
AS  
 select distinct cg.*  
 from ClientGroup cg with(nolock)  
 join ClientGroupClientMap cgum with(nolock) on cg.ClientGroupID = cgum.ClientGroupID  
 join UserClientSecurityGroupMap ucsgm with(nolock) on ucsgm.ClientID = cgum.ClientID
 where ucsgm.UserID = @UserID
		and ucsgm.IsActive = 1
		and cgum.IsActive = 1
		and cg.IsActive = 1
GO
