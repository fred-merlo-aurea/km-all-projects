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
/* For 6460, reducing encryption table to one row and deleting application id column*/
begin tran

alter table KMCOMMON..Encryption drop column ApplicationID


declare @rows int = 0
set @rows = (select COUNT(*) - 1 from KMCommon..Encryption)

set rowcount @rows

Delete from KMCommon..Encryption

set rowcount 0

commit
