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
INSERT INTO SocialMedia(DisplayName, IsActive, MatchString, ImagePath, ShareLink, CanShare, CanPublish, DateAdded, ReportImagePath)
VALUES('Facebook Like',1, 'ECN_Social_FBLike','/images/KMNew/facebooklike.png','https://www.facebook.com/',1,0,GETDATE(), ''),
('Forward To Friend',1,'ECN_Social_F2F','/images/KMNew/forward.png','',1,0,GETDATE(), '')