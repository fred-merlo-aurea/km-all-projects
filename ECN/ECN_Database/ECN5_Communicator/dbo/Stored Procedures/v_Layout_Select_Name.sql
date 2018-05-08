CREATE PROCEDURE [dbo].[v_Layout_Select_Name] 
(
@LayoutName varchar(250) = NULL,
@FolderID int = NULL,
@CustomerID int = NULL,
@UserID int = NULL,
@BaseChannelID int = NULL,
@UpdatedDateFrom datetime = NULL,
@UpdatedDateTo datetime = NULL,
@CurrentPage int,
@PageSize int,
@SortDirection varchar(20),
@SortColumn varchar(50),
@ArchiveFilter varchar(20) = 'all',
@ValidatedOnly int =0
)
AS

BEGIN
   WITH Results
            AS (SELECT ROW_NUMBER() OVER (ORDER BY
                  
                  CASE WHEN (@SortColumn = 'FolderName' AND @SortDirection='ASC') THEN FolderName END ASC,
                  CASE WHEN (@SortColumn = 'FolderName' AND @SortDirection='DESC') THEN FolderName END DESC,
            
                  CASE WHEN (@SortColumn = 'LayoutName' AND @SortDirection='ASC') THEN LayoutName END ASC,
                  CASE WHEN (@SortColumn = 'LayoutName' AND @SortDirection='DESC') THEN LayoutName END DESC,
                  CASE WHEN (@SortColumn = 'UpdatedDate' AND @SortDirection='ASC') THEN l.UpdatedDate END ASC,
                  CASE WHEN (@SortColumn = 'UpdatedDate' AND @SortDirection='DESC') THEN l.UpdatedDate END DESC,
                  CASE WHEN (@SortColumn = 'CreatedDate' AND @SortDirection='ASC') THEN ISNULL(l.CreatedDate,l.UpdatedDate) END ASC,
                  CASE WHEN (@SortColumn = 'CreatedDate' AND @SortDirection='DESC') THEN ISNULL(l.CreatedDate,l.UpdatedDate) END DESC                      
            ) AS ROWNUM,
            Count(*) over () AS TotalCount,
                  l.LayoutID,l.TemplateID,l.CustomerID,l.FolderID,l.LayoutName,l.ContentSlot1,l.ContentSlot2,l.ContentSlot3,l.ContentSlot4,l.ContentSlot5,l.ContentSlot6
                  ,l.ContentSlot7,l.ContentSlot8,l.ContentSlot9,l.CreatedUserID,l.UpdatedDate,l.TableOptions,l.DisplayAddress,l.SetupCost,l.OutboundCost,l.InboundCost
                  ,l.DesignCost,l.OtherCost,l.MessageTypeID,l.CreatedDate,l.IsDeleted,l.UpdatedUserID
                  ,ISNULL(f.FolderName,'') as FolderName,ISNULL(l.Archived,0) as Archived
                  FROM Layout l (nolock)
                  Left Join Folder f (nolock) on f.FolderID = l.FolderID
                 Left Join Content c with(nolock) on c.ContentID = case when ISNULL(l.ContentSlot1,0) > 0 then l.ContentSlot1
																	when ISNULL(l.ContentSlot2,0) > 0 then l.ContentSlot2
																	when ISNULL(l.ContentSlot3,0) > 0 then l.ContentSlot3
																	when ISNULL(l.ContentSlot4,0) > 0 then l.ContentSlot4
																	when ISNULL(l.ContentSlot5,0) > 0 then l.ContentSlot5
																	when ISNULL(l.ContentSlot6,0) > 0 then l.ContentSlot6
																	when ISNULL(l.ContentSlot7,0) > 0 then l.ContentSlot7
																	when ISNULL(l.ContentSlot8,0) > 0 then l.ContentSlot8
																	when ISNULL(l.ContentSlot9,0) > 0 then l.ContentSlot9
																	END
                  WHERE l.CustomerID = @CustomerID 
                        and l.FolderID = case when @FolderID is not null then @FolderID ELSE l.FolderID END
                        and UPPER(LayoutName) like  case when @LayoutName is not null then '%' + UPPER(@LayoutName) + '%' else LayoutName END
                        and ISNULL(l.UpdatedUserID,0) = Case when @UserID is not null then @UserID ELSE ISNULL(l.UpdatedUserID,0) END             
                        and l.UpdatedDate >= case when @UpdatedDateFrom is not null then @UpdatedDateFrom ELSE l.UpdatedDate END
                        and l.UpdatedDate <= case when @UpdatedDateTo is not null then @UpdatedDateTo ELSE l.UpdatedDate END
                        and l.IsDeleted = 0
                        and ISNULL(l.ContentSlot1,0) = Case when ISNULL(l.ContentSlot1,0) = 0 then 0
                                          WHEN @ValidatedOnly = 0 then ISNULL(l.ContentSlot1,0)
                                          WHEN @ValidatedOnly = 1 and ISNULL(c.IsValidated, 0) = 1 THEN l.ContentSlot1                              
                                          ELSE null END     
                        and ISNULL(l.ContentSlot2,0) = Case when ISNULL(l.ContentSlot2,0) = 0  then 0
                                          WHEN @ValidatedOnly = 0 then ISNULL(l.ContentSlot2,0)
                                          WHEN @ValidatedOnly = 1 and ISNULL(c.IsValidated, 0) = 1 THEN l.ContentSlot2                                          
                                          ELSE null END
                        and ISNULL(l.ContentSlot3,0) = Case when ISNULL(l.ContentSlot3,0) = 0 then  0
                                          WHEN @ValidatedOnly = 0 then ISNULL(l.ContentSlot3,0)
                                          WHEN @ValidatedOnly = 1 and ISNULL(c.IsValidated, 0) = 1 THEN l.ContentSlot3                                       
                                          ELSE null END     
                        and ISNULL(l.ContentSlot4,0) = Case when ISNULL(l.ContentSlot4,0) = 0 then  0
                                          WHEN @ValidatedOnly = 0 then ISNULL(l.ContentSlot4,0)
                                          WHEN @ValidatedOnly = 1 and ISNULL(c.IsValidated, 0) = 1 THEN l.ContentSlot4                                                
                                          ELSE null END
                        and ISNULL(l.ContentSlot5,0) = Case when ISNULL(l.ContentSlot5,0) = 0 then  0
                                          WHEN @ValidatedOnly = 0 then ISNULL(l.ContentSlot5,0)
                                          WHEN @ValidatedOnly = 1 and ISNULL(c.IsValidated, 0) = 1 THEN l.ContentSlot5                                          
                                          ELSE null END
                        and ISNULL(l.ContentSlot6,0) = Case when ISNULL(l.ContentSlot6,0) = 0 then  0
                                          WHEN @ValidatedOnly = 0 then ISNULL(l.ContentSlot6,0)
                                          WHEN @ValidatedOnly = 1 and ISNULL(c.IsValidated, 0) = 1 THEN l.ContentSlot6                                          
                                          ELSE null END   
                         and ISNULL(l.ContentSlot7,0) = Case when ISNULL(l.ContentSlot7,0) = 0 then  0
                                          WHEN @ValidatedOnly = 0 then ISNULL(l.ContentSlot7,0)
                                          WHEN @ValidatedOnly = 1 and ISNULL(c.IsValidated, 0) = 1 THEN l.ContentSlot7                                          
                                          ELSE  null END
                        and ISNULL(l.ContentSlot8,0) = Case when ISNULL(l.ContentSlot8,0) = 0 then  0
                                          WHEN @ValidatedOnly = 0 then ISNULL(l.ContentSlot8,0)
                                          WHEN @ValidatedOnly = 1 and ISNULL(c.IsValidated, 0) = 1 THEN l.ContentSlot8                                          
                                          ELSE  null END  
                        and ISNULL(l.ContentSlot9,0) = Case when ISNULL(l.ContentSlot9,0) = 0 then  0
                                          WHEN @ValidatedOnly = 0 then ISNULL(l.ContentSlot9,0)
                                          WHEN @ValidatedOnly = 1 and ISNULL(c.IsValidated, 0) = 1 THEN l.ContentSlot9                                          
                                          ELSE  null END                  
                        and ISNULL(l.Archived,0) = case when @ArchiveFilter = 'archived' then 1 when @ArchiveFilter = 'all' then ISNULL(l.Archived,0) when @ArchiveFilter = 'active' then 0 END
            )
            SELECT * , t.SlotsTotal, (convert(varchar(255),LayoutID) + '&chID='  + convert(varchar,@BaseChannelID)  + '&cuID='  + convert(varchar,@CustomerID) ) as LayoutIDPlus
            FROM Results 
            INNER JOIN Template t (nolock) on t.TemplateID =Results.TemplateID
            WHERE ROWNUM between ((@CurrentPage - 1) * @PageSize + 1) and (@CurrentPage * @PageSize)
END
