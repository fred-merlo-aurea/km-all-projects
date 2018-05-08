CREATE proc [dbo].[sp_rptSubFields]    
(    
 @ReportID int,  
 @ResponseID int,
 @Filters TEXT,
 @PrintColumns varchar(4000),
 @Download char(1)       
)    
as     
Begin    

	declare	@publicationID int,  
			@count int,
			@row varchar(100)

	set nocount on
	
	if len(ltrim(rtrim(@PrintColumns))) > 0 
	Begin
		set @PrintColumns  = ', ' + @PrintColumns 
	end

	set @count = 0

	create table #SubscriptionID (SubscriptionID int, copies int)  
	select @publicationID = publicationID, @row = [ROW]  from PublicationReports where reportID = @reportID 
  
	declare @PublicationCode varchar(20)
	select @PublicationCode  = Publicationcode from Publication where PublicationID = @PublicationID


	Insert into #SubscriptionID   
	 exec sp_getSubscribers_using_XMLFilters @publicationID, @Filters, 1
		 

	CREATE UNIQUE CLUSTERED INDEX IX_1 on #SubscriptionID (SubscriptionID)

	create table #responseID (responseID VARCHAR(10), responsevalue varchar(100)) 
	
	if @row = 'DEMO7'
	Begin
		select     
			d.deliverabilitycode as responseID, case when DeliverabilityCode = 'A' then 'A. Print Edition' when DeliverabilityCode = 'B' then 'B. Digital Edition' when DeliverabilityCode = 'C' then 'C. Both' end as responsevalue, SUM(SF.COPIES) as counts 
		From    
			Subscription s join
			#SubscriptionID sf on sf.SubscriptionID = s.SubscriptionID join 
			Deliverability d on d.DeliverabilityID  = s.DeliverabilityID
		group by       
			d.deliverabilitycode,  
			d.deliverabilityName
		order by responsevalue
			
	END
	if @row = 'DEMO3' or @row = 'DEMO4' or @row = 'DEMO5' 
	Begin
		insert into #responseID values ('Y', 'Yes')
		insert into #responseID values ('N', 'No')
	END
	if @row = 'DEMO31' or @row = 'DEMO32' or @row = 'DEMO33' or @row = 'DEMO34' or @row = 'DEMO35' 
	Begin
		insert into #responseID values ('1', 'Yes')
		insert into #responseID values ('0', 'No')
	END
	
	declare @Sqlstmt varchar(MAX)
	
	set @Sqlstmt = '
				select r.responseID,r.responsevalue, SUM(SF.COPIES) as counts 
				from subscription s join #SubscriptionID sf on sf.SubscriptionID = s.SubscriptionID right outer join 
					#responseID r on s.' + @row + ' = r.responseID 
				where s.magazineID = '+ cast(@PublicationID as varchar(50))+ '
				group by r.responseID,r.responsevalue order by r.responseID,r.responsevalue'
	exec(@Sqlstmt)	

	drop table #SubscriptionID 
End