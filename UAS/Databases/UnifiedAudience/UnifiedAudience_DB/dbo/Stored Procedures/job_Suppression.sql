CREATE PROCEDURE [dbo].[job_Suppression]
	@ProcessCode varchar(50)
AS
BEGIN

	SET NOCOUNT ON

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
	SELECT DISTINCT STRecordIdentifier,SFRecordIdentifier,'',1,0,0,1,0,GETDATE(),null,1 
	FROM SubscriberFinal SF 
		inner join SuppressionFileDetail ts on left(sf.FName,3) = left(ts.FirstName,3) and left(sf.LName,6) = left(ts.LastName,6) and left(sf.Address,15) = left(ts.Address1,15) and left(sf.Zip,5) = left(ts.ZipCode,5)
	WHERE ISNULL(sf.FNAME,'')!='' and ISNULL(sf.LNAME,'')!='' AND ISNULL(sf.Address,'')!='' AND ISNULL(sf.Zip,'')!='' AND ProcessCode = @ProcessCode
		AND SF.SFRecordIdentifier NOT IN (SELECT SFRecordIdentifier FROM Suppressed)
	
	SET @AddrSupp = @@ROWCOUNT

	INSERT INTO Suppressed(STRecordIdentifier,SFRecordIdentifier,Source,IsSuppressed,IsEmailMatch,IsPhoneMatch,IsAddressMatch,IsCompanyMatch,ProcessCode,DateCreated,DateUpdated,CreatedByUserID)	
	SELECT DISTINCT STRecordIdentifier,SFRecordIdentifier,'',1,0,0,1,0,SF.ProcessCode,GETDATE(),null,1 
	FROM SubscriberFinal SF 
		INNER JOIN (SELECT IGRP_NO FROM SubscriberFinal WHERE SFRecordIdentifier in (SELECT SFRecordIdentifier FROM Suppressed) GROUP BY IGrp_No) as ins
			ON SF.IGrp_No = INS.IGrp_No
	WHERE SF.ProcessCode = @ProcessCode AND SF.SFRecordIdentifier NOT IN (SELECT SFRecordIdentifier FROM Suppressed)
	
	SET @AddrIgrpNo = @@ROWCOUNT
	
	--DELETE FROM #tmp_IgrpNoSupp

	--
	-- Match on Fname,LName,Company
	--
	INSERT INTO Suppressed(STRecordIdentifier,SFRecordIdentifier,Source,IsSuppressed,IsEmailMatch,IsPhoneMatch,IsAddressMatch,IsCompanyMatch,ProcessCode,DateCreated,DateUpdated,CreatedByUserID)	
	SELECT DISTINCT STRecordIdentifier,SFRecordIdentifier,'',1,0,0,0,1,SF.ProcessCode,GETDATE(),null,1 
	FROM SubscriberFinal SF
		inner join SuppressionFileDetail ts on left(sf.FName,3) = left(ts.FirstName,3) and left(sf.LName,6) = left(ts.LastName,6) and left(sf.Company,8) = left(ts.Company,8)
	WHERE ISNULL(sf.FNAME,'')!='' and ISNULL(sf.LNAME,'')!='' AND ISNULL(sf.Company,'')!='' AND ProcessCode = @ProcessCode
		AND SF.SFRecordIdentifier NOT IN (SELECT SFRecordIdentifier FROM Suppressed) 
	
	SET @CompanySupp = @@ROWCOUNT
	
	INSERT INTO Suppressed(STRecordIdentifier,SFRecordIdentifier,Source,IsSuppressed,IsEmailMatch,IsPhoneMatch,IsAddressMatch,IsCompanyMatch,ProcessCode,DateCreated,DateUpdated,CreatedByUserID)	
	SELECT DISTINCT STRecordIdentifier,SFRecordIdentifier,'',1,0,0,0,1,SF.ProcessCode,GETDATE(),null,1 
	FROM SubscriberFinal SF 
		INNER JOIN (SELECT IGRP_NO FROM SubscriberFinal WHERE SFRecordIdentifier in (SELECT SFRecordIdentifier FROM Suppressed) GROUP BY IGrp_No) as ins
			ON SF.IGrp_No = INS.IGrp_No
	WHERE SF.ProcessCode = @ProcessCode AND SF.SFRecordIdentifier NOT IN (SELECT SFRecordIdentifier FROM Suppressed)
	
	SET @CompanyIgrpNo = @@ROWCOUNT
	
	--DELETE FROM #tmp_IgrpNoSupp
	
	--
	-- Match on Fname,LName,email
	--
	INSERT INTO Suppressed(STRecordIdentifier,SFRecordIdentifier,Source,IsSuppressed,IsEmailMatch,IsPhoneMatch,IsAddressMatch,IsCompanyMatch,ProcessCode,DateCreated,DateUpdated,CreatedByUserID)	
	SELECT DISTINCT STRecordIdentifier,SFRecordIdentifier,'',1,1,0,0,0,SF.ProcessCode,GETDATE(),null,1 
	FROM SubscriberFinal SF 
		inner join SuppressionFileDetail ts on left(sf.FName,3) = left(ts.FirstName,3) and left(sf.LName,6) = left(ts.LastName,6) and sf.email = ts.Email
	WHERE ISNULL(sf.FNAME,'')!='' and ISNULL(sf.LNAME,'')!='' AND ISNULL(sf.email,'')!='' AND ProcessCode = @ProcessCode
		AND SF.SFRecordIdentifier NOT IN (SELECT SFRecordIdentifier FROM Suppressed) 
	
	SET @EmailSupp = @@ROWCOUNT
	
	INSERT INTO Suppressed(STRecordIdentifier,SFRecordIdentifier,Source,IsSuppressed,IsEmailMatch,IsPhoneMatch,IsAddressMatch,IsCompanyMatch,ProcessCode,DateCreated,DateUpdated,CreatedByUserID)
	SELECT DISTINCT STRecordIdentifier,SFRecordIdentifier,'',1,1,0,0,0,SF.ProcessCode,GETDATE(),null,1 
	FROM SubscriberFinal SF 
		INNER JOIN (SELECT IGRP_NO FROM SubscriberFinal WHERE SFRecordIdentifier in (SELECT SFRecordIdentifier FROM Suppressed) GROUP BY IGrp_No) as ins
			ON SF.IGrp_No = INS.IGrp_No
	WHERE SF.ProcessCode = @ProcessCode AND SF.SFRecordIdentifier NOT IN (SELECT SFRecordIdentifier FROM Suppressed)
	
	SET @EmailIgrpNo = @@ROWCOUNT
	
	--DELETE FROM #tmp_IgrpNoSupp
	
	--
	-- Match on Fname,LName,Phone
	--
	INSERT INTO Suppressed(STRecordIdentifier,SFRecordIdentifier,Source,IsSuppressed,IsEmailMatch,IsPhoneMatch,IsAddressMatch,IsCompanyMatch,ProcessCode,DateCreated,DateUpdated,CreatedByUserID)	
	SELECT DISTINCT STRecordIdentifier,SFRecordIdentifier,'',1,0,1,0,0,SF.ProcessCode,GETDATE(),null,1 
	FROM SubscriberFinal SF 
		inner join SuppressionFileDetail ts on left(sf.FName,3) = left(ts.FirstName,3) and left(sf.LName,6) = left(ts.LastName,6) and replace(sf.Phone,'-','') = replace(ts.Phone,'-','')
	WHERE ISNULL(sf.FNAME,'')!='' and ISNULL(sf.LNAME,'')!='' AND ISNULL(sf.Phone,'')!=''AND ProcessCode = @ProcessCode
		AND SF.SFRecordIdentifier NOT IN (SELECT SFRecordIdentifier FROM Suppressed) 
	
	SET @PhoneSupp = @@ROWCOUNT
	
	INSERT INTO Suppressed(STRecordIdentifier,SFRecordIdentifier,Source,IsSuppressed,IsEmailMatch,IsPhoneMatch,IsAddressMatch,IsCompanyMatch,ProcessCode,DateCreated,DateUpdated,CreatedByUserID)
	SELECT DISTINCT STRecordIdentifier,SFRecordIdentifier,'',1,0,1,0,0,SF.ProcessCode,GETDATE(),null,1 
	FROM SubscriberFinal SF 
		INNER JOIN (SELECT IGRP_NO FROM SubscriberFinal WHERE SFRecordIdentifier in (SELECT SFRecordIdentifier FROM Suppressed) GROUP BY IGrp_No) as ins
			ON SF.IGrp_No = INS.IGrp_No
	WHERE SF.ProcessCode = @ProcessCode AND SF.SFRecordIdentifier NOT IN (SELECT SFRecordIdentifier FROM Suppressed)
	
	SET @PhoneIgrpNo = @@ROWCOUNT
	
	--
	-- Any Email that matches, blank out
	--
	--SELECT STRecordIdentifier,SFRecordIdentifier,@SuppFileName,1,1,0,0,0,SF.ProcessCode,GETDATE(),null,1
	UPDATE SF
	SET Email = Null 
	FROM SubscriberFinal SF 
		inner join SuppressionFileDetail ts on sf.Email = ts.Email
	WHERE ISNULL(sf.Email,'')!='' AND SF.ProcessCode = @ProcessCode 
		--AND SF.SFRecordIdentifier NOT IN (SELECT SFRecordIdentifier FROM Suppressed)

	--
	-- Any phone that matches, blank out
	--SELECT STRecordIdentifier,SFRecordIdentifier,@SuppFileName,1,0,1,0,0,SF.ProcessCode,GETDATE(),null,1 
	UPDATE SF
	SET Phone = null
	FROM SubscriberFinal SF 
		inner join SuppressionFileDetail ts on replace(sf.Phone,'-','') = replace(ts.Phone,'-','')
	WHERE ISNULL(sf.Phone,'')!='' AND SF.ProcessCode = @ProcessCode -- AND SF.SFRecordIdentifier NOT IN (SELECT SFRecordIdentifier FROM Suppressed)

	
	--DROP TABLE #tmp_IgrpNoSupp
		
	-- Update subFinal
	UPDATE sf
	SET Ignore = 1
	FROM SubscriberFinal sf 
		INNER JOIN Suppressed sp on sf.SFRecordIdentifier = sp.SFRecordIdentifier
	WHERE SF.ProcessCode = @ProcessCode and sp.Source != 'DONO'


	-- Total up affected rows
	DECLARE @SuppCount int
	SET @SuppCount = @AddrSupp + @CompanySupp + @EmailSupp + @PhoneSupp + @AddrIgrpNo + @CompanyIgrpNo + @EmailIgrpNo + @PhoneIgrpNo
	
	SELECT @SuppCount
END