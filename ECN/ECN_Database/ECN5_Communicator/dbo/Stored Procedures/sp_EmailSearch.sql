CREATE proc [dbo].[sp_EmailSearch] 
(
	--Payload filters
    @FilterBy varchar(50) = '',
    @FilterType varchar(50) = '',
    @searchTerm varchar(MAX) = '',
    @BaseChannelID int = 92,
    --Sort filters
    @CurrentPage int = 1,
    @PageSize int = 15,
	@SortBy VARCHAR(30) = 'EmailAddress ASC',    
	--Local variables
    @EmailPart varchar(MAX) = '',
    @ChannelPartOne varchar(MAX) = '',
    @ChannelPartTwo varchar(MAX) = '' 	
)
as

--declare
--    --Payload filters
--    @FilterBy varchar(50) = '',
--    @FilterType varchar(50) = 'like',
--    @searchTerm varchar(MAX) = 'Bill',
--    @BaseChannelID int = 92,
--    --Sort filters
--    @CurrentPage int = 1,
--    @PageSize int = 15,
--	@SortBy VARCHAR(30) = 'EmailAddress DESC',
--	--Local variables
--    @EmailPart varchar(MAX) = '',
--    @ChannelPartOne varchar(MAX) = '',
--    @ChannelPartTwo varchar(MAX) = ''     
    

if @FilterType = 'equals'
	Begin
		SET @EmailPart = 'AND e.EmailAddress = ''' + @searchTerm + ''''
    End
if @FilterType = 'starts'
    Begin
        SET @EmailPart = 'AND e.EmailAddress like ''' + @searchTerm + '%'''
    End
if @FilterType = 'like'
    Begin
        SET @EmailPart = 'AND e.EmailAddress like ''%' + @searchTerm + '%'''
    End
if @FilterType = 'ends'
    Begin
        SET @EmailPart = 'AND e.EmailAddress like ''%' + @searchTerm + ''''
    End
      
if @BaseChannelID <> 0
    Begin
        SET @ChannelPartOne = 'AND bc.BaseChannelID = '+CONVERT(VARCHAR(10),@BaseChannelID)
        SET @ChannelPartTwo = 'AND e.BaseChannelID = '+CONVERT(VARCHAR(10),@BaseChannelID)
    END   
      
BEGIN
Create Table #tmp (
	BaseChannelName VARCHAR(100), 
	CustomerName VARCHAR(100),  
	GroupName VARCHAR(100), 
	EmailAddress VARCHAR(100),  
	SubscribeTypeCode VARCHAR(100),
	DateCreated DATETIME, 
	DateModified DATETIME)

	EXEC ('     
            INSERT INTO #tmp
			select 
				  bc.BaseChannelName, 
							 c.CustomerName, 
							 g.GroupName, 
							 e.EmailAddress, 
				  case when eg.SubscribeTypeCode = ''S''  then ''Subscribed'' 
							 when eg.SubscribeTypeCode = ''U'' then ''Unsubscribed'' 
							 when eg.SubscribeTypeCode = ''P'' then ''Pending'' 
							 when eg.SubscribeTypeCode = ''M'' then ''Master Suppressed'' else eg.SubscribeTypeCode end as ''Subscribe'', 
				  eg.CreatedOn as ''DateCreated'', eg.LastChanged as ''DateModified''
				  from
				  Emails e with (nolock)
				  join ECN5_ACCOUNTS..Customer c with (nolock) on e.CustomerID = c.CustomerID
				  join ECN5_ACCOUNTS..Basechannel bc with (nolock) on c.BaseChannelID = bc.BaseChannelID
				  join EmailGroups eg with (nolock) on e.EmailID = eg.EmailID
				  join Groups g with (nolock) on eg.GroupID = g.GroupID and g.MasterSupression = 0
			where
				  1=1
				  '+@ChannelPartOne+'
				  '+@EmailPart+'
				  
			UNION

			select 
				  bc.BaseChannelName, '''' as ''CustomerName'', '''' as ''GroupName'', EmailAddress, ''Channel Suppressed'' as ''Subscribe'', e.CreatedDate as ''DateCreated'', e.UpdatedDate as ''DateModified''
			from
				  ChannelMasterSuppressionList e with (nolock)
				  join ECN5_ACCOUNTS..Basechannel bc with (nolock) on e.BaseChannelID = bc.BaseChannelID
			where
				  e.IsDeleted = 0
				  '+@ChannelPartTwo+'
				  '+@EmailPart+'
			order by '+@SortBy
			);
			
    WITH Results
       AS (SELECT ROW_NUMBER() OVER (ORDER BY @SortBy
    ) AS ROWNUM, Count(*) over () AS TotalCount, *
       FROM #tmp
       )
       SELECT *
       FROM Results
       WHERE ROWNUM between ((@CurrentPage - 1) * @PageSize + 1) and (@CurrentPage * @PageSize)
drop table #tmp                          
END
GO