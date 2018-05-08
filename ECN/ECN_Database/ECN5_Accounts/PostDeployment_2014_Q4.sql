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
INSERT INTO LandingPageOption(LPID, Name, Description, IsActive)
VALUES(1,'Redirect URL', 'URL to redirect to', 1),
		(1, 'Redirect Delay','Number of seconds to delay the redirect',1)

INSERT INTO LandingPageOption(LPID, Name, Description, IsActive)
VALUES(1,'Reason Options','Available reasons for unsubscribing',1)