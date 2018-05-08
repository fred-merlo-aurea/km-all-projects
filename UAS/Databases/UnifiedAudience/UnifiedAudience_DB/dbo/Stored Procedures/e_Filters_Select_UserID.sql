CREATE PROCEDURE [dbo].[e_Filters_Select_UserID]
@UserID int
AS
Begin
	select f.FilterID, f.Name, f.CreatedDate, f.CreatedUserID, f.FilterType, f.PubID, f.IsDeleted, f.UpdatedUserID, f.UpdatedDate,
		f.BrandID, ISNULL(f.FilterCategoryID, 0) as FilterCategoryID, f.AddtoSalesView, ISNULL(f.QuestionCategoryID, 0) as QuestionCategoryID, f.QuestionName,
		p.PubName, b.brandID, b.brandname, fc.CategoryName as FilterCategoryName, qc.CategoryName as QuestionCategoryName
	from Filters f  WITH (NOLOCK)
		left outer join Pubs p WITH (NOLOCK) on f.PubID = p.PubID 
		left outer join brand b WITH (NOLOCK) on b.brandID = f.brandID 
		left outer join FilterCategory fc WITH (NOLOCK) on fc.FilterCategoryID = f.FilterCategoryID 
		left outer join QuestionCategory qc WITH (NOLOCK) on  qc.QuestionCategoryID = f.QuestionCategoryID		
	where f.IsDeleted = 0 and isnull(b.IsDeleted,0) = 0 and f.CreatedUserID=@UserID 
	order by Name ASC
End
