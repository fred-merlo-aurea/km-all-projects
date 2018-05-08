CREATE proc [dbo].[sp_rpt_SubsrcReport]
(  
	@magazineID int,
	@Filters TEXT,
	@PrintColumns varchar(4000),
	@Download char(1)         
)
as   
BEGIN

	SET NOCOUNT ON
	
	declare @publicationID int
	
	set @publicationID = @MagazineID
	
	declare @PublicationCode varchar(20)
	select @PublicationCode  = Publicationcode 
	from Publication 
	where PublicationID = @PublicationID

	if len(ltrim(rtrim(@PrintColumns))) > 0 
		Begin
			set @PrintColumns  = ', ' + @PrintColumns 
		end

	create table #SubscriptionID (SubscriptionID int, copies int)  

	Insert into #SubscriptionID   
	exec sp_getSubscribers_using_XMLFilters @PublicationID, @Filters, 1
		
	CREATE UNIQUE CLUSTERED INDEX IX_1 on #SubscriptionID (SubscriptionID)


	select s.OriginalSubscriberSourceCode as Subsrc, SUM(s.copies) as counts 
	from subscription s 
		join #SubscriptionID sf on sf.SubscriptionID = s.SubscriptionID
	where s.PublicationID = @magazineID
	group by OriginalSubscriberSourceCode
	order by OriginalSubscriberSourceCode

	drop table #SubscriptionID 

end