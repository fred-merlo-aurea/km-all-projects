CREATE PROCEDURE [dbo].[spEmailsDeliveredByPercentage]
(@fromdate date,  
 @todate date,
 @customerID int )

AS
BEGIN 	

		declare @distinctsent int
		set @distinctsent = 0
		
		create table #tempA 
		(   
			EmailID int,
			SendCount int
		)
		
		insert into #tempA  
		select emailID, COUNT(b.BlastID)  
		from ecn5_communicator..blast b join ecn_activity..BlastActivitySends bas on b.BlastID = bas.BlastID 
		where 
				CustomerID = @customerID 
				and CAST(b.sendtime as date) between @fromdate and @todate 
				and TestBlast = 'N'
		group by  emailID

		select @distinctsent = count(distinct emailID) from #tempA
		

		select t.range as [range], count(*) as TotalCount, (convert(decimal(18,2),count(*)) * 100 / convert(decimal(18,2),@distinctsent)) as Percentage
		from (
		  select case  
			when SendCount between 1 and 9 then '1+'
			when SendCount between 10 and 19 then '10+'
			when SendCount between 20 and 29 then '20+'
			when SendCount between 30 and 39 then '30+'
			when SendCount between 40 and 49 then '40+'
			when SendCount between 50 and 59 then '50+'
			when SendCount between 60 and 69 then '60+'
			when SendCount between 70 and 79 then '70+'
			when SendCount between 80 and 89 then '80+'  
			when SendCount between 90 and 99 then '90+'
			else '100' end as range
		  from #tempA ) t
		group by t.range		
		ORDER BY CAST(REPLACE(t.range, '+', '') AS INTEGER) ASC
				
	 drop table #tempA  

END