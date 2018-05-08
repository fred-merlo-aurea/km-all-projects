-- Procedure
CREATE procedure [dbo].[rpt_DigitalEditionBilling]
(	
	@Month int,
	@year int
)
as

Begin
	Set nocount on
	
	declare @stdt datetime,
			@eddt datetime

	if @month > 0 and @year > 0
	Begin
		set @stdt = Convert(datetime, convert(varchar,@month) + '/' + '01/' + convert(varchar,@year))
		set @eddt = dateadd(s, -1, Dateadd(m,1,Convert(datetime,@stdt)))
	end
	else
	Begin
		set @stdt = Convert(datetime, convert(varchar,month(getdate()))+ '/' + '01/' + convert(varchar,year(getdate())))
		set @eddt = dateadd(s, -1, Dateadd(m,1,Convert(datetime,@stdt)))
	end

	select	b.basechannelID, b.basechannelName, c.customerID, c.customerName, p.publicationID, p.publicationName, e.editionID, e.editionName, Status, e.pages, inn.*
	from	ecn5_publisher..[EDITION] e join 
			ecn5_publisher..[PUBLICATION] p on e.publicationID = p.publicationID join
			[Customer] c on p.customerID = c.customerID join
			[BaseChannel] b on b.basechannelID = c.basechannelID join
		(
			select	editionID, 
					convert(varchar(10),max(activatedDate),101) as activatedDate, 
					convert(varchar(10),max(ArchievedDate),101) as ArchievedDate, 
					convert(varchar(10),max(DeactivatedDate),101)  as DeactivatedDate  from ecn5_publisher..editionhistory
			where 
					(ActivatedDate between @stdt and @eddt) or
					(ArchievedDate between @stdt and @eddt) or 
					(DeActivatedDate between @stdt and @eddt) or 
					(ActivatedDate < @stdt and DeActivatedDate >= @stdt) or 
					(ActivatedDate < @stdt and isnull(DeActivatedDate,'') = '') or 
					(ArchievedDate < @stdt and isnull(DeActivatedDate,'') = '') 
			group by editionID
		) inn on e.editionID = inn.editionID
	order by c.customerName
End
