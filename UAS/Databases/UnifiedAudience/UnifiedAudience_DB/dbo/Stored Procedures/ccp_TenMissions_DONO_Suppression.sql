create procedure [dbo].[ccp_TenMissions_DONO_Suppression]
@SourceFileID int,
@ProcessCode varchar(50),
@ClientId int = 1,
@Xml xml
AS
BEGIN

	set nocount on
	--Create temp table for xml
	CREATE TABLE #tmp_SuppFile
		(
			FName varchar(100),
            LName varchar(100),
            Company varchar(255),
            Address varchar(255),
            City varchar(50),
            State varchar(50),
            Zip varchar(50),
            Phone varchar(100),
            Fax varchar(100),
            Email varchar(100)
        )
    --CREATE INDEX idx_SuppFile ON #tmp_SuppFile(FName,LName,Company,[Address],City,State,Zip,Phone,Fax,Email)

	
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;
	
	DECLARE @docHandle int
	EXEC sp_xml_preparedocument @docHandle OUTPUT, @Xml
	
	insert into #tmp_SuppFile 
	(
		 FName,LName,Company,Address,City,State,Zip,Phone,Fax,Email
	)  
	
	SELECT 
		FName,LName,Company,Address,City,State,Zip,Phone,Fax,Email
	FROM OPENXML(@docHandle, N'/recordset/R1') 
	WITH   
	(  
		FName varchar(100) 'FNAME',
        LName varchar(100) 'LNAME',
        Company varchar(255) 'COMPANY',
        Address varchar(255) 'ADDRESS',
        City varchar(50) 'CITY',
        State varchar(50) 'STATE',
        Zip varchar(50) 'ZIP',
        Phone varchar(100) 'PHONE',
        Fax varchar(100) 'FAX',
        Email varchar(100) 'EMAIL'
	)  


	EXEC sp_xml_removedocument @docHandle   

	declare @AddrSupp int
	declare @AddrIgrpNo int
	declare @CompanySupp int
	declare @CompanyIgrpNo int
	declare @EmailSupp int
	declare @EmailIgrpNo int
	declare @PhoneSupp int
	declare @PhoneIgrpNo int
	
	--CREATE TABLE #tmp_IgrpNoSupp(IGRP_NO uniqueidentifier,SuppFileName varchar(100))

	--
	-- Match on FName,LName,Address,Zip
	--
	INSERT INTO Suppressed(STRecordIdentifier,SFRecordIdentifier,Source,IsSuppressed,IsEmailMatch,IsPhoneMatch,IsAddressMatch,IsCompanyMatch,DateCreated,DateUpdated,CreatedByUserID)

	SELECT DISTINCT STRecordIdentifier,SFRecordIdentifier,'DONO',1,0,0,1,0,GETDATE(),null,1 
	FROM SubscriberFinal SF inner join #tmp_SuppFile ts on left(sf.FName,3) = left(ts.FName,3) and left(sf.LName,6) = left(ts.LName,6) and left(sf.Address,15) = left(ts.Address,15) and left(sf.Zip,5) = left(ts.Zip,5)
	WHERE ISNULL(sf.FNAME,'')!='' and ISNULL(sf.LNAME,'')!='' AND ISNULL(sf.Address,'')!='' AND ISNULL(sf.Zip,'')!='' AND SourceFileID = @SourceFileId AND ProcessCode = @ProcessCode
		AND SF.SFRecordIdentifier NOT IN (SELECT SFRecordIdentifier FROM Suppressed)
	
	SET @AddrSupp = @@ROWCOUNT

	INSERT INTO Suppressed(STRecordIdentifier,SFRecordIdentifier,Source,IsSuppressed,IsEmailMatch,IsPhoneMatch,IsAddressMatch,IsCompanyMatch,ProcessCode,DateCreated,DateUpdated,CreatedByUserID)
	
	SELECT DISTINCT STRecordIdentifier,SFRecordIdentifier,'DONO',1,0,0,1,0,SF.ProcessCode,GETDATE(),null,1 
	FROM SubscriberFinal SF INNER JOIN (SELECT IGRP_NO FROM SubscriberFinal WHERE SFRecordIdentifier in (SELECT SFRecordIdentifier FROM Suppressed) GROUP BY IGrp_No) as ins
			ON SF.IGrp_No = INS.IGrp_No
	WHERE SF.SourceFileID = @SourceFileId AND SF.ProcessCode = @ProcessCode AND SF.SFRecordIdentifier NOT IN (SELECT SFRecordIdentifier FROM Suppressed)
	
	SET @AddrIgrpNo = @@ROWCOUNT
	
	--DELETE FROM #tmp_IgrpNoSupp

	--
	-- Match on Fname,LName,Company
	--
	INSERT INTO Suppressed(STRecordIdentifier,SFRecordIdentifier,Source,IsSuppressed,IsEmailMatch,IsPhoneMatch,IsAddressMatch,IsCompanyMatch,ProcessCode,DateCreated,DateUpdated,CreatedByUserID)
	
	SELECT DISTINCT STRecordIdentifier,SFRecordIdentifier,'DONO',1,0,0,0,1,SF.ProcessCode,GETDATE(),null,1 
	FROM SubscriberFinal SF inner join #tmp_SuppFile ts on left(sf.FName,3) = left(ts.FName,3) and left(sf.LName,6) = left(ts.LName,6) and left(sf.Company,8) = left(ts.Company,8)
	WHERE ISNULL(sf.FNAME,'')!='' and ISNULL(sf.LNAME,'')!='' AND ISNULL(sf.Company,'')!=''AND SourceFileID = @SourceFileId AND ProcessCode = @ProcessCode
		AND SF.SFRecordIdentifier NOT IN (SELECT SFRecordIdentifier FROM Suppressed) 
	
	SET @CompanySupp = @@ROWCOUNT
	
	INSERT INTO Suppressed(STRecordIdentifier,SFRecordIdentifier,Source,IsSuppressed,IsEmailMatch,IsPhoneMatch,IsAddressMatch,IsCompanyMatch,ProcessCode,DateCreated,DateUpdated,CreatedByUserID)
	
	SELECT DISTINCT STRecordIdentifier,SFRecordIdentifier,'DONO',1,0,0,0,1,SF.ProcessCode,GETDATE(),null,1 
	FROM SubscriberFinal SF INNER JOIN (SELECT IGRP_NO FROM SubscriberFinal WHERE SFRecordIdentifier in (SELECT SFRecordIdentifier FROM Suppressed) GROUP BY IGrp_No) as ins
			ON SF.IGrp_No = INS.IGrp_No
	WHERE SF.SourceFileID = @SourceFileId AND SF.ProcessCode = @ProcessCode AND SF.SFRecordIdentifier NOT IN (SELECT SFRecordIdentifier FROM Suppressed)
	
	SET @CompanyIgrpNo = @@ROWCOUNT
	
	--DELETE FROM #tmp_IgrpNoSupp

	--
	-- Any email that matches, set Demo33 to false
	--
	INSERT INTO Suppressed(STRecordIdentifier,SFRecordIdentifier,Source,IsSuppressed,IsEmailMatch,IsPhoneMatch,IsAddressMatch,IsCompanyMatch,ProcessCode,DateCreated,DateUpdated,CreatedByUserID)
	
	SELECT DISTINCT STRecordIdentifier,SFRecordIdentifier,'DONO',1,1,0,0,0,SF.ProcessCode,GETDATE(),null,1 
	FROM SubscriberFinal SF inner join #tmp_SuppFile ts on sf.Email = ts.Email
	WHERE ISNULL(sf.Email,'')!='' AND SF.SourceFileID = @SourceFileId AND SF.ProcessCode = @ProcessCode 
		AND SF.SFRecordIdentifier NOT IN (SELECT SFRecordIdentifier FROM Suppressed)
	
	SET @EmailSupp = @@ROWCOUNT
	
	INSERT INTO Suppressed(STRecordIdentifier,SFRecordIdentifier,Source,IsSuppressed,IsEmailMatch,IsPhoneMatch,IsAddressMatch,IsCompanyMatch,ProcessCode,DateCreated,DateUpdated,CreatedByUserID)
	
	SELECT DISTINCT STRecordIdentifier,SFRecordIdentifier,'DONO',1,1,0,0,0,SF.ProcessCode,GETDATE(),null,1 
	FROM SubscriberFinal SF INNER JOIN (SELECT IGRP_NO FROM SubscriberFinal WHERE SFRecordIdentifier in (SELECT SFRecordIdentifier FROM Suppressed) GROUP BY IGrp_No) as ins
			ON SF.IGrp_No = INS.IGrp_No
	WHERE SF.SourceFileID = @SourceFileId AND SF.ProcessCode = @ProcessCode AND SF.SFRecordIdentifier NOT IN (SELECT SFRecordIdentifier FROM Suppressed)
	
	SET @EmailIgrpNo = @@ROWCOUNT
	
	--DELETE FROM #tmp_IgrpNoSupp

	--
	-- Any phone that matches, set Demo34 and Demo35 to false
	--
	INSERT INTO Suppressed(STRecordIdentifier,SFRecordIdentifier,Source,IsSuppressed,IsEmailMatch,IsPhoneMatch,IsAddressMatch,IsCompanyMatch,ProcessCode,DateCreated,DateUpdated,CreatedByUserID)
	
	SELECT DISTINCT STRecordIdentifier,SFRecordIdentifier,'DONO',1,0,1,0,0,SF.ProcessCode,GETDATE(),null,1 
	FROM SubscriberFinal SF inner join #tmp_SuppFile ts on replace(sf.Phone,'-','') = replace(ts.Phone,'-','')
	WHERE ISNULL(sf.Phone,'')!='' AND SF.SourceFileID = @SourceFileId AND SF.ProcessCode = @ProcessCode  AND SF.SFRecordIdentifier NOT IN (SELECT SFRecordIdentifier FROM Suppressed)
	
	SET @PhoneSupp = @@ROWCOUNT
	
	INSERT INTO Suppressed(STRecordIdentifier,SFRecordIdentifier,Source,IsSuppressed,IsEmailMatch,IsPhoneMatch,IsAddressMatch,IsCompanyMatch,ProcessCode,DateCreated,DateUpdated,CreatedByUserID)
	
	SELECT DISTINCT STRecordIdentifier,SFRecordIdentifier,'DONO',1,0,1,0,0,SF.ProcessCode,GETDATE(),null,1 
	FROM SubscriberFinal SF INNER JOIN (SELECT IGRP_NO FROM SubscriberFinal WHERE SFRecordIdentifier in (SELECT SFRecordIdentifier FROM Suppressed) GROUP BY IGrp_No) as ins
			ON SF.IGrp_No = INS.IGrp_No
	WHERE SF.SourceFileID = @SourceFileId AND SF.ProcessCode = @ProcessCode AND SF.SFRecordIdentifier NOT IN (SELECT SFRecordIdentifier FROM Suppressed)
	
	SET @PhoneIgrpNo = @@ROWCOUNT
	
	DROP TABLE #tmp_SuppFile
	--DROP TABLE #tmp_IgrpNoSupp
		
	-- Update subFinal
	UPDATE sf
	SET MailPermission = 'false', FaxPermission = 'false', PhonePermission = 'false', OtherProductsPermission = 'false', ThirdPartyPermission = 'false', EmailRenewPermission = 'false', TextPermission = 'false'
	FROM SubscriberFinal sf INNER JOIN Suppressed sp on sf.SFRecordIdentifier = sp.SFRecordIdentifier
	WHERE SF.SourceFileID = @SourceFileId AND SF.ProcessCode = @ProcessCode


	-- Total up affected rows
	DECLARE @SuppCount int
	SET @SuppCount = @AddrSupp + @CompanySupp + @EmailSupp + @PhoneSupp + @AddrIgrpNo + @CompanyIgrpNo + @EmailIgrpNo + @PhoneIgrpNo
	
	SELECT @SuppCount
END
