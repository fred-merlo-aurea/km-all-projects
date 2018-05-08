--INSERT
CREATE PROCEDURE [dbo].[e_ApplicationUsers_Search]
	@startdate varchar(25),
	@enddate varchar(25),
	@email varchar(50),
	@company varchar(100),
	@SalesForceID varchar(15),
	@SalesRepID uniqueidentifier,
	@UserID uniqueidentifier
AS
BEGIN

	set nocount on

	if (@UserID = cast(cast(0 as binary) as uniqueidentifier))
		begin
			 select au.UserID, 
					au.Email,
					au.username,
					al.LastLoginDate,
					au.IsLockedOut,
					au.CreatedDate as DateAdded, 
					au.FullName as FullName,
					au.CompanyName as CompanyName,
					au.SalesForceID as SalesForceID,
					au.PackageLevel as PackageLevel,
					au.TrialExpireDate as TrialExpireDate,
					au.IsApproved,
					case when au.IsApproved = 0 then 'Inactive' else
					(case 
						when au.TrialExpireDate is null then 'Active'
						when au.TrialExpireDate < GETDATE() then 'Expired' else 'Active' end) 
					End as Status,
					r.RoleName,
					au1.FullName as SalesRepName
			 from ApplicationUsers au with(nolock) join  
				  Roles r with(nolock) on r.RoleId = au.RoleId left outer join
				  ApplicationUsers au1 with(nolock) on au.SalesRepID = au1.userID left outer join
				  (select userID, MAX(ActivityDate) as LastLoginDate from ActivityLog with(nolock) Group by userID) al on  au.userID = al.userID
			 where 
				 (len(@company)=0 or au.CompanyName LIKE @company + '%')  and
				 (len(@SalesForceID)=0 or au.SalesForceID = @SalesForceID ) and
				 (len(@email)=0 or au.Email LIKE '%' + @email + '%') and
				 (len(@startdate)=0  or au.TrialExpireDate  >= @startdate )and 
				 (len(@enddate)=0  or au.TrialExpireDate <= @enddate + ' 23:59:59') and
				 (@SalesRepID = cast(cast(0 as binary) as uniqueidentifier) or au.SalesRepID = @SalesRepID)  
			 order by au.CreatedDate
		end 
	else
		begin
			 select au.UserID, 
					au.Email,
					au.username,
					al.LastLoginDate,
					au.IsLockedOut,
					au.CreatedDate as DateAdded, 
					au.FullName as FullName,
					au.CompanyName as CompanyName,
					au.SalesForceID as SalesForceID,
					au.PackageLevel as PackageLevel,
					au.TrialExpireDate as TrialExpireDate,
					au.IsApproved,
					case when au.IsApproved = 0 then 'Inactive' else
					(case 
						when au.TrialExpireDate is null then 'Active'
						when au.TrialExpireDate < GETDATE() then 'Expired' else 'Active' end) 
					End as Status,
					r.RoleName,
					au1.FullName as SalesRepName
			 from ApplicationUsers au with(nolock) join  
				  Roles r with(nolock) on r.RoleId = au.RoleId left outer join
				  ApplicationUsers au1 with(nolock) on au.SalesRepID = au1.userID left outer join
				  (select userID, MAX(ActivityDate) as LastLoginDate from ActivityLog with(nolock) Group by userID) al on  au.userID = al.userID
			 where 
				 (len(@company)=0 or au.CompanyName LIKE @company + '%')  and
				 (len(@SalesForceID)=0 or au.SalesForceID = @SalesForceID ) and
				 (len(@email)=0 or au.Email LIKE '%' + @email + '%') and
				 (len(@startdate)=0  or au.TrialExpireDate  >= @startdate )and 
				 (len(@enddate)=0  or au.TrialExpireDate <= @enddate + ' 23:59:59') and
				 (@SalesRepID = cast(cast(0 as binary) as uniqueidentifier)  or au.SalesRepID = @SalesRepID) and
				 au.SalesRepID = @UserID
			 
			 order by au.CreatedDate
		end

END