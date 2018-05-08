CREATE proc [dbo].[sp_GetBlastReportComparision]
(
	@blastIDs  XML
)
as
    
Begin        

create table #Blasts (bID INT)
 
Create table #BlastData 
       (
       ActionTypeCode VARCHAR,
       BlastID VARCHAR,
       DistinctCount int,
       SendTime DATETIME,
       EmailSubject VARCHAR,
       TotalCount INT,
       TotalSent INT,
       Perc FLOAT)
 
INSERT INTO #BlastData
SELECT '',bID,null,null,null,null,null,null
FROM #blasts
 
INSERT INTO #blasts
SELECT BlastValues.ID.value('./@BlastID','INT')
FROM @blastIDs.nodes('/Blasts') AS BlastValues(ID)
 
SELECT
       c.ActionTypeCode,
       CAST(c.BlastID AS VARCHAR) AS 'BlastID' ,
       c.DistinctCount,
       c.SendTime,
       c.EmailSubject,
       c.total AS 'TotalCount',
       d.total AS 'TotalSent',
       case 
       when d.total > 0
	       then isnull(ROUND(((CONVERT(FLOAT,c.DistinctCount)/d.total)*100),2),0)
		when
			ISNULL(d.Total,0) = 0
			then 0
			end as 'Perc'
FROM 
(
       SELECT
              a.ActionTypeCode ,
              a.BlastID,
              a.DistinctCount,
              a.total,
              b.SendTime,
              b.EmailSubject,
              DATENAME(MONTH, b.SendTime) AS 'Month',
              DATEPART(YEAR, b.SendTime) AS 'Year'
       FROM (
              SELECT 
                     ISNULL(SUM(DistinctCount),0) AS DistinctCount,
                     ISNULL(SUM(total),0) AS total,
                     'click'  AS ActionTypeCode ,
                     inn.BlastID   
              FROM (       
                     SELECT 
                           ISNULL(COUNT(distinct URL),0) AS DistinctCount,
                           ISNULL(COUNT(bac.EmailID),0) AS total,
                           b.bID as BlastID       
                     FROM
                           BlastActivityClicks bac WITH (NOLOCK)
                           RIGHT JOIN #blasts b ON bac.BlastID = b.bID
                     GROUP BY 
                           URL,
                           bac.EmailID ,
                           b.bID     
                     ) AS inn
              GROUP BY inn.BlastID       
 
              UNION 
 
              SELECT
                     ISNULL(COUNT( DISTINCT EmailID),0) AS DistinctCount,
                     ISNULL(COUNT(EmailID),0) AS total,
                     'open' AS ActionTypeCode,
                     b.bID as BlastID
              FROM
                     BlastActivityOpens WITH (NOLOCK)
                     RIGHT JOIN #blasts b ON BlastID = b.bID
              GROUP BY 
                     b.bID
 
              UNION
 
              SELECT
                     ISNULL(COUNT( DISTINCT EmailID),0) AS DistinctCount,
                     ISNULL(COUNT(EmailID),0) AS total,
                     'bounce' AS ActionTypeCode,
                     b.bID AS BlastID
             FROM
                     BlastActivityBounces WITH (NOLOCK) 
                     RIGHT JOIN #blasts b ON BlastID = b.bID
              GROUP BY 
                     b.bID
          
              UNION
             
              SELECT 
                     ISNULL(COUNT(DISTINCT bau.EmailID),0) AS DistinctCount,
                     ISNULL(COUNT(bau.EmailID),0) AS total,
                     'complaint' AS ActionTypeCode,
                     b.bID AS BlastID
              FROM 
					#Blasts b
					left outer join BlastActivityUnSubscribes bau WITH (NOLOCK) on b.bID = bau.BlastID and bau.UnsubscribeCodeID in (2,4)
                    left outer JOIN UnsubscribeCodes uc WITH (NOLOCK) ON bau.UnsubscribeCodeID = uc.UnsubscribeCodeID
              GROUP BY
                     b.bID
 
              UNION
             
              SELECT 
                     ISNULL(COUNT(DISTINCT bau.EmailID),0) AS DistinctCount,
                     ISNULL(COUNT(bau.EmailID),0) AS total,
                     ISNULL(uc.UnsubscribeCode,'subscribe')  AS ActionTypeCode,
                     b.bID As BlastID
              FROM 
					#Blasts b
					left outer join BlastActivityUnSubscribes bau WITH (NOLOCK) on b.bID = bau.BlastID and bau.UnsubscribeCodeID = 3
                    left outer JOIN UnsubscribeCodes uc WITH (NOLOCK) ON bau.UnsubscribeCodeID = uc.UnsubscribeCodeID
              GROUP BY
                     uc.UnsubscribeCode,b.bID
              ) a
       INNER JOIN ecn5_communicator.dbo.[BLAST] AS b ON a.BlastID=b.BlastID) c
       LEFT OUTER JOIN (
              SELECT
                     ISNULL(COUNT( DISTINCT EmailID),0) AS DistinctCount,
                     ISNULL(COUNT(EmailID),0) AS total,
                     'send' AS ActionTypeCode ,
                     b.bID as BlastID
              FROM
                     BlastActivitySends WITH (NOLOCK)
                     RIGHT JOIN #blasts b ON BlastID = b.bID
              GROUP BY 
                     b.bID
                     ) d 
                     ON c.BlastID=d.BlastID
              END
              
              drop table #BlastData
              drop table #Blasts
