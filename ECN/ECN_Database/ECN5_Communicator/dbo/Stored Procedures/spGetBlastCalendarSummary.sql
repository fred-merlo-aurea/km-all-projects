CREATE PROCEDURE [dbo].[spGetBlastCalendarSummary]
	@CustomerID int, 
	@ChannelID int,
	@UserID int
AS
BEGIN
	SELECT
		 convert(datetime,convert(varchar(10),b.SendTime,101)) as 'SendDate', 
		 COUNT(case when b.StatusCode = 'sent' then ISNULL(b.SendTotal,0) else 0 end) as 'SentTotal', 
		 COUNT(case when b.StatusCode = 'pending' then ISNULL(b.SendTotal,0) else 0 end) as 'Pending',      
		 COUNT(case when b.StatusCode = 'active' then ISNULL(b.SendTotal,0) else 0 end) as 'Active'    
		 FROM 
		 [ECN5_ACCOUNTS].[DBO].[BASECHANNEL] ch 
		 join [ECN5_ACCOUNTS].[DBO].[CUSTOMER]  cu on ch.BaseChannelID = cu.BaseChannelID
		 join [BLAST] b on b.CustomerID = cu.CustomerID
	WHERE 
		 ch.BaseChannelID = @ChannelID
	GROUP BY
		 convert(datetime,convert(varchar(10),b.SendTime,101))   	
	ORDER BY 
		 convert(datetime,convert(varchar(10),b.SendTime,101)) DESC 
END
