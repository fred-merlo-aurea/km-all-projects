
/* This script is used to add a 'Platform Home' entry to Domain Tracking's Menu */

update Menu
set MenuOrder = -1
where MenuID = 129

insert into Menu(ApplicationID ,IsServiceFeature ,ServiceFeatureID ,MenuName ,Description ,IsParent ,ParentMenuID ,URL ,IsActive ,MenuOrder ,HasFeatures ,ImagePath ,DateCreated ,DateUpdated ,CreatedByUserID ,UpdatedByUserID ,ServiceID ,IsClientGroupService ,IsSysAdmin ,IsChannelAdmin ,IsCustomerAdmin)
VALUES(23, 0, -1, 'Platform Home', NULL, 0, 123, '/ecn.accounts/main/default.aspx', 1 ,-1 ,0 , '', GetDate(), NULL ,1 ,NULL ,0 ,NULL ,NULL ,NULL ,NULL)
