CREATE PROCEDURE [dbo].[e_FilterSegmentation_Select_ViewType_UserID]
@UserID int,
@PubID int,
@BrandID int,
@ViewType varchar(50),
@IsAdmin bit,
@FilterCategoryID int,
@SearchText varchar(50) = NULL,
@SearchCriteria varchar (20) = NULL
AS
BEGIN

	SET NOCOUNT ON
	
	declare @query varchar(Max)
	
	set @query = 'select fs.*, f.*, b.brandname, p.PubName, fc.CategoryName
					from FilterSegmentation fs with(nolock)  
						 join Filters f with(nolock) on fs.filterID = f.FilterID 
						 left outer join brand b with(nolock) on f.brandID = b.BrandID 
						 left outer join Pubs p with(nolock) on f.PubID = p.PubID 
						 left outer join FilterCategory fc with(nolock) on fc.FilterCategoryID = f.FilterCategoryID 
						 where fs.IsDeleted=0 and isnull(b.IsDeleted,0) = 0 '

	if @ViewType = 'ProductView'
		set @query += ' and filterType = ''ProductView'' and p.pubID = ' + CONVERT(varchar(10),@PubID)
	else if  @ViewType = 'CrossProductView'	
		set @query += ' and filterType = ''CrossProductView'''
	else if  @ViewType = 'ConsensusView'	or @ViewType = 'RecencyView'
		set @query += ' and (Isnull(filterType, '''') = ''ConsensusView'' or filterType = ''RecencyView'') and Isnull(p.pubID,0)=0' 
		
	if @IsAdmin	= 0 and @UserID > 0
		set @query += ' and fs.CreatedUserID = ' + CONVERT(varchar(10),@UserID)  
		
    if @BrandID > 0
        set @query += ' and b.brandID = ' + CONVERT(varchar(10),@BrandID)
    else
        set @query += ' and (b.brandID = 0 or b.brandID is null)'	
     
     -- selected all users 
     if @FilterCategoryID > 0
		set @query += ' and f.FilterCategoryID = ' + CONVERT(varchar(10), isnull(@FilterCategoryID, 0))  

	if @SearchText <> ''    
	Begin    
		set @query += ' and ((UPPER(fs.FilterSegmentationName) LIKE case when ''' + @SearchCriteria + ''' = ''equal'' then ''' + @SearchText + '''
									    when ''' + @SearchCriteria + ''' = ''start with'' then ''' + @SearchText + ''' + ''%''
										when ''' + @SearchCriteria + ''' = ''end with'' then ''%'' + ''' + @SearchText + '''
										when ''' + @SearchCriteria + ''' = ''contains'' then ''%'' + ''' + @SearchText + ''' + ''%''
										when ''' + @SearchText + ''' is null then UPPER(fs.FilterSegmentationName) END ) or
						(UPPER(f.Name) LIKE case when ''' + @SearchCriteria + ''' = ''equal'' then ''' + @SearchText + '''
									    when ''' + @SearchCriteria + ''' = ''start with'' then ''' + @SearchText + ''' + ''%''
										when ''' + @SearchCriteria + ''' = ''end with'' then ''%'' + ''' + @SearchText + '''
										when ''' + @SearchCriteria + ''' = ''contains'' then ''%'' + ''' + @SearchText + ''' + ''%''
										when ''' + @SearchText + ''' is null then UPPER(f.Name) END ))'    
	End       
        
    set @query += ' order by Name ASC'	   
    
    execute(@query)
End
