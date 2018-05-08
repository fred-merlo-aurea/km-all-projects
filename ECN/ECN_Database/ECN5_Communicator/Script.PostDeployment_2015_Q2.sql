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
UPDATE Groups
SET groupname = 'Master Suppression'
WHERE MasterSupression = 1 and groupname = 'Master Supression'

update Reports
set ControlName = 'EmailPerformanceByDomain.ascx'
where reportid = 3

--Backfilling data for new gateway functionality
insert into Gateway(CustomerID, Name, PubCode, TypeCode, GroupID, Header, Footer, ShowForgotPassword, ForgotPasswordText, ShowSignup, SignupText, SignupURL, SubmitText, UseStyleFrom,Style, UseConfirmation, ConfirmationMessage, ConfirmationText, UseRedirect, RedirectURL, RedirectDelay, LoginOrCapture, ValidateEmail, ValidatePassword, ValidateCustom, CreatedDate, CreatedUserID, IsDeleted)
VALUES(1, 'KM Gateway','DEMO','DE',49195,'<a href="/Accounts/Login?pubcode=DEMO&typecode=DE">KM Gateway</a>','',1,'Forgot Password?',1,'Sign Up','http://jointformstest.kmpsgroup.com/jointforms/Forms/Subscription.aspx?pubcode=DEMO','Login','default','',1,'','',1,'http://specialprojects.ecn5.com/ecn.webservice/gatewaytest.html',0,'login',1,1,0,GETDATE(),5739,0)--DEMO DE
,(3780, 'Trustee','TRU','DE',224816,'<a href="/Accounts/Login?pubcode=TRU&typecode=DE">Trustee</a>','',1,'Forgot Password?',1,'Sign Up','http://eforms.kmpsgroup.com/jointforms/Forms/Subscription.aspx?pubcode=TRU','Login','default','',1,'','',1,'http://digital.trusteemag.com/',0,'login',1,1,0,GETDATE(),7464,0)--TRU DE
,(2633,'FenderBender','FBAP','APP',229892,'<a href="/Accounts/Login?pubcode=FBAP&typecode=APP">FenderBender</a>','',0,'',0,'','','Login','default','',0,'','',1,'http://www.fenderbender-digital.com/fenderbender/current',0,'capture',0,0,0,GETDATE(), 6412, 0)--FBAP APP
,(2633,'FenderBender','FBAPDE','APP',247346,'<a href="/Accounts/Login?pubcode=FBAPDE&typecode=APP">FenderBender</a>','',0,'',0,'','','Login','default','',0,'','',1,'https://pubsrv.texterity.com/cgi-bin/fenderbender/redirect.cgi',0,'capture',0,0,0,GETDATE(), 6412, 0)--FBAPDE APP
,(3780,'Health Facilities Management','HFM','APP',222071,'<a href="/Accounts/Login?pubcode=HFM&typecode=APP">Health Facilities Management</a>','',0,'',0,'','','Login','default','',0,'Link to APP','You have successfully logged in and will be redirected momentarily.',0,'',0,'capture',0,0,0,GETDATE(), 7464, 0)--HFM APP
,(3780,'Hospitals & Health Networks','HHN','APP',224809,'<a href="/Accounts/Login?pubcode=HFM&typecode=APP">Hospitals & Health Networks</a>','',0,'',0,'','','Login','default','',0,'Link to APP','You have successfully logged in and will be redirected momentarily.',0,'',0,'capture',0,0,0,GETDATE(), 7464, 0)--HHN APP
