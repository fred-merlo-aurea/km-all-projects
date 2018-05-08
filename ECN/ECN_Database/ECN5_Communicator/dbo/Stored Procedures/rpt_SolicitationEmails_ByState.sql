﻿CREATE PROCEDURE [rpt_SolicitationEmails_ByState]
	@from datetime, 
	@to datetime
AS
BEGIN
	SET @from = @from + ' 00:00:00'
	SET @to = @to + ' 23:59:00'
	
	DECLARE @tblSolicitationEmails table (Charity varchar(250), MonthYear varchar(250), Emails int, EmailType varchar(50), ECNGroupID int)  
	
	INSERT INTO @tblSolicitationEmails
	SELECT
	   	c.CustomerName as 'Charity',  
		CONVERT(varchar, MONTH(SendTime)) + '/' + CONVERT(varchar, YEAR(SendTime)) as 'MonthYear', 
		SUM(sendtotal) as 'Emails', 'SOLICITATIONS' as 'EmailType', b.GroupID as 'ECNGroupID'
	FROM
		blasts b 
		join ECN5_ACCOUNTS..Customers c on c.CustomerID = b.CustomerID
		join Filters f on f.FilterID = b.FilterID and f.CustomerID = b.CustomerID
	WHERE 
		c.customerID in (91,112,1511,1060,1309,2461,2661,1939,2385,1059,2590,3165,1897,3175,3183,3199,3249,3267,3241,3314,3333,3334,3354,3436,3510) 
		and SendTime between @from and @to and b.TestBlast = 'N'
		and f.FilterName like '%Mailing Schedule%'
	GROUP BY
		c.CustomerID,c.CustomerName,YEAR(SendTime), MONTH(SendTime), b.GroupID  
	ORDER BY
		c.CustomerName
		
	-- for Northwest center solicitation emails are defined by their category
	INSERT INTO @tblSolicitationEmails	
	SELECT
		c.CustomerName as 'Charity',  
		CONVERT(varchar, MONTH(SendTime)) + '/' + CONVERT(varchar, YEAR(SendTime)) as 'MonthYear', 
		SUM(sendtotal) as 'Emails', 'SOLICITATIONS' as 'EmailType', b.GroupID as 'ECNGroupID'
	FROM
		blasts b 
		join ECN5_ACCOUNTS..Customers c on c.CustomerID = b.CustomerID
		join Filters f on f.FilterID = b.FilterID and f.CustomerID = b.CustomerID		
	WHERE 
		c.customerID = 2843
		and SendTime between '05/01/2012' and '05/31/2012' and b.TestBlast = 'N'
		and b.CodeID = 334
	GROUP BY
		c.CustomerID,c.CustomerName,YEAR(SendTime), MONTH(SendTime), b.GroupID  
	ORDER BY
		c.CustomerName
			
	-- for purple heart PA
	INSERT INTO @tblSolicitationEmails
	SELECT 'Purple Heart (PA Office)' as Charity,  
			CONVERT(varchar, MONTH(SendTime)) + '/' + CONVERT(varchar, YEAR(SendTime)) as 'MonthYear', 		
			SUM(case when f.FilterName like '%Mailing Schedule(PA)%' then b.SendTotal end) as 'Emails',
			'SOLICITATIONS' as 'EmailType', b.GroupID as 'ECNGroupID'				
	FROM 
			Blasts b left join Filters f on b.CustomerID = f.CustomerID and b.FilterID = f.FilterID
	WHERE 
			b.CustomerID = 2908 and b.SendTime between @from and @to and b.TestBlast = 'N'
	GROUP BY
			YEAR(SendTime), MONTH(SendTime), b.GroupID   
			
	
	-- for purple heart MD
	INSERT INTO @tblSolicitationEmails
	SELECT 'Purple Heart (MD Office)' as Charity,  
			CONVERT(varchar, MONTH(SendTime)) + '/' + CONVERT(varchar, YEAR(SendTime)) as 'MonthYear', 		
			SUM(case when f.FilterName like '%Mailing Schedule(MD)%' then b.SendTotal end) as 'Emails',
			'SOLICITATIONS' as 'EmailType', b.GroupID as 'ECNGroupID'							
	FROM 
			Blasts b left join Filters f on b.CustomerID = f.CustomerID and b.FilterID = f.FilterID
	WHERE 
			b.CustomerID = 2908 and b.SendTime between @from and @to and b.TestBlast = 'N'
	GROUP BY
			YEAR(SendTime), MONTH(SendTime), b.GroupID   			
			
	
	-- get the reminders and confirmations from ecn
	
	INSERT INTO @tblSolicitationEmails
	SELECT
		c.CustomerName as 'Charity',  
		CONVERT(varchar, MONTH(SendTime)) + '/' + CONVERT(varchar, YEAR(SendTime)) as 'MonthYear', 
		SUM(sendtotal) as 'Emails', 'REMINDERS' as 'EmailType', b.GroupID as 'ECNGroupID'	
	FROM
		blasts b 
		join ECN5_ACCOUNTS..Customers c on c.CustomerID = b.CustomerID
		join Filters f on f.FilterID = b.FilterID and f.CustomerID = b.CustomerID
	WHERE 
		c.customerID in (91,112,159,1511,1060,1309,2461,2661,1939,2385,2843,1059,2590,3165,1897,3175,3183,3199,3249,3267,3241,3314,3333,3334,3354,3436,3510) 
		and SendTime between @from and @to and b.TestBlast = 'N'
		and f.FilterName like '%reminder%'
	GROUP BY
		c.CustomerID,c.CustomerName,YEAR(SendTime), MONTH(SendTime), b.GroupID   
	ORDER BY
		c.CustomerName 
	
	
	--reminders purple heart pa
	INSERT INTO @tblSolicitationEmails
	SELECT 'Purple Heart (PA Office)' as Charity,  
			CONVERT(varchar, MONTH(SendTime)) + '/' + CONVERT(varchar, YEAR(SendTime)) as 'MonthYear', 		
			SUM(case when f.FilterName like '%Reminder Email PA%' then b.SendTotal end) as 'Emails',
			'REMINDERS' as 'EmailType', b.GroupID as 'ECNGroupID'				
	FROM 
			Blasts b left join Filters f on b.CustomerID = f.CustomerID and b.FilterID = f.FilterID
	WHERE 
			b.CustomerID = 2908 and b.SendTime between @from and @to and b.TestBlast = 'N'
	GROUP BY
			YEAR(SendTime), MONTH(SendTime), b.GroupID   
				
	
	-- reminders purple heart MD
	INSERT INTO @tblSolicitationEmails
	SELECT 'Purple Heart (MD Office)' as Charity,  
			CONVERT(varchar, MONTH(SendTime)) + '/' + CONVERT(varchar, YEAR(SendTime)) as 'MonthYear', 		
			SUM(case when f.FilterName like '%Reminder Email MD%' then b.SendTotal end) as 'Emails',
			'REMINDERS' as 'EmailType', b.GroupID as 'ECNGroupID'							
	FROM 
			Blasts b left join Filters f on b.CustomerID = f.CustomerID and b.FilterID = f.FilterID
	WHERE 
			b.CustomerID = 2908 and b.SendTime between @from and @to and b.TestBlast = 'N'
	GROUP BY
			YEAR(SendTime), MONTH(SendTime), b.GroupID   
			
			
	--solicitations for lfa and lupus az			
	INSERT INTO @tblSolicitationEmails
	SELECT 'Lupus Foundation of America' as Charity,  
			CONVERT(varchar, MONTH(SendTime)) + '/' + CONVERT(varchar, YEAR(SendTime)) as 'MonthYear', 		
			SUM(case when f.FilterName like '%Mailing Schedule%' then b.SendTotal end) as 'Emails',
			'SOLICITATIONS' as 'EmailType', b.GroupID as 'ECNGroupID'							
	FROM 
			Blasts b left join Filters f on b.CustomerID = f.CustomerID and b.FilterID = f.FilterID
	WHERE 
			b.CustomerID = 159 and b.SendTime between @from and @to and b.TestBlast = 'N'
	GROUP BY
			YEAR(SendTime), MONTH(SendTime), b.GroupID 
			
	declare @lupusazemailcount int
	select @lupusazemailcount = COUNT(distinct e.EmailID) from Blasts b JOIN Emails e ON e.CustomerID = b.CustomerID 	
	join EmailGroups eg on eg.EmailID = e.EmailID 
	where b.CustomerID = 159 and eg.GroupID = 2683 and eg.SubscribeTypeCode = 'S' and e.State = 'AZ'			
	and b.SendTime between @from and @to and b.TestBlast = 'N'
	
	update @tblSolicitationEmails set Emails = Emails - @lupusazemailcount where ECNGroupID = 2683
	
	INSERT INTO @tblSolicitationEmails
	SELECT 'Lupus Foundation of America (AZ)' as Charity,  
			CONVERT(varchar, MONTH(@from)) + '/' + CONVERT(varchar, YEAR(@from)) as 'MonthYear', 		
			@lupusazemailcount as 'Emails',
			'SOLICITATIONS' as 'EmailType', 2683 as 'ECNGroupID'								
	
	
	SELECT case when CHARINDEX('-',Charity) = 0 then Charity else  SUBSTRING(Charity,0,CHARINDEX('-',Charity)) end as 'Charity', MonthYear, Emails,EmailType,ECNGroupID 
	FROM @tblSolicitationEmails order by Charity asc
END
