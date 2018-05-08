CREATE PROCEDURE [dbo].[e_ClientGroup_SelectForUserAuthorization_UserID]  
@UserID int  
AS  
select distinct cg.*   
 from 
	UserClientSecurityGroupMap ucsgm  join 
	ClientGroupClientMap cgcm on ucsgm.ClientID = cgcm.ClientID join
	ClientGroup cg on cg.ClientGroupID = cgcm.ClientGroupID
where ucsgm.UserID   = @UserID 
  and cg.IsActive    = 1  
  and cgcm.IsActive  = 1 
  and ucsgm.IsActive = 1
