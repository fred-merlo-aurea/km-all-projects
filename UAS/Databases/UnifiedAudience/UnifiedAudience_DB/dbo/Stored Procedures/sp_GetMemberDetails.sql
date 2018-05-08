CREATE PROCEDURE [dbo].[sp_GetMemberDetails]
	@SalesRepID uniqueidentifier
AS
BEGIN
	
	SET NOCOUNT ON

	select au.SalesForceID as SalesForceID,
		au.CompanyName as CompanyName,
		au.FullName as FullName,
		CONVERT(VARCHAR(10), au.CreatedDate,101) as DateAdded, 
		case when au.IsApproved = 0 then 'Inactive' else
		(case 
			when au.TrialExpireDate is null then 'Active'
			when au.TrialExpireDate < GETDATE() then 'Expired' else 'Active' end) 
		End as Status,	
		au.TrialExpireDate as ExpireDate,
		au.PackageLevel as PackageLevel,
		au.FreeOpportunityDownload as Opportunity,
		au.FreeContactDownload as Contact,
		au1.FullName as SalesRep
	 from ApplicationUsers au WITH(NOLOCK)
		join Roles r WITH(NOLOCK) on r.RoleId = au.RoleId 
		left outer join ApplicationUsers au1 WITH(NOLOCK) on au.SalesRepID = au1.userID 
	 where r.RoleName = 'paiduser' and
		(au.SalesRepID =  @SalesRepID or @SalesRepID = cast(cast(0 as binary) as uniqueidentifier))

END