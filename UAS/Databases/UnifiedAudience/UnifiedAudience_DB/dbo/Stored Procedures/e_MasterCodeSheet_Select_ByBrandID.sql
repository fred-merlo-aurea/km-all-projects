CREATE  PROCEDURE [dbo].[e_MasterCodeSheet_Select_ByBrandID]   
(
	@BrandID int
)
AS
BEGIN

	SET NOCOUNT ON

	SELECT DISTINCT mc.* 
	From vw_Mapping v  WITH (NOLOCK)
		join Mastercodesheet mc WITH (NOLOCK) on v.MasterID = mc.MasterID 
		join branddetails bd WITH (NOLOCK) on bd.pubid = v.pubid   
	WHERE  bd.brandid = @BrandID 
	ORDER  BY mc.sortorder  

End