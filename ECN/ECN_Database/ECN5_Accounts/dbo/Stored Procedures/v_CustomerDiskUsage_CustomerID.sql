CREATE PROCEDURE [dbo].[v_CustomerDiskUsage_CustomerID]   
(  
@CustomerID int = NULL  
)  
AS  
  
SELECT cd.ChannelID, Cd.customerID, C.customerName,   
Convert(varchar(10),( SELECT ISNULL(SUM(Quantity * (CASE WHEN CONVERT(INT,(REPLACE(REPLACE(Code,'OSLIB',''),'MB',''))) = '' THEN 25 ELSE CONVERT(INT,(REPLACE(REPLACE(Code,'OSLIB',''),'MB',''))) end)),0) + 10  
FROM QuoteItem qi JOIN Quote q ON q.QuoteID = qi.QuoteID  
WHERE qi.Code LIKE 'OSLIB%' AND q.Status = '1' AND getDate() BETWEEN q.ApproveDate AND (q.ApproveDate+365) AND q.customerID = Cd.customerID  
)) AS AllowedStorage  
FROM CustomerDiskUsage CD JOIN Customer C ON CD.customerID = C.CustomerID  
WHERE C.CustomerID = @CustomerID  
GROUP BY cd.ChannelID, Cd.customerID, C.customerName  
HAVING AVG (CONVERT(DECIMAL(18,2),SizeInBytes)) > 0  

