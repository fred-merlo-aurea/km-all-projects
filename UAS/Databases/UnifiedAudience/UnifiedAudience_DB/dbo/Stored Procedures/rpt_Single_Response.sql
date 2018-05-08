CREATE PROCEDURE [dbo].[rpt_Single_Response]  
 @Demo varchar(100),  
 @PubID int,  
 @Filters TEXT = '<XML><Filters></Filters></XML>',  
 @AdHocFilters TEXT = '<XML></XML>',  
 @IssueID int = 0,  
 @IncludeReportGroup bit = 0  
AS  
BEGIN  
  
 DECLARE 
 @LocalDemo varchar(100)=@Demo,  
 @LocalPubID int=@PubID,  
 @LocalFilters varchar(max) = @Filters,  
 @LocalAdHocFilters varchar(max) = @AdHocFilters,  
 @LocalIssueID int = @IssueID,  
 @LocalIncludeReportGroup bit = @IncludeReportGroup 
   
 IF 1=0   
 BEGIN  
  SET FMTONLY OFF  
 END  
   
 CREATE TABLE #Subscriptions (PubSubscriptionID int PRIMARY KEY)  
  
 DECLARE @DeliverID int = (SELECT CodeTypeID FROM UAD_Lookup..CodeType WHERE CodeTypeName = 'Deliver')  
 DECLARE @Groups varchar(MAX)  
 SELECT @Groups = STUFF( (SELECT ',' + '''' + ResponseGroupName + '''' FROM ResponseGroups WHERE PubID = @LocalPubID  for XML PATH('')), 1,1,'')  
 DECLARE @LocalDemoID int = (SELECT ResponseGroupID FROM ResponseGroups WHERE ResponseGroupName = @LocalDemo and PubID = @LocalPubID )  
 DECLARE @MaxDisplayOrder int = (SELECT ISNULL(MAX(DisplayOrder) + 1,0) FROM ReportGroups)  
 declare @colgrandTotalUniqueRespondents int  
   
 SELECT c.Responsevalue + '. ' + c.Responsedesc as 'Responsedesc', c.DisplayOrder  
 INTO #TmpRow  
 FROM CodeSheet c   
  JOIN ResponseGroups rg ON rg.ResponseGroupID = c.ResponseGroupID   
 WHERE rg.PubID = @LocalPubID  AND rg.DisplayName = @LocalDemo  
   
 IF @LocalIssueID = 0 --Query Current Issue  
 BEGIN  
  
  INSERT INTO #Subscriptions  
  EXEC rpt_GetSubscriptionIDs_Copies_From_Filter_XML @LocalFilters, @LocalAdHocFilters   
    
  select @colgrandTotalUniqueRespondents = isnull(count(distinct ps.PubSubscriptionID),0)    
  from PubSubscriptions ps WITH (NOLOCK)  
   JOIN #Subscriptions s WITH (NOLOCK) ON ps.PubSubscriptionID = s.PubSubscriptionID  
   JOIN UAD_Lookup..Code c WITH (NOLOCK) ON c.CodeValue = ps.demo7 AND c.CodeTypeId = @DeliverID  
   JOIN UAD_Lookup..CategoryCode cc WITH (NOLOCK) ON cc.CategoryCodeID = ps.PubCategoryID  
   JOIN UAD_Lookup..CategoryCodeType cct WITH (NOLOCK) ON cct.CategoryCodeTypeID = cc.CategoryCodeTypeID  
  where  
   ps.pubID = @LocalPubID   
  
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
   ps.pubID = @LocalPubID   
  group by ps.demo7  
    
  IF CHARINDEX(@LocalDemo, @Groups) > 0  
  BEGIN  
   SELECT  
    SUM(ps.Copies) 'Copies',  
    c.DisplayName AS 'Demo7',  
    ISNULL(cs.Responsevalue + '. ' + cs.Responsedesc, 'ZZ. No Response') AS 'Row',  
    ISNULL(cs.DisplayOrder, 100) AS 'DisplayOrder',  
    COUNT(distinct ps.PubSubscriptionID) AS RecordCount,  
    ISNULL(CONVERT(varchar(50), cs.CodeSheetID), 'ZZ') AS CodeSheetID,  
    @LocalDemoID AS 'ResponseGroupID',  
    c.CodeId AS 'DemoID',  
    (CASE WHEN  @LocalIncludeReportGroup = 1 THEN ISNULL(rpts.DisplayName, '') ELSE '' END) AS 'GroupDisplay',  
    (CASE WHEN  @LocalIncludeReportGroup = 1 THEN ISNULL(rpts.DisplayOrder, @MaxDisplayOrder) ELSE 0 END) AS 'GroupDisplayOrder',  
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
      rg.ResponseGroupName = @LocalDemo AND   
      rg.PubID = @LocalPubID   
    ) cs ON cs.PubSubscriptionID = s.PubSubscriptionID  
    JOIN UAD_Lookup..CategoryCode cc WITH (NOLOCK) ON cc.CategoryCodeID = ps.PubCategoryID  
    JOIN UAD_Lookup..CategoryCodeType cct WITH (NOLOCK) ON cct.CategoryCodeTypeID = cc.CategoryCodeTypeID  
    LEFT JOIN ReportGroups rpts WITH (NOLOCK) ON rpts.ReportGroupID = cs.ReportGroupID  
    join #tmpColUniqueCounts tmp on ps.demo7 = tmp.demo7  
   WHERE   
    ps.PubID = @LocalPubID   
   GROUP BY   
    c.DisplayName,  
    ISNULL(cs.Responsevalue + '. ' + cs.Responsedesc, 'ZZ. No Response'),  
    ISNULL(cs.DisplayOrder, 100),  
    ISNULL(CONVERT(varchar(50), cs.CodeSheetID), 'ZZ'),  
    c.CodeId,  
    (CASE WHEN  @LocalIncludeReportGroup = 1 THEN ISNULL(rpts.DisplayName, '') ELSE '' END),  
    (CASE WHEN  @LocalIncludeReportGroup = 1 THEN ISNULL(rpts.DisplayOrder, @MaxDisplayOrder) ELSE 0 END),  
    (CASE WHEN cct.CategoryCodeTypeName IN ('Qualified Free', 'Qualified Paid') THEN 'Qualified' ELSE 'NonQualified' END),  
    colTotalUniqueRespondents  
  
  END  
  ELSE IF @LocalDemo <> 'SCF'  
  BEGIN  
   EXEC ('SELECT Sum(ps.Copies) as ''Copies'', c.DisplayName as ''Demo7'', ISNULL(NULLIF(NULLIF(CONVERT(VARCHAR(500),ps.[' + @LocalDemo + ']), ''''), ''0''), ''No Response'') as ''Row'',   
     1 as DisplayOrder, count(distinct ps.PubSubscriptionID) as RecordCount, c.CodeId as ''DemoID'', ' +  
     '(CASE WHEN cct.CategoryCodeTypeName in (''Qualified Free'', ''Qualified Paid'') THEN ''Qualified'' ELSE ''NonQualified'' END) as CategoryType, colTotalUniqueRespondents, '    
     + @colgrandTotalUniqueRespondents + 'as colgrandTotalUniqueRespondents ' +  
     ' FROM PubSubscriptions ps WITH (NOLOCK) JOIN #Subscriptions s WITH (NOLOCK) ON s.PubSubscriptionID = ps.PubSubscriptionID ' +   
     'JOIN UAD_Lookup..Code c WITH (NOLOCK) ON c.CodeValue = ps.demo7 and c.CodeTypeID = ' + @DeliverID +   
     'JOIN UAD_Lookup..CategoryCode cc WITH (NOLOCK) ON cc.CategoryCodeID = ps.PubCategoryID ' +  
     'JOIN UAD_Lookup..CategoryCodeType cct WITH (NOLOCK) ON cct.CategoryCodeTypeID = cc.CategoryCodeTypeID  
     join #tmpColUniqueCounts tmp on ps.demo7 = tmp.demo7  
     where Ps.PubID = ' + @LocalPubID  + '  
     group by c.DisplayName,  ISNULL(NULLIF(NULLIF(CONVERT(VARCHAR(500),ps.[' + @LocalDemo + ']), ''''), ''0''), ''No Response''), c.CodeId,   
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
    Ps.PubID = @LocalPubID   
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
  INSERT INTO #Subscriptions  
  EXEC rpt_GetSubscriptionIDs_Copies_From_Filter_XML @LocalFilters, @LocalAdHocFilters, 0, 1, @LocalIssueID   
  
  select @colgrandTotalUniqueRespondents = isnull(count(distinct ps.PubSubscriptionID),0)    
  from IssueArchiveProductSubscription ps WITH (NOLOCK)  
   JOIN #Subscriptions s WITH (NOLOCK) ON ps.PubSubscriptionID = s.PubSubscriptionID  
   JOIN UAD_Lookup..Code c WITH (NOLOCK) ON c.CodeValue = ps.demo7 AND c.CodeTypeId = @DeliverID  
   JOIN UAD_Lookup..CategoryCode cc WITH (NOLOCK) ON cc.CategoryCodeID = ps.PubCategoryID  
   JOIN UAD_Lookup..CategoryCodeType cct WITH (NOLOCK) ON cct.CategoryCodeTypeID = cc.CategoryCodeTypeID  
  where  
   ps.pubID = @LocalPubID  and  
   ps.IssueID = @LocalIssueID  
  
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
   ps.pubID = @LocalPubID  and  
   ps.IssueID = @LocalIssueID  
  group by ps.demo7  
    
  
  IF CHARINDEX(@LocalDemo, @Groups) > 0  
  BEGIN  
   SELECT  
    SUM(ps.Copies) Copies,  
    c.DisplayName AS 'Demo7',  
    ISNULL(cs.Responsevalue + '. ' + cs.Responsedesc, 'ZZ. No Response') AS 'Row',  
    ISNULL(cs.DisplayOrder, 100) AS 'DisplayOrder',  
    count(distinct ps.PubSubscriptionID) AS RecordCount,  
    ISNULL(CONVERT(varchar(50), cs.CodeSheetID), 'ZZ') AS CodeSheetID,  
    @LocalDemoID AS 'ResponseGroupID',  
    c.CodeId AS 'DemoID',  
    (CASE WHEN  @LocalIncludeReportGroup = 1 THEN ISNULL(rpts.DisplayName, '') ELSE '' END) AS 'GroupDisplay',  
    (CASE WHEN  @LocalIncludeReportGroup = 1 THEN ISNULL(rpts.DisplayOrder, @MaxDisplayOrder) ELSE 0 END) AS 'GroupDisplayOrder',  
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
      rg.ResponseGroupName = @LocalDemo  
      AND rg.PubID = @LocalPubID   
      AND ias.IssueID = @LocalIssueID  
    ) cs  
     ON cs.PubSubscriptionID = s.PubSubscriptionID  
    JOIN UAD_Lookup..CategoryCode cc WITH (NOLOCK) ON cc.CategoryCodeID = ps.PubCategoryID  
    JOIN UAD_Lookup..CategoryCodeType cct WITH (NOLOCK) ON cct.CategoryCodeTypeID = cc.CategoryCodeTypeID  
    LEFT JOIN ReportGroups rpts WITH (NOLOCK) ON rpts.ReportGroupID = cs.ReportGroupID  
    left join #tmpColUniqueCounts2 tmp on ps.demo7 = tmp.demo7  
   WHERE   
    ps.IssueID = @LocalIssueID  
   Group by  
    c.DisplayName,  
    ISNULL(cs.Responsevalue + '. ' + cs.Responsedesc, 'ZZ. No Response'),  
    ISNULL(cs.DisplayOrder, 100) ,  
    ISNULL(CONVERT(varchar(50), cs.CodeSheetID), 'ZZ'),  
    c.CodeId,  
    (CASE WHEN  @LocalIncludeReportGroup = 1 THEN ISNULL(rpts.DisplayName, '') ELSE '' END),  
    (CASE WHEN  @LocalIncludeReportGroup = 1 THEN ISNULL(rpts.DisplayOrder, @MaxDisplayOrder) ELSE 0 END) ,  
    (CASE WHEN cct.CategoryCodeTypeName IN ('Qualified Free', 'Qualified Paid') THEN 'Qualified' ELSE 'NonQualified' END),  
    colTotalUniqueRespondents  
      
  END  
  ELSE IF @LocalDemo <> 'SCF'  
  BEGIN  
   EXEC ('SELECT Sum(ps.Copies) Copies, c.DisplayName as ''Demo7'', ISNULL(NULLIF(NULLIF(CONVERT(VARCHAR(500),ps.[' + @LocalDemo + ']), ''''), ''0''), ''No Response'') as ''Row'',   
     1 as DisplayOrder, count(distinct ps.PubSubscriptionID) as RecordCount, c.CodeId as ''DemoID'' ' +  
     '(CASE WHEN cct.CategoryCodeTypeName in (''Qualified Free'', ''Qualified Paid'') THEN ''Qualified'' ELSE ''NonQualified'' END) as CategoryType, colTotalUniqueRespondents ' +  
     @colgrandTotalUniqueRespondents + 'as colgrandTotalUniqueRespondents' +  
     'FROM IssueArchiveProductSubscription ps WITH (NOLOCK) JOIN #Subscriptions s ON s.PubSubscriptionID = ps.PubSubscriptionID ' +   
     'JOIN UAD_Lookup..Code c WITH (NOLOCK) ON c.CodeValue = ps.demo7 and c.CodeTypeID = ' + @DeliverID +  
     'JOIN UAD_Lookup..CategoryCode cc WITH (NOLOCK) ON cc.CategoryCodeID = ps.PubCategoryID ' +  
     'JOIN UAD_Lookup..CategoryCodeType cct WITH (NOLOCK) ON cct.CategoryCodeTypeID = cc.CategoryCodeTypeID  
     join #tmpColUniqueCounts2 tmp on ps.demo7 = tmp.demo7  
     where Ps.IssueID = ' + @LocalIssueID + '  
     group by  
      c.DisplayName, ISNULL(NULLIF(NULLIF(CONVERT(VARCHAR(500),ps.[' + @LocalDemo + ']), ''''), ''0''), ''No Response'') ,  
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
    ps.IssueID = @LocalIssueID  
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


