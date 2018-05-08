/*
Post-Deployment Script Template							
--------------------------------------------------------------------------------------
 This file contains SQL statements that will be appended to the build script.		
 Use SQLCMD syntax to include a file in the post-deployment script.			
 Example:      :r .\myfile.sql								
 Use SQLCMD syntax to reference a variable in the post-deployment script.		
 Example:      :setvar TableName MyTable							
               SELECT * FROM [$(TableName)]					
--------------------------------------------------------------------------------------
*/



--27
--start home menu
declare @HomeMENUID int 
insert into Menu(ApplicationID,	IsServiceFeature,	ServiceFeatureID,	MenuName,	Description,	IsParent,	ParentMenuID,	URL,	IsActive,	MenuOrder,	HasFeatures,	ImagePath,	DateCreated,	DateUpdated,	CreatedByUserID,	UpdatedByUserID, ServiceID)
VALUES(11,	0,	-1,	'HOME',	NULL,	1,	-1,	'/ecn.accounts/main/default.aspx',	1,	0,	0,	'',	GETDATE(),	NULL,	1,	NULL, 0)
select @HomeMENUID = @@IDENTITY;

insert into Menu(ApplicationID,	IsServiceFeature,	ServiceFeatureID,	MenuName,	Description,	IsParent,	ParentMenuID,	URL,	IsActive,	MenuOrder,	HasFeatures,	ImagePath,	DateCreated,	DateUpdated,	CreatedByUserID,	UpdatedByUserID, ServiceID)
VALUES(11,	0,	-1,	'Email Marketing',	NULL,	0,	@HomeMENUID,	'/ecn.communicator/main/default.aspx',	1,	0,	0,	'',	GETDATE(),	NULL,	1,	NULL, 3)

insert into Menu(ApplicationID,	IsServiceFeature,	ServiceFeatureID,	MenuName,	Description,	IsParent,	ParentMenuID,	URL,	IsActive,	MenuOrder,	HasFeatures,	ImagePath,	DateCreated,	DateUpdated,	CreatedByUserID,	UpdatedByUserID, ServiceID)
VALUES(11,	0,	-1,	'Unified Audience Database',	NULL,	0,	@HomeMENUID,	'',	1,	1,	0,	'',	GETDATE(),	NULL,	1,	NULL, 2)

insert into Menu(ApplicationID,	IsServiceFeature,	ServiceFeatureID,	MenuName,	Description,	IsParent,	ParentMenuID,	URL,	IsActive,	MenuOrder,	HasFeatures,	ImagePath,	DateCreated,	DateUpdated,	CreatedByUserID,	UpdatedByUserID, ServiceID)
VALUES(11,	0,	-1,	'Surveys',	NULL,	0,	@HomeMENUID,	'/ecn.collector/main/default.aspx',	1,	2,	0,	'',	GETDATE(),	NULL,	1,	NULL, 6)

insert into Menu(ApplicationID,	IsServiceFeature,	ServiceFeatureID,	MenuName,	Description,	IsParent,	ParentMenuID,	URL,	IsActive,	MenuOrder,	HasFeatures,	ImagePath,	DateCreated,	DateUpdated,	CreatedByUserID,	UpdatedByUserID, ServiceID)
VALUES(11,	0,	-1,	'Product Reporting',	NULL,	0,	@HomeMENUID,	'',	1,	3,	0,	'',	GETDATE(),	NULL,	1,	NULL, 11)

insert into Menu(ApplicationID,	IsServiceFeature,	ServiceFeatureID,	MenuName,	Description,	IsParent,	ParentMenuID,	URL,	IsActive,	MenuOrder,	HasFeatures,	ImagePath,	DateCreated,	DateUpdated,	CreatedByUserID,	UpdatedByUserID, ServiceID)
VALUES(11,	0,	-1,	'Digital Editions',	NULL,	0,	@HomeMENUID,	'/ecn.publisher/main/default.aspx',	1,	4,	0,	'',	GETDATE(),	NULL,	1,	NULL, 10)

insert into Menu(ApplicationID,	IsServiceFeature,	ServiceFeatureID,	MenuName,	Description,	IsParent,	ParentMenuID,	URL,	IsActive,	MenuOrder,	HasFeatures,	ImagePath,	DateCreated,	DateUpdated,	CreatedByUserID,	UpdatedByUserID, ServiceID, IsClientGroupService)
VALUES(11,	0,	-1,	'Domain Tracking',	NULL,	0,	@HomeMENUID,	'/ecn.domaintracking/Main/Index',	1,	5,	0,	'',	GETDATE(),	NULL,	1,	NULL, 7, 1)

insert into Menu(ApplicationID,	IsServiceFeature,	ServiceFeatureID,	MenuName,	Description,	IsParent,	ParentMenuID,	URL,	IsActive,	MenuOrder,	HasFeatures,	ImagePath,	DateCreated,	DateUpdated,	CreatedByUserID,	UpdatedByUserID, ServiceID)
VALUES(11,	0,	-1,	'Form Designer',	NULL,	0,	@HomeMENUID,	'/KMWeb/Forms',	1,	6,	0,	'',	GETDATE(),	NULL,	1,	NULL, 4)

insert into Menu(ApplicationID,	IsServiceFeature,	ServiceFeatureID,	MenuName,	Description,	IsParent,	ParentMenuID,	URL,	IsActive,	MenuOrder,	HasFeatures,	ImagePath,	DateCreated,	DateUpdated,	CreatedByUserID,	UpdatedByUserID, ServiceID, IsClientGroupService)
VALUES(11,	0,	-1,	'Marketing Automation',	NULL,	0,	@HomeMENUID,	'/ecn.marketingautomation/',	1,	7,	0,	'',	GETDATE(),	NULL,	1,	NULL, 15, 1)
--end home menu

--Start Groups menu
declare @GroupMenuID int
insert into Menu(ApplicationID,	IsServiceFeature,	ServiceFeatureID,	MenuName,	Description,	IsParent,	ParentMenuID,	URL,	IsActive,	MenuOrder,	HasFeatures,	ImagePath,	DateCreated,	DateUpdated,	CreatedByUserID,	UpdatedByUserID, ServiceID)
VALUES(11,	1,	26,	'Groups',	NULL,	1,	-1,	'/ecn.communicator.mvc/Group/',	1,	0,	0,	'',	GETDATE(),	NULL,	1,	NULL, 0)
select @GroupMenuID = @@IDENTITY;

insert into Menu(ApplicationID,	IsServiceFeature,	ServiceFeatureID,	MenuName,	Description,	IsParent,	ParentMenuID,	URL,	IsActive,	MenuOrder,	HasFeatures,	ImagePath,	DateCreated,	DateUpdated,	CreatedByUserID,	UpdatedByUserID, ServiceID)
VALUES(11,	1,	26,	'Manage Groups',	NULL,	0,	@GroupMenuID,	'/ecn.communicator.mvc/Group/',	1,	0,	0,	'',	GETDATE(),	NULL,	1,	NULL, 0)

insert into Menu(ApplicationID,	IsServiceFeature,	ServiceFeatureID,	MenuName,	Description,	IsParent,	ParentMenuID,	URL,	IsActive,	MenuOrder,	HasFeatures,	ImagePath,	DateCreated,	DateUpdated,	CreatedByUserID,	UpdatedByUserID, ServiceID)
VALUES(11,	1,	26,	'Add Group',	NULL,	0,	@GroupMenuID,	'/ecn.communicator.mvc/Group/Add',	1,	1,	0,	'',	GETDATE(),	NULL,	1,	NULL, 0)

insert into Menu(ApplicationID,	IsServiceFeature,	ServiceFeatureID,	MenuName,	Description,	IsParent,	ParentMenuID,	URL,	IsActive,	MenuOrder,	HasFeatures,	ImagePath,	DateCreated,	DateUpdated,	CreatedByUserID,	UpdatedByUserID, ServiceID)
VALuES(11,	1,	45,	'Add Emails',	NULL,	0,	@GroupMenuID,	'/ecn.communicator.mvc/Group/EmailLoader',	1,	2,	0,	'',	GETDATE(),	NULL,	1,	NULL, 0)

insert into Menu(ApplicationID,	IsServiceFeature,	ServiceFeatureID,	MenuName,	Description,	IsParent,	ParentMenuID,	URL,	IsActive,	MenuOrder,	HasFeatures,	ImagePath,	DateCreated,	DateUpdated,	CreatedByUserID,	UpdatedByUserID, ServiceID)
VALuES(11,	1,	45,	'Import Data',	NULL,	0,	@GroupMenuID,	'/ecn.communicator.mvc/Group/ImportManager',	1,	3,	0,	'',	GETDATE(),	NULL,	1,	NULL, 0)

insert into Menu(ApplicationID,	IsServiceFeature,	ServiceFeatureID,	MenuName,	Description,	IsParent,	ParentMenuID,	URL,	IsActive,	MenuOrder,	HasFeatures,	ImagePath,	DateCreated,	DateUpdated,	CreatedByUserID,	UpdatedByUserID, ServiceID)
VALuES(11,	1,	45,	'Clean Emails',	NULL,	0,	@GroupMenuID,	'/ecn.communicator.mvc/Group/EmailVerifier',	1,	4,	0,	'',	GETDATE(),	NULL,	1,	NULL, 0)

insert into Menu(ApplicationID,	IsServiceFeature,	ServiceFeatureID,	MenuName,	Description,	IsParent,	ParentMenuID,	URL,	IsActive,	MenuOrder,	HasFeatures,	ImagePath,	DateCreated,	DateUpdated,	CreatedByUserID,	UpdatedByUserID, ServiceID)
VALuES(11,	1,	45,	'Suppression',	NULL,	0,	@GroupMenuID,	'/ecn.communicator.mvc/Group/Suppression',	1,	0,	5,	'',	GETDATE(),	NULL,	1,	NULL, 0)

insert into Menu(ApplicationID,	IsServiceFeature,	ServiceFeatureID,	MenuName,	Description,	IsParent,	ParentMenuID,	URL,	IsActive,	MenuOrder,	HasFeatures,	ImagePath,	DateCreated,	DateUpdated,	CreatedByUserID,	UpdatedByUserID, ServiceID)
VALuES(11,	0,	-1,	'Reports',	NULL,	0,	@GroupMenuID,	'/ecn.communicator/main/Reports/ListReports.aspx',	1,	6,	0,	'',	GETDATE(),	NULL,	1,	NULL, 0)

insert into Menu(ApplicationID,	IsServiceFeature,	ServiceFeatureID,	MenuName,	Description,	IsParent,	ParentMenuID,	URL,	IsActive,	MenuOrder,	HasFeatures,	ImagePath,	DateCreated,	DateUpdated,	CreatedByUserID,	UpdatedByUserID, ServiceID)
VALuES(11,	1,	30,	'Group Config',	NULL,	0,	@GroupMenuID,	'/ecn.communicator.mvc/Group/GroupConfig',	1,	0,	7,	'',	GETDATE(),	NULL,	1,	NULL, 0)

insert into Menu(ApplicationID,	IsServiceFeature,	ServiceFeatureID,	MenuName,	Description,	IsParent,	ParentMenuID,	URL,	IsActive,	MenuOrder,	HasFeatures,	ImagePath,	DateCreated,	DateUpdated,	CreatedByUserID,	UpdatedByUserID, ServiceID)
VALuES(11,	1,	211,	'Email Search',	NULL,	0,	@GroupMenuID,	'/ecn.communicator.mvc/Group/EmailSearch',	1,	8,	0,	'',	GETDATE(),	NULL,	1,	NULL, 0)

insert into Menu(ApplicationID,	IsServiceFeature,	ServiceFeatureID,	MenuName,	Description,	IsParent,	ParentMenuID,	URL,	IsActive,	MenuOrder,	HasFeatures,	ImagePath,	DateCreated,	DateUpdated,	CreatedByUserID,	UpdatedByUserID, ServiceID, IsChannelAdmin)
VALuES(11,	1,	45,	'Update Email',	NULL,	0,	@GroupMenuID,	'/ecn.communicator.mvc/Group/UpdateEmailAddress',	1,	9,	0,	'',	GETDATE(),	NULL,	1,	NULL, 0, 1)

--Done with Groups Menu Items

--Start Content/Messages menu items
declare @ContentMenuID int
insert into Menu(ApplicationID,	IsServiceFeature,	ServiceFeatureID,	MenuName,	Description,	IsParent,	ParentMenuID,	URL,	IsActive,	MenuOrder,	HasFeatures,	ImagePath,	DateCreated,	DateUpdated,	CreatedByUserID,	UpdatedByUserID, ServiceID)
VALUES(11,	1,	33,	'Content/Messages',	NULL,	1,	-1,	'/ecn.communicator/main/Content/',	1,	0,	0,	'',	GETDATE(),	NULL,	1,	NULL, 0)
select @ContentMenuID = @@IDENTITY;

insert into Menu(ApplicationID,	IsServiceFeature,	ServiceFeatureID,	MenuName,	Description,	IsParent,	ParentMenuID,	URL,	IsActive,	MenuOrder,	HasFeatures,	ImagePath,	DateCreated,	DateUpdated,	CreatedByUserID,	UpdatedByUserID, ServiceID)
VALuES(11,	1,	33,	'Manage Content',	NULL,	0,	@ContentMenuID,	'/ecn.communicator/main/Content/',	1,	0,	0,	'',	GETDATE(),	NULL,	1,	NULL, 0)

insert into Menu(ApplicationID,	IsServiceFeature,	ServiceFeatureID,	MenuName,	Description,	IsParent,	ParentMenuID,	URL,	IsActive,	MenuOrder,	HasFeatures,	ImagePath,	DateCreated,	DateUpdated,	CreatedByUserID,	UpdatedByUserID, ServiceID)
VALuES(11,	1,	33,	'Manage Messages',	NULL,	0,	@ContentMenuID,	'/ecn.communicator/main/content/defaultMsg.aspx',	1,	1,	0,	'',	GETDATE(),	NULL,	1,	NULL, 0)

insert into Menu(ApplicationID,	IsServiceFeature,	ServiceFeatureID,	MenuName,	Description,	IsParent,	ParentMenuID,	URL,	IsActive,	MenuOrder,	HasFeatures,	ImagePath,	DateCreated,	DateUpdated,	CreatedByUserID,	UpdatedByUserID, ServiceID)
VALuES(11,	1,	33,	'Create Content',	NULL,	0,	@ContentMenuID,	'/ecn.communicator/main/content/contenteditor.aspx',	1,	2,	0,	'',	GETDATE(),	NULL,	1,	NULL, 0)

insert into Menu(ApplicationID,	IsServiceFeature,	ServiceFeatureID,	MenuName,	Description,	IsParent,	ParentMenuID,	URL,	IsActive,	MenuOrder,	HasFeatures,	ImagePath,	DateCreated,	DateUpdated,	CreatedByUserID,	UpdatedByUserID, ServiceID)
VALuES(11,	1,	33,	'Create Message',	NULL,	0,	@ContentMenuID,	'/ecn.communicator/main/content/layouteditor.aspx',	1,	3,	0,	'',	GETDATE(),	NULL,	1,	NULL, 0)

insert into Menu(ApplicationID,	IsServiceFeature,	ServiceFeatureID,	MenuName,	Description,	IsParent,	ParentMenuID,	URL,	IsActive,	MenuOrder,	HasFeatures,	ImagePath,	DateCreated,	DateUpdated,	CreatedByUserID,	UpdatedByUserID, ServiceID)
VALuES(11,	1,	44,	'Dynamic Tags',	NULL,	0,	@ContentMenuID,	'/ecn.communicator/main/content/dynamictaglist.aspx',	1,	4,	0,	'',	GETDATE(),	NULL,	1,	NULL, 0)

insert into Menu(ApplicationID,	IsServiceFeature,	ServiceFeatureID,	MenuName,	Description,	IsParent,	ParentMenuID,	URL,	IsActive,	MenuOrder,	HasFeatures,	ImagePath,	DateCreated,	DateUpdated,	CreatedByUserID,	UpdatedByUserID, ServiceID)
VALuES(11,	1,	48,	'Images/Storage',	NULL,	0,	@ContentMenuID,	'/ecn.communicator/main/content/filemanager.aspx',	1,	5,	0,	'',	GETDATE(),	NULL,	1,	NULL, 0)

insert into Menu(ApplicationID,	IsServiceFeature,	ServiceFeatureID,	MenuName,	Description,	IsParent,	ParentMenuID,	URL,	IsActive,	MenuOrder,	HasFeatures,	ImagePath,	DateCreated,	DateUpdated,	CreatedByUserID,	UpdatedByUserID, ServiceID)
VALuES(11,	1,	78,	'Link Source',	NULL,	0,	@ContentMenuID,	'/ecn.communicator/main/content/linksource.aspx',	1,	0,	6,	'',	GETDATE(),	NULL,	1,	NULL, 0)

insert into Menu(ApplicationID,	IsServiceFeature,	ServiceFeatureID,	MenuName,	Description,	IsParent,	ParentMenuID,	URL,	IsActive,	MenuOrder,	HasFeatures,	ImagePath,	DateCreated,	DateUpdated,	CreatedByUserID,	UpdatedByUserID, ServiceID, IsChannelAdmin)
VALuES(11,	1,	83,	'Message Types',	NULL,	0,	@ContentMenuID,	'/ecn.communicator/main/content/MessageTypeSetup.aspx',	1,	7,	0,	'',	GETDATE(),	NULL,	1,	NULL, 0, 1)

insert into Menu(ApplicationID,	IsServiceFeature,	ServiceFeatureID,	MenuName,	Description,	IsParent,	ParentMenuID,	URL,	IsActive,	MenuOrder,	HasFeatures,	ImagePath,	DateCreated,	DateUpdated,	CreatedByUserID,	UpdatedByUserID, ServiceID, IsChannelAdmin)
VALuES(11,	1,	83,	'Message Type Priority',	NULL,	0,	@ContentMenuID,	'/ecn.communicator/main/content/MessageTypeSort.aspx',	1,	8,	0,	'',	GETDATE(),	NULL,	1,	NULL, 0, 1)

insert into Menu(ApplicationID,	IsServiceFeature,	ServiceFeatureID,	MenuName,	Description,	IsParent,	ParentMenuID,	URL,	IsActive,	MenuOrder,	HasFeatures,	ImagePath,	DateCreated,	DateUpdated,	CreatedByUserID,	UpdatedByUserID, ServiceID)
VALuES(11,	1,	177,	'RSS Feeds',	NULL,	0,	@ContentMenuID,	'/ecn.communicator/main/content/rssList.aspx',	1,	9,	0,	'',	GETDATE(),	NULL,	1,	NULL, 0)
--Done with Content/Message menu items

--Start Blasts/Reporting menu
declare @BlastMenuID int
insert into Menu(ApplicationID,	IsServiceFeature,	ServiceFeatureID,	MenuName,	Description,	IsParent,	ParentMenuID,	URL,	IsActive,	MenuOrder,	HasFeatures,	ImagePath,	DateCreated,	DateUpdated,	CreatedByUserID,	UpdatedByUserID, ServiceID)
VALUES(11,	1,	35,	'Blasts/Reporting',	NULL,	1,	-1,	'/ecn.communicator/main/ecnwizard/',	1,	0,	0,	'',	GETDATE(),	NULL,	1,	NULL, 0)
select @BlastMenuID = @@IDENTITY;

insert into Menu(ApplicationID,	IsServiceFeature,	ServiceFeatureID,	MenuName,	Description,	IsParent,	ParentMenuID,	URL,	IsActive,	MenuOrder,	HasFeatures,	ImagePath,	DateCreated,	DateUpdated,	CreatedByUserID,	UpdatedByUserID, ServiceID)
VALuES(11,	1,	35,	'Setup Blast',	NULL,	0,	@BlastMenuID,	'/ecn.communicator/main/ecnwizard/',	1,	0,	0,	'',	GETDATE(),	NULL,	1,	NULL, 0)

insert into Menu(ApplicationID,	IsServiceFeature,	ServiceFeatureID,	MenuName,	Description,	IsParent,	ParentMenuID,	URL,	IsActive,	MenuOrder,	HasFeatures,	ImagePath,	DateCreated,	DateUpdated,	CreatedByUserID,	UpdatedByUserID, ServiceID)
VALuES(11,	1,	35,	'Active Customer Blasts',	NULL,	0,	@BlastMenuID,	'/ecn.communicator/main/blasts/active.aspx',	1,	1,	0,	'',	GETDATE(),	NULL,	1,	NULL, 0)

insert into Menu(ApplicationID,	IsServiceFeature,	ServiceFeatureID,	MenuName,	Description,	IsParent,	ParentMenuID,	URL,	IsActive,	MenuOrder,	HasFeatures,	ImagePath,	DateCreated,	DateUpdated,	CreatedByUserID,	UpdatedByUserID, ServiceID)
VALuES(11,	1,	129,	'Blast Envelopes',	NULL,	0,	@BlastMenuID,	'/ecn.communicator/main/blasts/BlastEnvelopes.aspx',	1,	2,	0,	'',	GETDATE(),	NULL,	1,	NULL, 0)

insert into Menu(ApplicationID,	IsServiceFeature,	ServiceFeatureID,	MenuName,	Description,	IsParent,	ParentMenuID,	URL,	IsActive,	MenuOrder,	HasFeatures,	ImagePath,	DateCreated,	DateUpdated,	CreatedByUserID,	UpdatedByUserID, ServiceID)
VALuES(11,	1,	41,	'Reports',	NULL,	0,	@BlastMenuID,	'/ecn.communicator/main/Reports/BlastReports.aspx',	1,	3,	0,	'',	GETDATE(),	NULL,	1,	NULL, 0)

insert into Menu(ApplicationID,	IsServiceFeature,	ServiceFeatureID,	MenuName,	Description,	IsParent,	ParentMenuID,	URL,	IsActive,	MenuOrder,	HasFeatures,	ImagePath,	DateCreated,	DateUpdated,	CreatedByUserID,	UpdatedByUserID, ServiceID)
VALuES(11,	1,	97,	'Campaign Item Templates',	NULL,	0,	@BlastMenuID,	'/ecn.communicator/main/ecnwizard/templatelist.aspx',	1,	4,	0,	'',	GETDATE(),	NULL,	1,	NULL, 0)

insert into Menu(ApplicationID,	IsServiceFeature,	ServiceFeatureID,	MenuName,	Description,	IsParent,	ParentMenuID,	URL,	IsActive,	MenuOrder,	HasFeatures,	ImagePath,	DateCreated,	DateUpdated,	CreatedByUserID,	UpdatedByUserID, ServiceID)
VALuES(11,	1,	42,	'Manage Campaigns',	NULL,	0,	@BlastMenuID,	'/ecn.communicator/main/blasts/ManageCampaigns.aspx',	1,	5,	0,	'',	GETDATE(),	NULL,	1,	NULL, 0)

insert into Menu(ApplicationID,	IsServiceFeature,	ServiceFeatureID,	MenuName,	Description,	IsParent,	ParentMenuID,	URL,	IsActive,	MenuOrder,	HasFeatures,	ImagePath,	DateCreated,	DateUpdated,	CreatedByUserID,	UpdatedByUserID, ServiceID, IsSysAdmin)
VALuES(11,	1,	35,	'View Blast Links',	NULL,	0,	@BlastMenuID,	'/ecn.communicator/main/blasts/ViewBlastLinks.aspx',	1,	6,	0,	'',	GETDATE(),	NULL,	1,	NULL, 0, 1)


--Done with Blasts/Reporting menu items

--Starting Folders menu
insert into Menu(ApplicationID,	IsServiceFeature,	ServiceFeatureID,	MenuName,	Description,	IsParent,	ParentMenuID,	URL,	IsActive,	MenuOrder,	HasFeatures,	ImagePath,	DateCreated,	DateUpdated,	CreatedByUserID,	UpdatedByUserID, ServiceID)
VALUES(11,	0,	-1,	'Folders',	NULL,	1,	-1,	'/ecn.communicator/main/folders/folderseditor.aspx',	1,	0,	0,	'',	GETDATE(),	NULL,	1,	NULL, 0)


--Done with Folders Menu
--starting message triggers menu
insert into Menu(ApplicationID,	IsServiceFeature,	ServiceFeatureID,	MenuName,	Description,	IsParent,	ParentMenuID,	URL,	IsActive,	MenuOrder,	HasFeatures,	ImagePath,	DateCreated,	DateUpdated,	CreatedByUserID,	UpdatedByUserID, ServiceID)
VALUES(11,	1,	46,	'Message Triggers',	NULL,	1,	-1,	'/ecn.communicator/main/events/messagetriggers.aspx',	1,	0,	0,	'',	GETDATE(),	NULL,	1,	NULL, 0)

--Done with Message triggers menu
--Starting Reports menu
declare @ReportsMenuID int
insert into Menu(ApplicationID,	IsServiceFeature,	ServiceFeatureID,	MenuName,	Description,	IsParent,	ParentMenuID,	URL,	IsActive,	MenuOrder,	HasFeatures,	ImagePath,	DateCreated,	DateUpdated,	CreatedByUserID,	UpdatedByUserID, ServiceID)
VALUES(11,	0,	-1,	'Reports',	NULL,	1,	-1,	'/ecn.communicator/main/Reports/SentCampaignsReport.aspx',	1,	0,	0,	'',	GETDATE(),	NULL,	1,	NULL, 0)
select @ReportsMenuID = @@IDENTITY;



insert into Menu(ApplicationID,	IsServiceFeature,	ServiceFeatureID,	MenuName,	Description,	IsParent,	ParentMenuID,	URL,	IsActive,	MenuOrder,	HasFeatures,	ImagePath,	DateCreated,	DateUpdated,	CreatedByUserID,	UpdatedByUserID, ServiceID)
VALuES(11,	1,	41,	'Sent Campaign Reports',	NULL,	0,	@ReportsMenuID,	'/ecn.communicator/main/Reports/SentCampaignsReport.aspx',	1,	0,	0,	'',	GETDATE(),	NULL,	1,	NULL, 0)

insert into Menu(ApplicationID,	IsServiceFeature,	ServiceFeatureID,	MenuName,	Description,	IsParent,	ParentMenuID,	URL,	IsActive,	MenuOrder,	HasFeatures,	ImagePath,	DateCreated,	DateUpdated,	CreatedByUserID,	UpdatedByUserID, ServiceID)
VALuES(11,	1,	41,	'Blast Reports',	NULL,	0,	@ReportsMenuID,	'/ecn.communicator/main/Reports/SentCampaignsReport.aspx',	1,	0,	0,	'',	GETDATE(),	NULL,	1,	NULL, 0)

insert into Menu(ApplicationID,	IsServiceFeature,	ServiceFeatureID,	MenuName,	Description,	IsParent,	ParentMenuID,	URL,	IsActive,	MenuOrder,	HasFeatures,	ImagePath,	DateCreated,	DateUpdated,	CreatedByUserID,	UpdatedByUserID, ServiceID)
VALuES(11,	0,	-1,	'Group Reports',	NULL,	0,	@ReportsMenuID,	'/ecn.communicator/main/Reports/ListReports.aspx',	1,	1,	0,	'',	GETDATE(),	NULL,	1,	NULL, 0)

insert into Menu(ApplicationID,	IsServiceFeature,	ServiceFeatureID,	MenuName,	Description,	IsParent,	ParentMenuID,	URL,	IsActive,	MenuOrder,	HasFeatures,	ImagePath,	DateCreated,	DateUpdated,	CreatedByUserID,	UpdatedByUserID, ServiceID)
VALuES(11,	0,	-1,	'Scheduled Reports',	NULL,	0,	@ReportsMenuID,	'/ecn.communicator/main/Reports/scheduledReportlist.aspx',	1,	2,	0,	'',	GETDATE(),	NULL,	1,	NULL, 0)

--Done with Reports menu

--Starting Admin menu	
declare @AdminMenuID int
insert into Menu(ApplicationID,	IsServiceFeature,	ServiceFeatureID,	MenuName,	Description,	IsParent,	ParentMenuID,	URL,	IsActive,	MenuOrder,	HasFeatures,	ImagePath,	DateCreated,	DateUpdated,	CreatedByUserID,	UpdatedByUserID, ServiceID, IsCustomerAdmin)
VALUES(11,	0,	-1,	'Admin',	NULL,	1,	-1,	'',	1,	0,	0,	'',	GETDATE(),	NULL,	1,	NULL, 0, 1)
select @AdminMenuID = @@IDENTITY;

declare @BCAdminMenuID int
insert into Menu(ApplicationID,	IsServiceFeature,	ServiceFeatureID,	MenuName,	Description,	IsParent,	ParentMenuID,	URL,	IsActive,	MenuOrder,	HasFeatures,	ImagePath,	DateCreated,	DateUpdated,	CreatedByUserID,	UpdatedByUserID, ServiceID, IsChannelAdmin)
VALuES(11,	0,	-1,	'Base Channel',	NULL,	1,	@AdminMenuID,	'',	1,	0,	0,	'',	GETDATE(),	NULL,	1,	NULL, 0, 1)
select @BCAdminMenuID = @@IDENTITY;

insert into Menu(ApplicationID,	IsServiceFeature,	ServiceFeatureID,	MenuName,	Description,	IsParent,	ParentMenuID,	URL,	IsActive,	MenuOrder,	HasFeatures,	ImagePath,	DateCreated,	DateUpdated,	CreatedByUserID,	UpdatedByUserID, ServiceID, IsChannelAdmin)
VALuES(11,	0,	-1,	'Landing Pages',	NULL,	0,	@BCAdminMenuID,	'/ecn.communicator/main/admin/landingpages/BaseChannelMain.aspx',	1,	0,	0,	'',	GETDATE(),	NULL,	1,	NULL, 0,1)

insert into Menu(ApplicationID,	IsServiceFeature,	ServiceFeatureID,	MenuName,	Description,	IsParent,	ParentMenuID,	URL,	IsActive,	MenuOrder,	HasFeatures,	ImagePath,	DateCreated,	DateUpdated,	CreatedByUserID,	UpdatedByUserID, ServiceID, IsChannelAdmin)
VALuES(11,	1,	99,	'Subscription Mgmt',	NULL,	0,	@BCAdminMenuID,	'/ecn.communicator/main/admin/subscriptionmanagement/SubscriptionManagementList.aspx',	1,	1,	0,	'',	GETDATE(),	NULL,	1,	NULL, 0,1)

insert into Menu(ApplicationID,	IsServiceFeature,	ServiceFeatureID,	MenuName,	Description,	IsParent,	ParentMenuID,	URL,	IsActive,	MenuOrder,	HasFeatures,	ImagePath,	DateCreated,	DateUpdated,	CreatedByUserID,	UpdatedByUserID, ServiceID, IsChannelAdmin)
VALuES(11,	1,	90,	'Omniture',	NULL,	0,	@BCAdminMenuID,	'/ecn.communicator/main/Omniture/OmnitureBaseChannelSetup.aspx',	1,	2,	0,	'',	GETDATE(),	NULL,	1,	NULL, 0,1)

insert into Menu(ApplicationID,	IsServiceFeature,	ServiceFeatureID,	MenuName,	Description,	IsParent,	ParentMenuID,	URL,	IsActive,	MenuOrder,	HasFeatures,	ImagePath,	DateCreated,	DateUpdated,	CreatedByUserID,	UpdatedByUserID, ServiceID, IsChannelAdmin)
VALuES(11,	0,	-1,	'Salesforce',	NULL,	0,	@BCAdminMenuID,	'/ecn.communicator/main/Salesforce/ECN_SF_Integration.aspx',	1,	3,	0,	'',	GETDATE(),	NULL,	1,	NULL, 0,1)

insert into Menu(ApplicationID,	IsServiceFeature,	ServiceFeatureID,	MenuName,	Description,	IsParent,	ParentMenuID,	URL,	IsActive,	MenuOrder,	HasFeatures,	ImagePath,	DateCreated,	DateUpdated,	CreatedByUserID,	UpdatedByUserID, ServiceID, IsChannelAdmin)
VALuES(11,	1,	82,	'Message Thresholds',	NULL,	0,	@BCAdminMenuID,	'/ecn.communicator/main/blasts/setmessagethreshold.aspx',	1,	4,	0,	'',	GETDATE(),	NULL,	1,	NULL, 0,1)

insert into Menu(ApplicationID,	IsServiceFeature,	ServiceFeatureID,	MenuName,	Description,	IsParent,	ParentMenuID,	URL,	IsActive,	MenuOrder,	HasFeatures,	ImagePath,	DateCreated,	DateUpdated,	CreatedByUserID,	UpdatedByUserID, ServiceID, IsChannelAdmin)
VALuES(11,	1,	35,	'Blast',	NULL,	0,	@BCAdminMenuID,	'/ecn.communicator/main/admin/QuickTestBlastConfiguration/BaseChannelSetup.aspx',	1,	5,	0,	'',	GETDATE(),	NULL,	1,	NULL, 0,1)

declare @CAdminMenuID int
insert into Menu(ApplicationID,	IsServiceFeature,	ServiceFeatureID,	MenuName,	Description,	IsParent,	ParentMenuID,	URL,	IsActive,	MenuOrder,	HasFeatures,	ImagePath,	DateCreated,	DateUpdated,	CreatedByUserID,	UpdatedByUserID, ServiceID, IsCustomerAdmin)
VALuES(11,	0,	-1,	'Customer',	NULL,	1,	@AdminMenuID,	'',	1,	1,	0,	'',	GETDATE(),	NULL,	1,	NULL, 0,1)
select @CAdminMenuID = @@IDENTITY;

insert into Menu(ApplicationID,	IsServiceFeature,	ServiceFeatureID,	MenuName,	Description,	IsParent,	ParentMenuID,	URL,	IsActive,	MenuOrder,	HasFeatures,	ImagePath,	DateCreated,	DateUpdated,	CreatedByUserID,	UpdatedByUserID, ServiceID, IsCustomerAdmin)
VALuES(11,	0,	-1,	'Landing Pages',	NULL,	0,	@CAdminMenuID,	'/ecn.communicator/main/admin/landingpages/CustomerMain.aspx',	1,	0,	0,	'',	GETDATE(),	NULL,	1,	NULL, 0,1)

insert into Menu(ApplicationID,	IsServiceFeature,	ServiceFeatureID,	MenuName,	Description,	IsParent,	ParentMenuID,	URL,	IsActive,	MenuOrder,	HasFeatures,	ImagePath,	DateCreated,	DateUpdated,	CreatedByUserID,	UpdatedByUserID, ServiceID, IsCustomerAdmin)
VALuES(11,	1,	90,	'Omniture',	NULL,	0,	@CAdminMenuID,	'/ecn.communicator/main/Omniture/OmnitureCustomerSetup.aspx',	1,	1,	0,	'',	GETDATE(),	NULL,	1,	NULL, 0,1)

insert into Menu(ApplicationID,	IsServiceFeature,	ServiceFeatureID,	MenuName,	Description,	IsParent,	ParentMenuID,	URL,	IsActive,	MenuOrder,	HasFeatures,	ImagePath,	DateCreated,	DateUpdated,	CreatedByUserID,	UpdatedByUserID, ServiceID, IsCustomerAdmin)
VALuES(11,	0,	-1,	'Gateway',	NULL,	0,	@CAdminMenuID,	'/ecn.communicator/main/admin/Gateway/GatewayList.aspx',	1,	2,	0,	'',	GETDATE(),	NULL,	1,	NULL, 0,1)

insert into Menu(ApplicationID,	IsServiceFeature,	ServiceFeatureID,	MenuName,	Description,	IsParent,	ParentMenuID,	URL,	IsActive,	MenuOrder,	HasFeatures,	ImagePath,	DateCreated,	DateUpdated,	CreatedByUserID,	UpdatedByUserID, ServiceID, IsCustomerAdmin)
VALuES(11,	1,	35,	'Blast',	NULL,	0,	@CAdminMenuID,	'/ecn.communicator/main/admin/QuickTestBlastConfiguration/CustomerSetup.aspx',	1,	3,	0,	'',	GETDATE(),	NULL,	1,	NULL, 0,1)


--Done with Admin menu


--remove smart form service feature from all tables

delete from SecurityGroupPermission where ServiceFeatureAccessMapID in (16,17,18,19,20,21)
delete from ServiceFeatureAccessMap where ServiceFeatureAccessMapID in (16,17,18,19,20,21)
delete from ClientServiceFeatureMap where ServiceFeatureID in (31,32)
delete from ServiceFeature where ServiceFeatureID in (31,32)
