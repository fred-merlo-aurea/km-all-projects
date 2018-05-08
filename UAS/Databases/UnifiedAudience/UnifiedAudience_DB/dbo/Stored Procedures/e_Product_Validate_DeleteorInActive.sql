CREATE PROCEDURE [dbo].[e_Product_Validate_DeleteorInActive]
	@PubID int
AS
BEGIN

	set nocount on

	declare @ColReferenceID varchar(100)

	select @ColReferenceID = PubID  from pubs
	where PubID = @PubID

	declare @references table  (Reference varchar(100), ReferenceName varchar(100), ReferenceID1 int, ReferenceID2 int)

	if exists (select top 1 1 from Filters with (NOLOCK) where PubID = @ColReferenceID)
		Begin
			  insert into @references
			  select distinct 
					'Filter', 
					f.Name,
					f.filterID,
					0
			  from 
					Filters f  with (NOLOCK) 
			  where
					PubID = @ColReferenceID and 
					f.IsDeleted = 0
		End
	    
	create table #tmp1 (filtergroupID int, xmlfilter xml)            
	create table #tmp2 (filtergroupID int)

	Insert into #tmp1
	SELECT fd.FilterGroupID,
		  cast('<X>'+replace(fd.[values],',','</X><X>')+'</X>' as XML) as xmlfilter 
	from 
		  filterdetails fd with (NOLOCK)
	where 
		  fd.[group] = 'product'
		  and fd.[values] like '%' + convert(varchar(100),@PubID ) + '%'

	insert into #tmp2
	SELECT distinct
		  f1.FilterGroupID
	FROM
	#tmp1 F1
	CROSS APPLY
	( 
	SELECT fdata.D.value('.','varchar(50)') as splitdata 
	FROM f1.xmlfilter.nodes('X') as fdata(D)
	where fdata.D.value('.','varchar(50)') = convert(varchar(100),@PubID )
	) O  

	if exists (select * from #tmp2)
	Begin
		  insert into @references
		  Select 
				'Filter', 
				Name,
				f.FilterID,
				0
		   from   #tmp2 t  join 
				filtergroup fg with (NOLOCK) on t.FilterGroupID = fg.FilterGroupID join
				filters f with (NOLOCK) on fg.FilterID = f.FilterID
		  where f.IsDeleted = 0       
	End  
	    
	drop table #tmp2
	drop table #tmp1     
      
	if exists (select top 1 1 from BrandDetails with (NOLOCK) where PubID = @ColReferenceID)
		Begin
			  insert into @references
			  select distinct 
					'Brand', 
					b.BrandName,
					b.BrandID,
					0
			  from 
					BrandDetails bd with (NOLOCK) join 
					Brand b  with (NOLOCK)on b.BrandID = bd.BrandID 
			  where
					pubID = @ColReferenceID and 
					b.IsDeleted = 0
		End

	if exists (select top 1 1 from DownloadTemplate with (NOLOCK) where PubID = @ColReferenceID)
		Begin
			  insert into @references
			  select distinct 
					'Download Template', 
					d.DownloadTemplateName,
					d.DownloadTemplateID,
					0
			  from 
					DownloadTemplate d  with (NOLOCK)
			  where
					PubID = @ColReferenceID and 
					d.IsDeleted = 0
		End   

	if exists (select top 1 1 from CrossTabReport with (NOLOCK) where PubID = @ColReferenceID)
		Begin
			  insert into @references
			  select distinct 
					'CrossTab Report', 
					c.CrossTabReportName,
					c.CrossTabReportID,
					0
			  from 
					CrossTabReport c  with (NOLOCK)
			  where
					PubID = @ColReferenceID and 
					c.IsDeleted = 0
		End   

	select * from @references

End