CREATE PROCEDURE [dbo].[e_MasterGroup_Select_ByBrandID]   
(
	@BrandID INT
)
AS
BEGIN
	SET NOCOUNT ON

	SELECT mg.*
	FROM MasterGroups mg WITH (NOLOCK) join
	(
		SELECT distinct mc.MasterGroupID
		FROM 
				BrandDetails bd WITH (NOLOCK) join
				ResponseGroups rg WITH (NOLOCK) on bd.PubID = rg.PubID join
				CodeSheet c WITH (NOLOCK) on c.ResponseGroupID = rg.ResponseGroupID join 
				CodeSheet_Mastercodesheet_Bridge cmb WITH (NOLOCK) on cmb.CodeSheetID = c.CodeSheetID join
				Mastercodesheet mc WITH (NOLOCK) ON mc.masterID = cmb.masterID
		WHERE bd.BrandID = @BrandID	
	)
	x on x.MasterGroupID = mg.MasterGroupID
	ORDER BY mg.SortOrder

	--SELECT DISTINCT 
	--	MG.*
	--FROM
	--	BrandDetails bd WITH(NOLOCK) 
	--	INNER JOIN vw_Mapping v  WITH(NOLOCK) ON bd.PubId = v.PubId    
	--	INNER JOIN MasterGroups mg WITH(NOLOCK) ON v.MasterGroupID = mg.MasterGroupID
	--WHERE  
	--	bd.BrandId = @BrandID 
	--ORDER  BY 
	--	mg.SortOrder   
END