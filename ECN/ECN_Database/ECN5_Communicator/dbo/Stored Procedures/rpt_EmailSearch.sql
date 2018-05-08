CREATE PROCEDURE [dbo].[rpt_EmailSearch] (
--Payload filters
@FilterBy varchar(50) = '',
@FilterType varchar(50) = '',
@searchTerm varchar(max) = '',
@BaseChannelID varchar(MAX) = '0',
@CustomerID varchar(MAX) = '0',
--Sort filters
@CurrentPage int = 1,
@PageSize int = 15,
@SortBy varchar(30) = 'EmailAddress ASC',
--Local variables
@EmailPart varchar(max) = '',
@ChannelPartOne varchar(max) = '',
@ChannelPartTwo varchar(max) = '')
AS
BEGIN
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

SET @searchTerm = REPLACE(@searchTerm,CHAR(39), CHAR(39) + CHAR(39))

  IF @FilterType = 'equals'
  BEGIN
    SET @EmailPart = 'AND e.EmailAddress = ''' + @searchTerm + ''''
  END
  IF @FilterType = 'starts'
  BEGIN
    SET @EmailPart = 'AND e.EmailAddress like ''' + @searchTerm + '%'''
  END
  IF @FilterType = 'like'
  BEGIN
    SET @EmailPart = 'AND e.EmailAddress like ''%' + @searchTerm + '%'''
  END
  IF @FilterType = 'ends'
  BEGIN
    SET @EmailPart = 'AND e.EmailAddress like ''%' + @searchTerm + ''''
  END

  IF @BaseChannelID <> '0'
  BEGIN
    SET @ChannelPartOne = 'AND bc.BaseChannelID in (' + @BaseChannelID + ')'
    SET @ChannelPartTwo = 'AND e.BaseChannelID in (' + @BaseChannelID + ')'
  END
  ELSE IF @CustomerID <> '0'
  BEGIN
	SET @ChannelPartOne = ' AND c.CustomerID in (' + @CustomerID + ')'
	SET @ChannelPartTwo = ' AND bc.BaseChannelID in (0)'
  END


  CREATE TABLE #tmp (
    BaseChannelName varchar(100),
    CustomerName varchar(100),
    GroupName varchar(100),
    EmailAddress varchar(100),
    SubscribeTypeCode varchar(100),
    DateCreated datetime,
    DateModified datetime
  )

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
							 when eg.SubscribeTypeCode = ''D'' then ''Bad Record'' 
							 when eg.SubscribeTypeCode = ''M'' then ''Master Suppressed'' else eg.SubscribeTypeCode end as ''Subscribe'', 
				  eg.CreatedOn as ''DateCreated'', eg.LastChanged as ''DateModified''
				  from
				  Emails e with (nolock)
				  join ECN5_ACCOUNTS..Customer c with (nolock) on e.CustomerID = c.CustomerID and c.IsDeleted = 0
				  join ECN5_ACCOUNTS..Basechannel bc with (nolock) on c.BaseChannelID = bc.BaseChannelID and bc.IsDeleted = 0
				  join EmailGroups eg with (nolock) on e.EmailID = eg.EmailID
				  join Groups g with (nolock) on eg.GroupID = g.GroupID and IsNull(g.MasterSupression, 0) = 0
			where
				  1=1
				  ' + @ChannelPartOne + '
				  ' + @EmailPart + '
				  
			UNION

			select 
				  bc.BaseChannelName, '''' as ''CustomerName'', '''' as ''GroupName'', EmailAddress, ''Channel Suppressed'' as ''Subscribe'', e.CreatedDate as ''DateCreated'', e.UpdatedDate as ''DateModified''
			from
				  ChannelMasterSuppressionList e with (nolock)
				  join ECN5_ACCOUNTS..Basechannel bc with (nolock) on e.BaseChannelID = bc.BaseChannelID and bc.IsDeleted = 0
			where
				  e.IsDeleted = 0
				  ' + @ChannelPartTwo + '
				  ' + @EmailPart + '
			order by ' + @SortBy
  );

  WITH Results
  AS (SELECT
    ROW_NUMBER() OVER (ORDER BY @SortBy
    ) AS ROWNUM,
    COUNT(*) OVER () AS TotalCount,
    *
  FROM #tmp)
  SELECT
    *
  FROM Results
  WHERE ROWNUM BETWEEN ((@CurrentPage - 1) * @PageSize + 1) AND (@CurrentPage * @PageSize)
  DROP TABLE #tmp
END