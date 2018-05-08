
/* This script is used to add a 'Platform Home' entry to Communicator.MVC's Menu, and sort the sebmenu entries */

update Menu
set MenuOrder = -1
where MenuID = 135

insert into Menu(ApplicationID ,IsServiceFeature ,ServiceFeatureID ,MenuName ,Description ,IsParent ,ParentMenuID ,URL ,IsActive ,MenuOrder ,HasFeatures ,ImagePath ,DateCreated ,DateUpdated ,CreatedByUserID ,UpdatedByUserID ,ServiceID ,IsClientGroupService ,IsSysAdmin ,IsChannelAdmin ,IsCustomerAdmin)
VALUES(11, 0, -1 ,'Platform Home', NULL, 0, 134, '/ecn.accounts/main/default.aspx', 1, 0, 0, NULL, GETDATE(), NULL ,1 ,NULL ,0 ,NULL ,NULL ,NULL ,NULL)
