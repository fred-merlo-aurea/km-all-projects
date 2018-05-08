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
Insert into ecn5_accounts..LandingPage(Name,Description, IsActive)
VALUES('Change Email Address','For email address change request',1)
Declare @PageTextID int,@OldEmailID int, @NewEmailID int, @ButtonLabel int, @RequireEmail int, @ReEnterEmail int,
@ConfirmationPage int, @EmailHeader int, @EmailFooter int, @EmailBody int,@FromEmail int, @EmailSubject int,
@FinalConfirmation int
insert into ecn5_accounts..LandingPageOption(LPID, Name, Description, IsActive)
VALUES(5,'Page Text','Message to display on the Page',1)
SELECT @PageTextID = @@IDENTITY;

insert into ecn5_accounts..LandingPageOption(LPID, Name, Description, IsActive)
VALUES(5,'Old Email Label','Text to display before the Old Email textbox',1)
SELECT @OldEmailID = @@IDENTITY;

insert into ecn5_accounts..LandingPageOption(LPID, Name, Description, IsActive)
VALUES(5,'New Email Label','Text to display before the New Email textbox',1)
SELECT @NewEmailID = @@IDENTITY;

insert into ecn5_accounts..LandingPageOption(LPID, Name, Description, IsActive)
VALUES(5,'Button Label','Text to display in the Submit button',1)
SELECT @ButtonLabel = @@IDENTITY;

insert into ecn5_accounts..LandingPageOption(LPID, Name, Description, IsActive)
VALUES(5,'Require Email Re-Entry','Require users to re-enter their New Email',1)
SELECT @RequireEmail = @@IDENTITY;

insert into ecn5_accounts..LandingPageOption(LPID, Name, Description, IsActive)
VALUES(5,'Re-Enter Email Label','Text to display before the Re-Enter Email textbox',1)
SELECT @ReEnterEmail = @@IDENTITY;

insert into ecn5_accounts..LandingPageOption(LPID, Name, Description, IsActive)
VALUES(5,'Confirmation Page Text','Text to display upon successful update',1)
SELECT @ConfirmationPage = @@IDENTITY;

insert into ecn5_accounts..LandingPageOption(LPID, Name, Description, IsActive)
VALUES(5,'Email Header', 'Text/HTML to display in the Header of the Confirmation Email',1)
SELECT @EmailHeader = @@IDENTITY;

insert into ecn5_accounts..LandingPageOption(LPID, Name, Description, IsActive)
VALUES(5,'Email Footer', 'Text/HTML to display in the Footer of the Confirmation Email',1)
SELECT @EmailFooter = @@IDENTITY;

insert into ecn5_accounts..LandingPageOption(LPID, Name, Description, IsActive)
VALUES(5,'Email Body', 'Text/HTML to display in the body of the Confirmation Email',1)
SELECT @EmailBody = @@IDENTITY;

insert into ecn5_accounts..LandingPageOption(LPID, Name, Description, IsActive)
VALUES(5,'From Email','From Email Address for the Confirmation Email',1)
SELECT @FromEmail = @@IDENTITY;

insert into ecn5_accounts..LandingPageOption(LPID, Name, Description, IsActive)
VALUES(5,'Email Subject','Subject of the Confirmation Email',1)
SELECT @EmailSubject = @@IDENTITY;

insert into ecn5_accounts..LandingPageOption(LPID, Name, Description, IsActive)
VALUES(5, 'Final Confirmation Text', 'Text to display after subscriber confirms update',1)
SELECT @FinalConfirmation = @@IDENTITY;

declare @DefaultID int
insert into ecn5_accounts..LandingPageAssign(LPID, IsDefault, Label,Header, Footer, CreatedUserID, CreatedDate)
VALUES(5,1,'Change Email Address','<table width="750" border="0" cellpadding="0" cellspacing="0" id="Table1">    <tr>        <td align="center" style="font-family: Arial, Helvetica, sans-serif; font-size: 10px;"></td>        <tr>            <td align="left" style="padding: 0 0 0 30"><a href="http://www.ecn5.com/index.htm">                <img src="http://www.ecn5.com/ecn.images/Channels/12/kmlogo.jpg" border="0"></a></td>            </td>  </tr>    <tr>        <td align="center" bgcolor="#666666" height="10px"></td>    </tr>    <tr>        <td height="20px"></td>    </tr></table>','<table width="750" border="0" cellpadding="0" cellspacing="0">    <tr>        <td align="left" valign="top" colspan="2">            <table align="center">                <tr>                    <td>                        <center><hr width="750"/></center>                    </td>                </tr>                <tr>                    <td>                        <center><p><a href="http://www.knowledgemarketing.com/privacy-policy" target="_blank" style="color: gray;">Privacy Policy</a> | <a href="http://www.knowledgemarketing.com/anti-spam-policy" target="_blank" style="color: gray;">Anti-Spam Policy</a> | <a href="http://www.knowledgemarketing.com/index.php/about-km/contact-us/" target="_blank" style="color: gray;">Contact Us</a><font size="1.5" face="arial" color="gray" align="center"><br/> Copyright &copy;<script type="text/javascript">copyright = new Date(); update = copyright.getFullYear(); document.write(update);</script> Knowledge Marketing, Inc., All Rights Reserved. </font></p></center>                    </td>                </tr>            </table>        </td>    </tr></table>',4496,GETDATE())
Select @DefaultID = @@IDENTITY;
insert into ecn5_accounts..LandingPageAssignContent(LPAID, LPOID, Display, CreatedUserID, CreatedDate, IsDeleted, SortOrder)
VALUES(@DefaultID, @PageTextID, 'Please enter your old and new email address and click Submit to start the change email address process',5739,GETDATE(),0,0),
(@DefaultID, @OldEmailID, 'Old Email Address',5739,GETDATE(),0,1),
(@DefaultID, @NewEmailID, 'New Email Address',5739,GETDATE(),0,2),
(@DefaultID, @ButtonLabel, 'Submit',5739,GETDATE(),0,3),
(@DefaultID, @RequireEmail, 'true',5739,GETDATE(),0,4),
(@DefaultID, @ReEnterEmail, 'Re-Enter New Email Address',5739,GETDATE(),0,5),
(@DefaultID, @ConfirmationPage, '<p>Thank you.  You should receive an email shortly.  Please click the link in the email to confirm the email address change.  If you no longer have access to your old email inbox, please contact your customer service representative to update your email address.</p>',5739,GETDATE(),0,6),
(@DefaultID, @EmailHeader, '<table width="750" border="0" cellpadding="0" cellspacing="0" id="Table1">    <tr>        <td align="center" style="font-family: Arial, Helvetica, sans-serif; font-size: 10px;"></td>        <tr>            <td align="left" style="padding: 0 0 0 30"><a href="http://www.ecn5.com/index.htm">                <img src="http://www.ecn5.com/ecn.images/Channels/12/kmlogo.jpg" border="0"></a></td>            </td>  </tr>    <tr>        <td align="center" bgcolor="#666666" height="10px"></td>    </tr>    <tr>        <td height="20px"></td>    </tr></table>',5739,GETDATE(),0,7),
(@DefaultID, @EmailFooter, '<table width="750" border="0" cellpadding="0" cellspacing="0">    <tr>        <td align="left" valign="top" colspan="2">            <table align="center">                <tr>                    <td>                        <center><hr width="750"/></center>                    </td>                </tr>                <tr>                    <td>                        <center><p><a href="http://www.knowledgemarketing.com/privacy-policy" target="_blank" style="color: gray;">Privacy Policy</a> | <a href="http://www.knowledgemarketing.com/anti-spam-policy" target="_blank" style="color: gray;">Anti-Spam Policy</a> | <a href="http://www.knowledgemarketing.com/index.php/about-km/contact-us/" target="_blank" style="color: gray;">Contact Us</a><font size="1.5" face="arial" color="gray" align="center"><br/> Copyright &copy;<script type="text/javascript">copyright = new Date(); update = copyright.getFullYear(); document.write(update);</script> Knowledge Marketing, Inc., All Rights Reserved. </font></p></center>                    </td>                </tr>            </table>        </td>    </tr></table>',5739,GETDATE(),0,8),
(@DefaultID, @EmailBody, '<table width="750" border="0" cellpadding="0" cellspacing="0"><tr><td><p>Thank you for updating your email address.</p></td></tr></table>',5739,GETDATE(),0,9),
(@DefaultID, @FromEmail, 'info@knowledgemarketing.com',5739,GETDATE(),0,10),
(@DefaultID, @EmailSubject, 'Email Address Update',5739,GETDATE(),11),
(@DefaultID, @FinalConfirmation, '<p>Thank you for updating your email address.</p>',5739,GETDATE(),0,12)

update ecn5_accounts..LandingPage
set BaseChannel = 1, Customer = 1
where LPID < 5 and LPID <> 2

update ecn5_accounts..LandingPage
set BaseChannel = 1, Customer = 0
where LPID = 5

update ecn5_accounts..LandingPage
set BaseChannel = 0, Customer = 0
where LPID = 2


update Basechannel
set TestBlastLimit = 50 
where BaseChannelID = 45

update Customer
set TestBlastLimit = 30
where CustomerID = 1209

update Customer
set TestBlastLimit = 30
where CustomerID = 1797

update Customer 
set TestBlastLimit = 20
where CustomerID = 1216

update Basechannel
set TestBlastLimit = 30 
where BaseChannelID = 6

update Customer
set TestBlastLimit = 20
where CustomerID = 2617

update Customer
set TestBlastLimit = 20
where CustomerID = 3053

update Customer
set TestBlastLimit = 20
where CustomerID = 3059

update Customer
set TestBlastLimit = 20
where CustomerID = 3083

update Customer
set TestBlastLimit = 20
where CustomerID = 3243

update Customer
set TestBlastLimit = 20
where CustomerID = 3420

update Customer
set TestBlastLimit = 20
where CustomerID = 3421

update Customer
set TestBlastLimit = 20
where CustomerID = 3422

update Customer
set TestBlastLimit = 20
where CustomerID = 3423

update Customer
set TestBlastLimit = 20
where CustomerID = 3424

update Basechannel
set TestBlastLimit = 20
where BaseChannelID = 139


--Added new column to basechannel, need to populate column in existing basechannels with data - Corwin
UPDATE ecn5_accounts..Basechannel WITH(ROWLOCK) 
   SET AccessKey = NEWID() 
 WHERE AccessKey IS NULL
--end

--We now will have a FormsUser user for each customer.  This is created by the business layer when creating a new customer.  We need to create this user for existing customers. - Corwin
use ecn5_accounts;
 
DECLARE @CreatedByUserID int = 4496; -- users created by bill in dev
DECLARE @ExecuteDateTime DateTime = GETDATE();

DECLARE @FormsUserName varchar(100) = 'F0rm5U5er';

DECLARE @TEST_ONLY bit = 0;

 
CREATE TABLE #Temp_Users(
	[TEMP_UserID] [int] IDENTITY(1,1) NOT NULL,
	[CustomerID] [int] NULL,
	[UserName] [varchar](100) NULL,
	[Password] [varchar](50) NULL,
	[CommunicatorOptions] [varchar](50) NULL,
	[CollectorOptions] [varchar](50) NULL,
	[CreatorOptions] [varchar](50) NULL,
	[AccountsOptions] [varchar](50) NULL,
	[ActiveFlag] [varchar](1) NULL,
	[CreatedDate] [datetime] NULL,
	[AcceptTermsDate] [datetime] NULL,
	[RoleID] [int] NULL,
	[AccessKey] [uniqueidentifier] NULL,
	[CreatedUserID] [int] NULL,
	[IsDeleted] [bit] NULL
);

-- CREATE TABLE #TEMP_UserActions( [TEMP_UserActionID] [int] IDENTITY(1,1) NOT NULL, [UserID] [int] NULL, [ActionID] [int] NULL,[Active] [char](1) NOT NULL);

CREATE TABLE #TEMP_UserActionsTemplate(
    [TEMP_UserActionsTemplateID] [int] IDENTITY(1,1) NOT NULL,
	[ActionID] [int] NOT NULL
);

INSERT INTO #TEMP_UserActionsTemplate (ActionID)
VALUES (15),(14),(13),(4),(5),(6),(7),(8),(9),(10),(11),(18),(12),(16),(17);


WITH FormsUserCount(CustomerID,Count) As (SELECT CustomerID, COUNT(UserId) [Count] FROM Users WHERE UserName = @FormsUserName GROUP BY CustomerID) 
INSERT INTO #Temp_Users (
			CustomerID, 
			UserName, 
			Password, 
			CommunicatorOptions, 
			CollectorOptions, 
			CreatorOptions,
			AccountsOptions, 
			ActiveFlag, 
			CreatedDate, 
			AcceptTermsDate, 
			RoleID, 
			AccessKey, 
			CreatedUserID, 
			IsDeleted )
SELECT 			
			C.CustomerID , -- CustomerID, 
			@FormsUserName, -- UserName, 
			SUBSTRING(CAST(NEWID() AS VARCHAR(36)),0,7) , -- Password, 
			'000', -- CommunicatorOptions, 
			'000', -- CollectorOptions, 
			'000', -- CreatorOptions
			'001000', -- AccountsOptions, 
			'Y', -- ActiveFlag, 
			@ExecuteDateTime, -- CreatedDate, 
			@ExecuteDateTime, -- AcceptTermsDate, 
			R.RoleID, -- RoleID, 
			NEWID(), -- AccessKey, 
			@CreatedByUserID, -- CreatedUserID, 
			0 -- IsDeleted
 FROM Customer C
 LEFT OUTER JOIN Role R ON(R.CustomerID = C.CustomerID AND LOWER(R.RoleName) = 'everything')
 LEFT OUTER JOIN FormsUserCount FUC ON (FUC.CustomerID = C.CustomerID)
 WHERE FUC.Count IS NULL;

BEGIN TRANSACTION
BEGIN TRY

	INSERT INTO Users (
				CustomerID, 
				UserName, 
				Password, 
				CommunicatorOptions, 
				CollectorOptions, 
				CreatorOptions,
				AccountsOptions, 
				ActiveFlag, 
				CreatedDate, 
				AcceptTermsDate, 
				RoleID, 
				AccessKey, 
				CreatedUserID, 
				IsDeleted )
	 SELECT CustomerID, 
			UserName, 
			Password, 
			CommunicatorOptions, 
			CollectorOptions, 
			CreatorOptions,
			AccountsOptions, 
			ActiveFlag, 
			CreatedDate, 
			AcceptTermsDate, 
			RoleID, 
			AccessKey, 
			CreatedUserID, 
			IsDeleted
	   FROM #Temp_Users;

	INSERT UserActions (UserID,ActionID,Active)
	SELECT U.UserID, -- UserID
		   UAT.ActionID, -- ActionID
		   'Y'  -- Active
	  FROM #Temp_Users UT
	  JOIN Users U ON (U.CustomerID = UT.CustomerID AND U.UserName = UT.UserName)
	  CROSS JOIN #TEMP_UserActionsTemplate UAT
	ORDER BY UT.CustomerID ASC, UAT.TEMP_UserActionsTemplateID ASC;

	select * from Users 
	 WHERE CreatedDate = @ExecuteDateTime AND CreatedUserID = @CreatedByUserID 
	 ORDER BY UserID ASC

	select * from UserActions 
	 WHERE UserID in (select UserID from Users WHERE CreatedDate = @ExecuteDateTime AND CreatedUserID = @CreatedByUserID)
	ORDER BY UserActionID ASC

	IF @TEST_ONLY = 1
		ROLLBACK TRANSACTION  -- FOR TESTING
	ELSE 
		COMMIT TRANSACTION

END TRY
BEGIN CATCH
  ROLLBACK TRANSACTION
	SELECT  ERROR_NUMBER() AS ErrorNumber, ERROR_SEVERITY() AS Severity, ERROR_MESSAGE() AS ErrorMessage, ERROR_LINE() AS ErrorLine, ERROR_PROCEDURE() AS ErrorProcedure
END CATCH  

DROP TABLE #TEMP_UserActionsTemplate;
DROP TABLE #Temp_Users;	 
--end