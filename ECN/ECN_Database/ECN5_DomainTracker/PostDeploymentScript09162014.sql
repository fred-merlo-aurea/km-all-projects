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


--Alter Table DomainTracker add BaseChannelID int not null default -1 
--Alter Table DomainTrackerUserProfile add BaseChannelID int not null default -1

use ECN5_DomainTracker
GO

--Update Domain Tracker values
Update
	DomainTracker
Set
	DomainTracker.BaseChannelID = c.BaseChannelID
from
	ecn5_accounts..Customer c
	inner join DomainTracker on c.CustomerID = DomainTracker.CustomerID


--Update Domain TrackerUserProfile values
Update
	DomainTrackerUserProfile
Set
	DomainTrackerUserProfile.BaseChannelID = c.BaseChannelID
from
	ecn5_accounts..Customer c
	inner join DomainTrackerUserProfile on c.CustomerID = DomainTrackerUserProfile.CustomerID


begin tran
--Verify values
--Drop old column
--Alter Table DomainTracker drop CustomerID
--Alter Table DomainTracker drop CustomerID
--Drop default
--Alter Table DomainTracker drop constraint DF__DomainTra__BaseC__239E4DCF 
--Alter Table DomainTrackerActivity drop constraint DF__DomainTra__BaseC__22AA2996
rollback

GO

---------------------------------------------------------------------------------------------------------
-- Consolidate multiple DomainTracker records into a single instance for the Basechannel
--
---------------------------------------------------------------------------------------------------------

---------------------
---------------------
--Backup Tables
---------------------

IF EXISTS (SELECT 1 FROM Sys.Tables WHERE Name = 'DomainTrackerValue_Bak' ) DROP TABLE DomainTrackerValue_Bak
IF EXISTS (SELECT 1 FROM Sys.Tables WHERE Name = 'DomainTrackerFields_Bak' ) DROP TABLE DomainTrackerFields_Bak
IF EXISTS (SELECT 1 FROM Sys.Tables WHERE Name = 'DomainTrackerUserProfile_Bak' ) DROP TABLE DomainTrackerUserProfile_Bak
IF EXISTS (SELECT 1 FROM Sys.Tables WHERE Name = 'DomainTrackerActivity_Bak' ) DROP TABLE DomainTrackerActivity_Bak
IF EXISTS (SELECT 1 FROM Sys.Tables WHERE Name = 'DomainTracker_Bak' ) DROP TABLE DomainTracker_Bak

SELECT * INTO DomainTrackerValue_Bak FROM DomainTrackerValue
SELECT * INTO DomainTrackerFields_Bak FROM DomainTrackerFields
SELECT * INTO DomainTrackerUserProfile_Bak FROM DomainTrackerUserProfile
SELECT * INTO DomainTrackerActivity_Bak FROM DomainTrackerActivity
SELECT * INTO DomainTracker_Bak FROM DomainTracker


---------------------
--Create Temp Tables
---------------------

--Consolidate Domaintracker on Domain and BasechannelId

SELECT 
	(SELECT MIN(DomaintrackerID) FROM DomainTracker dt2 WHERE dt.Domain = dt2.Domain AND dt.BasechannelId = dt2.BaseChannelId) As DomainTrackerIdToKeep,
	dt.*
INTO 
	#DomainTrackerIdMap
from 
	DomainTracker dt
ORDER BY
	1,2,4

--Consolidate Profile on EmailAddress and BasechannelId

SELECT 
	(SELECT MIN(ProfileId) FROM DomainTrackerUserProfile up2 WHERE up.EmailAddress = up2.EmailAddress AND up.BasechannelId = up2.BaseChannelId) As ProfileIdToKeep,
	up.*
INTO 
	#ProfileIdMap
FROM 
	DomainTrackerUserProfile up
ORDER BY
	1,2,4

--------------------------------------------------------------
-- Manually set DomainTrackerId for known TrackerKeys in use
--------------------------------------------------------------

UPDATE #DomainTrackerIdMap SET DomainTrackerIdToKeep = (SELECT DomaintrackerId FROM DomainTracker WHERE Domain = 'staging.wattagnet.com' AND TrackerKey = '1c1008dc-407b-4551-a8ce-6408fe961e08') WHERE Domain = 'staging.wattagnet.com'
UPDATE #DomainTrackerIdMap SET DomainTrackerIdToKeep = (SELECT DomaintrackerId  FROM DomainTracker WHERE Domain = 'www.wattagnet.com' AND TrackerKey = 'a5eea54a-5cf6-46d4-bab5-e41c824df53d') WHERE Domain = 'www.wattagnet.com'
UPDATE #DomainTrackerIdMap SET DomainTrackerIdToKeep = (SELECT DomaintrackerId  FROM DomainTracker WHERE Domain = 'www.petfoodindustry.com' AND TrackerKey = '9422b3fa-32dc-413f-8c85-83cdce289492') WHERE Domain = 'www.petfoodindustry.com'
UPDATE #DomainTrackerIdMap SET DomainTrackerIdToKeep = (SELECT DomaintrackerId  FROM DomainTracker WHERE Domain = 'www.athleticbusinessconference.com' AND TrackerKey = '6c48f53f-86d5-48d4-9c11-0db08d61eafb') WHERE Domain = 'www.athleticbusinessconference.com'
UPDATE #DomainTrackerIdMap SET DomainTrackerIdToKeep = (SELECT DomaintrackerId  FROM DomainTracker WHERE Domain = 'www.knowledgemarketing.com' AND TrackerKey = '14b3de58-ed42-47ac-939d-53b15efe7753') WHERE Domain = 'www.knowledgemarketing.com'

---------------------------------------------------------
--Update DomainTrackerId on DomainTrackerFields, soft delete duplicates
---------------------------------------------------------

UPDATE
	dtf
SET 
	DomainTrackerId = DomainTrackerIdToKeep,
	IsDeleted = 1
FROM 
	DomainTrackerFields dtf
	INNER JOIN #DomainTrackerIdMap m on m.domaintrackerId = dtf.DomainTrackerId
WHERE
	dtf.domaintrackerId != DomainTrackerIdToKeep

---------------------------------------------------------
--Update DomainTrackerId and ProfileId on DomainTrackerActivity
---------------------------------------------------------

UPDATE 
	dta
SET 
	DomainTrackerId = DomainTrackerIdToKeep
FROM
	DomainTrackerActivity dta
	INNER JOIN #DomainTrackerIdMap m on m.domaintrackerId = dta.DomainTrackerId
WHERE
	dta.domaintrackerId != DomainTrackerIdToKeep
	
UPDATE
	dta
SET 
	ProfileId = ProfileIdToKeep
FROM
	DomainTrackerActivity dta
	INNER JOIN #ProfileIdMap m on m.ProfileID = dta.ProfileID
WHERE
	dta.ProfileID != ProfileIDToKeep


---------------------------------------------------------
--Update DomainTrackerFieldsId, DomainTrackerActivityId on DomainTrackerValue
---------------------------------------------------------

SELECT 
	(SELECT MIN(DomaintrackerFieldsID) FROM domaintrackerfields dtf2 WHERE dtf2.ISdeleted = 0 AND dtf.DomainTrackerId = dtf2.DomainTrackerId AND dtf.FieldName = dtf2.FieldName) As DomainTrackerFieldIdToKeep,
	dtf.*
INTO 	
	#DomainFieldIdMap
FROM 
	domaintrackerfields dtf
ORDER BY
	1,2,4

UPDATE 
	dtv
SET
	DomainTrackerFieldsId= DomainTrackerFieldIdToKeep
FROM
	DomainTrackerValue dtv
	INNER JOIN #DomainFieldIdMap dtf on dtv.DomainTrackerFieldsId = dtf.DomainTrackerFieldsId
WHERE
	dtv.DomainTrackerFieldsId != DomainTrackerFieldIdToKeep



---------------------------------------------------------
-- Soft Delete Old records from DomainTracker
---------------------------------------------------------

--DELETE FROM DT
UPDATE 
	dt 
SET 
	IsDeleted = 1
FROM 
	DomainTracker dt
	INNER JOIN #DomainTrackerIdMap m on dt.DomainTrackerId = m.DomainTrackerId 
WHERE   
	dt.DomainTrackerId != m.DomainTrackerIdToKeep


---------------------------------------------------------
-- Soft Delete Old records from DomainTrackerUserProfile
---------------------------------------------------------

--DELETE FROM up
UPDATE 
	up 
SET 
	IsDeleted = 1
FROM 
	DomainTrackerUserProfile up 
	INNER JOIN 	#ProfileIdMap m on up.ProfileId = m.ProfileId
WHERE
	up.ProfileId != m.ProfileIdToKeep


-------------------------
-- Clean up temp Tables
-------------------------

DROP TABLE #DomainTrackerIdMap
DROP TABLE #ProfileIdMap
DROP TABLE #DomainFieldIdMap

