CREATE PROCEDURE [dbo].[e_MarketingAutomation_GetAllMarketingAutomationsbySearch]
(
	@AutomationName varchar(100) = NULL,
	@State varchar(20)='',
	@SearchCriteria varchar (20)=NULL,
	@BaseChannelID int = NULL,
	@CurrentPage int,
	@PageSize int,
	@SortDirection varchar(20),
	@SortColumn varchar(50)
)
AS
	BEGIN
	if(@State <> 'Completed')
	BEGIN
		WITH Results
		AS (SELECT ROW_NUMBER() OVER (ORDER BY
			CASE WHEN (@SortColumn = 'Name' AND @SortDirection='ASC') THEN ma.Name END ASC,
			CASE WHEN (@SortColumn = 'Name' AND @SortDirection='DESC') THEN ma.Name END DESC,
			CASE WHEN (@SortColumn = 'State' AND @SortDirection='ASC') THEN ma.State END ASC,
			CASE WHEN (@SortColumn = 'State' AND @SortDirection='DESC') THEN ma.State END DESC,
			CASE WHEN (@SortColumn = 'StartDate' AND @SortDirection='ASC') THEN ma.StartDate END ASC,
			CASE WHEN (@SortColumn = 'StartDate' AND @SortDirection='DESC') THEN ma.StartDate END DESC,
			CASE WHEN (@SortColumn = 'EndDate' AND @SortDirection='ASC') THEN ma.EndDate END ASC,
			CASE WHEN (@SortColumn = 'EndDate' AND @SortDirection='DESC') THEN ma.EndDate END DESC,
			CASE WHEN (@SortColumn = 'CreatedDate' AND @SortDirection = 'ASC') then ISNULL(ma.CreatedDate,ma.UpdatedDate) END ASC,
			CASE WHEN (@SortColumn = 'CreatedDate' AND @SortDirection = 'DESC') then ISNULL(ma.CreatedDate,ma.UpdatedDate) END DESC,
			CASE WHEN (@SortColumn = 'UpdatedDate' AND @SortDirection='ASC') THEN ma.UpdatedDate END ASC,
			CASE WHEN (@SortColumn = 'UpdatedDate' AND @SortDirection='DESC') THEN ma.UpdatedDate END DESC,
			CASE WHEN (@SortColumn = 'LastPublishedDate' AND @SortDirection='ASC') THEN  MAX(mah.HistoryDate) END ASC,
			CASE WHEN (@SortColumn = 'LastPublishedDate' AND @SortDirection='DESC') THEN  MAX(mah.HistoryDate) END DESC				
		) AS ROWNUM,
			Count(ma.MarketingAutomationID) over () AS TotalCount, 
			ma.MarketingAutomationID,ma.Name,
			CASE WHEN ((ma.EndDate < cast(Getdate() as date)) and ma.State ='Published') THEN 'Completed' ELSE ma.State END as State
			,ma.StartDate,ma.EndDate,ma.CreatedDate, ma.UpdatedDate, MAX(mah.HistoryDate) as 'LastPublishedDate'
			FROM MarketingAutomation ma (nolock)
			join ECn5_Accounts..Customer c with(nolock) on ma.CustomerID = c.CustomerID
			left outer join MarketingAutomationHistory mah with(nolock) on ma.MarketingAutomationID = mah.MarketingAutomationID and mah.Action IN('Publish','UnPause')
			WHERE UPPER(ma.Name) LIKE case when @SearchCriteria = 'equals' and @AutomationName is not null then @AutomationName
											when @SearchCriteria = 'starts' and @AutomationName is not null then @AutomationName + '%'
											when @SearchCriteria = 'ends' and @AutomationName is not null then '%' + @AutomationName
											when @SearchCriteria = 'like' and @AutomationName is not null then '%' + @AutomationName + '%'
											when @AutomationName is null then UPPER(ma.Name) END
				and ma.State = case when @State ='' then ma.State  ELSE  @State END
				and c.BaseChannelID = @BaseChannelID and ISNULL(ma.IsDeleted,0) = 0 
				group by ma.MarketingAutomationID,ma.Name,ma.State,ma.StartDate,ma.EndDate,ma.CreatedDate, ma.UpdatedDate
		)
		SELECT * FROM Results
		WHERE ROWNUM between ((@CurrentPage - 1) * @PageSize + 1) and (@CurrentPage * @PageSize)
	END
	ELSE if @State = 'Completed'
	BEGIN
		WITH Results
		AS (SELECT ROW_NUMBER() OVER (ORDER BY
			CASE WHEN (@SortColumn = 'Name' AND @SortDirection='ASC') THEN ma.Name END ASC,
			CASE WHEN (@SortColumn = 'Name' AND @SortDirection='DESC') THEN ma.Name END DESC,
			CASE WHEN (@SortColumn = 'State' AND @SortDirection='ASC') THEN ma.State END ASC,
			CASE WHEN (@SortColumn = 'State' AND @SortDirection='DESC') THEN ma.State END DESC,
			CASE WHEN (@SortColumn = 'StartDate' AND @SortDirection='ASC') THEN ma.StartDate END ASC,
			CASE WHEN (@SortColumn = 'StartDate' AND @SortDirection='DESC') THEN ma.StartDate END DESC,
			CASE WHEN (@SortColumn = 'EndDate' AND @SortDirection='ASC') THEN ma.EndDate END ASC,
			CASE WHEN (@SortColumn = 'EndDate' AND @SortDirection='DESC') THEN ma.EndDate END DESC,
			CASE WHEN (@SortColumn = 'CreatedDate' AND @SortDirection = 'ASC') then ISNULL(ma.CreatedDate,ma.UpdatedDate) END ASC,
			CASE WHEN (@SortColumn = 'CreatedDate' AND @SortDirection = 'DESC') then ISNULL(ma.CreatedDate,ma.UpdatedDate) END DESC,
			CASE WHEN (@SortColumn = 'UpdatedDate' AND @SortDirection='ASC') THEN ma.UpdatedDate END ASC,
			CASE WHEN (@SortColumn = 'UpdatedDate' AND @SortDirection='DESC') THEN ma.UpdatedDate END DESC,
			CASE WHEN (@SortColumn = 'LastPublishedDate' AND @SortDirection='ASC') THEN  MAX(mah.HistoryDate) END ASC,
			CASE WHEN (@SortColumn = 'LastPublishedDate' AND @SortDirection='DESC') THEN  MAX(mah.HistoryDate) END DESC				
		) AS ROWNUM,
			Count(ma.MarketingAutomationID) over () AS TotalCount, 
			ma.MarketingAutomationID,ma.Name,
			CASE WHEN ((ma.EndDate < cast(Getdate() as date)) and ma.State ='Published') THEN 'Completed' ELSE ma.State END as State
			,ma.StartDate,ma.EndDate,ma.CreatedDate, ma.UpdatedDate, MAX(mah.HistoryDate) as 'LastPublishedDate'
			FROM MarketingAutomation ma (nolock)
			join ECn5_Accounts..Customer c with(nolock) on ma.CustomerID = c.CustomerID
			left outer join MarketingAutomationHistory mah with(nolock) on ma.MarketingAutomationID = mah.MarketingAutomationID and mah.Action = 'Publish'
			WHERE UPPER(ma.Name) LIKE case when @SearchCriteria = 'equals' and @AutomationName is not null then @AutomationName
											when @SearchCriteria = 'starts' and @AutomationName is not null then @AutomationName + '%'
											when @SearchCriteria = 'ends' and @AutomationName is not null then '%' + @AutomationName
											when @SearchCriteria = 'like' and @AutomationName is not null then '%' + @AutomationName + '%'
											when @AutomationName is null then UPPER(ma.Name) END
				and ma.State = 'Published' and ma.EndDate < cast(Getdate() as date)
				and c.BaseChannelID = @BaseChannelID and ISNULL(ma.IsDeleted,0) = 0 
				group by ma.MarketingAutomationID,ma.Name,ma.State,ma.StartDate,ma.EndDate,ma.CreatedDate, ma.UpdatedDate
		)
		SELECT * FROM Results
		WHERE ROWNUM between ((@CurrentPage - 1) * @PageSize + 1) and (@CurrentPage * @PageSize)
	END
END


