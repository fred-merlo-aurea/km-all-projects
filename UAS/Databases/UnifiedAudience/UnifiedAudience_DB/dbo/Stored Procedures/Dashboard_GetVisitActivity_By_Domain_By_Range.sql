
CREATE PROC [dbo].[Dashboard_GetVisitActivity_By_Domain_By_Range]
(
	@startdate date = '',
	@endDate date = '',
	@DomainIDs varchar(800) = '',
	@brandID int = 0
	
)
as
Begin
	SET NOCOUNT ON;
	
	declare @tblDomains table (DomainTrackingID int)
	
	if @DomainIDs = ''
	Begin
		insert into @tblDomains
		select domaintrackingID from DomainTracking
	End
	Else
	Begin
		insert into @tblDomains
		select items from dbo.fn_Split(@domainIDs, ',')
	End
	
	if @startdate = null or @startdate = ''
	Begin
		set @startdate = DateAdd(day, -7, getdate()) 
	End
	
	if @endDate = null or @endDate = ''
	Begin
		set @endDate = GETDATE()
	End
	

	select  'dt' + Convert(varchar(20), s.DomainTrackingID) as DomainTrackingID, s.Date, isnull(SUM([Counts]),0)  as [Counts]
	from 
			dbo.[Summary_Data] s with (NOLOCK) join
			@tblDomains t on s.DomainTrackingID = t.DomainTrackingID
	where
			entityName='SubscriberVisitActivity' and [Type]='new' and s.Date between @startdate and @endDate and isnull(brandID,0) = @brandID
	group by s.DomainTrackingID, s.Date
	order by 1, 2
	
End
GO