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
declare @PBSFID int
insert into ServiceFeature
VALUES(3, 'Personalization Blast', 'Send blasts containing unique personalization content','PersonalizationBlast', 201, 1, 1, 0.00, 0, 0, GETDATE(), null, 1, null)
select @PBSFID = @@IDENTITY;

insert into ServiceFeatureAccessMap
VALUES(@PBSFID,14, 'Send blasts containing unique personalization content',1, GETDATE(), null, 1, null )