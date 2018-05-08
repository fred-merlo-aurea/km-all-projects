IF EXISTS (SELECT 1 FROM Sysobjects where name = 'job_DataMatching')
DROP Proc [job_DataMatching]
GO

SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE PROCEDURE [dbo].[job_DataMatching]

	@ProcessCode varchar(50),
	@SourceFileID int
AS

SET NOCOUNT ON
BEGIN

declare @TxtOut varchar(max)
declare @IsCirc	bit
--declare 
--	@ProcessCode varchar(50),
--	@SourceFileID int
--Select @ProcessCode = 'cmc7876Ilj9x_02212017_23:32:36', @SourceFileID = 5866


-- All records will be either Circ or Non Circ.  No mix
select @IsCirc =  IsNull((Select Top 1 IsCirc from SubscriberFinal sf join pubs p on p.PubCode = sf.PubCode WHERE SourceFileID = @SourceFileId AND ProcessCode = @ProcessCode),0)


--select *
--into tmp_SubscriberFinal
-- from subscriberFinal where processcode = @ProcessCode

--update subscriberFinal set Igrp_no = Null where  processcode = @ProcessCode

--update #Matchgroups set recordtype = null, matchid = null

--*********************************************
-- Create Temp #SubscriberFinal for
-- better performance.  At the end, update
-- real subscriberFinal table from Temp.
-- ********************************************
-- drop table #Subscriberfinal


PRINT ('Start / '  + CONVERT(VARCHAR(20), GETDATE(), 114))

Select * 
into #Subscriberfinal
from Subscriberfinal 
		WHERE SourceFileID = @SourceFileId AND ProcessCode = @ProcessCode 
--** Comment out line below to test
		and (IGrp_No is null or IGrp_No = '00000000-0000-0000-0000-000000000000')
--** 
CREATE INDEX idx_SubscriberFinal_SFRecordIdentifier ON #Subscriberfinal(SFRecordIdentifier)
CREATE INDEX idx_SubscriberFinal_FnameLname ON #Subscriberfinal(Fname,Lname)
CREATE INDEX idx_SubscriberFinal_LnameFname ON #Subscriberfinal(Lname,Fname)
CREATE INDEX idx_SubscriberFinal_IgrpNo ON #Subscriberfinal(IGRP_No)



-- Update Igrp_no to Null for later comparisions
update #Subscriberfinal set igrp_no = NULL where IGrp_no = '00000000-0000-0000-0000-000000000000'

--select * from #subscriberfinal
--update #Subscriberfinal set igrp_no = NULL
		
PRINT ('Temp Subscriberfinal loaded / '  + CONVERT(VARCHAR(20), GETDATE(), 114))
		
		--
		-- Temporary patch for records with ascii characters in email field, need to remove when fix is implemented
		--
		UPDATE #SubscriberFinal
		SET Email =  ''  
		WHERE LEN(LTRIM(rtrim(email))) > 0 and LEN(LTRIM(RTRIM(email))) <= 4

PRINT ('UPDATE SubscriberFinal / '  + Convert(VARCHAR,@@ROWCOUNT) + ' / ' + CONVERT(VARCHAR(20), GETDATE(), 114))

		--
		-- Begin CREATE TABLE
		--
--Drop table #MatchGroups
		CREATE TABLE #MatchGroups(
			 MGID int identity NOT NULL
			,RecordMatchFound Int default 0
			,SFRecordIdentifier UNIQUEIDENTIFIER
			,indNameAddress UNIQUEIDENTIFIER
			,indNameEmail UNIQUEIDENTIFIER
			,indNameCompany UNIQUEIDENTIFIER
			,indNamePhone UNIQUEIDENTIFIER
			,indNameNotBlankEmail UNIQUEIDENTIFIER
			,indDistEmail UNIQUEIDENTIFIER
			,IsOriginal bit
			,PubID int
			,EmailID int
			,SequenceID int
			,Igrp_no UNIQUEIDENTIFIER
			,FName varchar(100)
			,LName varchar(100)
			,Company varchar(100)
			,Title varchar(100)
			,Address varchar(255)
			,city varchar(50)
			,State varchar(50)
			,ZipCode varchar(50)
			,Phone varchar(100)
			,Email varchar(100)
			,FName3 varchar(3)
			,LName6 varchar(6)
			,Address15 varchar(15)
			,Address5 varchar(5)
			,Company5 varchar(8)
			,Title5 varchar(5)
			,RecordType varchar(20)
			,MatchID UNIQUEIDENTIFIER
			,Qdate DateTime
			,IsNewRecord bit default(0)
			,NameSplitRecord bit default(0) )

		INSERT INTO #MatchGroups(SFRecordIdentifier,FName,LName,Company,Title,Address,city,State,Zipcode,Phone,Email,FName3,LName6,Address15,Address5,Company5,Title5,IsOriginal, PubID, EmailID, SequenceID, RecordType, MatchID, Qdate)
		SELECT SFRecordIdentifier, isnull(FName,''), isnull(LName,''), isnull(Company,''), isnull(Title, ''), isnull(Address,''),ISNULL(city,''), isnull(State,'') , isnull(Zip,'') , isnull(Phone,'') , isnull(Email,'') , 
						IsNull(Left(FName, master.dbo.fn_smallest(Len(FName),3)),''),
						IsNull(Left(LName, master.dbo.fn_smallest(Len(LName),6)),''),
						left(isnull(Address,''),15), left(isnull(Address,''),5), left(isnull(Company,''),5 ), left(isnull(Title,''),5 ), 1, p.PubID, ISNULL(EmailID, 0), Sequence, NULL, NULL, IsNull(QDate, GetDate())
		FROM #SubscriberFinal WITH(NOLOCK) 
		LEFT JOIN pubs p on p.PubCode = #Subscriberfinal.PubCode

PRINT ('INSERT INTO #MatchGroups / '  + Convert(VARCHAR,@@ROWCOUNT) + ' / ' + CONVERT(VARCHAR(20), GETDATE(), 21))

		CREATE INDEX idx_MatchGroups_SFRecordIdentifier ON #MatchGroups(SFRecordIdentifier)
		CREATE INDEX idx_MatchGroups_MatchID_RecordType ON #MatchGroups(MatchID, RecordType)
		CREATE INDEX idx_MatchGroups_IgrpNo_RecordType ON #MatchGroups(Igrp_no, RecordType)

		CREATE INDEX idx_MatchGroups_Fname_Lname ON #MatchGroups(Fname, Lname)
		CREATE INDEX idx_MatchGroups_Fname3_Lname6 ON #MatchGroups(Fname3, Lname6)
		CREATE INDEX idx_MatchGroups_Phone ON #MatchGroups(Phone)
		CREATE INDEX idx_MatchGroups_Address ON #MatchGroups(Address)
		CREATE INDEX idx_MatchGroups_Company ON #MatchGroups(Company)
		CREATE INDEX idx_MatchGroups_Address15 ON #MatchGroups(Address15)
		CREATE INDEX idx_MatchGroups_Company5 ON #MatchGroups(Company5)
		CREATE INDEX idx_MatchGroups_Email ON #MatchGroups(Email)
		CREATE INDEX idx_MatchGroups_MatchID ON #MatchGroups(MatchID)
		CREATE INDEX idx_MatchGroups_RecordMatchFound ON #MatchGroups(RecordMatchFound)


-- select * from ( SELECT ROW_NUMBER() over (partition by fname3, lname6, email order by fname3, lname6, email, QDate DESC) as PartitionRow, * from #MatchGroups)x where partitionRow > 1

-- select IsOriginal, recordtype, matchid, igrp_no, fname3, lname6, email, qdate, pubid, fname, lname, sfrecordidentifier, address15, company5, phone from #Matchgroups order by email, recordtype desc, fname, lname, qdate desc
update #Matchgroups set MatchID = NULL

		-------------------------------------------------------------------------------------------------------------------------------------------------------
		-- Look for duplicate records in MatchGroups across all PubCode(PubID) and give them the same MatchID 
		-------------------------------------------------------------------------------------------------------------------------------------------------------
		-- Mark just the distinct records by EMAIL
		;WITH cte AS
		(
			SELECT
			ROW_NUMBER() over (partition by email order by email, qdate Desc) as PartitionRow, email, MatchID, RecordType from #MatchGroups  where email <> ''
		)
		UPDATE cte SET MatchID = NEWID()
		WHERE PartitionRow = 1

		-- Now Mark the Distinct records by partial FName and LName for those without an email address
		;WITH cte AS
		(
			SELECT
			ROW_NUMBER() over (partition by fname3, lname6 order by fname3, lname6) as PartitionRow,  Fname3, Lname6, Address15, MatchID, RecordType from #MatchGroups where email = ''
		)
		UPDATE cte SET MatchID = NEWID()
		WHERE PartitionRow = 1

--select  Count(*) from #matchGroups where matchID is null

		-- mark any records with the same MatchID based on matching email, company or address1 with name
		update mg set mg.MatchID = mstr.MatchID
-- select mstr.Fname, mg.Fname, mstr.LName, mg.Lname, mstr.Email, mg.Email, mstr.MatchID, mg.MatchID, mstr.SFRecordIdentifier, mg.SFRecordIdentifier		
		from #MatchGroups mg
		join #MatchGroups mstr on mstr.MatchID is NOT null and (mstr.FName <> '' OR mstr.LName <> '') and (mstr.lname6 = mg.lname6 AND mstr.fname3 = mg.fname3) 
				AND (mstr.Email <> '' AND mstr.Email = mg.Email)
		 where mg.MatchID is NULL 

		update mg set mg.MatchID = mstr.MatchID
-- select mstr.Fname, mg.Fname, mstr.LName, mg.Lname, mstr.Email, mg.Email, mstr.MatchID, mg.MatchID, mstr.SFRecordIdentifier, mg.SFRecordIdentifier		
		from #MatchGroups mg
		join #MatchGroups mstr on mstr.MatchID is NOT null and (mstr.FName <> '' OR mstr.LName <> '') and (mstr.lname6 = mg.lname6 AND mstr.fname3 = mg.fname3) 
				AND (mstr.Phone <> '' AND mstr.Phone = mg.Phone) 
		 where mg.MatchID is NULL 

		update mg set mg.MatchID = mstr.MatchID
-- select mstr.Fname, mg.Fname, mstr.LName, mg.Lname, mstr.Email, mg.Email, mstr.MatchID, mg.MatchID, mstr.SFRecordIdentifier, mg.SFRecordIdentifier		
		from #MatchGroups mg
		join #MatchGroups mstr on mstr.MatchID is NOT null and (mstr.FName <> '' OR mstr.LName <> '') and (mstr.lname6 = mg.lname6 AND mstr.fname3 = mg.fname3) 
				AND (mstr.address15 <> '' AND mstr.address15 = mg.address15 AND (mstr.Company5 <> '' AND mstr.Company5 = mg.Company5) ) 
		 where mg.MatchID is NULL 

		-- Update any remaining records that match on SFRecordIdentifier as duplicates
		update mg set mg.MatchID = mstr.MatchID
-- select mstr.Fname, mg.Fname, mstr.LName, mg.Lname, mstr.Email, mg.Email, mstr.MatchID, mg.MatchID, mstr.SFRecordIdentifier, mg.SFRecordIdentifier		
		from #MatchGroups mg
		join #MatchGroups mstr on mstr.SFRecordIdentifier = mg.SFRecordIdentifier and mstr.MatchID is NOT null
		 where mg.Matchid is NULL

		-- Update those matching on just email where there is not data for fname, lname and Company
		update mg set mg.MatchID = mstr.MatchID
-- select mstr.Fname, mg.Fname, mstr.LName, mg.Lname, mstr.Email, mg.Email, mstr.MatchID, mg.MatchID, mstr.SFRecordIdentifier, mg.SFRecordIdentifier		
		from #MatchGroups mg
		join #MatchGroups mstr on mstr.Email = mg.Email and mstr.MatchID is NOT null
		 where mg.Matchid is NULL and mstr.Email <> '' 
		 and ((mstr.FName = '' and mstr.LName = '' and mstr.Company5 = '' and mstr.Address15 = '') OR
			  (mg.FName = '' and mg.LName = '' and mg.Company5 = '' and mg.Address15 = ''))
		 
		-- remaining records become master records because of uniqueness of address1 and company and email.
		Update #MatchGroups set MatchID = NEWID()
		where MatchID is Null

PRINT ('Set MatchID in #MatchGroup / '  + Convert(VARCHAR,@@ROWCOUNT) + ' / ' + CONVERT(VARCHAR(20), GETDATE(), 114))

		--
		-- Split Names with special characters
		--	
		
		CREATE TABLE #NameSplit (SFRecordIdentifier uniqueidentifier, SplitChar varchar(256))
		
		INSERT INTO #NameSplit
		SELECT SFRecordIdentifier,f.items AS fnameSplit
		FROM #SubscriberFinal WITH(NOLOCK) 
		CROSS APPLY MASTER.dbo.fn_GetSpecialChar(fname) AS f

		UNION
		SELECT SFRecordIdentifier,l.items AS lnameSplit
		FROM #SubscriberFinal  WITH(NOLOCK) 		
		CROSS APPLY MASTER.dbo.fn_GetSpecialChar(lname) AS l

PRINT ('INSERT INTO NAME SPLIT / '  + Convert(VARCHAR,@@ROWCOUNT) + ' / ' + CONVERT(VARCHAR(20), GETDATE(), 114))
		
		
------- StripNonAlphaNumerics time consuming		
		INSERT INTO #MatchGroups(SFRecordIdentifier,FName,LName,Company,Address,City,State,ZipCode,Phone,Email,FName3,LName6,Address15,Address5,Company5,Title,Title5,IsOriginal,PubID,Qdate,MatchID,NameSplitRecord)
				SELECT DISTINCT sft.SFRecordIdentifier,a.items,b.items,sft.Company,sft.Address,sft.City,sft.State,sft.Zip,sft.Phone,sft.Email,'','',LEFT(sft.Address,15),LEFT(sft.Address,5),LEFT(sft.Company,5),sft.Title,LEFT(sft.Title,5),0,p.PubID, IsNull(sft.QDate, GetDate()), mg.MatchID, 1
				FROM #SubscriberFinal sft  WITH(NOLOCK) 
				JOIN #MatchGroups mg on mg.SFRecordIdentifier = sft.SFRecordIdentifier
				JOIN pubs p on p.PubCode = sft.PubCode
				INNER JOIN #NameSplit ns ON sft.SFRecordIdentifier = ns.SFRecordIdentifier
				CROSS APPLY MASTER.dbo.fn_split(sft.FName,ns.SplitChar) AS a
				CROSS APPLY MASTER.dbo.fn_split(sft.LName,ns.SplitChar) AS b
				ORDER BY sft.SFRecordIdentifier

PRINT ('INSERT INTO #MatchGroups Split Names  / '  + Convert(VARCHAR,@@ROWCOUNT) + ' / ' + CONVERT(VARCHAR(20), GETDATE(), 114))

		INSERT INTO #MatchGroups(SFRecordIdentifier,FName,LName,Company,Address,City,State,Zipcode,Phone,Email,FName3,LName6,Address15,Address5,Company5,Title,Title5,IsOriginal,PubID,Qdate,MatchID,NameSplitRecord)
			SELECT DISTINCT sft.SFRecordIdentifier,sft.fname,sft.lname,sft.Company,sft.Address,sft.City,sft.State,sft.Zip,sft.Phone,sft.Email,'','',LEFT(sft.Address,15),LEFT(sft.Address,5),LEFT(sft.Company,5),sft.Title,LEFT(sft.Title,5),0,p.PubID, IsNull(sft.QDate, GetDate()), mg.Matchid, 1
			FROM #SubscriberFinal sft 
			JOIN #MatchGroups mg on mg.SFRecordIdentifier = sft.SFRecordIdentifier
			INNER JOIN #NameSplit ns ON sft.SFRecordIdentifier = ns.SFRecordIdentifier
			JOIN pubs p on p.PubCode = sft.PubCode
			WHERE	ISNULL(sft.Fname,'') != ''  OR
					ISNULL(sft.LName,'') != ''	
		
PRINT ('INSERT SPECIAL CHAR REMOVED INTO #MatchGroups / '  + Convert(VARCHAR,@@ROWCOUNT) + ' / ' + CONVERT(VARCHAR(20), GETDATE(), 114))

		update #MatchGroups
		set fname = MASTER.dbo.fn_StripNonAlphaNumerics(fname)
		WHERE NameSplitRecord = 1 and LEN(fname) > 0 and fname LIKE '%[^a-zA-Z0-9]%'

PRINT ('Update #MatchGroups stripping Fname Chars / '  + Convert(VARCHAR,@@ROWCOUNT) + ' / ' + CONVERT(VARCHAR(20), GETDATE(), 114))

		update #MatchGroups
		set fname3 = LEFT(FName, master.dbo.fn_smallest(Len(FName),3))
		where NameSplitRecord = 1 and LEN(fname) > 0 and LEN(fname) > 0

PRINT ('Update #MatchGroups Fname3 / '  + Convert(VARCHAR,@@ROWCOUNT) + ' / ' + CONVERT(VARCHAR(20), GETDATE(), 114))

		update #MatchGroups
		set lname = MASTER.dbo.fn_StripNonAlphaNumerics(lname)
		WHERE NameSplitRecord = 1 and LEN(lname) > 0 and lname LIKE '%[^a-zA-Z0-9]%'

PRINT ('Update #MatchGroups stripping Lname Chars / '  + Convert(VARCHAR,@@ROWCOUNT) + ' / ' + CONVERT(VARCHAR(20), GETDATE(), 114))

		update #MatchGroups
		set lname6 = LEFT(LName, master.dbo.fn_smallest(Len(LName),6))
		where NameSplitRecord = 1 and LEN(lname) > 0

		DROP TABLE #NameSplit

PRINT ('Update #MatchGroups Lname6 / '  + Convert(VARCHAR,@@ROWCOUNT) + ' / ' + CONVERT(VARCHAR(20), GETDATE(), 114))

		-- Remove rows from #MatchGroups where fname or last name is only a name Suffix
		DECLARE @SuffixCodeTypeID INT = (SELECT codetypeid FROM UAD_Lookup..CodeType WHERE CodeTypeName = 'Suffix Name')
		CREATE TABLE #suffix (suffixName varchar(25))
		
		INSERT INTO #suffix
		SELECT suffixName FROM UAD_Lookup..Suffix WITH(NOLOCK) WHERE SuffixCodeTypeID = @SuffixCodeTypeID and IsActive = 1
		
		DELETE mg FROM #MatchGroups mg join #suffix s on mg.FName = s.suffixName where mg.IsOriginal = 0
		DELETE mg FROM #MatchGroups mg join #suffix s on mg.LName = s.suffixName where mg.IsOriginal = 0	

		DROP TABLE #suffix	

		--
		-- Mark Master record (row = 1) for 1 distinct First Name (3 chars) and Last Name (6 chars) for each PubID
		--
		;WITH cte AS
		(
			select ROW_NUMBER() over (partition by MatchID, PubID order by MatchID, IsOriginal DESC, QDate DESC) as PartitionRow, FName, LName, Email, PubID, IsOriginal, RecordType from #MatchGroups
		)
		UPDATE cte SET RecordType = CASE WHEN PartitionRow = 1 and IsOriginal = 1 THEN 'Master' ELSE 'Duplicate' END



--select MatchID m1, RecordType rt1, * from #MatchGroups order by MatchID, RecordType desc




		-- End Duplicate Record Search
		-------------------------------------------------------------------------------------------------------------------------------------------------------
		-- Begin Rollup of just the data used for matching MatchID's within each pubID from duplicates to master for MatchGroup Table. 
		-- This needs to be done for matching against the PubSubscriptions table and Subscriptions table.

		DECLARE @IgrpID				UniqueIdentifier		
		DECLARE @MatchID			UniqueIdentifier		
		DECLARE @MasterSFRecordIdentifier	UniqueIdentifier		
		DECLARE @SFRecordIdentifier	UniqueIdentifier		
		DECLARE @MAFField			varchar(100)


		-- store results into temp table so cursor is not effected by changed master data
		select MatchID, SFRecordIdentifier 
			into #TempMatchIDs
		from #MatchGroups 
		where MatchID in (select MatchID from #MatchGroups where RecordType = 'Duplicate' Group By MatchID  )
		and RecordType = 'master'

		
		DECLARE c_MatchIDs CURSOR LOCAL FORWARD_ONLY STATIC READ_ONLY FOR 
		
		select MatchID, SFRecordIdentifier from #TempMatchIDs 
	
		OPEN c_MatchIDs  
		FETCH NEXT FROM c_MatchIDs INTO @MatchID, @MasterSFRecordIdentifier

--print '@Matchid ' + CONVERT(varchar(100), @Matchid)

		-- Loop through all MatchIDs
		WHILE @@FETCH_STATUS = 0  
		BEGIN  

			DECLARE c_DupRecs CURSOR LOCAL FORWARD_ONLY STATIC READ_ONLY FOR Select mg.SfRecordIdentifier from #MatchGroups mg where MatchID = @MatchID and RecordType = 'Duplicate' and mg.SFRecordIdentifier <> @MasterSFRecordIdentifier order by QDate ASC
			OPEN c_DupRecs  

			FETCH NEXT from c_DupRecs INTO @SFRecordIdentifier
			WHILE @@FETCH_STATUS = 0  
			BEGIN  

--print '@Matchid ' + CONVERT(varchar(100), @Matchid) + ' / ' + '@SFRecordIdentifier ' + CONVERT(varchar(100), @SFRecordIdentifier)

				UPDATE mstr set 
					mstr.QDate = CASE when dups.Qdate > mstr.QDate THEN dups.Qdate else mstr.qdate end,
					mstr.address   = case when (dups.Address <> '' OR (dups.City <> '' AND dups.State <> '')) AND (mstr.Address = '' OR mstr.city = '' OR mstr.state = '' OR dups.Qdate > mstr.QDate) THEN dups.Address else mstr.address end, 
					mstr.Address5  = case when (dups.Address <> '' OR (dups.City <> '' AND dups.State <> '')) AND (mstr.Address = '' OR mstr.city = '' OR mstr.state = '' OR dups.Qdate > mstr.QDate) THEN dups.Address5 else mstr.Address5 end, 
					mstr.Address15 = case when (dups.Address <> '' OR (dups.City <> '' AND dups.State <> '')) AND (mstr.Address = '' OR mstr.city = '' OR mstr.state = '' OR dups.Qdate > mstr.QDate) THEN dups.Address15 else mstr.Address15 end, 
					mstr.city      = case when (dups.Address <> '' OR (dups.City <> '' AND dups.State <> '')) AND (mstr.Address = '' OR mstr.city = '' OR mstr.state = '' OR dups.Qdate > mstr.QDate) then dups.City else mstr.city end, 
					mstr.state     = case when (dups.Address <> '' OR (dups.City <> '' AND dups.State <> '')) AND (mstr.Address = '' OR mstr.city = '' OR mstr.state = '' OR dups.Qdate > mstr.QDate) then dups.state else mstr.state end,
					mstr.Zipcode   = case when (dups.Address <> '' OR (dups.City <> '' AND dups.State <> '')) AND (mstr.Address = '' OR mstr.city = '' OR mstr.state = '' OR dups.Qdate > mstr.QDate) then dups.Zipcode else mstr.Zipcode end,
					mstr.FName     = case when (dups.FName <> '' AND dups.LName <> '') AND (mstr.FName = '' OR mstr.LName = '' OR dups.Qdate > mstr.QDate) THEN dups.FName else mstr.FName end,
					mstr.FName3    = case when (dups.FName <> '' AND dups.LName <> '') AND (mstr.FName = '' OR mstr.LName = '' OR dups.Qdate > mstr.QDate) THEN dups.FName3 else mstr.FName3 end,
					mstr.LName     = case when (dups.FName <> '' AND dups.LName <> '') AND (mstr.FName = '' OR mstr.LName = '' OR dups.Qdate > mstr.QDate) THEN dups.LName else mstr.LName end,
					mstr.LName6    = case when (dups.FName <> '' AND dups.LName <> '') AND (mstr.FName = '' OR mstr.LName = '' OR dups.Qdate > mstr.QDate) THEN dups.LName6 else mstr.LName6 end,
					mstr.Email     = CASE when dups.Email <> '' AND (mstr.Email = '' OR dups.Qdate > mstr.QDate) THEN dups.Email else mstr.Email end,
					mstr.Company   = CASE when dups.Company <> '' AND (mstr.Company = '' OR dups.Qdate > mstr.QDate) THEN dups.Company else mstr.Company end,
					mstr.Company5  = CASE when dups.Company <> '' AND (mstr.Company = '' OR dups.Qdate > mstr.QDate) THEN dups.Company5 else mstr.Company5 end,
					mstr.Title     = CASE when dups.Title <> '' AND (mstr.Title = '' OR dups.Qdate > mstr.QDate) THEN dups.Title else mstr.Title end,
					mstr.Title5    = CASE when dups.Title <> '' AND (mstr.Title = '' OR dups.Qdate > mstr.QDate) THEN dups.Title5 else mstr.Title5 end,
					mstr.Phone     = CASE when dups.Phone <> '' AND (mstr.Phone = '' OR dups.Qdate > mstr.QDate) THEN dups.Phone else mstr.Phone end
				from #MatchGroups mstr
				join #MatchGroups dups on dups.MatchID = mstr.MatchID AND dups.PubID = mstr.PubID
				where mstr.MatchID = @MatchID and mstr.RecordType = 'Master'
					and dups.SFRecordIdentifier = @SFRecordIdentifier

				FETCH NEXT from c_DupRecs INTO @SFRecordIdentifier
			END
			CLOSE c_DupRecs  
			DEALLOCATE c_DupRecs
 
		FETCH NEXT FROM c_MatchIDs INTO @MatchID, @MasterSFRecordIdentifier
		END

		CLOSE c_MatchIDs  
		DEALLOCATE c_MatchIDs

		DROP table #TempMatchIDs

		-- End Rollup of data from duplicates to master
		-------------------------------------------------------------------------------------------------------------------------------------------------------
		-- Begin Data Match  on PubSubscriptions for Matching PubCode(PubID)

		--
		-- EMailID and SequenceID
		--
		UPDATE mg
		SET indDistEmail = s.igrp_no, RecordMatchFound = 1
--Select s.igrp_no i, ps.*		
		FROM #MatchGroups mg 
			JOIN PubSubscriptions ps with (NOLOCK) ON ps.PubID = mg.PubID 
				AND ps.EmailID = mg.EmailID 
				AND ps.SequenceID = mg.SequenceID 
			JOIN Subscriptions s with (NOLOCK) ON s.SubscriptionID = ps.SubscriptionID
		WHERE mg.SequenceID > 0 AND mg.EmailID > 0 

PRINT ('After Step 0 : (Matching Pubs in PubSubscriptions) - EmailID and SequenceID / '  + Convert(VARCHAR,@@ROWCOUNT) + ' / ' + CONVERT(VARCHAR(20), GETDATE(), 114))
		
		--
		-- Fname,Lname and Address match
		--
		UPDATE mg
		SET indNameAddress = s.igrp_no, RecordMatchFound = 1
--Select s.igrp_no i, ps.*		
		FROM #MatchGroups mg 
			JOIN PubSubscriptions ps with (NOLOCK) ON ps.PubID = mg.PubID 
				AND ( (ps.FirstName = mg.FName and ps.LastName = mg.LName) OR (ps.FirstName = mg.LName and ps.LastName = mg.FName) )
				AND ps.Address1 = mg.Address
			JOIN Subscriptions s with (NOLOCK) ON s.SubscriptionID = ps.SubscriptionID
		WHERE (mg.FName!='' OR mg.LName!='') AND mg.address != '' and RecordMatchFound = 0 

PRINT ('After Step 1 : (Matching Pubs in PubSubscriptions) - Fname,Lname Address match/SWAP on individual level / '  + Convert(VARCHAR,@@ROWCOUNT) + ' / ' + CONVERT(VARCHAR(20), GETDATE(), 114))

  		--
		-- Fname,Lname and Email match
		--
		UPDATE mg
		SET indNameEmail = s.igrp_no, RecordMatchFound = 1
--Select s.igrp_no, *		
		FROM #MatchGroups mg 
			JOIN PubSubscriptions ps with (NOLOCK) ON ps.PubID = mg.PubID 
				AND ps.FirstName = mg.FName and ps.LastName = mg.LName
				AND mg.Email = ps.EMAIL
			JOIN Subscriptions s with (NOLOCK) ON s.SubscriptionID = ps.SubscriptionID
		WHERE (mg.FName!='' OR mg.LName!='') AND mg.Email!='' and RecordMatchFound = 0  

		UPDATE mg
		SET indNameEmail = s.igrp_no, RecordMatchFound = 1
--Select s.igrp_no, *		
		FROM #MatchGroups mg 
			JOIN PubSubscriptions ps with (NOLOCK) ON ps.PubID = mg.PubID 
				AND ps.FirstName = mg.LName and ps.LastName = mg.FName
				AND mg.Email = ps.EMAIL
			JOIN Subscriptions s with (NOLOCK) ON s.SubscriptionID = ps.SubscriptionID
		WHERE (mg.FName!='' OR mg.LName!='') AND mg.Email!='' and RecordMatchFound = 0  


PRINT ('After Step 2 : (Matching Pubs in PubSubscriptions) - Fname,Lname Email/SWAP match on individual level / '  + Convert(VARCHAR,@@ROWCOUNT) + ' / ' + CONVERT(VARCHAR(20), GETDATE(), 114))
  		--
		-- Fname,Lname and Company match
		--
		UPDATE mg
		SET indNameCompany = s.igrp_no, RecordMatchFound = 1
--Select s.igrp_no, *		
		FROM #MatchGroups mg 
			JOIN PubSubscriptions ps with (NOLOCK) ON ps.PubID = mg.PubID 
				AND ps.FirstName = mg.FName and ps.LastName = mg.LName
				AND mg.Company = ps.Company
				AND mg.Email = '' AND ps.Email = ''
			JOIN Subscriptions s with (NOLOCK) ON s.SubscriptionID = ps.SubscriptionID
		WHERE (mg.FName!='' OR mg.LName!='') AND mg.Company!='' and RecordMatchFound = 0 

		UPDATE mg
		SET indNameCompany = s.igrp_no, RecordMatchFound = 1
--Select s.igrp_no, *		
		FROM #MatchGroups mg 
			JOIN PubSubscriptions ps with (NOLOCK) ON ps.PubID = mg.PubID 
				AND ps.FirstName = mg.LName and ps.LastName = mg.FName
				AND mg.Company = ps.Company
				AND mg.Email = '' AND ps.Email = ''
			JOIN Subscriptions s with (NOLOCK) ON s.SubscriptionID = ps.SubscriptionID
		WHERE (mg.FName!='' OR mg.LName!='') AND mg.Company!='' and RecordMatchFound = 0 

PRINT ('After Step 3 : (Matching Pubs in PubSubscriptions) - Fname,Lname and Company match/SWAP on individual level / '  + Convert(VARCHAR,@@ROWCOUNT) + ' / ' + CONVERT(VARCHAR(20), GETDATE(), 114))

  		--
		-- Fname,Lname and Phone match
		--
		UPDATE mg
		SET indNamePhone = s.igrp_no, RecordMatchFound = 1
--Select s.igrp_no, *		
		FROM #MatchGroups mg 
			JOIN PubSubscriptions ps with (NOLOCK) ON ps.PubID = mg.PubID 
				AND ps.FirstName = mg.FName and ps.LastName = mg.LName
				AND mg.Phone = ps.Phone
			JOIN Subscriptions s with (NOLOCK) ON s.SubscriptionID = ps.SubscriptionID
		WHERE (mg.FName!='' OR mg.LName!='') AND mg.Phone !='' and RecordMatchFound = 0  

		UPDATE mg
		SET indNamePhone = s.igrp_no, RecordMatchFound = 1
--Select s.igrp_no, *		
		FROM #MatchGroups mg 
			JOIN PubSubscriptions ps with (NOLOCK) ON ps.PubID = mg.PubID 
				AND ps.FirstName = mg.LName and ps.LastName = mg.FName
				AND mg.Phone = ps.Phone
			JOIN Subscriptions s with (NOLOCK) ON s.SubscriptionID = ps.SubscriptionID
		WHERE (mg.FName!='' OR mg.LName!='') AND mg.Phone !='' and RecordMatchFound = 0  

PRINT ('After Step 4 : (Matching Pubs in PubSubscriptions) - Fname,Lname and Phone match/SWAP on individual level / '  + Convert(VARCHAR,@@ROWCOUNT) + ' / ' + CONVERT(VARCHAR(20), GETDATE(), 114))
		
		--
		-- Email,Fname,Lname not blank and match against PubSubscriptions where Fname AND Lname blank and Email not blank
		--
		UPDATE mg
		SET indNameNotBlankEmail = s.Igrp_no, RecordMatchFound = 1
--Select s.igrp_no, *		
		FROM #MatchGroups mg 
			JOIN PubSubscriptions ps with (NOLOCK) ON ps.PubID = mg.PubID AND mg.Email = ps.EMAIL
			JOIN Subscriptions s with (NOLOCK) ON s.SubscriptionID = ps.SubscriptionID
		WHERE (mg.FName!='' OR mg.lname!='') AND mg.Email !='' AND ISNULL(ps.Email,'')!='' AND ISNULL(ps.FirstName,'')='' AND ISNULL(ps.LastName,'')='' and RecordMatchFound = 0 

PRINT ('After Step 5 : (Matching Pubs in PubSubscriptions) - Email,Fname,Lname not blank and match against Pub Subscriptions where Fname AND Lname blank and Email not blank / '  + Convert(VARCHAR,@@ROWCOUNT) + ' / ' + CONVERT(VARCHAR(20), GETDATE(), 114))

		--
		-- Email not blank and Fname OR Lname not blank and match against PubSubscriptions where Fname AND Lname blank and Email not blank - Opposite of the statement above
		--
		UPDATE mg
		SET indNameNotBlankEmail = s.Igrp_no, RecordMatchFound = 1
--Select s.igrp_no, *		
		FROM #MatchGroups mg 
			JOIN PubSubscriptions ps with (NOLOCK) ON ps.PubID = mg.PubID AND mg.Email = ps.EMAIL
			JOIN Subscriptions s with (NOLOCK) ON s.SubscriptionID = ps.SubscriptionID
		WHERE  mg.Email !='' AND mg.FName='' AND mg.lname=''
		AND ISNULL(ps.Email,'')!='' AND (ISNULL(ps.FirstName,'')!='' OR ISNULL(ps.LastName,'')!='') and RecordMatchFound = 0 

PRINT ('After Step 6: (Matching Pubs in PubSubscriptions) - Email,Fname,Lname not blank and match against Pub Subscriptions where Fname AND Lname blank and Email not blank - Opposite of the statement above / '  + Convert(VARCHAR,@@ROWCOUNT) + ' / ' + CONVERT(VARCHAR(20), GETDATE(), 114))

		--
		-- Distinct Email Source file does not have Fname Or LName match on PubSubscriptions
		--
		UPDATE mg
		SET indDistEmail = s.Igrp_no, RecordMatchFound = 1
--Select s.igrp_no, *		
		FROM #MatchGroups mg 
			JOIN PubSubscriptions ps with (NOLOCK) ON ps.PubID = mg.PubID AND mg.Email = ps.EMAIL
			JOIN Subscriptions s with (NOLOCK) ON s.SubscriptionID = ps.SubscriptionID
		WHERE mg.Email!='' AND mg.fname='' AND mg.lname='' and RecordMatchFound = 0 

PRINT ('After Step 7 : (Matching Pubs in PubSubscriptions) - Distinct Email Source file does not have Fname Or LName match on PubSubscriptions / '  + Convert(VARCHAR,@@ROWCOUNT) + ' / ' + CONVERT(VARCHAR(20), GETDATE(), 114))
 		
		--
 		-- FName, LName, and Email is Blank. Title, Company and Address1 not Blank match on PubSubscriptions
		--
 		-- We are using indDistEmail for this update because the match criteria assumes that this field should not have any values
 		UPDATE mg
 		SET indDistEmail = s.Igrp_no, RecordMatchFound = 1
--Select s.igrp_no, *		
 		FROM #MatchGroups mg 
 			JOIN PubSubscriptions ps with (NOLOCK) ON ps.PubID = mg.PubID AND mg.Title = ps.Title AND mg.Company = ps.Company AND mg.Address = ps.Address1
			JOIN Subscriptions s with (NOLOCK) ON s.SubscriptionID = ps.SubscriptionID
 		WHERE mg.FName = '' AND mg.LName = '' AND mg.EMAIL = '' AND mg.[Address] !='' AND mg.TITLE != '' AND mg.Company != '' and RecordMatchFound = 0
 		
PRINT ('After Step 8 : (Matching Pubs in PubSubscriptions) - FName, LName and Email is Blank. Title, Company and Address1 not Blank match on PubSubscriptions / '  + Convert(VARCHAR,@@ROWCOUNT) + ' / ' + CONVERT(VARCHAR(20), GETDATE(), 114))


-- select * from #Matchgroups order by PubID, fname3, lname6, qdate desc


        --END INDIVIDUAL MATCHING within Same PubCode (PubID)
----------------------------------------------------------------------------------------------------------------------------------------------------------------------
		--
		-------------------------------------------------------------------------------------------------------------------------------------------------------
		-- Begin Partial Data Match  on PubSubscriptions for Matching PubCode(PubID)
		--
		-- Fname,Lname and Address5. City match
		--
		UPDATE mg
		SET indNameAddress = s.igrp_no, RecordMatchFound = 10
--Select s.igrp_no i, ps.*		
		FROM #MatchGroups mg 
			JOIN PubSubscriptions ps with (NOLOCK) ON ps.PubID = mg.PubID 
				AND Left(ps.FirstName, master.dbo.fn_smallest(Len(ps.FirstName),Len(mg.FName3))) = Left(mg.FName3, master.dbo.fn_smallest(Len(ps.FirstName),Len(mg.FName3))) 
				AND Left(ps.LastName, master.dbo.fn_smallest(Len(ps.LastName),Len(mg.LName6))) = Left(mg.LName6, master.dbo.fn_smallest(Len(ps.LastName),Len(mg.LName6)))
				AND Left(ps.Address1,5) = mg.Address5
				AND ps.City = mg.City
			JOIN Subscriptions s with (NOLOCK) ON s.SubscriptionID = ps.SubscriptionID
		WHERE (mg.FName <> '' AND mg.LName <> '' AND ps.FirstName <> '' AND ps.LastName <> '') 
		AND master.dbo.fn_smallest(Len(ps.FirstName),Len(mg.FName3)) + master.dbo.fn_smallest(Len(ps.LastName),Len(mg.LName6)) > 5
		AND mg.address != '' and mg.City != '' and RecordMatchFound = 0 


PRINT ('After Step 9A : (Matching Pubs in PubSubscriptions) - Partial Fname,Lname Address5, City match on individual level / '  + Convert(VARCHAR,@@ROWCOUNT) + ' / ' + CONVERT(VARCHAR(20), GETDATE(), 114))

		--
		-- Fname,Lname and Address match
		--
		UPDATE mg
		SET indNameAddress = s.igrp_no, RecordMatchFound = 10
--Select s.igrp_no i, ps.*		
		FROM #MatchGroups mg 
			JOIN PubSubscriptions ps with (NOLOCK) ON ps.PubID = mg.PubID 
				AND Left(ps.FirstName, master.dbo.fn_smallest(Len(ps.FirstName),Len(mg.FName3))) = Left(mg.FName3, master.dbo.fn_smallest(Len(ps.FirstName),Len(mg.FName3))) 
				AND Left(ps.LastName, master.dbo.fn_smallest(Len(ps.LastName),Len(mg.LName6))) = Left(mg.LName6, master.dbo.fn_smallest(Len(ps.LastName),Len(mg.LName6)))
--				AND Left(ps.FirstName,3) = mg.FName3 and LEFT(ps.LastName,6) = mg.LName6
				AND Left(ps.Address1,5) = mg.Address5
				AND ps.Zipcode = mg.Zipcode
			JOIN Subscriptions s with (NOLOCK) ON s.SubscriptionID = ps.SubscriptionID
		WHERE (mg.FName <> '' AND mg.LName <> '' AND ps.FirstName <> '' AND ps.LastName <> '') 
		AND master.dbo.fn_smallest(Len(ps.FirstName),Len(mg.FName3)) + master.dbo.fn_smallest(Len(ps.LastName),Len(mg.LName6)) > 5
		AND mg.address != '' and mg.Zipcode != '' and RecordMatchFound = 0 

PRINT ('After Step 9B : (Matching Pubs in PubSubscriptions) - Partial Fname,Lname Address5, Zip match on individual level / '  + Convert(VARCHAR,@@ROWCOUNT) + ' / ' + CONVERT(VARCHAR(20), GETDATE(), 114))

  		--
		-- Fname,Lname and Email match
		--
		UPDATE mg
		SET indNameEmail = s.igrp_no, RecordMatchFound = 10
--Select s.igrp_no, *		
		FROM #MatchGroups mg 
			JOIN PubSubscriptions ps with (NOLOCK) ON ps.PubID = mg.PubID 
				AND Left(ps.FirstName, master.dbo.fn_smallest(Len(ps.FirstName),Len(mg.FName3))) = Left(mg.FName3, master.dbo.fn_smallest(Len(ps.FirstName),Len(mg.FName3))) 
				AND Left(ps.LastName, master.dbo.fn_smallest(Len(ps.LastName),Len(mg.LName6))) = Left(mg.LName6, master.dbo.fn_smallest(Len(ps.LastName),Len(mg.LName6)))
--				AND Left(ps.FirstName,3) = mg.FName3 and LEFT(ps.LastName,6) = mg.LName6
				AND mg.Email = ps.EMAIL
			JOIN Subscriptions s with (NOLOCK) ON s.SubscriptionID = ps.SubscriptionID
		WHERE (mg.FName <> '' AND mg.LName <> '' AND ps.FirstName <> '' AND ps.LastName <> '') 
		AND master.dbo.fn_smallest(Len(ps.FirstName),Len(mg.FName3)) + master.dbo.fn_smallest(Len(ps.LastName),Len(mg.LName6)) > 5
		AND mg.Email!='' and RecordMatchFound = 0  


PRINT ('After Step 10 : (Matching Pubs in PubSubscriptions) - Partial Fname,Lname Email match on individual level / '  + Convert(VARCHAR,@@ROWCOUNT) + ' / ' + CONVERT(VARCHAR(20), GETDATE(), 114))
  		--
		-- Fname,Lname and Company match
		--
		UPDATE mg
		SET indNameCompany = s.igrp_no, RecordMatchFound = 10
--Select s.igrp_no, *		
		FROM #MatchGroups mg 
			JOIN PubSubscriptions ps with (NOLOCK) ON ps.PubID = mg.PubID 
				AND Left(ps.FirstName, master.dbo.fn_smallest(Len(ps.FirstName),Len(mg.FName3))) = Left(mg.FName3, master.dbo.fn_smallest(Len(ps.FirstName),Len(mg.FName3))) 
				AND Left(ps.LastName, master.dbo.fn_smallest(Len(ps.LastName),Len(mg.LName6))) = Left(mg.LName6, master.dbo.fn_smallest(Len(ps.LastName),Len(mg.LName6)))
--				AND Left(ps.FirstName,3) = mg.FName3 and LEFT(ps.LastName,6) = mg.LName6
				AND Left(ps.Company,5) = mg.Company5
				AND Left(ps.Title,5) = mg.Title5
				AND mg.Email = '' AND ps.Email = ''
			JOIN Subscriptions s with (NOLOCK) ON s.SubscriptionID = ps.SubscriptionID
		WHERE (mg.FName <> '' AND mg.LName <> '' AND ps.FirstName <> '' AND ps.LastName <> '') 
		AND master.dbo.fn_smallest(Len(ps.FirstName),Len(mg.FName3)) + master.dbo.fn_smallest(Len(ps.LastName),Len(mg.LName6)) > 5
		AND mg.Company!='' and mg.Title!='' and RecordMatchFound = 0 

PRINT ('After Step 11 : (Matching Pubs in PubSubscriptions) - Partial Fname,Lname, Company5 and Title5 match on individual level / '  + Convert(VARCHAR,@@ROWCOUNT) + ' / ' + CONVERT(VARCHAR(20), GETDATE(), 114))

  		--
		-- Fname,Lname and Phone match
		--
		UPDATE mg
		SET indNamePhone = s.igrp_no, RecordMatchFound = 10
--Select s.igrp_no, *		
		FROM #MatchGroups mg 
			JOIN PubSubscriptions ps with (NOLOCK) ON ps.PubID = mg.PubID 
				AND Left(ps.FirstName, master.dbo.fn_smallest(Len(ps.FirstName),Len(mg.FName3))) = Left(mg.FName3, master.dbo.fn_smallest(Len(ps.FirstName),Len(mg.FName3))) 
				AND Left(ps.LastName, master.dbo.fn_smallest(Len(ps.LastName),Len(mg.LName6))) = Left(mg.LName6, master.dbo.fn_smallest(Len(ps.LastName),Len(mg.LName6)))
--				AND Left(ps.FirstName,3) = mg.FName3 and LEFT(ps.LastName,6) = mg.LName6
				AND mg.Phone = ps.Phone
			JOIN Subscriptions s with (NOLOCK) ON s.SubscriptionID = ps.SubscriptionID
		WHERE (mg.FName <> '' AND mg.LName <> '' AND ps.FirstName <> '' AND ps.LastName <> '') 
		AND master.dbo.fn_smallest(Len(ps.FirstName),Len(mg.FName3)) + master.dbo.fn_smallest(Len(ps.LastName),Len(mg.LName6)) > 5
		AND mg.Phone !='' and RecordMatchFound = 0  

PRINT ('After Step 12 : (Matching Pubs in PubSubscriptions) - Partial Fname,Lname and Phone match on individual level / '  + Convert(VARCHAR,@@ROWCOUNT) + ' / ' + CONVERT(VARCHAR(20), GETDATE(), 114))


-- select * from #Matchgroups order by PubID, fname3, lname6, qdate desc

		--
		-------------------------------------------------------------------------------------------------------------------------------------------------------
		-- Begin Data Match  on PubSubscriptions for NON-Matching PubCode(PubID)


		--
		-- EMailID and SequenceID
		--
		UPDATE mg
		SET indDistEmail = s.igrp_no, RecordMatchFound = 2
--Select s.igrp_no i, ps.*		
		FROM #MatchGroups mg 
			JOIN PubSubscriptions ps with (NOLOCK) ON ps.PubID <> mg.PubID 
				AND ps.EmailID = mg.EmailID 
				AND ps.SequenceID = mg.SequenceID 
			JOIN Subscriptions s with (NOLOCK) ON s.SubscriptionID = ps.SubscriptionID
		WHERE mg.SequenceID > 0 AND mg.EmailID > 0 

PRINT ('After Step 12.5 : (NON Matching Pubs in PubSubscriptions) - EmailID and SequenceID / '  + Convert(VARCHAR,@@ROWCOUNT) + ' / ' + CONVERT(VARCHAR(20), GETDATE(), 114))


		--
		-- Fname,Lname and Address match
		--
		UPDATE mg
		SET indNameAddress = s.igrp_no, RecordMatchFound = 2
--Select s.igrp_no i, ps.*		
		FROM #MatchGroups mg 
			JOIN PubSubscriptions ps with (NOLOCK) ON ps.PubID <> mg.PubID 
				AND ps.FirstName = mg.FName and ps.LastName = mg.LName
				AND ps.Address1 = mg.Address
			JOIN Subscriptions s with (NOLOCK) ON s.SubscriptionID = ps.SubscriptionID
		WHERE (mg.FName!='' OR mg.LName!='') AND mg.address != '' and RecordMatchFound = 0 

		UPDATE mg
		SET indNameAddress = s.igrp_no, RecordMatchFound = 2
--Select s.igrp_no i, ps.*		
		FROM #MatchGroups mg 
			JOIN PubSubscriptions ps with (NOLOCK) ON ps.PubID <> mg.PubID 
				AND ps.FirstName = mg.LName and ps.LastName = mg.FName
				AND ps.Address1 = mg.Address
			JOIN Subscriptions s with (NOLOCK) ON s.SubscriptionID = ps.SubscriptionID
		WHERE (mg.FName!='' OR mg.LName!='') AND mg.address != '' and RecordMatchFound = 0 

PRINT ('After Step 13 : (NON Matching Pubs in PubSubscriptions) - Fname,Lname Address match/SWAP on individual level / '  + Convert(VARCHAR,@@ROWCOUNT) + ' / ' + CONVERT(VARCHAR(20), GETDATE(), 114))

  		--
		-- Fname,Lname and Email match
		--
		UPDATE mg
		SET indNameEmail = s.igrp_no, RecordMatchFound = 2
--Select s.igrp_no, *		
		FROM #MatchGroups mg 
			JOIN PubSubscriptions ps with (NOLOCK) ON ps.PubID <> mg.PubID 
				AND ps.FirstName = mg.FName and ps.LastName = mg.LName
				AND mg.Email = ps.EMAIL
			JOIN Subscriptions s with (NOLOCK) ON s.SubscriptionID = ps.SubscriptionID
		WHERE (mg.FName!='' OR mg.LName!='') AND mg.Email!='' and RecordMatchFound = 0  

		UPDATE mg
		SET indNameEmail = s.igrp_no, RecordMatchFound = 2
--Select s.igrp_no, *		
		FROM #MatchGroups mg 
			JOIN PubSubscriptions ps with (NOLOCK) ON ps.PubID <> mg.PubID 
				AND ps.FirstName = mg.LName and ps.LastName = mg.FName
				AND mg.Email = ps.EMAIL
			JOIN Subscriptions s with (NOLOCK) ON s.SubscriptionID = ps.SubscriptionID
		WHERE (mg.FName!='' OR mg.LName!='') AND mg.Email!='' and RecordMatchFound = 0  


PRINT ('After Step 14 : (NON Matching Pubs in PubSubscriptions) - Fname,Lname Email/SWAP match on individual level / '  + Convert(VARCHAR,@@ROWCOUNT) + ' / ' + CONVERT(VARCHAR(20), GETDATE(), 114))
  		--
		-- Fname,Lname and Company match
		--
		UPDATE mg
		SET indNameCompany = s.igrp_no, RecordMatchFound = 2
--Select s.igrp_no, *		
		FROM #MatchGroups mg 
			JOIN PubSubscriptions ps with (NOLOCK) ON ps.PubID <> mg.PubID 
				AND ps.FirstName = mg.FName and ps.LastName = mg.LName
				AND mg.Company = ps.Company
				AND mg.Email = '' AND ps.Email = ''
			JOIN Subscriptions s with (NOLOCK) ON s.SubscriptionID = ps.SubscriptionID
		WHERE (mg.FName!='' OR mg.LName!='') AND mg.Company!='' and RecordMatchFound = 0 

		UPDATE mg
		SET indNameCompany = s.igrp_no, RecordMatchFound = 2
--Select s.igrp_no, *		
		FROM #MatchGroups mg 
			JOIN PubSubscriptions ps with (NOLOCK) ON ps.PubID <> mg.PubID 
				AND ps.FirstName = mg.LName and ps.LastName = mg.FName
				AND mg.Company = ps.Company
				AND mg.Email = '' AND ps.Email = ''
			JOIN Subscriptions s with (NOLOCK) ON s.SubscriptionID = ps.SubscriptionID
		WHERE (mg.FName!='' OR mg.LName!='') AND mg.Company!='' and RecordMatchFound = 0 

PRINT ('After Step 15 : (NON Matching Pubs in PubSubscriptions) - Fname,Lname and Company match/SWAP on individual level / '  + Convert(VARCHAR,@@ROWCOUNT) + ' / ' + CONVERT(VARCHAR(20), GETDATE(), 114))

  		--
		-- Fname,Lname and Phone match
		--
		UPDATE mg
		SET indNamePhone = s.igrp_no, RecordMatchFound = 2
--Select s.igrp_no, *		
		FROM #MatchGroups mg 
			JOIN PubSubscriptions ps with (NOLOCK) ON ps.PubID <> mg.PubID 
				AND ps.FirstName = mg.FName and ps.LastName = mg.LName
				AND mg.Phone = ps.Phone
			JOIN Subscriptions s with (NOLOCK) ON s.SubscriptionID = ps.SubscriptionID
		WHERE (mg.FName!='' OR mg.LName!='') AND mg.Phone !='' and RecordMatchFound = 0  

		UPDATE mg
		SET indNamePhone = s.igrp_no, RecordMatchFound = 2
--Select s.igrp_no, *		
		FROM #MatchGroups mg 
			JOIN PubSubscriptions ps with (NOLOCK) ON ps.PubID <> mg.PubID 
				AND ps.FirstName = mg.LName and ps.LastName = mg.FName
				AND mg.Phone = ps.Phone
			JOIN Subscriptions s with (NOLOCK) ON s.SubscriptionID = ps.SubscriptionID
		WHERE (mg.FName!='' OR mg.LName!='') AND mg.Phone !='' and RecordMatchFound = 0  

PRINT ('After Step 16 : (NON Matching Pubs in PubSubscriptions) - Fname,Lname and Phone match/SWAP on individual level / '  + Convert(VARCHAR,@@ROWCOUNT) + ' / ' + CONVERT(VARCHAR(20), GETDATE(), 114))
		
		--
		-- Email,Fname,Lname not blank and match against PubSubscriptions where Fname AND Lname blank and Email not blank
		--
		UPDATE mg
		SET indNameNotBlankEmail = s.Igrp_no, RecordMatchFound = 2
--Select s.igrp_no, *		
		FROM #MatchGroups mg 
			JOIN PubSubscriptions ps with (NOLOCK) ON ps.PubID <> mg.PubID AND mg.Email = ps.EMAIL
			JOIN Subscriptions s with (NOLOCK) ON s.SubscriptionID = ps.SubscriptionID
		WHERE (mg.FName!='' OR mg.lname!='') AND mg.Email !='' AND ISNULL(ps.Email,'')!='' AND ISNULL(ps.FirstName,'')='' AND ISNULL(ps.LastName,'')='' and RecordMatchFound = 0 

PRINT ('After Step 17 : (NON Matching Pubs in PubSubscriptions) - Email,Fname,Lname not blank and match against Subscriptions where Fname AND Lname blank and Email not blank / '  + Convert(VARCHAR,@@ROWCOUNT) + ' / ' + CONVERT(VARCHAR(20), GETDATE(), 114))

		--
		-- Email not blank and Fname OR Lname not blank and match against PubSubscriptions where Fname AND Lname blank and Email not blank - Opposite of the statement above
		--
		UPDATE mg
		SET indNameNotBlankEmail = s.Igrp_no, RecordMatchFound = 2
--Select s.igrp_no, *		
		FROM #MatchGroups mg 
			JOIN PubSubscriptions ps with (NOLOCK) ON ps.PubID <> mg.PubID AND mg.Email = ps.EMAIL
			JOIN Subscriptions s with (NOLOCK) ON s.SubscriptionID = ps.SubscriptionID
		WHERE  mg.Email !='' AND mg.FName='' AND mg.lname=''
		AND ISNULL(ps.Email,'')!='' AND (ISNULL(ps.FirstName,'')!='' OR ISNULL(ps.LastName,'')!='') and RecordMatchFound = 0 

PRINT ('After Step 18: (NON Matching Pubs in PubSubscriptions) - Email,Fname,Lname not blank and match against Subscriptions where Fname AND Lname blank and Email not blank - Opposite of the statement above / '  + Convert(VARCHAR,@@ROWCOUNT) + ' / ' + CONVERT(VARCHAR(20), GETDATE(), 114))

		--
		-- Distinct Email Source file does not have Fname Or LName match on PubSubscriptions
		--
		UPDATE mg
		SET indDistEmail = s.Igrp_no, RecordMatchFound = 2
--Select s.igrp_no, *		
		FROM #MatchGroups mg 
			JOIN PubSubscriptions ps with (NOLOCK) ON ps.PubID <> mg.PubID AND mg.Email = ps.EMAIL
			JOIN Subscriptions s with (NOLOCK) ON s.SubscriptionID = ps.SubscriptionID
		WHERE mg.Email!='' AND mg.fname='' AND mg.lname='' and RecordMatchFound = 0 

PRINT ('After Step 19 : (NON Matching Pubs in PubSubscriptions) - Distinct Email Source file does not have Fname Or LName match on Subscriptions / '  + Convert(VARCHAR,@@ROWCOUNT) + ' / ' + CONVERT(VARCHAR(20), GETDATE(), 114))
 		
		--
 		-- FName, LName, Address and Email is Blank. Title and Company not Blank match on Subscriptions
		--
 		-- We are using indDistEmail for this update because the match criteria assumes that this field should not have any values
 		UPDATE mg
 		SET indDistEmail = s.Igrp_no, RecordMatchFound = 2
--Select s.igrp_no, *		
 		FROM #MatchGroups mg 
 			JOIN PubSubscriptions ps with (NOLOCK) ON ps.PubID <> mg.PubID AND mg.Title = ps.Title AND mg.Company = ps.Company AND mg.Address = ps.Address1
			JOIN Subscriptions s with (NOLOCK) ON s.SubscriptionID = ps.SubscriptionID
 		WHERE mg.FName = '' AND mg.LName = '' AND mg.EMAIL = '' AND mg.[Address] != '' AND mg.TITLE != '' AND mg.Company != '' and RecordMatchFound = 0 
 		
PRINT ('After Step 20 : (NON Matching Pubs in PubSubscriptions) - FName, LName and Email is Blank. Title and Company not Blank match on Subscriptions / '  + Convert(VARCHAR,@@ROWCOUNT) + ' / ' + CONVERT(VARCHAR(20), GETDATE(), 114))


-- select * from #Matchgroups order by PubID, fname3, lname6, qdate desc


        --END Full MATCHING NON Matching PubCode (PubID)
----------------------------------------------------------------------------------------------------------------------------------------------------------------------
		--
		-------------------------------------------------------------------------------------------------------------------------------------------------------
		-- Begin Partial Data Match  on PubSubscriptions for NON Matching PubCode(PubID)
		--
		-- Fname,Lname and Address and City match
		--
		UPDATE mg
		SET indNameAddress = s.igrp_no, RecordMatchFound = 20
--Select s.igrp_no i, ps.*		
		FROM #MatchGroups mg 
			JOIN PubSubscriptions ps with (NOLOCK) ON ps.PubID <> mg.PubID 
				AND Left(ps.FirstName, master.dbo.fn_smallest(Len(ps.FirstName),Len(mg.FName3))) = Left(mg.FName3, master.dbo.fn_smallest(Len(ps.FirstName),Len(mg.FName3))) 
				AND Left(ps.LastName, master.dbo.fn_smallest(Len(ps.LastName),Len(mg.LName6))) = Left(mg.LName6, master.dbo.fn_smallest(Len(ps.LastName),Len(mg.LName6)))
--				AND Left(ps.FirstName,3) = mg.FName3 and LEFT(ps.LastName,6) = mg.LName6
				AND Left(ps.Address1,5) = mg.Address5
				AND ps.City = mg.City
			JOIN Subscriptions s with (NOLOCK) ON s.SubscriptionID = ps.SubscriptionID
		WHERE (mg.FName <> '' AND mg.LName <> '' AND ps.FirstName <> '' AND ps.LastName <> '') 
		AND master.dbo.fn_smallest(Len(ps.FirstName),Len(mg.FName3)) + master.dbo.fn_smallest(Len(ps.LastName),Len(mg.LName6)) > 5
		AND mg.address != '' and mg.City != '' and RecordMatchFound = 0 

PRINT ('After Step 21A : (NON Matching Pubs in PubSubscriptions) - Partial Fname,Lname Address5,City match on individual level / '  + Convert(VARCHAR,@@ROWCOUNT) + ' / ' + CONVERT(VARCHAR(20), GETDATE(), 114))

		--
		-- Fname,Lname and Address and City match
		--
		UPDATE mg
		SET indNameAddress = s.igrp_no, RecordMatchFound = 20
--Select s.igrp_no i, ps.*		
		FROM #MatchGroups mg 
			JOIN PubSubscriptions ps with (NOLOCK) ON ps.PubID <> mg.PubID 
				AND Left(ps.FirstName, master.dbo.fn_smallest(Len(ps.FirstName),Len(mg.FName3))) = Left(mg.FName3, master.dbo.fn_smallest(Len(ps.FirstName),Len(mg.FName3))) 
				AND Left(ps.LastName, master.dbo.fn_smallest(Len(ps.LastName),Len(mg.LName6))) = Left(mg.LName6, master.dbo.fn_smallest(Len(ps.LastName),Len(mg.LName6)))
--				AND Left(ps.FirstName,3) = mg.FName3 and LEFT(ps.LastName,6) = mg.LName6
				AND Left(ps.Address1,5) = mg.Address5
				AND ps.ZipCode = mg.Zipcode
			JOIN Subscriptions s with (NOLOCK) ON s.SubscriptionID = ps.SubscriptionID
		WHERE (mg.FName <> '' AND mg.LName <> '' AND ps.FirstName <> '' AND ps.LastName <> '') 
		AND master.dbo.fn_smallest(Len(ps.FirstName),Len(mg.FName3)) + master.dbo.fn_smallest(Len(ps.LastName),Len(mg.LName6)) > 5
		AND mg.address != '' and mg.Zipcode != '' and RecordMatchFound = 0 

PRINT ('After Step 21B : (NON Matching Pubs in PubSubscriptions) - Partial Fname,Lname Address5,Zip match on individual level / '  + Convert(VARCHAR,@@ROWCOUNT) + ' / ' + CONVERT(VARCHAR(20), GETDATE(), 114))

  		--
		-- Fname,Lname and Email match
		--
		UPDATE mg
		SET indNameEmail = s.igrp_no, RecordMatchFound = 20
--Select s.igrp_no, *		
		FROM #MatchGroups mg 
			JOIN PubSubscriptions ps with (NOLOCK) ON ps.PubID <> mg.PubID 
				AND Left(ps.FirstName, master.dbo.fn_smallest(Len(ps.FirstName),Len(mg.FName3))) = Left(mg.FName3, master.dbo.fn_smallest(Len(ps.FirstName),Len(mg.FName3))) 
				AND Left(ps.LastName, master.dbo.fn_smallest(Len(ps.LastName),Len(mg.LName6))) = Left(mg.LName6, master.dbo.fn_smallest(Len(ps.LastName),Len(mg.LName6)))
--				AND Left(ps.FirstName,3) = mg.FName3 and LEFT(ps.LastName,6) = mg.LName6
				AND mg.Email = ps.EMAIL
			JOIN Subscriptions s with (NOLOCK) ON s.SubscriptionID = ps.SubscriptionID
		WHERE (mg.FName <> '' AND mg.LName <> '' AND ps.FirstName <> '' AND ps.LastName <> '') 
		AND master.dbo.fn_smallest(Len(ps.FirstName),Len(mg.FName3)) + master.dbo.fn_smallest(Len(ps.LastName),Len(mg.LName6)) > 5
		AND mg.Email!='' and RecordMatchFound = 0  


PRINT ('After Step 22 : (NON Matching Pubs in PubSubscriptions) - Partial Fname,Lname Email match on individual level / '  + Convert(VARCHAR,@@ROWCOUNT) + ' / ' + CONVERT(VARCHAR(20), GETDATE(), 114))
  		--
		-- Fname,Lname and Company match
		--
		UPDATE mg
		SET indNameCompany = s.igrp_no, RecordMatchFound = 20
--Select s.igrp_no, *		
		FROM #MatchGroups mg 
			JOIN PubSubscriptions ps with (NOLOCK) ON ps.PubID <> mg.PubID 
				AND Left(ps.FirstName, master.dbo.fn_smallest(Len(ps.FirstName),Len(mg.FName3))) = Left(mg.FName3, master.dbo.fn_smallest(Len(ps.FirstName),Len(mg.FName3))) 
				AND Left(ps.LastName, master.dbo.fn_smallest(Len(ps.LastName),Len(mg.LName6))) = Left(mg.LName6, master.dbo.fn_smallest(Len(ps.LastName),Len(mg.LName6)))
--				AND Left(ps.FirstName,3) = mg.FName3 and LEFT(ps.LastName,6) = mg.LName6
				AND Left(ps.Company, 5) = mg.Company5
				AND Left(ps.Title, 5) = mg.Title5
				AND mg.Email = '' AND ps.Email = ''
			JOIN Subscriptions s with (NOLOCK) ON s.SubscriptionID = ps.SubscriptionID
		WHERE (mg.FName <> '' AND mg.LName <> '' AND ps.FirstName <> '' AND ps.LastName <> '') 
		AND master.dbo.fn_smallest(Len(ps.FirstName),Len(mg.FName3)) + master.dbo.fn_smallest(Len(ps.LastName),Len(mg.LName6)) > 5
		AND mg.Company!='' and mg.Title!='' and RecordMatchFound = 0 

PRINT ('After Step 23 : (MON Matching Pubs in PubSubscriptions) - Partial Fname,Lname, Company5 and Title5 match on individual level / '  + Convert(VARCHAR,@@ROWCOUNT) + ' / ' + CONVERT(VARCHAR(20), GETDATE(), 114))

  		--
		-- Fname,Lname and Phone match
		--
		UPDATE mg
		SET indNamePhone = s.igrp_no, RecordMatchFound = 20
--Select s.igrp_no, *		
		FROM #MatchGroups mg 
			JOIN PubSubscriptions ps with (NOLOCK) ON ps.PubID <> mg.PubID 
				AND Left(ps.FirstName, master.dbo.fn_smallest(Len(ps.FirstName),Len(mg.FName3))) = Left(mg.FName3, master.dbo.fn_smallest(Len(ps.FirstName),Len(mg.FName3))) 
				AND Left(ps.LastName, master.dbo.fn_smallest(Len(ps.LastName),Len(mg.LName6))) = Left(mg.LName6, master.dbo.fn_smallest(Len(ps.LastName),Len(mg.LName6)))
--				AND Left(ps.FirstName,3) = mg.FName3 and LEFT(ps.LastName,6) = mg.LName6
				AND mg.Phone = ps.Phone
			JOIN Subscriptions s with (NOLOCK) ON s.SubscriptionID = ps.SubscriptionID
		WHERE (mg.FName <> '' AND mg.LName <> '' AND ps.FirstName <> '' AND ps.LastName <> '') 
		AND master.dbo.fn_smallest(Len(ps.FirstName),Len(mg.FName3)) + master.dbo.fn_smallest(Len(ps.LastName),Len(mg.LName6)) > 5
		AND mg.Phone !='' and RecordMatchFound = 0  

PRINT ('After Step 24 : (NON Matching Pubs in PubSubscriptions) - Partial Fname,Lname and Phone match on individual level / '  + Convert(VARCHAR,@@ROWCOUNT) + ' / ' + CONVERT(VARCHAR(20), GETDATE(), 114))


-- select * from #Matchgroups order by PubID, fname3, lname6, qdate desc

        --END MATCHING at the PUB Subscription Level NON matching Pubs
----------------------------------------------------------------------------------------------------------------------------------------------------------------------

----------------------------------------------------------------------------------------------------------------------------------------------------------------------
		--
		-------------------------------------------------------------------------------------------------------------------------------------------------------
		-- Begin Data Match at the Subscription level
		
		--
		-- Fname,Lname and Address match on individual level
		-- ignore zip and just use first, last, address
		UPDATE mg
		SET indNameAddress = s.igrp_no, RecordMatchFound = 3
--Select s.igrp_no, *		
		FROM #MatchGroups mg 
			INNER JOIN Subscriptions s with (NOLOCK) ON mg.FName = s.fname AND mg.LName = s.LName AND mg.Address = s.Address
		WHERE (mg.FName <> '' OR mg.LName <> '') AND mg.address != ''  and RecordMatchFound = 0 

		UPDATE mg
		SET indNameAddress = s.igrp_no, RecordMatchFound = 3
--Select s.igrp_no, *		
		FROM #MatchGroups mg 
			INNER JOIN Subscriptions s with (NOLOCK) ON mg.FName = s.LNAME AND mg.LName = s.FNAME AND mg.Address = s.Address
		WHERE (mg.FName <> '' OR mg.LName <> '') AND mg.address != ''  and RecordMatchFound = 0 

PRINT ('After Step 25 : (Subscriptions) - Fname,Lname and Address match/SWAP on individual level / '  + Convert(VARCHAR,@@ROWCOUNT) + ' / ' + CONVERT(VARCHAR(20), GETDATE(), 114))
		
  		--
		-- Fname,Lname and Email match on individual level
		--
		UPDATE mg
		SET indNameEmail = s.igrp_no, RecordMatchFound = 3
		FROM #MatchGroups mg 
			INNER JOIN Subscriptions s with (NOLOCK) ON mg.FName = s.fname AND mg.LName = s.LName AND mg.Email = s.EMAIL
		WHERE (mg.FName <> '' OR mg.LName <> '') AND mg.Email <> '' and RecordMatchFound = 0 

		UPDATE mg
		SET indNameEmail = s.igrp_no, RecordMatchFound = 3
		FROM #MatchGroups mg 
			INNER JOIN Subscriptions s with (NOLOCK) ON mg.FName = s.LNAME AND mg.LName = s.FNAME AND mg.Email = s.EMAIL
		WHERE (mg.FName <> '' OR mg.LName <> '') AND mg.Email <> '' and RecordMatchFound = 0 

PRINT ('After Step 26 : (Subscriptions) - Fname,Lname and Email match/SWAP on individual level / '  + Convert(VARCHAR,@@ROWCOUNT) + ' / ' + CONVERT(VARCHAR(20), GETDATE(), 114))

  		--
		-- Fname,Lname and Company match on individual level
		--
		UPDATE mg
		SET indNameCompany = s.igrp_no, RecordMatchFound = 3
		FROM #MatchGroups mg 
			INNER JOIN Subscriptions s with (NOLOCK) ON mg.FName = s.fname AND mg.LName = s.LName AND  mg.Company = s.Company AND mg.Email = '' AND s.Email = ''
		WHERE (mg.FName <> '' OR mg.LName <> '') AND mg.Company <> '' and RecordMatchFound = 0 

		UPDATE mg
		SET indNameCompany = s.igrp_no, RecordMatchFound = 3
		FROM #MatchGroups mg 
			INNER JOIN Subscriptions s with (NOLOCK) ON mg.FName = s.LNAME AND mg.LName = s.FNAME AND  mg.Company = s.Company AND mg.Email = '' AND s.Email = ''
		WHERE (mg.FName <> '' OR mg.LName <> '') AND mg.Company <> '' and RecordMatchFound = 0 

PRINT ('After Step 27 : (Subscriptions) - Fname,Lname and Company match/SWAP on individual level / '  + Convert(VARCHAR,@@ROWCOUNT) + ' / ' + CONVERT(VARCHAR(20), GETDATE(), 114))
		
  		--
		-- Fname,Lname and Phone match on individual level
		--
		UPDATE mg
		SET indNamePhone = s.igrp_no, RecordMatchFound = 3
		FROM #MatchGroups mg 
			INNER JOIN Subscriptions s with (NOLOCK) ON mg.FName = s.fname AND mg.LName = s.LName AND mg.PHone = s.Phone
		WHERE (mg.FName <> '' OR mg.LName <> '') AND mg.Phone <> '' and RecordMatchFound = 0 

		UPDATE mg
		SET indNamePhone = s.igrp_no, RecordMatchFound = 3
		FROM #MatchGroups mg 
			INNER JOIN Subscriptions s with (NOLOCK) ON mg.FName = s.LNAME AND mg.LName = s.FNAME AND mg.PHone = s.Phone
		WHERE (mg.FName <> '' OR mg.LName <> '') AND mg.Phone <> '' and RecordMatchFound = 0 

PRINT ('After Step 28 : (Subscriptions) - Fname,Lname and Phone match/SWAP on individual level / '  + Convert(VARCHAR,@@ROWCOUNT) + ' / ' + CONVERT(VARCHAR(20), GETDATE(), 114))
		
		--
		-- Email,Fname,Lname not blank and match against Subscriptions where Fname AND Lname blank and Email not blank
		--
		UPDATE mg
		SET indNameNotBlankEmail = s.Igrp_no, RecordMatchFound = 3
		FROM #MatchGroups mg 
			INNER JOIN Subscriptions s with (NOLOCK) ON mg.Email = s.EMAIL
		WHERE mg.Email <> '' AND (mg.FName <> '' OR mg.lname <> '')
		AND ISNULL(s.Email,'') <> '' AND ISNULL(s.fname,'') = '' AND ISNULL(s.lname,'') = '' and RecordMatchFound = 0 

	
PRINT ('After Step 29 : (Subscriptions) - Email,Fname,Lname not blank and match against Subscriptions where Fname AND Lname blank and Email not blank / '  + Convert(VARCHAR,@@ROWCOUNT) + ' / ' + CONVERT(VARCHAR(20), GETDATE(), 114))

		--
		-- Email not blank and Fname OR Lname not blank and match against Subscriptions where Fname AND Lname blank and Email not blank - Opposite of the statement above
		--
		UPDATE mg
		SET indNameNotBlankEmail = s.Igrp_no, RecordMatchFound = 3
		FROM #MatchGroups mg 
			INNER JOIN Subscriptions s with (NOLOCK) ON  mg.Email = s.EMAIL
		WHERE  mg.Email <> '' AND mg.FName = '' AND mg.lname = ''
		AND ISNULL(s.Email,'') <> '' AND (ISNULL(s.fname,'') <> '' OR ISNULL(s.lname,'') <> '') and RecordMatchFound = 0 
		
PRINT ('After Step 30 : (Subscriptions) - Email,Fname,Lname not blank and match against Subscriptions where Fname AND Lname blank and Email not blank - Opposite of the statement above / '  + Convert(VARCHAR,@@ROWCOUNT) + ' / ' + CONVERT(VARCHAR(20), GETDATE(), 114))

		--
		-- Distinct Email Source file does not have Fname Or LName match on Subscriptions
		--
		UPDATE mg
		SET indDistEmail = s.Igrp_no, RecordMatchFound = 3
		FROM #MatchGroups mg 
			 INNER JOIN Subscriptions s with (NOLOCK) ON s.EMAIL = mg.Email
		WHERE mg.Email <> '' AND mg.fname = '' AND mg.lname = '' and RecordMatchFound = 0 

PRINT ('After Step 31 : (Subscriptions) - Distinct Email Source file does not have Fname Or LName match on Subscriptions / '  + Convert(VARCHAR,@@ROWCOUNT) + ' / ' + CONVERT(VARCHAR(20), GETDATE(), 114))
 		
 		-- We are using indDistEmail for this update because the match criteria assumes that this field should not have any values
 		-- FName, LName, Address and Email is Blank. Title and Company not Blank match on Subscriptions
 		UPDATE mg
 		SET indDistEmail = s.Igrp_no, RecordMatchFound = 3
 		FROM #MatchGroups mg 
 			INNER JOIN Subscriptions s with (NOLOCK) on mg.Title = s.Title AND mg.Company = s.Company AND mg.Address = s.Address
 		WHERE mg.FName = '' AND mg.LName = '' AND mg.EMAIL = '' AND mg.[Address] <> '' AND mg.TITLE <> '' AND mg.Company <> '' and RecordMatchFound = 0 
 		
PRINT ('After Step 32 : (Subscriptions) - FName, LName and Email is Blank. Title and Company not Blank match on Subscriptions / '  + Convert(VARCHAR,@@ROWCOUNT) + ' / ' + CONVERT(VARCHAR(20), GETDATE(), 114))

-- select * from #Matchgroups order by PubID, fname3, lname6, qdate desc

        --END INDIVIDUAL MATCHING at the Subscription Level
----------------------------------------------------------------------------------------------------------------------------------------------------------------------
		--
		-------------------------------------------------------------------------------------------------------------------------------------------------------
		-- Begin Partial Data Match  on Subscriptions level

		--
		-- Fname,Lname and Address match on individual level
		-- ignore zip and just use first, last, address
		UPDATE mg
		SET indNameAddress = s.igrp_no, RecordMatchFound = 30
		FROM #MatchGroups mg 
			INNER JOIN Subscriptions s with (NOLOCK) ON 
				Left(s.FName, master.dbo.fn_smallest(Len(s.FName),Len(mg.FName3))) = Left(mg.FName3, master.dbo.fn_smallest(Len(s.FName),Len(mg.FName3))) AND
				Left(s.LName, master.dbo.fn_smallest(Len(s.LName),Len(mg.LName6))) = Left(mg.LName6, master.dbo.fn_smallest(Len(s.LName),Len(mg.LName6))) AND
--				LEFT(s.fname, 3) = mg.FName3 AND LEFT(s.LName, 6) = mg.LName6 AND 
				mg.Address5 = LEFT(s.Address, 5) AND
				mg.City = s.City
		WHERE (mg.FName <> '' AND mg.LName <> '' AND s.FNAME <> '' AND s.LNAME <> '') 
		AND master.dbo.fn_smallest(Len(s.FName),Len(mg.FName3)) + master.dbo.fn_smallest(Len(s.LName),Len(mg.LName6)) > 5
		AND mg.address != '' AND mg.City != '' and RecordMatchFound = 0 

PRINT ('After Step 33A : (Subscriptions) - Partial Fname,Lname,Address5 and City match on individual level / '  + Convert(VARCHAR,@@ROWCOUNT) + ' / ' + CONVERT(VARCHAR(20), GETDATE(), 114))

		--
		-- Fname,Lname and Address match on individual level
		-- ignore zip and just use first, last, address
		UPDATE mg
		SET indNameAddress = s.igrp_no, RecordMatchFound = 30
		FROM #MatchGroups mg 
			INNER JOIN Subscriptions s with (NOLOCK) ON 
				Left(s.FName, master.dbo.fn_smallest(Len(s.FName),Len(mg.FName3))) = Left(mg.FName3, master.dbo.fn_smallest(Len(s.FName),Len(mg.FName3))) AND
				Left(s.LName, master.dbo.fn_smallest(Len(s.LName),Len(mg.LName6))) = Left(mg.LName6, master.dbo.fn_smallest(Len(s.LName),Len(mg.LName6))) AND
--				LEFT(s.fname, 3) mg.Fname3 AND LEFT(s.LName, 6) = mg.LName6 AND 
				mg.Address5 = LEFT(s.Address, 5) AND
				mg.Zipcode = s.Zip
		WHERE (mg.FName <> '' AND mg.LName <> '' AND s.FNAME <> '' AND s.LNAME <> '') 
		AND master.dbo.fn_smallest(Len(s.FName),Len(mg.FName3)) + master.dbo.fn_smallest(Len(s.LName),Len(mg.LName6)) > 5
		AND mg.address != '' AND mg.Zipcode != '' and RecordMatchFound = 0 

PRINT ('After Step 33B : (Subscriptions) - Partial Fname,Lname,Address5 and Zip match on individual level / '  + Convert(VARCHAR,@@ROWCOUNT) + ' / ' + CONVERT(VARCHAR(20), GETDATE(), 114))
		
  		--
		-- Fname,Lname and Email match on individual level
		--
		UPDATE mg
		SET indNameEmail = s.igrp_no, RecordMatchFound = 30
		FROM #MatchGroups mg 
			INNER JOIN Subscriptions s with (NOLOCK) ON 
				Left(s.FName, master.dbo.fn_smallest(Len(s.FName),Len(mg.FName3))) = Left(mg.FName3, master.dbo.fn_smallest(Len(s.FName),Len(mg.FName3))) AND
				Left(s.LName, master.dbo.fn_smallest(Len(s.LName),Len(mg.LName6))) = Left(mg.LName6, master.dbo.fn_smallest(Len(s.LName),Len(mg.LName6))) AND
--				LEFT(s.fname, 3) = mg.FName3 AND LEFT(s.LName, 6) = mg.LName6 AND 
				mg.Email = s.EMAIL
		WHERE (mg.FName <> '' AND mg.LName <> '' AND s.FNAME <> '' AND s.LNAME <> '') 
		AND master.dbo.fn_smallest(Len(s.FName),Len(mg.FName3)) + master.dbo.fn_smallest(Len(s.LName),Len(mg.LName6)) > 5
		AND mg.Email <> '' and RecordMatchFound = 0 

PRINT ('After Step 34 : (Subscriptions) - Partial Fname,Lname and Email match on individual level / '  + Convert(VARCHAR,@@ROWCOUNT) + ' / ' + CONVERT(VARCHAR(20), GETDATE(), 114))

  		--
		-- Fname,Lname and Company match on individual level
		--
		UPDATE mg
		SET indNameCompany = s.igrp_no, RecordMatchFound = 30
		FROM #MatchGroups mg 
			INNER JOIN Subscriptions s with (NOLOCK) ON 
				Left(s.FName, master.dbo.fn_smallest(Len(s.FName),Len(mg.FName3))) = Left(mg.FName3, master.dbo.fn_smallest(Len(s.FName),Len(mg.FName3))) AND
				Left(s.LName, master.dbo.fn_smallest(Len(s.LName),Len(mg.LName6))) = Left(mg.LName6, master.dbo.fn_smallest(Len(s.LName),Len(mg.LName6))) AND
--				LEFT(s.fname, 3) = mg.FName3 AND LEFT(s.LName, 6) = mg.LName6 AND  
				mg.Company5 = LEFT(s.Company, 5) AND 
				mg.Title5 = LEFT(s.Title, 5) AND 
				mg.Email = '' AND s.Email = ''
		WHERE (mg.FName <> '' AND mg.LName <> '' AND s.FNAME <> '' AND s.LNAME <> '') 
		AND master.dbo.fn_smallest(Len(s.FName),Len(mg.FName3)) + master.dbo.fn_smallest(Len(s.LName),Len(mg.LName6)) > 5
 		AND mg.Company <> '' AND mg.Title <> '' and RecordMatchFound = 0 

PRINT ('After Step 35 : (Subscriptions) - Partial Fname,Lname, Company and Title match on individual level / '  + Convert(VARCHAR,@@ROWCOUNT) + ' / ' + CONVERT(VARCHAR(20), GETDATE(), 114))
		
  		--
		-- Fname,Lname and Phone match on individual level
		--
		UPDATE mg
		SET indNamePhone = s.igrp_no, RecordMatchFound = 30
		FROM #MatchGroups mg 
			INNER JOIN Subscriptions s with (NOLOCK) ON 
				Left(s.FName, master.dbo.fn_smallest(Len(s.FName),Len(mg.FName3))) = Left(mg.FName3, master.dbo.fn_smallest(Len(s.FName),Len(mg.FName3))) AND
				Left(s.LName, master.dbo.fn_smallest(Len(s.LName),Len(mg.LName6))) = Left(mg.LName6, master.dbo.fn_smallest(Len(s.LName),Len(mg.LName6))) AND
--				LEFT(s.fname, 3) = mg.FName4 AND LEFT(s.LName, 6) = mg.LName6 AND 
				mg.Phone = s.Phone
		WHERE (mg.FName <> '' AND mg.LName <> '' AND s.FNAME <> '' AND s.LNAME <> '') 
		AND master.dbo.fn_smallest(Len(s.FName),Len(mg.FName3)) + master.dbo.fn_smallest(Len(s.LName),Len(mg.LName6)) > 5
		AND mg.Phone <> '' and RecordMatchFound = 0 

PRINT ('After Step 36 : (Subscriptions) - Partial Fname,Lname and Phone match on individual level / '  + Convert(VARCHAR,@@ROWCOUNT) + ' / ' + CONVERT(VARCHAR(20), GETDATE(), 114))

--select RecordMatchFound, isoriginal, COUNT(*) from #MatchGroups group by RecordMatchFound, IsOriginal

        --END INDIVIDUAL MATCHING at the Subscription Level
----------------------------------------------------------------------------------------------------------------------------------------------------------------------

        ----------------------------------------------------------------------------------------------------------------------------
        --BEGIN ASSIGN IGRP_NO

--select recordtype, IsOriginal, COUNT(*) from #MatchGroups group by RecordType, IsOriginal

		-- Set IsNewRecord,  If last selection is MatchID from Coalesce, then it is a new record
		UPDATE #MatchGroups set IsNewRecord = 1 
			where COALESCE(Igrp_no, indNameAddress, indNameEmail, indNameCompany, indNamePhone, indNameNotBlankEmail, indDistEmail, MatchID) = MatchID

		UPDATE #MatchGroups set Igrp_no = COALESCE(Igrp_no, indNameAddress, indNameEmail, indNameCompany, indNamePhone, indNameNotBlankEmail, indDistEmail, MatchID)

		UPDATE sf SET sf.IGrp_No = mg.Igrp_no, sf.IsNewRecord = mg.IsNewRecord
		from #MatchGroups mg
		inner join #Subscriberfinal sf on mg.SFRecordIdentifier = sf.SFRecordIdentifier
		where mg.RecordType = 'Master' and IsOriginal = 1 and sf.IGrp_No is Null

		UPDATE sf SET sf.IGrp_No = mg.Igrp_no, sf.IsNewRecord = mg.IsNewRecord
		from #MatchGroups mg
		inner join #Subscriberfinal sf on mg.SFRecordIdentifier = sf.SFRecordIdentifier
		where mg.RecordType <> 'Master' and IsOriginal = 1 and sf.IGrp_No is Null

		UPDATE sf SET sf.IGrp_No = mg.Igrp_no, sf.IsNewRecord = mg.IsNewRecord
		from #MatchGroups mg
		inner join #Subscriberfinal sf on mg.SFRecordIdentifier = sf.SFRecordIdentifier
		where mg.RecordType = 'Master' and IsOriginal <> 1 and sf.IGrp_No is Null

		UPDATE sf SET sf.IGrp_No = mg.Igrp_no, sf.IsNewRecord = mg.IsNewRecord
		from #MatchGroups mg
		inner join #Subscriberfinal sf on mg.SFRecordIdentifier = sf.SFRecordIdentifier
		where mg.RecordType <> 'Master' and IsOriginal <> 1 and sf.IGrp_No is Null

		UPDATE sf SET sf.IGrp_No = mg.Igrp_no, sf.IsNewRecord = mg.IsNewRecord
		from #MatchGroups mg
		inner join #Subscriberfinal sf on mg.SFRecordIdentifier = sf.SFRecordIdentifier
		where sf.IGrp_No is Null

PRINT ('END ASSIGN IGRP_NO / '  + CONVERT(VARCHAR(20), GETDATE(), 114))

--Select recordType, IsOriginal, igrp_no, matchid, * from #MatchGroups where sfrecordidentifier in (Select sfrecordidentifier from #Subscriberfinal where igrp_no is null)

        --END ASSIGN IGRP_NO
----------------------------------------------------------------------------------------------------------------------------------------------------------------------

        ----------------------------------------------------------------------------------------------------------------------------
        --BEGIN FULL ROLLUP of Data in #SubscriberFinal

		-- Build List of IGrp_no that have duplicates
		select ROW_NUMBER() over (partition by Igrp_no, PubCode order by Igrp_no, PubCode, QDate DESC) as PartitionRow, SfRecordIdentifier,Igrp_no, PubCode
			INTO #IGrpDupList
		 from #SubscriberFinal where igrp_no in (select igrp_no from #SubscriberFinal group by igrp_no having count(*) > 1)
		 
		IF EXISTS ( Select top 1 1 from #IGrpDupList )
		BEGIN

		CREATE INDEX IDX_IGrpDupList_IGrpNo ON #IgrpDupList(Igrp_no)
		
		
				
PRINT ('BEGIN FULL ROLLUP / '   + Convert(VARCHAR,@@ROWCOUNT) + ' / ' + CONVERT(VARCHAR(20), GETDATE(), 114))

		--DECLARE @IgrpID				UniqueIdentifier		
		--DECLARE @SFRecordIdentifier	UniqueIdentifier		
		--DECLARE @MAFField			varchar(100)


			DECLARE c_IgrpIDs CURSOR LOCAL FORWARD_ONLY STATIC READ_ONLY FOR select Distinct igrp_no from #IgrpDupList


			OPEN c_IgrpIDs  
			FETCH NEXT FROM c_IgrpIDs INTO @IgrpID

			-- Loop through all MatchIDs
			WHILE @@FETCH_STATUS = 0  
			BEGIN  
				DECLARE c_DupRecs CURSOR LOCAL FORWARD_ONLY STATIC READ_ONLY FOR Select dup.SfRecordIdentifier from #IgrpDupList dup where dup.Igrp_No = @IgrpID and dup.PartitionRow > 1 order by dup.PartitionRow ASC
				OPEN c_DupRecs  

				FETCH NEXT from c_DupRecs INTO @SFRecordIdentifier
				WHILE @@FETCH_STATUS = 0  
				BEGIN  
					UPDATE mstr set 
						mstr.QDate		= CASE when dups.Qdate > mstr.QDate THEN dups.Qdate else mstr.qdate end,
						mstr.QSourceID	= CASE when ISNULL(dups.QSourceID,'') <> '' AND (ISNULL(mstr.QSourceID,'') = '' OR dups.Qdate > mstr.Qdate) THEN dups.QSourceID ELSE mstr.QSourceID END ,
						-- move first and last name from the same record
						mstr.FName		= case when (ISNULL(dups.FName, '') <> '' AND ISNULL(dups.LName, '') <> '') AND (ISNULL(mstr.FName, '') = '' OR ISNULL(mstr.LName, '') = '' OR dups.Qdate > mstr.QDate) THEN dups.FName else mstr.FName end,
						mstr.LName		= case when (ISNULL(dups.FName, '') <> '' AND ISNULL(dups.LName, '') <> '') AND (ISNULL(mstr.FName, '') = '' OR ISNULL(mstr.LName, '') = '' OR dups.Qdate > mstr.QDate) THEN dups.LName else mstr.LName end,
						mstr.Email		= CASE when ISNULL(dups.Email, '') <> '' AND (IsNull(mstr.Email, '') = '' OR dups.Qdate > mstr.QDate) THEN dups.Email else mstr.Email end,
						-- move all adress info from the same record, do not split the fields up
						mstr.address	= case when (ISNULL(dups.Address, '') <> '' OR (ISNULL(dups.City, '') <> '' AND ISNULL(dups.State, '') <> '')) AND (ISNULL(mstr.Address, '') = '' OR ISNULL(mstr.city, '') = '' OR ISNULL(mstr.state, '') = '' OR dups.Qdate > mstr.QDate) THEN dups.Address else mstr.address end, 
						mstr.MailStop	= case when (ISNULL(dups.Address, '') <> '' OR (ISNULL(dups.City, '') <> '' AND ISNULL(dups.State, '') <> '')) AND (ISNULL(mstr.Address, '') = '' OR ISNULL(mstr.city, '') = '' OR ISNULL(mstr.state, '') = '' OR dups.Qdate > mstr.QDate) THEN dups.MailStop ELSE mstr.MailStop END ,
						mstr.Address3	= case when (ISNULL(dups.Address, '') <> '' OR (ISNULL(dups.City, '') <> '' AND ISNULL(dups.State, '') <> '')) AND (ISNULL(mstr.Address, '') = '' OR ISNULL(mstr.city, '') = '' OR ISNULL(mstr.state, '') = '' OR dups.Qdate > mstr.QDate) THEN dups.Address3 ELSE mstr.Address3 END ,
						mstr.city		= case when (ISNULL(dups.Address, '') <> '' OR (ISNULL(dups.City, '') <> '' AND ISNULL(dups.State, '') <> '')) AND (ISNULL(mstr.Address, '') = '' OR ISNULL(mstr.city, '') = '' OR ISNULL(mstr.state, '') = '' OR dups.Qdate > mstr.QDate) then dups.City else mstr.city end, 
						mstr.state		= case when (ISNULL(dups.Address, '') <> '' OR (ISNULL(dups.City, '') <> '' AND ISNULL(dups.State, '') <> '')) AND (ISNULL(mstr.Address, '') = '' OR ISNULL(mstr.city, '') = '' OR ISNULL(mstr.state, '') = '' OR dups.Qdate > mstr.QDate) then dups.state else mstr.state end,
						mstr.Zip		= case when (ISNULL(dups.Address, '') <> '' OR (ISNULL(dups.City, '') <> '' AND ISNULL(dups.State, '') <> '')) AND (ISNULL(mstr.Address, '') = '' OR ISNULL(mstr.city, '') = '' OR ISNULL(mstr.state, '') = '' OR dups.Qdate > mstr.QDate) THEN dups.Zip ELSE mstr.Zip END ,
						mstr.Plus4		= case when (ISNULL(dups.Address, '') <> '' OR (ISNULL(dups.City, '') <> '' AND ISNULL(dups.State, '') <> '')) AND (ISNULL(mstr.Address, '') = '' OR ISNULL(mstr.city, '') = '' OR ISNULL(mstr.state, '') = '' OR dups.Qdate > mstr.QDate) THEN dups.Plus4 ELSE mstr.Plus4 END ,
						mstr.ForZip		= case when (ISNULL(dups.Address, '') <> '' OR (ISNULL(dups.City, '') <> '' AND ISNULL(dups.State, '') <> '')) AND (ISNULL(mstr.Address, '') = '' OR ISNULL(mstr.city, '') = '' OR ISNULL(mstr.state, '') = '' OR dups.Qdate > mstr.QDate) THEN dups.ForZip ELSE mstr.ForZip END ,
						mstr.County		= case when (ISNULL(dups.Address, '') <> '' OR (ISNULL(dups.City, '') <> '' AND ISNULL(dups.State, '') <> '')) AND (ISNULL(mstr.Address, '') = '' OR ISNULL(mstr.city, '') = '' OR ISNULL(mstr.state, '') = '' OR dups.Qdate > mstr.QDate) THEN dups.County ELSE mstr.County END ,
						mstr.Country	= case when (ISNULL(dups.Address, '') <> '' OR (ISNULL(dups.City, '') <> '' AND ISNULL(dups.State, '') <> '')) AND (ISNULL(mstr.Address, '') = '' OR ISNULL(mstr.city, '') = '' OR ISNULL(mstr.state, '') = '' OR dups.Qdate > mstr.QDate) THEN dups.Country ELSE mstr.Country END ,
						mstr.CountryID	= case when (ISNULL(dups.Address, '') <> '' OR (ISNULL(dups.City, '') <> '' AND ISNULL(dups.State, '') <> '')) AND (ISNULL(mstr.Address, '') = '' OR ISNULL(mstr.city, '') = '' OR ISNULL(mstr.state, '') = '' OR dups.Qdate > mstr.QDate) THEN dups.CountryID ELSE mstr.CountryID END ,
						mstr.Home_Work_Address=case when (ISNULL(dups.Address, '') <> '' OR (ISNULL(dups.City, '') <> '' AND ISNULL(dups.State, '') <> '')) AND (ISNULL(mstr.Address, '') = '' OR ISNULL(mstr.city, '') = '' OR ISNULL(mstr.state, '') = '' OR dups.Qdate > mstr.QDate) THEN dups.Home_Work_Address ELSE mstr.Home_Work_Address END ,

						mstr.Company	=CASE when dups.Company <> '' AND (IsNull(mstr.Company, '') = '' OR dups.Qdate > mstr.QDate) THEN dups.Company else mstr.Company end,
						mstr.Title		=CASE when dups.Title <> '' AND (ISNULL(mstr.Title,'') = '' OR dups.Qdate > mstr.Qdate) THEN dups.Title ELSE mstr.Title END ,
						mstr.Phone		=CASE when dups.Phone <> '' AND (IsNull(mstr.Phone, '') = '' OR dups.Qdate > mstr.QDate) THEN dups.Phone else mstr.Phone end,
						mstr.Fax		=CASE when ISNULL(dups.Fax,'') <> '' AND (ISNULL(mstr.Fax,'') = '' OR dups.Qdate > mstr.Qdate) THEN dups.Fax ELSE mstr.Fax END ,
						mstr.Mobile		=CASE when ISNULL(dups.Mobile,'') <> '' AND (ISNULL(mstr.Mobile,'') = '' OR dups.Qdate > mstr.Qdate) THEN dups.Mobile ELSE mstr.Mobile END ,
						mstr.Gender		=CASE when ISNULL(dups.Gender,'') <> '' AND (ISNULL(mstr.Gender,'') = '' OR dups.Qdate > mstr.Qdate) THEN dups.Gender ELSE mstr.Gender END ,
						mstr.Sequence	=CASE when ISNULL(dups.Sequence,0) <> 0 AND (ISNULL(mstr.Sequence,0) = 0 OR dups.Qdate > mstr.Qdate) THEN dups.Sequence ELSE mstr.Sequence END ,
						mstr.CategoryID	=CASE when ISNULL(dups.CategoryID,0) <> 0 AND (ISNULL(mstr.CategoryID,0) = 0 OR dups.Qdate > mstr.Qdate) THEN dups.CategoryID ELSE mstr.CategoryID END ,
						mstr.TransactionID=CASE when ISNULL(dups.TransactionID,0) <> 0 AND (ISNULL(mstr.TransactionID,0) = 0 OR dups.Qdate > mstr.Qdate) THEN dups.TransactionID ELSE mstr.TransactionID END ,
						mstr.TransactionDate=CASE when ISNULL(dups.TransactionDate,'1/1/1900') <> '1/1/1900' AND (ISNULL(mstr.TransactionDate,'1/1/1900') = '1/1/1900' OR dups.Qdate > mstr.Qdate) THEN dups.TransactionDate ELSE mstr.TransactionDate END ,
						mstr.RegCode	=CASE when ISNULL(dups.RegCode,'') <> '' AND (ISNULL(mstr.RegCode,'') = '' OR dups.Qdate > mstr.Qdate) THEN dups.RegCode ELSE mstr.RegCode END ,
						mstr.Verified	=CASE when ISNULL(dups.Verified,'') <> '' AND (ISNULL(mstr.Verified,'') = '' OR dups.Qdate > mstr.Qdate) THEN dups.Verified ELSE mstr.Verified END ,
						mstr.SubSrc		=CASE when ISNULL(dups.SubSrc,'') <> '' AND (ISNULL(mstr.SubSrc,'') = '' OR dups.Qdate > mstr.Qdate) THEN dups.SubSrc ELSE mstr.SubSrc END ,
						mstr.OrigsSrc	=CASE when ISNULL(dups.OrigsSrc,'') <> '' AND (ISNULL(mstr.OrigsSrc,'') = '' OR dups.Qdate > mstr.Qdate) THEN dups.OrigsSrc ELSE mstr.OrigsSrc END ,
						mstr.Par3C		=CASE when ISNULL(dups.Par3C,'') <> '' AND (ISNULL(mstr.Par3C,'') = '' OR dups.Qdate > mstr.Qdate) THEN dups.Par3C ELSE mstr.Par3C END ,
						mstr.Source		=CASE when ISNULL(dups.Source,'') <> '' AND (ISNULL(mstr.Source,'') = '' OR dups.Qdate > mstr.Qdate) THEN dups.Source ELSE mstr.Source END ,
						mstr.Priority	=CASE when ISNULL(dups.Priority,'') <> '' AND (ISNULL(mstr.Priority,'') = '' OR dups.Qdate > mstr.Qdate) THEN dups.Priority ELSE mstr.Priority END ,
						mstr.StatList	=CASE when ISNULL(dups.StatList,0) <> 0 AND (ISNULL(mstr.StatList,0) = 0 OR dups.Qdate > mstr.Qdate) THEN dups.StatList ELSE mstr.StatList END ,
						mstr.Sic		=CASE when ISNULL(dups.Sic,'') <> '' AND (ISNULL(mstr.Sic,'') = '' OR dups.Qdate > mstr.Qdate) THEN dups.Sic ELSE mstr.Sic END ,
						mstr.SicCode	=CASE when ISNULL(dups.SicCode,'') <> '' AND (ISNULL(mstr.SicCode,'') = '' OR dups.Qdate > mstr.Qdate) THEN dups.SicCode ELSE mstr.SicCode END ,
						mstr.Demo7		=CASE when ISNULL(dups.Demo7,'') <> '' AND (ISNULL(mstr.Demo7,'') = '' OR dups.Qdate > mstr.Qdate) THEN dups.Demo7 ELSE mstr.Demo7 END ,
						mstr.Latitude	=CASE when ISNULL(dups.Latitude,0) <> 0 AND (ISNULL(mstr.Latitude,0) = 0 OR dups.Qdate > mstr.Qdate) THEN dups.Latitude ELSE mstr.Latitude END ,
						mstr.Longitude	=CASE when ISNULL(dups.Longitude,0) <> 0 AND (ISNULL(mstr.Longitude,0) = 0 OR dups.Qdate > mstr.Qdate) THEN dups.Longitude ELSE mstr.Longitude END ,
						mstr.IsLatLonValid=CASE when ISNULL(dups.IsLatLonValid,0) <> 0 AND (ISNULL(mstr.IsLatLonValid,0) = 0 OR dups.Qdate > mstr.Qdate) THEN dups.IsLatLonValid ELSE mstr.IsLatLonValid END ,
						mstr.LatLonMsg	=CASE when ISNULL(dups.LatLonMsg,'') <> '' AND (ISNULL(mstr.LatLonMsg,'') = '' OR dups.Qdate > mstr.Qdate) THEN dups.LatLonMsg ELSE mstr.LatLonMsg END ,
						mstr.EmailStatusID=CASE when ISNULL(dups.EmailStatusID,0) <> 0 AND (ISNULL(mstr.EmailStatusID,0) = 0 OR dups.Qdate > mstr.Qdate) THEN dups.EmailStatusID ELSE mstr.EmailStatusID END ,
						mstr.Ignore		=CASE when ISNULL(dups.Ignore,0) <> 0 AND (ISNULL(mstr.Ignore,0) = 0 OR dups.Qdate > mstr.Qdate) THEN dups.Ignore ELSE mstr.Ignore END ,
						mstr.IsMailable	=CASE when ISNULL(dups.IsMailable,0) <> 0 AND (ISNULL(mstr.IsMailable,0) = 0 OR dups.Qdate > mstr.Qdate) THEN dups.IsMailable ELSE mstr.IsMailable END ,
						mstr.IsActive	=CASE when ISNULL(dups.IsActive,0) <> 0 AND (ISNULL(mstr.IsActive,0) = 0 OR dups.Qdate > mstr.Qdate) THEN dups.IsActive ELSE mstr.IsActive END ,
						mstr.ExternalKeyId=CASE when ISNULL(dups.ExternalKeyId,0) <> 0 AND (ISNULL(mstr.ExternalKeyId,0) = 0 OR dups.Qdate > mstr.Qdate) THEN dups.ExternalKeyId ELSE mstr.ExternalKeyId END ,
						mstr.AccountNumber=CASE when ISNULL(dups.AccountNumber,'') <> '' AND (ISNULL(mstr.AccountNumber,'') = '' OR dups.Qdate > mstr.Qdate) THEN dups.AccountNumber ELSE mstr.AccountNumber END ,
						mstr.EmailID	=CASE when ISNULL(dups.EmailID,0) <> 0 AND (ISNULL(mstr.EmailID,0) = 0 OR dups.Qdate > mstr.Qdate) THEN dups.EmailID ELSE mstr.EmailID END ,
						mstr.Copies		=CASE when ISNULL(dups.Copies,0) <> 0 AND (ISNULL(mstr.Copies,0) = 0 OR dups.Qdate > mstr.Qdate) THEN dups.Copies ELSE mstr.Copies END ,
						mstr.GraceIssues=CASE when ISNULL(dups.GraceIssues,0) <> 0 AND (ISNULL(mstr.GraceIssues,0) = 0 OR dups.Qdate > mstr.Qdate) THEN dups.GraceIssues ELSE mstr.GraceIssues END ,
						mstr.IsComp		=CASE when ISNULL(dups.IsComp,0) <> 0 AND (ISNULL(mstr.IsComp,0) = 0 OR dups.Qdate > mstr.Qdate) THEN dups.IsComp ELSE mstr.IsComp END ,
						mstr.IsPaid		=CASE when ISNULL(dups.IsPaid,0) <> 0 AND (ISNULL(mstr.IsPaid,0) = 0 OR dups.Qdate > mstr.Qdate) THEN dups.IsPaid ELSE mstr.IsPaid END ,
						mstr.IsSubscribed=CASE when ISNULL(dups.IsSubscribed,0) <> 0 AND (ISNULL(mstr.IsSubscribed,0) = 0 OR dups.Qdate > mstr.Qdate) THEN dups.IsSubscribed ELSE mstr.IsSubscribed END ,
						mstr.Occupation	=CASE when ISNULL(dups.Occupation,'') <> '' AND (ISNULL(mstr.Occupation,'') = '' OR dups.Qdate > mstr.Qdate) THEN dups.Occupation ELSE mstr.Occupation END ,
						mstr.SubscriptionStatusID=CASE when ISNULL(dups.SubscriptionStatusID,0) <> 0 AND (ISNULL(mstr.SubscriptionStatusID,0) = 0 OR dups.Qdate > mstr.Qdate) THEN dups.SubscriptionStatusID ELSE mstr.SubscriptionStatusID END ,
						mstr.SubsrcID	=CASE when ISNULL(dups.SubsrcID,0) <> 0 AND (ISNULL(mstr.SubsrcID,0) = 0 OR dups.Qdate > mstr.Qdate) THEN dups.SubsrcID ELSE mstr.SubsrcID END ,
						mstr.Website	=CASE when ISNULL(dups.Website,'') <> '' AND (ISNULL(mstr.Website,'') = '' OR dups.Qdate > mstr.Qdate) THEN dups.Website ELSE mstr.Website END ,

						-- Permission fields, do not base on Qdate,  IF permission for any is 0, use 0					
						mstr.MailPermission=CASE when mstr.MailPermission IS NULL OR (dups.MailPermission IS NOT NULL AND ISNULL(dups.MailPermission, 0) = 0) THEN dups.MailPermission ELSE mstr.MailPermission END ,
						mstr.FaxPermission=CASE when mstr.FaxPermission IS NULL OR (dups.FaxPermission IS NOT NULL AND ISNULL(dups.FaxPermission,0) = 0) THEN dups.FaxPermission ELSE mstr.FaxPermission END ,
						mstr.PhonePermission=CASE when mstr.PhonePermission IS NULL OR (dups.PhonePermission IS NOT NULL AND ISNULL(dups.PhonePermission,0) = 0) THEN dups.PhonePermission ELSE mstr.PhonePermission END ,
						mstr.OtherProductsPermission=CASE when mstr.OtherProductsPermission IS NULL OR (dups.OtherProductsPermission IS NOT NULL AND ISNULL(dups.OtherProductsPermission,0) = 0) THEN dups.OtherProductsPermission ELSE mstr.OtherProductsPermission END ,
						mstr.ThirdPartyPermission=CASE when mstr.ThirdPartyPermission IS NULL OR (dups.ThirdPartyPermission IS NOT NULL AND ISNULL(dups.ThirdPartyPermission,0) = 0) THEN dups.ThirdPartyPermission ELSE mstr.ThirdPartyPermission END ,
						mstr.EmailRenewPermission=CASE when mstr.EmailRenewPermission IS NULL OR (dups.EmailRenewPermission IS NOT NULL AND ISNULL(dups.EmailRenewPermission,0) = 0) THEN dups.EmailRenewPermission ELSE mstr.EmailRenewPermission END ,
						mstr.TextPermission=CASE when mstr.TextPermission IS NULL OR (dups.TextPermission IS NOT NULL AND ISNULL(dups.TextPermission,0) = 0) THEN dups.TextPermission ELSE mstr.TextPermission END ,

						mstr.SubGenSubscriberID=CASE when ISNULL(dups.SubGenSubscriberID,0) <> 0 AND (ISNULL(mstr.SubGenSubscriberID,0) = 0 OR dups.Qdate > mstr.Qdate) THEN dups.SubGenSubscriberID ELSE mstr.SubGenSubscriberID END ,
						mstr.SubGenSubscriptionID=CASE when ISNULL(dups.SubGenSubscriptionID,0) <> 0 AND (ISNULL(mstr.SubGenSubscriptionID,0) = 0 OR dups.Qdate > mstr.Qdate) THEN dups.SubGenSubscriptionID ELSE mstr.SubGenSubscriptionID END ,
						mstr.SubGenPublicationID=CASE when ISNULL(dups.SubGenPublicationID,0) <> 0 AND (ISNULL(mstr.SubGenPublicationID,0) = 0 OR dups.Qdate > mstr.Qdate) THEN dups.SubGenPublicationID ELSE mstr.SubGenPublicationID END ,
						mstr.SubGenMailingAddressId=CASE when ISNULL(dups.SubGenMailingAddressId,0) <> 0 AND (ISNULL(mstr.SubGenMailingAddressId,0) = 0 OR dups.Qdate > mstr.Qdate) THEN dups.SubGenMailingAddressId ELSE mstr.SubGenMailingAddressId END ,
						mstr.SubGenBillingAddressId=CASE when ISNULL(dups.SubGenBillingAddressId,0) <> 0 AND (ISNULL(mstr.SubGenBillingAddressId,0) = 0 OR dups.Qdate > mstr.Qdate) THEN dups.SubGenBillingAddressId ELSE mstr.SubGenBillingAddressId END ,
						mstr.IssuesLeft=CASE when ISNULL(dups.IssuesLeft,0) <> 0 AND (ISNULL(mstr.IssuesLeft,0) = 0 OR dups.Qdate > mstr.Qdate) THEN dups.IssuesLeft ELSE mstr.IssuesLeft END ,
						mstr.UnearnedReveue=CASE when ISNULL(dups.UnearnedReveue,0) <> 0 AND (ISNULL(mstr.UnearnedReveue,0) = 0 OR dups.Qdate > mstr.Qdate) THEN dups.UnearnedReveue ELSE mstr.UnearnedReveue END ,
						mstr.SubGenIsLead=CASE when ISNULL(dups.SubGenIsLead,0) <> 0 AND (ISNULL(mstr.SubGenIsLead,0) = 0 OR dups.Qdate > mstr.Qdate) THEN dups.SubGenIsLead ELSE mstr.SubGenIsLead END ,
						mstr.SubGenRenewalCode=CASE when ISNULL(dups.SubGenRenewalCode,'') <> '' AND (ISNULL(mstr.SubGenRenewalCode,'') = '' OR dups.Qdate > mstr.Qdate) THEN dups.SubGenRenewalCode ELSE mstr.SubGenRenewalCode END ,
						mstr.SubGenSubscriptionRenewDate=CASE when ISNULL(dups.SubGenSubscriptionRenewDate,'1/1/1900') <> '1/1/1900' AND (ISNULL(mstr.SubGenSubscriptionRenewDate,'1/1/1900') = '1/1/1900' OR dups.Qdate > mstr.Qdate) THEN dups.SubGenSubscriptionRenewDate ELSE mstr.SubGenSubscriptionRenewDate END ,
						mstr.SubGenSubscriptionExpireDate=CASE when ISNULL(dups.SubGenSubscriptionExpireDate,'1/1/1900') <> '1/1/1900' AND (ISNULL(mstr.SubGenSubscriptionExpireDate,'1/1/1900') = '1/1/1900' OR dups.Qdate > mstr.Qdate) THEN dups.SubGenSubscriptionExpireDate ELSE mstr.SubGenSubscriptionExpireDate END ,
						mstr.SubGenSubscriptionLastQualifiedDate=CASE when ISNULL(dups.SubGenSubscriptionLastQualifiedDate,'1/1/1900') <> '1/1/1900' AND (ISNULL(mstr.SubGenSubscriptionLastQualifiedDate,'1/1/1900') = '1/1/1900' OR dups.Qdate > mstr.Qdate) THEN dups.SubGenSubscriptionLastQualifiedDate ELSE mstr.SubGenSubscriptionLastQualifiedDate END ,
						mstr.ConditionApplied=CASE when ISNULL(dups.ConditionApplied,0) <> 0 AND (ISNULL(mstr.ConditionApplied,0) = 0 OR dups.Qdate > mstr.Qdate) THEN dups.ConditionApplied ELSE mstr.ConditionApplied END
					from #IgrpDupList I
					join #Subscriberfinal mstr on mstr.SFRecordIdentifier = I.SFRecordIdentifier
					join #Subscriberfinal dups on dups.SFRecordIdentifier = @SFRecordIdentifier
					where I.Igrp_no = @IGrpID and I.PartitionRow = 1


					DECLARE c_DupDemos CURSOR LOCAL FORWARD_ONLY STATIC READ_ONLY FOR select MAFField from SubscriberDemographicFinal where SFRecordIdentifier = @SFRecordIdentifier
					OPEN c_DupDemos  

					FETCH NEXT from c_DupDemos INTO @MAFField
					WHILE @@FETCH_STATUS = 0  
					BEGIN  
						-- BEGIN ROLLUP SUBSCRIBER DEMOGRAPHICS
						-- Rollup duplicate demographics if they do not exist at master level.

-- per TFS ticket, we cannot rollup multiple demographic data for CIRC products, but we need to rollup all demographic NonCirc data
						IF @IsCirc = 0 OR NOT EXISTS (select top 1 1 
											FROM #IgrpDupList I 
											join #Subscriberfinal sfmstr on sfmstr.SFRecordIdentifier = I.SFRecordIdentifier 
											join SubscriberDemographicFinal sdfmstr on sdfmstr.SFRecordIdentifier = sfmstr.SFRecordIdentifier 
											where I.Igrp_no = @IgrpID and I.PartitionRow = 1 and sdfmstr.MAFField = @MAFField)
						BEGIN
							-- SET all duplicate SFRecordIdentifier to that of the Master record
							--Select 'Demographic Update', * 
							UPDATE sdfdup set sdfdup.SFRecordIdentifier = I.SFRecordIdentifier
								FROM #IgrpDupList I2 
								join #Subscriberfinal sfdup on sfdup.SFRecordIdentifier = I2.SFRecordIdentifier 
								join SubscriberDemographicFinal sdfdup on sdfdup.SFRecordIdentifier = sfdup.SFRecordIdentifier   -- not needed
								join #IgrpDupList I on I.Igrp_no = I2.Igrp_no AND I.PartitionRow = 1
							where I2.Igrp_no = @IgrpID and I2.PartitionRow > 1 and sdfdup.MAFField = @MAFField
						END				

						-- END ROLLUP SUBSCRIBER DEMOGRAPHICS

						FETCH NEXT from c_DupDemos INTO @MAFField
					END
					CLOSE c_DupDemos  
					DEALLOCATE c_DupDemos

					FETCH NEXT from c_DupRecs INTO @SFRecordIdentifier
				END
				CLOSE c_DupRecs  
				DEALLOCATE c_DupRecs
	 
			FETCH NEXT FROM c_IgrpIDs INTO @IgrpID
			END

			CLOSE c_IgrpIDs  
			DEALLOCATE c_IgrpIDs

PRINT ('END FULL ROLLUP / '  + CONVERT(VARCHAR(20), GETDATE(), 114))


		END 

		update sf set sf.igrp_rank = CASE WHEN IsNull(I.PartitionRow, 1) = 1 THEN 'M' ELSE 'S' END		
		from #Subscriberfinal sf left join #IgrpDupList I on I.SFRecordIdentifier = sf.SFRecordIdentifier

--select Igrp_rank, count(*) from #SubscriberFinal group by Igrp_rank
 
        --END FULL ROLLUP
        -------------------------------------------------------------------------------------------------------------
        --BEGIN REMOVE INTERNAL DUPLICATES

PRINT ('BEGIN REMOVE INTERNAL DUPLICATES / '  + CONVERT(VARCHAR(20), GETDATE(), 114))

		-- Insert duplicate records into SubscriberArchive	

		INSERT INTO SubscriberArchive
		(
			 SFRecordIdentifier,SourceFileID,PubCode,Sequence,FName,LName,Title,Company,Address,MailStop,City,State,Zip,Plus4,ForZip,County,Country,CountryID,Phone,PhoneExists,
			 Fax,FaxExists,Email,EmailExists,CategoryID,TransactionID,TransactionDate,QDate,QSourceID,RegCode,Verified,SubSrc,OrigsSrc,Par3C,Source,
			 Priority,IGrp_No,IGrp_Cnt,CGrp_No,CGrp_Cnt,StatList,Sic,SicCode,Gender,IGrp_Rank,CGrp_Rank,Address3,Home_Work_Address,Demo7,Mobile,Latitude,Longitude,
			 EmailStatusID,SARecordIdentifier,DateCreated,DateUPDATEd,CreatedByUserID,UPDATEdByUserID,IsMailable,ProcessCode,ImportRowNumber,MailPermission,FaxPermission,PhonePermission,
			 OtherProductsPermission,ThirdPartyPermission,EmailRenewPermission,TextPermission
		)  
		SELECT 
			 sft.SFRecordIdentifier,sft.SourceFileID,sft.PubCode,sft.Sequence,sft.FName,sft.LName,sft.Title,sft.Company,sft.Address,sft.MailStop,sft.City,sft.State,sft.Zip,sft.Plus4,
			 sft.ForZip,sft.County,sft.Country,sft.CountryID,sft.Phone,(CASE WHEN ISNULL(sft.Phone,'')!='' THEN 1 ELSE 0 END),sft.Fax,
			 (CASE WHEN ISNULL(sft.Fax,'')!='' THEN 1 ELSE 0 END),sft.Email,(CASE WHEN ISNULL(sft.Email,'')!='' THEN 1 ELSE 0 END),sft.CategoryID,sft.TransactionID,sft.TransactionDate,
			 sft.QDate,sft.QSourceID,sft.RegCode,sft.Verified,sft.SubSrc,sft.OrigsSrc,sft.Par3C,sft.Source,sft.Priority,sft.IGrp_No,sft.IGrp_Cnt,sft.CGrp_No,sft.CGrp_Cnt,sft.StatList,
			 sft.Sic,sft.SicCode,sft.Gender,sft.IGrp_Rank,sft.CGrp_Rank,sft.Address3,sft.Home_Work_Address,sft.Demo7,sft.Mobile,sft.Latitude,sft.Longitude,
			 sft.EmailStatusID,NEWID(),sft.DateCreated,sft.DateUPDATEd,sft.CreatedByUserID,sft.UPDATEdByUserID,sft.IsMailable,sft.ProcessCode,sft.ImportRowNumber,sft.MailPermission,
			 sft.FaxPermission,sft.PhonePermission,sft.OtherProductsPermission,sft.ThirdPartyPermission,sft.EmailRenewPermission,sft.TextPermission
		FROM 
			#SubscriberFinal sft With(NoLock)
			JOIN #IGrpDupList I ON I.SFRecordIdentifier = sft.SFRecordIdentifier
		WHERE I.PartitionRow > 1


PRINT ('INSERT TO SUBSCRIBER ARCHIVE / '  + Convert(VARCHAR,@@ROWCOUNT) + ' / ' + CONVERT(VARCHAR(20), GETDATE(), 114))	

		Insert into SubscriberDemographicArchive (PubID,SARecordIdentifier,MAFField,Value,NotExists,DateCreated,DateUpdated,CreatedByUserID,UpdatedByUserID,DemographicUpdateCodeId,IsAdhoc,ResponseOther)
		Select sdf.PubID,sa.SARecordIdentifier,sdf.MAFField,sdf.Value,sdf.NotExists,sdf.DateCreated,sdf.DateUpdated,sdf.CreatedByUserID,sdf.UpdatedByUserID,sdf.DemographicUpdateCodeId,sdf.IsAdhoc,sdf.ResponseOther 
			from SubscriberDemographicFinal sdf WITH(NOLOCK)
			JOIN SubscriberArchive sa WITH(NOLOCK) on sdf.SFRecordIdentifier = sa.SFRecordIdentifier
			JOIN #IGrpDupList I ON I.SFRecordIdentifier = sa.SFRecordIdentifier
		WHERE I.PartitionRow > 1

PRINT ('INSERT TO SUBSCRIBER DEMO ARCHIVE / '  + Convert(VARCHAR,@@ROWCOUNT) + ' / ' + CONVERT(VARCHAR(20), GETDATE(), 114))	

		DELETE sdf 
--select *
		FROM 
			SubscriberDemographicFinal sdf 
			JOIN #IGrpDupList I ON I.SFRecordIdentifier = sdf.SFRecordIdentifier
		WHERE I.PartitionRow > 1

PRINT ('DELETE FROM SUBSCRIBER DEMO ARCHIVE / '  + Convert(VARCHAR,@@ROWCOUNT) + ' / ' + CONVERT(VARCHAR(20), GETDATE(), 114))	
		
		DELETE sf 
--select *		
		FROM 
			SubscriberFinal sf 
			JOIN #IGrpDupList I ON I.SFRecordIdentifier = sf.SFRecordIdentifier
		WHERE I.PartitionRow > 1

PRINT ('DELETE FROM SUBSCRIBER FINAL / '  + Convert(VARCHAR,@@ROWCOUNT) + ' / ' + CONVERT(VARCHAR(20), GETDATE(), 114))	

		-- REMOVE records from Temp SubscriberFinal Table.
		DELETE sft
		FROM 
			#SubscriberFinal sft
			JOIN #IGrpDupList I ON I.SFRecordIdentifier = sft.SFRecordIdentifier
		WHERE I.PartitionRow > 1

PRINT ('DELETE FROM SUBSCRIBER TEMP FINAL / '  + Convert(VARCHAR,@@ROWCOUNT) + ' / ' + CONVERT(VARCHAR(20), GETDATE(), 114))	

--select sft.Igrp_no, sf.Igrp_no, sft.Igrp_rank, sft.* FROM #SubscriberFinal sft join SubscriberFinal sf on sf.SubscriberFinalID = sft.SubscriberFinalID order by sft.FName, sft.LName, sft.Email, sft.PubCode, sft.IGrp_No

		-- UPDATE IGRP_NO in Real Subscriber Final Table from the Temp Subscriber Final.
		UPDATE sf
		SET sf.Igrp_no = sft.Igrp_no, sf.IGrp_Rank = sft.IGrp_Rank
		FROM #SubscriberFinal sft
		join SubscriberFinal sf on sf.SubscriberFinalID = sft.SubscriberFinalID

		--
		-- Drop temp tables as they are no longer needed
		--
		DROP TABLE #MatchGroups
		DROP TABLE #SubscriberFinal
		drop table #IgrpDupList
				
		--END REMOVE INTERNAL DUPLICATES
		
PRINT ('END / '  + Convert(VARCHAR,@@ROWCOUNT) + ' / ' + CONVERT(VARCHAR(20), GETDATE(), 114))	
END

GO

