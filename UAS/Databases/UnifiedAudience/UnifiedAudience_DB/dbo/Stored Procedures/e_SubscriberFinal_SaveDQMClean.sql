CREATE PROCEDURE [dbo].[e_SubscriberFinal_SaveDQMClean]
@ProcessCode varchar(50),
@FileType varchar(50) = ''
AS
BEGIN

	SET NOCOUNT ON

	declare @dt datetime = getdate()

	exec e_SubscriberFinal_DisableIndexes
	exec e_SubscriberDemographicFinal_DisableIndexes

	DECLARE @TranIDFromValue int = (Select TransactionCodeValue From UAD_Lookup..TransactionCode where TransactionCodeValue = 10)
	DECLARE @CatIDFromValue int = (Select CategoryCodeValue From UAD_Lookup..CategoryCode where CategoryCodeValue = 10)

	INSERT INTO SubscriberFinal 
	(
		 STRecordIdentifier,SourceFileID,PubCode,Sequence,FName,LName,Title,Company,Address,MailStop,City,State,Zip,Plus4,ForZip,County,Country,CountryID,Phone,
		 Fax,Email,CategoryID,TransactionID,TransactionDate,QDate,QSourceID,RegCode,Verified,SubSrc,OrigsSrc,Par3C,Source,
		 Priority,Sic,SicCode,Gender,Address3,Home_Work_Address,Demo7,Mobile,Latitude,Longitude,IsLatLonValid,LatLonMsg,EmailStatusID,SFRecordIdentifier,DateCreated,
		 DateUpdated,CreatedByUserID,UpdatedByUserID,ProcessCode,ImportRowNumber,IsActive,ExternalKeyId,AccountNumber,EmailID,
		 Copies,GraceIssues,IsComp,IsPaid,IsSubscribed,Occupation,SubscriptionStatusID,SubsrcID,Website,
		 Ignore,IsDQMProcessFinished,IsUpdatedInLive,MailPermission,FaxPermission,PhonePermission,OtherProductsPermission,ThirdPartyPermission,
		 EmailRenewPermission,TextPermission,SubGenSubscriberID,SubGenSubscriptionID,SubGenPublicationID,SubGenMailingAddressId,SubGenBillingAddressId,IssuesLeft,UnearnedReveue,
		 SubGenIsLead,SubGenRenewalCode,SubGenSubscriptionRenewDate,SubGenSubscriptionExpireDate,SubGenSubscriptionLastQualifiedDate
	)  
	SELECT 
		 st.STRecordIdentifier,st.SourceFileID,st.PubCode,st.Sequence,st.FName,st.LName,st.Title,st.Company,st.Address,st.MailStop,st.City,st.State,st.Zip,st.Plus4,st.ForZip,st.County,st.Country,st.CountryID,st.Phone,
		 st.Fax,st.Email,st.CategoryID,st.TransactionID,st.TransactionDate,st.QDate,st.QSourceID,st.RegCode,st.Verified,st.SubSrc,st.OrigsSrc,st.Par3C,st.Source,
		 st.Priority,st.Sic,st.SicCode,st.Gender,st.Address3,st.Home_Work_Address,st.Demo7,st.Mobile,st.Latitude,st.Longitude,st.IsLatLonValid,
		 st.LatLonMsg,st.EmailStatusID, NEWID() AS SFRecordIdentifier, st.DateCreated,st.DateUpdated,st.CreatedByUserID,st.UpdatedByUserID,st.ProcessCode,st.ImportRowNumber,st.IsActive,
		 st.ExternalKeyId,st.AccountNumber,st.EmailID,
		 st.Copies,st.GraceIssues,st.IsComp,st.IsPaid,st.IsSubscribed,st.Occupation,st.SubscriptionStatusID,st.SubsrcID,st.Website,
		 'false','false','false',st.MailPermission,st.FaxPermission,st.PhonePermission,st.OtherProductsPermission,st.ThirdPartyPermission,
		 st.EmailRenewPermission,st.TextPermission,st.SubGenSubscriberID,st.SubGenSubscriptionID,st.SubGenPublicationID,st.SubGenMailingAddressId,st.SubGenBillingAddressId,st.IssuesLeft,st.UnearnedReveue,
		 st.SubGenIsLead,st.SubGenRenewalCode,st.SubGenSubscriptionRenewDate,st.SubGenSubscriptionExpireDate,st.SubGenSubscriptionLastQualifiedDate
	FROM SubscriberTransformed st With(NoLock)
		--LEFT OUTER JOIN  SubscriberFinal sf with(NoLock) on st.STRecordIdentifier = sf.STRecordIdentifier
	WHERE st.ProcessCode = @ProcessCode 
	--AND sf.SFRecordIdentifier is null

	CREATE TABLE #sdt (
	[PubID] [int] NULL,
	[STRecordIdentifier] [uniqueidentifier] NOT NULL,
	[MAFField] [varchar](255) NULL,
	[Value] [varchar](max) NULL,
	[ResponseOther] [varchar](256) NULL,
	[DateCreated] datetime NULL,
	[IsDemoDate] bit
	)

	CREATE INDEX idx_sdt_STRecordIdentifier ON #sdt(STRecordIdentifier)
	
	insert into #sdt
	select sdt.PubID,sdt.STRecordIdentifier,sdt.MAFField,sdt.Value,sdt.ResponseOther, sdt.DateCreated, sdt.IsDemoDate
	from  SubscriberDemographicTransformed sdt with (NOLOCK) join 
		  SubscriberTransformed st with (NOLOCK) on sdt.STRecordIdentifier = st.STRecordIdentifier
	where
		ProcessCode = @ProcessCode  and sdt.NotExists = 0

	--Insert non-duplicate records into subscriberDemographicFinal table
	INSERT INTO SubscriberDemographicFinal (PubID,SFRecordIdentifier,MAFField,Value,NotExists,DateCreated,DateUpdated,CreatedByUserID,UpdatedByUserID,ResponseOther,IsDemoDate)
	SELECT sdt.PubID,sf.SFRecordIdentifier,sdt.MAFField,sdt.Value,0, sdt.DateCreated,@dt,1,1,sdt.ResponseOther,sdt.IsDemoDate
	FROM SubscriberFinal sf  with(nolock)
		JOIN #sdt sdt with(nolock) ON sdt.STRecordIdentifier = sf.STRecordIdentifier
	WHERE sf.ProcessCode = @ProcessCode 

	exec e_SubscriberFinal_EnableIndexes
	exec e_SubscriberDemographicFinal_EnableIndexes

	if (@FileType = 'Field_Update' or @FileType = 'QuickFill')
		BEGIN
			--Update invalid Trans to -1, later sproc will maintain old Tran
			UPDATE SubscriberFinal
			SET TransactionID = -1
			WHERE ProcessCode = @ProcessCode 
					AND (TransactionID NOT IN (SELECT TransactionCodeValue FROM UAD_Lookup..[TransactionCode] with(nolock)))
	
			--Update invalid Cat to -1, later sproc will maintain old Cat
			UPDATE SubscriberFinal
			SET CategoryID = -1
			WHERE ProcessCode = @ProcessCode 
					AND (CategoryID NOT IN (SELECT CategoryCodeValue FROM UAD_Lookup..CategoryCode with(nolock)))

			--job_FieldUpdate sproc will assign the correct Value based on the ID
		END
	ELSE
		BEGIN
			UPDATE SubscriberFinal
			SET TransactionID = @TranIDFromValue
			WHERE ProcessCode = @ProcessCode 
					AND (TransactionID = 0 OR TransactionID NOT IN (SELECT TransactionCodeValue FROM UAD_Lookup..[TransactionCode] with(nolock)))
	
			UPDATE SubscriberFinal
			SET CategoryID = @CatIDFromValue
			WHERE ProcessCode = @ProcessCode 
					AND (CategoryID = 0 OR CategoryID NOT IN (SELECT CategoryCodeValue FROM UAD_Lookup..CategoryCode with(nolock)))

			UPDATE SubscriberFinal
			set CategoryID = a.CategoryCodeID,
				TransactionID = a.TransactionCodeID
			FROM SubscriberFinal sf 
				join uad_lookup..vw_Action a on a.CategoryCodeValue = sf.CategoryID and a.TransactionCodeValue = sf.TransactionID and actiontypeid = 2 --and a.isFreeCategoryCodeType = a.isFreeTransactionCodeType
			WHERE ProcessCode = @ProcessCode 
		END

	drop table #sdt

END


GO