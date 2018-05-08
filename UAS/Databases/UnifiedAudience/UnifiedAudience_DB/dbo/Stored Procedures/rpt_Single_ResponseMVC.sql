CREATE PROCEDURE [dbo].[rpt_Single_ResponseMVC]  
 @Queries VARCHAR(MAX),  
 @Demo varchar(100),  
 @PubID int,  
 @IssueID int = 0,  
 @IncludeReportGroup bit = 0  
AS  
BEGIN  
  
 DECLARE @LocQueries varchar(MAX)  
 DECLARE @LocDemo varchar(100)  
 DECLARE @LocPubID int  
 DECLARE @LocIssueID int  
 DECLARE @LocIncludeReportGroup bit  
   
 SET @LocQueries = @Queries  
 SET @LocDemo = @Demo  
 SET @LocPubID = @PubID  
 SET @LocIssueID = @IssueID  
 SET @LocIncludeReportGroup = @IncludeReportGroup  
   
   
 --DECLARE  
 --@Demo varchar(100) = 'BUSINESS',  
 --@PubID int = 6,  
 --@IssueID int = 8,  
 --@Filters varchar(MAX) = '<XML><Filters><ProductID>6</ProductID></Filters></XML>',  
 --@AdHocFilters varchar(MAX) = '<XML></XML>',  
 --@IncludeReportGroup bit = 0  
   
 IF 1=0   
 BEGIN  
  SET FMTONLY OFF  
 END  
   
 CREATE TABLE #Subscriptions (PubSubscriptionID int PRIMARY KEY)  
 INSERT INTO #Subscriptions  
 EXEC (@LocQueries)   
  
 DECLARE @DeliverID int = (SELECT CodeTypeID FROM UAD_Lookup..CodeType WHERE CodeTypeName = 'Deliver')  
 DECLARE @Groups varchar(MAX)  
 SELECT @Groups = STUFF( (SELECT ',' + '''' + ResponseGroupName + '''' FROM ResponseGroups WHERE PubID = @LocPubID for XML PATH('')), 1,1,'')  
 DECLARE @DemoID int = (SELECT ResponseGroupID FROM ResponseGroups WHERE ResponseGroupName = @Demo and PubID = @LocPubID)  
 DECLARE @MaxDisplayOrder int = (SELECT ISNULL(MAX(DisplayOrder) + 1,0) FROM ReportGroups)  
 declare @colgrandTotalUniqueRespondents int  
   
 SELECT c.Responsevalue + '. ' + c.Responsedesc as 'Responsedesc', c.DisplayOrder  
 INTO #TmpRow  
 FROM CodeSheet c   
  JOIN ResponseGroups rg ON rg.ResponseGroupID = c.ResponseGroupID   
 WHERE rg.PubID = @LocPubID AND rg.DisplayName = @LocDemo  
   
 IF @LocIssueID = 0 --Query Current Issue  
 BEGIN  
  
    
    
  select @colgrandTotalUniqueRespondents = isnull(count(distinct ps.PubSubscriptionID),0)    
  from PubSubscriptions ps WITH (NOLOCK)  
   JOIN #Subscriptions s WITH (NOLOCK) ON ps.PubSubscriptionID = s.PubSubscriptionID  
   JOIN UAD_Lookup..Code c WITH (NOLOCK) ON c.CodeValue = ps.demo7 AND c.CodeTypeId = @DeliverID  
   JOIN UAD_Lookup..CategoryCode cc WITH (NOLOCK) ON cc.CategoryCodeID = ps.PubCategoryID  
   JOIN UAD_Lookup..CategoryCodeType cct WITH (NOLOCK) ON cct.CategoryCodeTypeID = cc.CategoryCodeTypeID  
  where  
   ps.pubID = @LocPubID  
  
  SELECT  
   ps.demo7, isnull(count(distinct s.PubSubscriptionID),0) as colTotalUniqueRespondents  
   into #tmpColUniqueCounts  
  FROM   
   PubSubscriptions ps WITH (NOLOCK)  
   JOIN #Subscriptions s WITH (NOLOCK) ON ps.PubSubscriptionID = s.PubSubscriptionID  
   JOIN UAD_Lookup..Code c WITH (NOLOCK) ON c.CodeValue = ps.demo7 AND c.CodeTypeId = @DeliverID  
   JOIN UAD_Lookup..CategoryCode cc WITH (NOLOCK) ON cc.CategoryCodeID = ps.PubCategoryID  
   JOIN UAD_Lookup..CategoryCodeType cct WITH (NOLOCK) ON cct.CategoryCodeTypeID = cc.CategoryCodeTypeID  
  where  
   ps.pubID = @LocPubID  
  group by ps.demo7  
    
  IF CHARINDEX(@LocDemo, @Groups) > 0  
  BEGIN  
   SELECT  
    SUM(ps.Copies) 'Copies',  
    c.DisplayName AS 'Demo7',  
    ISNULL(cs.Responsevalue + '. ' + cs.Responsedesc, 'ZZ. No Response') AS 'Row',  
    ISNULL(cs.DisplayOrder, 100) AS 'DisplayOrder',  
    COUNT(distinct ps.PubSubscriptionID) AS RecordCount,  
    ISNULL(CONVERT(varchar(50), cs.CodeSheetID), 'ZZ') AS CodeSheetID,  
    @DemoID AS 'ResponseGroupID',  
    c.CodeId AS 'DemoID',  
    (CASE WHEN @LocIncludeReportGroup = 1 THEN ISNULL(rpts.DisplayName, '') ELSE '' END) AS 'GroupDisplay',  
    (CASE WHEN @LocIncludeReportGroup = 1 THEN ISNULL(rpts.DisplayOrder, @MaxDisplayOrder) ELSE 0 END) AS 'GroupDisplayOrder',  
    (CASE WHEN cct.CategoryCodeTypeName IN ('Qualified Free', 'Qualified Paid') THEN 'Qualified' ELSE 'NonQualified' END) AS CategoryType,  
    colTotalUniqueRespondents,  
    @colgrandTotalUniqueRespondents as colgrandTotalUniqueRespondents  
   FROM   
    PubSubscriptions ps WITH (NOLOCK)  
    JOIN #Subscriptions s WITH (NOLOCK) ON ps.PubSubscriptionID = s.PubSubscriptionID  
    JOIN UAD_Lookup..Code c WITH (NOLOCK) ON c.CodeValue = ps.demo7 AND c.CodeTypeId = @DeliverID  
    LEFT JOIN   
    (  
     SELECT  
      psd.PubSubscriptionID, cs.ReportGroupID, cs.Responsevalue, cs.Responsedesc, cs.DisplayOrder, cs.CodeSheetID  
     FROM   
      #Subscriptions s WITH (NOLOCK)  
      JOIN PubSubscriptionDetail psd WITH (NOLOCK) ON s.PubSubscriptionID = psd.PubSubscriptionID  
      JOIN CodeSheet cs WITH (NOLOCK) ON cs.CodeSheetID = psd.CodesheetID  
      JOIN ResponseGroups rg WITH (NOLOCK) ON rg.ResponseGroupID = cs.ResponseGroupID  
     WHERE   
      rg.ResponseGroupName = @LocDemo AND   
      rg.PubID = @LocPubID  
    ) cs ON cs.PubSubscriptionID = s.PubSubscriptionID  
    JOIN UAD_Lookup..CategoryCode cc WITH (NOLOCK) ON cc.CategoryCodeID = ps.PubCategoryID  
    JOIN UAD_Lookup..CategoryCodeType cct WITH (NOLOCK) ON cct.CategoryCodeTypeID = cc.CategoryCodeTypeID  
    LEFT JOIN ReportGroups rpts WITH (NOLOCK) ON rpts.ReportGroupID = cs.ReportGroupID  
    join #tmpColUniqueCounts tmp on ps.demo7 = tmp.demo7  
   WHERE   
    ps.PubID = @LocPubID  
   GROUP BY   
    c.DisplayName,  
    ISNULL(cs.Responsevalue + '. ' + cs.Responsedesc, 'ZZ. No Response'),  
    ISNULL(cs.DisplayOrder, 100),  
    ISNULL(CONVERT(varchar(50), cs.CodeSheetID), 'ZZ'),  
    c.CodeId,  
    (CASE WHEN @LocIncludeReportGroup = 1 THEN ISNULL(rpts.DisplayName, '') ELSE '' END),  
    (CASE WHEN @LocIncludeReportGroup = 1 THEN ISNULL(rpts.DisplayOrder, @MaxDisplayOrder) ELSE 0 END),  
    (CASE WHEN cct.CategoryCodeTypeName IN ('Qualified Free', 'Qualified Paid') THEN 'Qualified' ELSE 'NonQualified' END),  
    colTotalUniqueRespondents  
  
  END  
  ELSE IF @LocDemo <> 'SCF'  
  BEGIN  
   EXEC ('SELECT Sum(ps.Copies) as ''Copies'', c.DisplayName as ''Demo7'', ISNULL(NULLIF(NULLIF(CONVERT(VARCHAR(500),ps.[' + @LocDemo + ']), ''''), ''0''), ''No Response'') as ''Row'',   
     1 as DisplayOrder, count(distinct ps.PubSubscriptionID) as RecordCount, c.CodeId as ''DemoID'', ' +  
     '(CASE WHEN cct.CategoryCodeTypeName in (''Qualified Free'', ''Qualified Paid'') THEN ''Qualified'' ELSE ''NonQualified'' END) as CategoryType, colTotalUniqueRespondents, '    
     + @colgrandTotalUniqueRespondents + 'as colgrandTotalUniqueRespondents ' +  
     ' FROM PubSubscriptions ps WITH (NOLOCK) JOIN #Subscriptions s WITH (NOLOCK) ON s.PubSubscriptionID = ps.PubSubscriptionID ' +   
     'JOIN UAD_Lookup..Code c WITH (NOLOCK) ON c.CodeValue = ps.demo7 and c.CodeTypeID = ' + @DeliverID +   
     'JOIN UAD_Lookup..CategoryCode cc WITH (NOLOCK) ON cc.CategoryCodeID = ps.PubCategoryID ' +  
     'JOIN UAD_Lookup..CategoryCodeType cct WITH (NOLOCK) ON cct.CategoryCodeTypeID = cc.CategoryCodeTypeID  
     join #tmpColUniqueCounts tmp on ps.demo7 = tmp.demo7  
     where Ps.PubID = ' + @LocPubID + '  
     group by c.DisplayName,  ISNULL(NULLIF(NULLIF(CONVERT(VARCHAR(500),ps.[' + @LocDemo + ']), ''''), ''0''), ''No Response''), c.CodeId,   
     (CASE WHEN cct.CategoryCodeTypeName in (''Qualified Free'', ''Qualified Paid'') THEN ''Qualified'' ELSE ''NonQualified'' END),colTotalUniqueRespondents')  
  END  
  ELSE  
  BEGIN  
   SELECT  
    SUM(ps.Copies) AS 'Copies',  
    c.DisplayName AS 'Demo7',  
    LEFT(ISNULL(NULLIF(NULLIF(CONVERT(varchar(500), ps.[ZipCode]), ''), '0'), 'No Response'), 3) AS 'Row',  
    1 AS DisplayOrder,  
    COUNT(ps.PubSubscriptionID) AS RecordCount,  
    c.CodeId AS 'DemoID',  
    (CASE  
     WHEN cct.CategoryCodeTypeName IN ('Qualified Free', 'Qualified Paid') THEN 'Qualified'  
     ELSE 'NonQualified'  
    END) AS CategoryType,  
    colTotalUniqueRespondents,  
    @colgrandTotalUniqueRespondents  as colgrandTotalUniqueRespondents  
   FROM   
    PubSubscriptions ps WITH (NOLOCK)  
    JOIN #Subscriptions s WITH (NOLOCK) ON s.PubSubscriptionID = ps.PubSubscriptionID  
    JOIN UAD_Lookup..Code c WITH (NOLOCK) ON c.CodeValue = ps.demo7 AND c.CodeTypeID = @DeliverID  
    JOIN UAD_Lookup..CategoryCode cc WITH (NOLOCK) ON cc.CategoryCodeID = ps.PubCategoryID  
    JOIN UAD_Lookup..CategoryCodeType cct WITH (NOLOCK) ON cct.CategoryCodeTypeID = cc.CategoryCodeTypeID  
    join #tmpColUniqueCounts tmp on ps.demo7 = tmp.demo7  
   WHERE   
    Ps.PubID = @LocPubID  
   GROUP BY   
    c.DisplayName,  
    LEFT(ISNULL(NULLIF(NULLIF(CONVERT(varchar(500), ps.[ZipCode]), ''), '0'), 'No Response'), 3),  
    c.CodeId,  
    (CASE WHEN cct.CategoryCodeTypeName IN ('Qualified Free', 'Qualified Paid') THEN 'Qualified' ELSE 'NonQualified' END),  
    colTotalUniqueRespondents  
  END  
 END  
 ELSE --Query Archive  
 BEGIN  
    
  
  select @colgrandTotalUniqueRespondents = isnull(count(distinct ps.PubSubscriptionID),0)    
  from IssueArchiveProductSubscription ps WITH (NOLOCK)  
   JOIN #Subscriptions s WITH (NOLOCK) ON ps.PubSubscriptionID = s.PubSubscriptionID  
   JOIN UAD_Lookup..Code c WITH (NOLOCK) ON c.CodeValue = ps.demo7 AND c.CodeTypeId = @DeliverID  
   JOIN UAD_Lookup..CategoryCode cc WITH (NOLOCK) ON cc.CategoryCodeID = ps.PubCategoryID  
   JOIN UAD_Lookup..CategoryCodeType cct WITH (NOLOCK) ON cct.CategoryCodeTypeID = cc.CategoryCodeTypeID  
  where  
   ps.pubID = @LocPubID and  
   ps.IssueID = @LocIssueID  
  
  SELECT  
   ps.demo7, isnull(count(distinct s.PubSubscriptionID),0) as colTotalUniqueRespondents  
   into #tmpColUniqueCounts2  
  FROM   
   IssueArchiveProductSubscription ps WITH (NOLOCK)  
   JOIN #Subscriptions s WITH (NOLOCK) ON ps.PubSubscriptionID = s.PubSubscriptionID  
   JOIN UAD_Lookup..Code c WITH (NOLOCK) ON c.CodeValue = ps.demo7 AND c.CodeTypeId = @DeliverID  
   JOIN UAD_Lookup..CategoryCode cc WITH (NOLOCK) ON cc.CategoryCodeID = ps.PubCategoryID  
   JOIN UAD_Lookup..CategoryCodeType cct WITH (NOLOCK) ON cct.CategoryCodeTypeID = cc.CategoryCodeTypeID  
  where  
   ps.pubID = @LocPubID and  
   ps.IssueID = @LocIssueID  
  group by ps.demo7  
    
  
  IF CHARINDEX(@LocDemo, @Groups) > 0  
  BEGIN  
   SELECT  
    SUM(ps.Copies) Copies,  
    c.DisplayName AS 'Demo7',  
    ISNULL(cs.Responsevalue + '. ' + cs.Responsedesc, 'ZZ. No Response') AS 'Row',  
    ISNULL(cs.DisplayOrder, 100) AS 'DisplayOrder',  
    count(distinct ps.PubSubscriptionID) AS RecordCount,  
    ISNULL(CONVERT(varchar(50), cs.CodeSheetID), 'ZZ') AS CodeSheetID,  
    @DemoID AS 'ResponseGroupID',  
    c.CodeId AS 'DemoID',  
    (CASE WHEN @LocIncludeReportGroup = 1 THEN ISNULL(rpts.DisplayName, '') ELSE '' END) AS 'GroupDisplay',  
    (CASE WHEN @LocIncludeReportGroup = 1 THEN ISNULL(rpts.DisplayOrder, @MaxDisplayOrder) ELSE 0 END) AS 'GroupDisplayOrder',  
    (CASE WHEN cct.CategoryCodeTypeName IN ('Qualified Free', 'Qualified Paid') THEN 'Qualified' ELSE 'NonQualified' END) AS CategoryType,  
    colTotalUniqueRespondents,  
    @colgrandTotalUniqueRespondents as colgrandTotalUniqueRespondents  
   FROM   
    IssueArchiveProductSubscription ps WITH (NOLOCK)  
    JOIN #Subscriptions s WITH (NOLOCK) ON ps.PubSubscriptionID = s.PubSubscriptionID  
    JOIN UAD_Lookup..Code c WITH (NOLOCK) ON c.CodeValue = ps.demo7 AND c.CodeTypeId = @DeliverID  
    LEFT JOIN   
    (  
     SELECT  
      psd.PubSubscriptionID, cs.ReportGroupID, cs.Responsevalue, cs.Responsedesc, cs.DisplayOrder, cs.CodeSheetID  
     FROM   
      #Subscriptions s WITH (NOLOCK)  
      JOIN IssueArchiveProductSubscription ias WITH (NOLOCK) ON ias.PubSubscriptionID = s.PubSubscriptionID  
      JOIN IssueArchiveProductSubscriptionDetail psd WITH (NOLOCK) ON ias.IssueArchiveSubscriptionID = psd.IssueArchiveSubscriptionID  
      JOIN CodeSheet cs WITH (NOLOCK) ON cs.CodeSheetID = psd.CodesheetID  
      JOIN ResponseGroups rg WITH (NOLOCK) ON rg.ResponseGroupID = cs.ResponseGroupID  
     WHERE   
      rg.ResponseGroupName = @LocDemo  
      AND rg.PubID = @LocPubID  
      AND ias.IssueID = @LocIssueID  
    ) cs  
     ON cs.PubSubscriptionID = s.PubSubscriptionID  
    JOIN UAD_Lookup..CategoryCode cc WITH (NOLOCK) ON cc.CategoryCodeID = ps.PubCategoryID  
    JOIN UAD_Lookup..CategoryCodeType cct WITH (NOLOCK) ON cct.CategoryCodeTypeID = cc.CategoryCodeTypeID  
    LEFT JOIN ReportGroups rpts WITH (NOLOCK) ON rpts.ReportGroupID = cs.ReportGroupID  
    left join #tmpColUniqueCounts2 tmp on ps.demo7 = tmp.demo7  
   WHERE   
    ps.IssueID = @LocIssueID  
   Group by  
    c.DisplayName,  
    ISNULL(cs.Responsevalue + '. ' + cs.Responsedesc, 'ZZ. No Response'),  
    ISNULL(cs.DisplayOrder, 100) ,  
    ISNULL(CONVERT(varchar(50), cs.CodeSheetID), 'ZZ'),  
    c.CodeId,  
    (CASE WHEN @LocIncludeReportGroup = 1 THEN ISNULL(rpts.DisplayName, '') ELSE '' END),  
    (CASE WHEN @LocIncludeReportGroup = 1 THEN ISNULL(rpts.DisplayOrder, @MaxDisplayOrder) ELSE 0 END) ,  
    (CASE WHEN cct.CategoryCodeTypeName IN ('Qualified Free', 'Qualified Paid') THEN 'Qualified' ELSE 'NonQualified' END),  
    colTotalUniqueRespondents  
      
  END  
  ELSE IF @LocDemo <> 'SCF'  
  BEGIN  
   EXEC ('SELECT Sum(ps.Copies) Copies, c.DisplayName as ''Demo7'', ISNULL(NULLIF(NULLIF(CONVERT(VARCHAR(500),ps.[' + @LocDemo + ']), ''''), ''0''), ''No Response'') as ''Row'',   
     1 as DisplayOrder, count(distinct ps.PubSubscriptionID) as RecordCount, c.CodeId as ''DemoID'' ' +  
     '(CASE WHEN cct.CategoryCodeTypeName in (''Qualified Free'', ''Qualified Paid'') THEN ''Qualified'' ELSE ''NonQualified'' END) as CategoryType, colTotalUniqueRespondents ' +  
     @colgrandTotalUniqueRespondents + 'as colgrandTotalUniqueRespondents' +  
     'FROM IssueArchiveProductSubscription ps WITH (NOLOCK) JOIN #Subscriptions s ON s.PubSubscriptionID = ps.PubSubscriptionID ' +   
     'JOIN UAD_Lookup..Code c WITH (NOLOCK) ON c.CodeValue = ps.demo7 and c.CodeTypeID = ' + @DeliverID +  
     'JOIN UAD_Lookup..CategoryCode cc WITH (NOLOCK) ON cc.CategoryCodeID = ps.PubCategoryID ' +  
     'JOIN UAD_Lookup..CategoryCodeType cct WITH (NOLOCK) ON cct.CategoryCodeTypeID = cc.CategoryCodeTypeID  
     join #tmpColUniqueCounts2 tmp on ps.demo7 = tmp.demo7  
     where Ps.IssueID = ' + @LocIssueID + '  
     group by  
      c.DisplayName, ISNULL(NULLIF(NULLIF(CONVERT(VARCHAR(500),ps.[' + @LocDemo + ']), ''''), ''0''), ''No Response'') ,  
      c.CodeId,  
      (CASE WHEN cct.CategoryCodeTypeName in (''Qualified Free'', ''Qualified Paid'') THEN ''Qualified'' ELSE ''NonQualified'' END),  
      colTotalUniqueRespondents  
     ')  
  END  
  ELSE  
  BEGIN  
   SELECT  
    sum(ps.Copies) Copies,  
    c.DisplayName AS 'Demo7',  
    LEFT(ISNULL(NULLIF(NULLIF(CONVERT(varchar(500), ps.[ZipCode]), ''), '0'), 'No Response'), 3) AS 'Row',  
    1 AS DisplayOrder,  
    count(ps.PubSubscriptionID) AS RecordCount,  
    c.CodeId AS 'DemoID',  
    (CASE WHEN cct.CategoryCodeTypeName IN ('Qualified Free', 'Qualified Paid') THEN 'Qualified' ELSE 'NonQualified' END) AS CategoryType,  
    colTotalUniqueRespondents,  
    @colgrandTotalUniqueRespondents as colgrandTotalUniqueRespondents  
   FROM   
    IssueArchiveProductSubscription ps WITH (NOLOCK)  
    JOIN #Subscriptions s ON s.PubSubscriptionID = ps.PubSubscriptionID  
    JOIN UAD_Lookup..Code c WITH (NOLOCK) ON c.CodeValue = ps.demo7 AND c.CodeTypeID = @DeliverID   
    JOIN UAD_Lookup..CategoryCode cc WITH (NOLOCK) ON cc.CategoryCodeID = ps.PubCategoryID  
    JOIN UAD_Lookup..CategoryCodeType cct WITH (NOLOCK) ON cct.CategoryCodeTypeID = cc.CategoryCodeTypeID  
    join #tmpColUniqueCounts2 tmp on ps.demo7 = tmp.demo7  
   Where  
    ps.IssueID = @LocIssueID  
   Group by  
    c.DisplayName,  
    LEFT(ISNULL(NULLIF(NULLIF(CONVERT(varchar(500), ps.[ZipCode]), ''), '0'), 'No Response'), 3) ,  
    c.CodeId ,  
    (CASE WHEN cct.CategoryCodeTypeName IN ('Qualified Free', 'Qualified Paid') THEN 'Qualified' ELSE 'NonQualified' END),  
    colTotalUniqueRespondents  
  
  END  
 END  
   
 DROP TABLE #Subscriptions  
 DROP TABLE #TmpRow  
 IF OBJECT_ID('tempdb.dbo.#tmpColUniqueCounts', 'U') IS NOT NULL   
  DROP TABLE  #tmpColUniqueCounts  
 IF OBJECT_ID('tempdb.dbo.#tmpColUniqueCounts2', 'U') IS NOT NULL   
  DROP TABLE  #tmpColUniqueCounts2  
  
END  
  
GO


