CREATE PROCEDURE [dbo].[job_IssueComp_IssueCompDetail_Add]
       @ProcessCode varchar(50),
       @PublicationID int,
       @SourceFileId int
AS
BEGIN

	SET NOCOUNT ON
 
	DECLARE @distinctComps TABLE (CompName VARCHAR(200), CompCount int, IssueID int)       
	DECLARE @PubCode varchar(100) = (Select PubCode FROM Pubs where PubID = @PublicationID)       

	Create table #tbl1 (SubscriberFinalID int, Igrp_No uniqueidentifier, Igrp_Rank varchar(2), PubID int, SubscriptionID int)

	CREATE CLUSTERED INDEX IDX_C_tbl1_IncomingDataID ON #tbl1(SubscriberFinalID)
    
	CREATE INDEX IDX_tbl1_Igrp_No ON #tbl1(Igrp_No)
	CREATE INDEX IDX_tbl1_SubscriptionID ON #tbl1(SubscriptionID)       
	CREATE INDEX IDX_tbl1_Igrp_Rank ON #tbl1(Igrp_Rank)


	/* Get SubscriberFinal search by IGrp_No	(List should include Sequence Matches on Name where IGrp was updated and other existing records) */
	Select SF.SFRecordIdentifier, PS.PubSubscriptionID, S.SubscriptionID
	into #tmpPubSubMatches
	from SubscriberFinal SF
		left join Subscriptions S on SF.IGrp_No = S.IGrp_No
		left join Pubs P on SF.PubCode = P.PubCode
		left join PubSubscriptions PS on S.SubscriptionID = PS.SubscriptionID and P.PubID = PS.PubID	
	where SF.ProcessCode = @ProcessCode and PS.PubSubscriptionID is not null


	CREATE CLUSTERED INDEX IDX_tmpPubSubMatches_PubSubscriptionID_TM ON #tmpPubSubMatches(PubSubscriptionID)   
	CREATE INDEX IDX_tmpPubSubMatches_SFRecordIdentifier_TM ON #tmpPubSubMatches(SFRecordIdentifier)


	/* Rest of SubscriberFinal these will be the records without a Sub or PubSub entry from above */
	Insert into #tmpPubSubMatches
	Select SF.SFRecordIdentifier, isnull(PS.PubSubscriptionID, 0), isnull(S.SubscriptionID, 0)
	from SubscriberFinal SF
		left join Subscriptions S on SF.IGrp_No = S.IGrp_No
		left join Pubs P on SF.PubCode = P.PubCode
		left join PubSubscriptions PS on S.SubscriptionID = PS.SubscriptionID and P.PubID = PS.PubID	
	where SF.ProcessCode = @ProcessCode and SF.SubscriberFinalID not in (Select SubscriberFinalID from #tmpPubSubMatches)


	/* Set Copies to database record */
	Update sf
	set sf.Copies = (CASE WHEN ISNULL(sf.Copies,0) < 1 THEN 1 ELSE sf.Copies END),		
		DateUpdated = GETDATE(),
		UpdatedByUserID = 1
	from SubscriberFinal sf With(NoLock)
		left join #tmpPubSubMatches t on sf.SFRecordIdentifier = t.SFRecordIdentifier		
	where sf.ProcessCode = @ProcessCode


	insert into #tbl1 (SubscriberFinalID, IGRP_NO, Igrp_Rank, PubID)
	select MAX(sf.SubscriberFinalID), sf.IGrp_No, min(sf.IGrp_Rank), p.PubID
	from 
			(select sf1.IGrp_No, min(sf1.IGrp_Rank) as IGrp_Rank, PubCode 
			from SubscriberFinal sf1 With(NoLock) 
			where sf1.ProcessCode = @ProcessCode and Ignore = 0 and isUpdatedinLIVE = 0  
			group by sf1.IGrp_No, PubCode) x
				join SubscriberFinal sf on sf.IGrp_No = x.IGrp_No and sf.IGrp_Rank = x.IGrp_Rank and sf.PubCode = x.PubCode 
				JOIN Pubs p WITH (nolock) ON p.PubCode = sf.PubCode    
			where Ignore = 0 and isUpdatedinLIVE = 0 and sf.ProcessCode = @ProcessCode 
	GROUP BY sf.IGrp_No, p.PubID
	order by 2,3


	/* GET LIST OF DISTINCT COMPS AND COUNT OF ALL TOTAL WITH WHO IS IN COMP */
	INSERT INTO @distinctComps (CompName, CompCount)
		Select PubCode,COUNT(PubCode)--,SF.SFRecordIdentifier
		FROM SubscriberFinal SF join #tbl1 t on SF.SubscriberFinalID = t.SubscriberFinalID
		WHERE SF.ProcessCode = @ProcessCode 
			and PubCode = @PubCode
		GROUP BY SF.PubCode  

	UPDATE IC
		set IsActive = 'false'
		from IssueComp IC where IC.IssueId = 
		(SELECT ISNULL(IssueID,0) FROM Issue I join Pubs P ON I.PublicationId = P.PubID WHERE IsComplete = 0 and P.PubID = @PublicationID)


	/* APPEND THE ISSUEID TO THE LIST */
	UPDATE DC
		SET IssueID = (SELECT ISNULL(IssueID,0) FROM Issue I join Pubs P ON I.PublicationId = P.PubID WHERE IsComplete = 0 and P.PubCode = DC.CompName)
		FROM @distinctComps DC 
			JOIN SubscriberFinal SF ON DC.CompName = SF.PubCode--DC.SFRecordIdentifier = SF.SFRecordIdentifier
		WHERE SF.ProcessCode = @ProcessCode 


	/* ANY ISSUEID OF ZERO WILL BE CONSIDERED ERRORS AND SHOULD BE MESSAGED */
	INSERT INTO IssueCompError (CompName, SFRecordIdentifier, ProcessCode, DateCreated, CreatedByUserID)
		SELECT CompName, SFRecordIdentifier, @ProcessCode, GETDATE(), 1 
		FROM @distinctComps DC 
			join SubscriberFinal SF on DC.CompName = SF.PubCode 
		where IssueID = 0 and SF.ProcessCode = @ProcessCode


	/* INSERT ISSUEID'S THAT AREN'T ZERO */
	INSERT INTO IssueComp (IssueId, ImportedDate, IssueCompCount, DateCreated, CreatedByUserID, IsActive)
		SELECT 
				IssueID,
				GETDATE(),
				CompCount,
				GETDATE(),
				1,
				'true'
		FROM @distinctComps WHERE IssueID > 0                                


	Delete from IssueCompDetail where IssueCompID in (Select IssueCompID from IssueComp with(nolock) where IsActive = 'false')


	--Change Sequence to be zero #25733 on 07172015 by Jason Meier
	/* INSERT RECORD INTO DETAIL FOR THE SUBSCRIBER */
	INSERT INTO IssueCompDetail (IssueCompID,[PubID],[Demo7],[QualificationDate],[PubQSourceID],[PubCategoryID],[PubTransactionID],[EmailStatusID],[StatusUpdatedDate],[StatusUpdatedReason],
								[Email],[DateCreated],[DateUpdated],[CreatedByUserID],[UpdatedByUserID],[IsComp],[SubscriptionStatusID],[AddRemoveID],[Copies],[GraceIssues],[IMBSEQ],[IsActive],[IsPaid],[IsSubscribed],
								[MemberGroup],[OnBehalfOf],[OrigsSrc],[Par3CID],[SequenceID],[Status],[SubscriberSourceCode],[SubSrcID],[Verify],ExternalKeyID,FirstName,LastName,Company,Title,Occupation,AddressTypeID,Address1,
								Address2,Address3,City,RegionCode,RegionID,ZipCode,Plus4,CarrierRoute,County,Country,CountryID,Latitude,Longitude,IsAddressValidated,AddressValidationDate,AddressValidationSource,
								AddressValidationMessage,Phone,Fax,Mobile,Website,Birthdate,Age,Income,Gender,PhoneExt,AddressTypeCodeId,AddressLastUpdatedDate,AddressUpdatedSourceTypeCodeId, 
								IsInActiveWaveMailing, WaveMailingID, IGrp_No, SFRecordIdentifier)
		SELECT (SELECT ISNULL(IssueCompID,0) FROM IssueComp WHERE IssueID = DC.IssueID and IsActive = 'true'),
				(SELECT PubID FROM Pubs WHERE PubCode = SF.PubCode),
				SF.Demo7,SF.QDate,SF.QSourceID,SF.CategoryID,SF.TransactionID,SF.EmailStatusID,NULL,NULL,
				SF.Email,SF.DateCreated,SF.DateUpdated,SF.CreatedByUserID,SF.UpdatedByUserID,1,NULL,0,Copies,NULL,'0',SF.IsActive,0,0,
				NULL,NULL,SF.OrigsSrc,SF.Par3C,0,NULL,SF.SubSrc,NULL,SF.Verified,NULL,SF.FNAME,SF.LName,SF.Company,SF.Title,NULL,0,SF.Address,
				SF.MailStop,SF.Address3,SF.City,SF.State,
				(SELECT RegionID FROM UAD_Lookup..[Region] WHERE RegionCode = SF.State),
				SF.Zip,SF.Plus4,NULL,SF.County,SF.Country,SF.CountryID,SF.Latitude,SF.Longitude,NULL,NULL,NULL,
				NULL,SF.Phone,SF.Fax,SF.Mobile,Website,NULL,NULL,NULL,NULL,NULL,NULL,NULL,NULL, 
				NULL, NULL, SF.IGrp_No, SF.SFRecordIdentifier					   						   						   						   
		FROM SubscriberFinal SF with(nolock)                  
			JOIN @distinctComps DC ON SF.PubCode = DC.CompName --.SFRecordIdentifier = DC.SFRecordIdentifier					 
		WHERE SF.ProcessCode = @ProcessCode

	drop table #tbl1
END