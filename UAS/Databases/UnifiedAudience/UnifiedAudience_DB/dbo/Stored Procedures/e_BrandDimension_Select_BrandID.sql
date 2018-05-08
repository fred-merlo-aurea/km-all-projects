CREATE PROCEDURE [dbo].[e_BrandDimension_Select_BrandID]   
@BrandID int
AS
BEGIN

	set nocount on

    select DisplayName, bd.* 
    from BrandDimension bd  WITH (NOLOCK)
		join MasterGroups mg WITH (NOLOCK) on bd.MasterGroupID = mg.MasterGroupID  
	where brandID = @brandID and mg.IsActive = 1
	union
		SELECT DISTINCT mg.DisplayName, 0, @BrandID, mg.MasterGroupID, 0,0,0, SortOrder = (select isnull(MAX(sortOrder),0)+1 from branddimension where BrandID = @BrandID)
		From vw_Mapping v  WITH (NOLOCK)
			join Mastercodesheet mc WITH (NOLOCK) on v.MasterID = mc.MasterID 
			join MasterGroups mg WITH (NOLOCK) on mg.MasterGroupID = mc.MasterGroupID 
			join branddetails bd WITH (NOLOCK) on bd.pubid = v.pubid   
		WHERE bd.brandid = @BrandID and
			mg.IsActive = 1 and 
			mg.MasterGroupID not in 
			(select bd.MasterGroupID 
			from BrandDimension bd  WITH (NOLOCK)
				join MasterGroups mg WITH (NOLOCK) on bd.MasterGroupID = mg.MasterGroupID 
			where brandID = @brandID) 
		ORDER  BY SortOrder

END