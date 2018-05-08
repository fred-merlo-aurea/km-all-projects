CREATE PROCEDURE [dbo].[Job_GetCircDataForECN]
@DaysOld integer
AS
BEGIN   

	SET NOCOUNT ON 


	IF EXISTS (SELECT 1 FROM Sysobjects where name = 'tmp_CirctDataForECN')
	DROP TABLE tmp_CirctDataForECN

	--drop table ecn_udf

	-------------
	---PROD CODE
	-------------
	DECLARE @Fields TABLE(ResponseGroup VARCHAR(50))    
	DECLARE @ColumnList VARCHAR(MAX)
	DECLARE @Query VARCHAR(MAX)

	--EXEC Job_GetCircDataForECN 36
	--Select * from tmp_CirctDataForECN

	IF @DaysOld < 0
		BEGIN
			-- DaysOld for this proc needs to be a positive number
			SET @DaysOld = @DaysOld * -1
		END

	INSERT INTO @Fields
	SELECT DISTINCT(Responsegroup) 
	FROM PubSubscriptions ps 
		JOIN PubSubscriptionDetail psd on ps.PubSubscriptionId = psd.PubSubscriptionId
		JOIN Codesheet cs on psd.codesheetid = cs.codesheetid
		JOIN Pubs p on ps.pubid = p.pubid
		JOIN PubGroups pg on pg.pubid = p.pubid
	WHERE isCirc =1
	ORDER BY 1

	--SELECT @ColumnList = STUFF(( SELECT DISTINCT '],[UDF_' + ResponseGroup FROM @Fields ORDER BY '],[UDF_' + ResponseGroup FOR XML PATH('') ), 1, 2, '') + ']'
	SELECT @ColumnList = STUFF(( SELECT DISTINCT '],[' + ResponseGroup FROM @Fields ORDER BY '],[' + ResponseGroup FOR XML PATH('') ), 1, 2, '') + ']'

	SET @Query = '
	SELECT 
		*
	INTO tmp_CirctDataForECN
	FROM
		(
		SELECT ps.pubsubscriptionid,
			ps.EmailID,
			CASE WHEN COALESCE(ps.Email,'''') = '''' THEN 
					CONVERT(VARCHAR(40),ps.SequenceID) + ''@'' +pubcode + ''.kmpsgroup.com'' 
			ELSE ps.Email END AS Email,
			ps.FirstName,
			ps.LastName,
			ps.Company,
			ps.Title,
			ps.Occupation,
			ps.Address1,
			ps.Address2,
			ps.City,
			ps.RegionCode,
 			CASE WHEN IsNull(ps.CountryID, 1) in (1,2) THEN ps.ZipCode ELSE NULL END as ZipCode,
			ps.Country,
			ps.CountryID,
			ps.Phone,
			ps.Fax,
			ps.Mobile,
			ps.Website,
			ps.Birthdate,
			ps.Age,
			ps.Income,
			ps.Gender,
			ps.PhoneExt,
			ps.SequenceID,
			cs.ResponseGroup,
			cs.ResponseValue,
			pg.GroupId,
			IsNull(ps.DateUpdated, ps.DateCreated) AS TransactionDate,
			'''' AS UDF_STARTINGPOSITION,
			CONVERT(VARCHAR(255),ps.SequenceID) as SubscriberID, 
			CONVERT(VARCHAR(255),ps.demo7) as Demo7, 
			CONVERT(VARCHAR(255),ps.Qualificationdate) as Qdate,
			CONVERT(VARCHAR(255),lupQ.DisplayOrder) as QSource,
			CONVERT(VARCHAR(255),lupC.CategoryCodeValue) as Cat,
			CONVERT(VARCHAR(255),lupT.TransactionCodeValue) as XACT,
			CONVERT(VARCHAR(255),ps.Copies) as Copies, 
			CONVERT(VARCHAR(255),lupP.DisplayOrder) as Par3C,
			CONVERT(VARCHAR(255),ps.Verify) as Verify,
			CONVERT(VARCHAR(255),ps.Plus4) as Plus4, 
			CONVERT(VARCHAR(255),ps.SubscriberSourceCode) as SUBSRC ,
			CONVERT(VARCHAR(255),ps.PubTransactionDate) as XACTDATE,
			CONVERT(VARCHAR(255),CASE WHEN IsNull(ps.CountryID, 1) NOT IN (1,2) THEN ps.ZipCode ELSE NULL END) as FORZIP,
			CONVERT(VARCHAR(255),ps.MailPermission) as Demo31,
			CONVERT(VARCHAR(255),ps.FaxPermission) as Demo32,
			CONVERT(VARCHAR(255),ps.PhonePermission) as Demo33,
			CONVERT(VARCHAR(255),ps.OtherProductsPermission) as Demo34,
			CONVERT(VARCHAR(255),ps.ThirdPartyPermission) as Demo35,
			CONVERT(VARCHAR(255),ps.EmailRenewPermission) as Demo36,
			CONVERT(VARCHAR(255),(SELECT MAX(BatchNumber) FROM Batch b JOIN History h ON b.BatchID = h.BatchID WHERE h.PubSubscriptionID = ps.PubSubscriptionID)) AS Batch
		FROM PubSubscriptions ps 
			JOIN Subscriptions s on ps.SubscriptionID = s.Subscriptionid
			JOIN PubSubscriptionDetail psd on ps.PubSubscriptionId = psd.PubSubscriptionId
			JOIN Codesheet cs on psd.codesheetid = cs.codesheetid
			JOIN Pubs p on ps.pubid = p.pubid
			JOIN PubGroups pg on pg.pubid = p.pubid
			left outer join UAD_Lookup..Code lupQ on lupQ.CodeId = ps.PubQSourceID
			left outer join UAD_Lookup..CategoryCode lupC on lupC.CategoryCodeID = ps.PubCategoryID
			left outer join UAD_Lookup..TransactionCode lupT on lupT.TransactionCodeID = ps.PubTransactionID
			left outer join UAD_Lookup..Code lupP on lupP.CodeId = ps.Par3CID
		WHERE isCirc =1
			AND (
			DATEDIFF(DAY,ps.DateCreated,GETDATE() ) <= ' + Convert(varchar(5),@DaysOld) + ' 
			OR DATEDIFF(DAY,ps.DateUpdated,GETDATE() ) <= ' + Convert(varchar(5),@DaysOld) + '
			) 
		) p
		PIVOT (MIN(ResponseValue)
		FOR ResponseGroup IN  (' + @ColumnList + ')) AS PivotTable
	ORDER BY
		PubSubscriptionId'
			
	--PRINT @Query
	EXEC (@Query)
	EXEC sp_RENAME 'tmp_CirctDataForECN.Pubcode', 'PublicationCode', 'COLUMN'

	--DROP TABLE #PubSubscriptions

END