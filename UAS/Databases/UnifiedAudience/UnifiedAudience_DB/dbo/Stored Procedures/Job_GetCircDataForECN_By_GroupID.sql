CREATE PROCEDURE [dbo].[Job_GetCircDataForECN_By_GroupID]
@DaysOld integer,
@GroupID integer,
@FullExport integer = 0

AS

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

--EXEC Job_GetCircDataForECN_By_GroupID -10, 183883     -- FranceMasterDB
--EXEC Job_GetCircDataForECN_By_GroupID -1, 204440		-- TradePressMasterDB
--EXEC Job_GetCircDataForECN_By_GroupID -1, 190625, 1	-- MTG
--EXEC Job_GetCircDataForECN_By_GroupID -1, 365507	-- MacFadden
--EXEC Job_GetCircDataForECN_By_GroupID -3, 374224	-- MacFadden
--EXEC Job_GetCircDataForECN_By_GroupID -1500, 383535, 1-- SourceMedia
--Select t.* from tmp_CirctDataForECN T order by email
--Select Min(transactionDate), MAX(transactionDate) from tmp_CirctDataForECN T 


IF @DaysOld < 0
BEGIN
	-- DaysOld for this proc needs to be a positive number
	SET @DaysOld = @DaysOld * -1
END

INSERT INTO  
	@Fields
SELECT 
	DISTINCT(Responsegroup) 
FROM 
	PubSubscriptions ps 
	JOIN PubSubscriptionDetail psd on ps.PubSubscriptionId = psd.PubSubscriptionId
	JOIN Codesheet cs on psd.codesheetid = cs.codesheetid
	JOIN Pubs p on ps.pubid = p.pubid
	JOIN PubGroups pg on pg.pubid = p.pubid
WHERE
	isCirc =1 and ResponseGroup not in ('PubCode')  -- ignore the field from codesheet and use the value from the pubs table
ORDER BY 1

--SELECT @ColumnList = STUFF(( SELECT DISTINCT '],[UDF_' + ResponseGroup FROM @Fields ORDER BY '],[UDF_' + ResponseGroup FOR XML PATH('') ), 1, 2, '') + ']'
SELECT @ColumnList = STUFF(( SELECT DISTINCT '],[' + ResponseGroup FROM @Fields ORDER BY '],[' + ResponseGroup FOR XML PATH('') ), 1, 2, '') + ']'

SET @Query = '
SELECT 
	*
INTO tmp_CirctDataForECN
FROM
	(
	SELECT 
		ps.pubsubscriptionid,
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
		p.pubcode AS PublicationCode,
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
	FROM 
		PubSubscriptions ps 
		JOIN Subscriptions s on ps.SubscriptionID = s.Subscriptionid
		JOIN Pubs p on ps.pubid = p.pubid
		JOIN PubGroups pg on pg.pubid = p.pubid
		LEFT OUTER JOIN PubSubscriptionDetail psd on ps.PubSubscriptionId = psd.PubSubscriptionId
		LEFT OUTER JOIN Codesheet cs on psd.codesheetid = cs.codesheetid
		left outer join UAD_Lookup..Code lupQ on lupQ.CodeId = ps.PubQSourceID
		left outer join UAD_Lookup..CategoryCode lupC on lupC.CategoryCodeID = ps.PubCategoryID
		left outer join UAD_Lookup..TransactionCode lupT on lupT.TransactionCodeID = ps.PubTransactionID
		left outer join UAD_Lookup..Code lupP on lupP.CodeId = ps.Par3CID
	WHERE
		isCirc =1
		AND pg.GroupID = ' + Convert(varchar(10), @GroupID)

	IF @FullExport = 0
	BEGIN
		SET @Query = @Query + '
		AND ( IsNull(ps.emailid, 0) = 0 
			OR DATEDIFF(DAY,ps.DateCreated,GETDATE() ) between 1 and ' + Convert(varchar(5),@DaysOld) + ' 
			OR DATEDIFF(DAY,ps.DateUpdated,GETDATE() ) between 1 and ' + Convert(varchar(5),@DaysOld) + '
			) '
	END
	SET @Query = @Query + '
	) p
	PIVOT (MAX(ResponseValue)
	FOR ResponseGroup IN  (' + @ColumnList + ')) AS PivotTable
	ORDER BY PubSubscriptionId'
			
--PRINT @Query
EXEC (@Query)


-- THIS update of duplicate email addresses has moved to Job_SyncUADCircDataToECN
-- UPDATE the duplicate email addresses to subscriber/pubcode.dmpsgroup.com. Preserve the first email address based on the order of email, transactionDate, SequenceID
UPDATE tmp_CirctDataForECN set Email = CONVERT(VARCHAR(40),SubscriberID) + '@' + PublicationCode + '.kmpsgroup.com' 
where pubsubscriptionid in (
SELECT pubsubscriptionID 
FROM
(
Select
     --ROW_NUMBER() OVER (partition by email ORDER BY email) AS DupCountEmailAddress,
     --ROW_NUMBER() OVER (partition by email ORDER BY email, TransactionDate DESC, EmailID DESC) AS QualDateOrder,
	 ROW_NUMBER() OVER (partition by email ORDER BY email, TransactionDate DESC, SequenceID DESC) AS DupQaulDateSubscriberOrder
	 , t.* 
from tmp_CirctDataForECN T
) x
where x.DupQaulDateSubscriberOrder > 1
)
GO