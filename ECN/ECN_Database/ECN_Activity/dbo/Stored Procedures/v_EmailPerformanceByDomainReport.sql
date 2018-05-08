CREATE  proc [dbo].[v_EmailPerformanceByDomainReport]  
(  
  @customerID int,  
  @startdate date,  
  @enddate date,
  @DrillDownOther bit = 0
)
as  
  
Begin  

set nocount on
  
declare @domain table (domain varchar(100))

insert into @domain values ('hotmail.com'), ('yahoo.com'), ('yahoo.ca'), ('gmail.com'), ('comcast.net'), ('live.com'), ('aol.com'), ('verizon.net'), ('msn.com'), ('earthlink.net')
, ('att.net'), ('cox.net'), ('bellsouth.net'), ('juno.com'), ('netzero.net'), ('pacbell.net') , ('sbcglobal.net') 


declare @blasts table (blastID int)

insert into @blasts
select b.blastID from ecn5_communicator..blast b with (NOLOCK) 
where b.customerID = @customerID 
		and CAST(b.sendtime as date) between @startdate and @enddate 
		and statuscode = 'sent' and b.testblast='N'

declare @BlastActivity Table (BlastID int, domain varchar(100), tcounts int, ucounts int, actiontypecode varchar(50))

	insert into @BlastActivity
	SELECT bas.BlastID, SUBSTRING(emailaddress, charindex('@', emailaddress) + 1, len(emailaddress)), COUNT(bas.SendID), COUNT(DISTINCT bas.EmailID), 'send'
		FROM BlastActivitySends bas WITH (NOLOCK) join @blasts b on bas.BlastID = b.blastID  join ecn5_communicator..Emails e on e.EmailID = bas.EmailID
		GROUP BY bas.BlastID  , SUBSTRING(emailaddress, charindex('@', emailaddress) + 1, len(emailaddress)) 
	UNION 		
	SELECT bao.BlastID, SUBSTRING(emailaddress, charindex('@', emailaddress) + 1, len(emailaddress)) , COUNT(bao.OpenID), COUNT(DISTINCT bao.EmailID),'open'
		FROM BlastActivityOpens bao WITH (NOLOCK) join @blasts b on bao.BlastID = b.blastID join ecn5_communicator..Emails e on e.EmailID = bao.EmailID
		GROUP BY bao.BlastID  , SUBSTRING(emailaddress, charindex('@', emailaddress) + 1, len(emailaddress)) 
	UNION 
		SELECT	bab.BlastID, SUBSTRING(emailaddress, charindex('@', emailaddress) + 1, len(emailaddress)), COUNT(bab.BounceID), COUNT(DISTINCT bab.EmailID), 'bounce'
		FROM BlastActivityBounces bab WITH (NOLOCK) join @blasts b on bab.BlastID = b.blastID
		 join ecn5_communicator..Emails e on e.EmailID = bab.EmailID
		GROUP BY bab.BlastID, SUBSTRING(emailaddress, charindex('@', emailaddress) + 1, len(emailaddress))   
	UNION 
		SELECT bau.BlastID, SUBSTRING(emailaddress, charindex('@', emailaddress) + 1, len(emailaddress)), COUNT(bau.UnsubscribeID), COUNT(DISTINCT bau.EmailID), 'subscribe' 
		FROM BlastActivityUnSubscribes bau WITH (NOLOCK) JOIN UnsubscribeCodes uc WITH (NOLOCK) on bau.UnsubscribeCodeID = uc.UnsubscribeCodeID join @blasts b on bau.BlastID = b.blastID
		 join ecn5_communicator..Emails e on e.EmailID = bau.EmailID
		WHERE uc.UnsubscribeCode = 'subscribe'
	GROUP BY bau.BlastID, SUBSTRING(emailaddress, charindex('@', emailaddress) + 1, len(emailaddress))
	UNION
	SELECT bac.BlastID, SUBSTRING(emailaddress, charindex('@', emailaddress) + 1, len(emailaddress)), COUNT(bac.ClickID), COUNT(DISTINCT bac.EmailID), 'click' 
	FROM BlastActivityClicks bac WITH (NOLOCK) join @blasts b on bac.BlastID = b.blastID  join ecn5_communicator..Emails e on e.EmailID = bac.EmailID
	group by bac.BlastID, SUBSTRING(emailaddress, charindex('@', emailaddress) + 1, len(emailaddress))
  	  
  	
  	SELECT *
	INTO #Temp
	FROM 
  	  
		(select case when d.domain IS not null then d.domain else case when @DrillDownOther = 0 then 'Other' else ba.domain end end as Domain,
			sum(case when  actiontypecode='send' then ucounts else 0 end) as SendTotal,  
			sum(case when  actiontypecode='open' then ucounts else 0 end) as Opens,
			sum(case when  actiontypecode='click' then ucounts else 0 end) as Clicks,  
			sum(case when  actiontypecode='bounce' then ucounts else 0 end) as Bounce,  
			sum(case when  actiontypecode='subscribe' then ucounts else 0 end) as Unsubscribe,
			(sum(case when  actiontypecode='send' then ucounts else 0 end) - 
			sum(case when  actiontypecode='bounce' then ucounts else 0 end)) as  Delivered 
		 from  
		 @BlastActivity ba 
		 join @blasts b1 on ba.blastID = b1.blastID 
		 left outer join @domain d on ba.domain = d.domain
		 group by case when d.domain IS not null then d.domain else case when @DrillDownOther = 0 then 'Other' else ba.domain end end) data
	if @DrillDownOther = 1
		SELECT top 50 * FROM #Temp order by SendTotal DESC
	ELSE 
		SELECT * FROM #Temp
	
	drop table #Temp
 
END