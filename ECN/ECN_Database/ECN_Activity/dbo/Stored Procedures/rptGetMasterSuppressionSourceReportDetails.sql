CREATE PROCEDURE [dbo].[rptGetMasterSuppressionSourceReportDetails](
	@CustomerID INT, 
	@UnsubscribeCode VARCHAR(50),
	@PageSize INT, 
	@CurrentPage INT, 
	@SortDirection varchar(50), 
	@SortedColumn varchar(100), 
	@FromDate date, 
	@ToDate date
	)
AS
--DECLARE
--@CustomerID INT = 1, 
----@UnsubscribeCode VARCHAR(50) = 'MASTSUP_UNSUB',
--@UnsubscribeCode VARCHAR(50) = 'All',
--@PageSize INT = 100, 
--@CurrentPage INT = 1, 
--@SortDirection varchar(50) = 'ASC', 
--@SortedColumn varchar(100) = 'EmailAddress', 
--@FromDate DATETIME = '1/27/2015', 
--@ToDate DATETIME = '4/27/2015'	

SET NOCOUNT ON
DECLARE @loc_ToDate DATETIME
DECLARE @loc_FromDate DATETIME
DECLARE @loc_MasterSuppressionGroupID INT
SELECT @loc_MasterSuppressionGroupID = groupID 
      FROM ECN5_COMMUNICATOR..Groups 
      WHERE CustomerID = @customerID 
      AND MasterSupression = 1 

	SET @loc_ToDate = DATEADD(SECOND,86399, CAST(@ToDate as DateTime))
	SET @loc_FromDate = @FromDate

BEGIN
      
      WITH Results
      AS (SELECT ROW_NUMBER() OVER (ORDER BY
            CASE WHEN (@SortedColumn = 'EmailAddress' AND @SortDirection='Ascending') THEN emailaddress END ASC,
            CASE WHEN (@SortedColumn = 'EmailAddress' AND @SortDirection='Descending') THEN emailaddress END DESC,
            CASE WHEN (@SortedColumn = 'UnsubscribeDateTime' AND @SortDirection='Ascending') THEN UnsubscribeTime END ASC,
            CASE WHEN (@SortedColumn = 'UnsubscribeDateTime' AND @SortDirection='Descending') THEN UnsubscribeTime END DESC       
    ) AS ROWNUM,
    Count(*) over () AS TotalCount
            ----
                        ,UnsubscribeCode, 
                        EmailAddress,
                        bs.Comments AS 'Reason', 
                       MAX(ISNULL(bs.UnsubscribeTime, ISNULL(eg.LastChanged, eg.CreatedOn)))  AS UnsubscribeDateTime, 
                        eg.GroupID
                  FROM 
                        ECN5_COMMUNICATOR.dbo.EmailGroups eg WITH(NOLOCK)
                        JOIN ECN5_COMMUNICATOR.dbo.Emails e WITH(NOLOCK) ON e.EmailID = eg.EmailID 
                        LEFT OUTER JOIN BlastActivityUnSubscribes bs WITH(NOLOCK) ON eg.EmailID = bs.EmailID 
                        LEFT OUTER JOIN UnsubscribeCodes uc WITH(NOLOCK) ON uc.UnsubscribeCodeID = bs.UnsubscribeCodeID 
                  WHERE
                        GroupID = @loc_MasterSuppressionGroupID AND                       
                      cast(  ISNULL(bs.UnsubscribeTime, ISNULL(eg.LastChanged, eg.CreatedOn)) as date) BETWEEN @loc_FromDate AND @loc_ToDate 
                        AND ISNULL(uc.UnsubscribeCode,'') = CASE WHEN @UnsubscribeCode = 'All' then ISNULL(uc.UnsubscribeCode,'')
																WHEN @UnsubscribeCode = 'Blank' then ''
																ELSE @UnsubscribeCode END
                       
                  GROUP BY 
                      UnsubscribeCode, 
                        EmailAddress,
                        Comments,                     
                        GroupID,
                        UnsubscribeTime
            ----
      )
      SELECT *
      FROM Results
      WHERE ROWNUM between ((@CurrentPage - 1) * @PageSize + 1) and (@CurrentPage * @PageSize)
            
END