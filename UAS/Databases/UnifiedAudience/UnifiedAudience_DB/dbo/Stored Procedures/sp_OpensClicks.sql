CREATE PROCEDURE [dbo].[sp_OpensClicks]
	
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;

	 select --MONTH, Week, 
		monthdt, yeardt,weekdt,
		sum(case when type='open' then totalClickcounts end) as totalOpencounts,
		sum(case when type='click' then totalClickcounts end) as totalClickcounts
	from (

	select 'click' as type, --DATENAME(MONTH, activitydate)+' '+CAST(YEAR(activitydate) as varchar) as 'Month', 'Week '+CAST(DATEPART(WK, activitydate) as varchar)+' '+CAST(YEAR(activitydate) as varchar) as 'Week', 
				 MONTH(activitydate) as monthdt, YEAR(activitydate) as yeardt, DATEPART(WK, activitydate) as weekdt, COUNT(ClickActivityID) as totalClickCounts  
				 from SubscriberClickActivity s join PubSubscriptions ps on s.PubSubscriptionID = ps.PubSubscriptionID 
				 group by MONTH(activitydate),YEAR(activitydate), DATEPART(WK, activitydate)
				 --, DATENAME(MONTH, activitydate)+' '+CAST(YEAR(activitydate) as varchar),'Week '+CAST(DATEPART(WK, activitydate) as varchar)+' '+CAST(YEAR(activitydate) as varchar) 
             
				 union 
                              
	select 'open' as type, --DATENAME(MONTH, activitydate)+' '+CAST(YEAR(activitydate) as varchar) as 'Month', 'Week '+CAST(DATEPART(WK, activitydate) as varchar)+' '+CAST(YEAR(activitydate) as varchar) as 'Week', 
				 MONTH(activitydate) as monthdt, YEAR(activitydate) as yeardt, DATEPART(WK, activitydate) as weekdt,  COUNT(OpenActivityID) as 'totalOpenCounts'  
				 from SubscriberOpenActivity s join PubSubscriptions ps on s.PubSubscriptionID = ps.PubSubscriptionID 
				 group by MONTH(activitydate),YEAR(activitydate), DATEPART(WK, activitydate)
				 --, DATENAME(MONTH, activitydate)+' '+CAST(YEAR(activitydate) as varchar),'Week '+CAST(DATEPART(WK, activitydate) as varchar)+' '+CAST(YEAR(activitydate) as varchar) 
             
    ) inn
	group by                               
			   --MONTH,Week, 
			   monthdt, yeardt,weekdt    
	order by 2,1,3

END