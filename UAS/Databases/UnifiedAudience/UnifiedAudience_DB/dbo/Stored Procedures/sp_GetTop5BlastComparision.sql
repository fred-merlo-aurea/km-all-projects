CREATE proc [dbo].[sp_GetTop5BlastComparision]

as
    
BEGIN

	SET NOCOUNT ON;
  

	with cte (BlastID)
	as
	(
		 SELECT TOP 5 BlastID 
		 from [ecn5_communicator].[dbo].Blasts a with(nolock) 
			inner join [Pubs] as b with(nolock) on a.GroupID=b.GroupID
		where a.TestBlast='n' and a.StatusCode='sent'
		order by a.SendTime DESC
	)


	select c.ActionTypeCode,CAST(c.BlastID as varchar) as 'BlastID' ,c.DistinctCount,c.SendTime, c.EmailSubject,c.total as 'TotalCount', d.total as 'TotalSent',  DATENAME(MONTH,c.SendTime) as 'Month',DATEPART(YEAR, c.SendTime) as 'Year',ROUND(((CONVERT(float,c.DistinctCount)/d.total)*100),2)  'Perc' from  
		(
			Select a.ActionTypeCode ,a.BlastID, a.DistinctCount,a.total,b.SendTime, b.EmailSubject 
			from (
			SELECT  BlastID,
					ISNULL(SUM(DistinctCount),0) AS DistinctCount, 
					ISNULL(SUM(total),0) AS total,
					'click'  as ActionTypeCode        
			FROM (  SELECT  cte.BlastID,COUNT(distinct URL) AS DistinctCount, COUNT(bac.EmailID) AS total         
					FROM   [ecn_Activity].[dbo].BlastActivityClicks bac WITH (NOLOCK)	
						join cte on bac.BlastID=cte.BlastID		
			group by cte.BlastID, URL, bac.EmailID        
				) AS inn   
			group by BlastID  
			UNION
		
			SELECT cte.BlastID,ISNULL(COUNT( DISTINCT EmailID),0) AS DistinctCount, ISNULL(COUNT(EmailID),0) AS total, 'open' AS ActionTypeCode 
			FROM [ecn_Activity].[dbo].BlastActivityOpens bao WITH (NOLOCK)  
				join cte on bao.BlastID=cte.BlastID		
			group by cte.BlastID   
			UNION 
		
			SELECT cte.BlastID,ISNULL(COUNT( DISTINCT EmailID),0) AS DistinctCount, ISNULL(COUNT(EmailID),0) AS total, 'bounce' AS ActionTypeCode 
			FROM [ecn_Activity].[dbo].BlastActivityBounces bab WITH (NOLOCK)  
				join cte on bab.BlastID=cte.BlastID		
			group by cte.BlastID   
			UNION
		
			SELECT cte.BlastID,ISNULL(COUNT(DISTINCT bau.EmailID),0) AS DistinctCount,
					ISNULL(COUNT(bau.EmailID),0) AS total, 
					'complaint'   as ActionTypeCode
			FROM  [ecn_Activity].[dbo].BlastActivityUnSubscribes bau WITH (NOLOCK) 
				JOIN [ecn_Activity].[dbo].UnsubscribeCodes uc WITH (NOLOCK) ON bau.UnsubscribeCodeID = uc.UnsubscribeCodeID 
				join cte on bau.BlastID=cte.BlastID				
			where uc.UnsubscribeCodeID=2 OR  uc.UnsubscribeCodeID=4 		
			group by cte.BlastID ,uc.UnsubscribeCode
			UNION
		
			SELECT cte.BlastID,ISNULL(COUNT(DISTINCT bau.EmailID),0) AS DistinctCount,
					ISNULL(COUNT(bau.EmailID),0) AS total, 
					uc.UnsubscribeCode  as  ActionTypeCode
			FROM [ecn_Activity].[dbo].BlastActivityUnSubscribes bau WITH (NOLOCK) 
				JOIN [ecn_Activity].[dbo].UnsubscribeCodes uc WITH (NOLOCK) ON bau.UnsubscribeCodeID = uc.UnsubscribeCodeID 
				join cte on bau.BlastID=cte.BlastID				
			where uc.UnsubscribeCodeID=3   		
			group by cte.BlastID ,uc.UnsubscribeCode
		
		 ) a
		inner join ecn5_communicator.dbo.Blasts as b on a.BlastID=b.BlastID) c
		inner join (SELECT cte.BlastID, ISNULL(COUNT(EmailID),0) AS total, 'send' AS ActionTypeCode 
			FROM [ecn_Activity].[dbo].BlastActivitySends basd WITH (NOLOCK)  
				inner join cte on basd.BlastID=cte.BlastID		
			group by cte.BlastID) d on c.BlastID=d.BlastID

End