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
update Content
set IsValidated = 1

insert into QuickTestBlastConfig(IsDefault, BaseChannelID, BaseChannelDoesOverride, CustomerCanOverride, CustomerID, CustomerDoesOverride,
	AllowAdhocEmails, AutoCreateGroup , AutoArchiveGroup, CreatedUserID, CreatedDate)
	values (1, NULL, NULL, NULL, NULL, NULL, 1, 1, 1, 9869, GETDATE() )