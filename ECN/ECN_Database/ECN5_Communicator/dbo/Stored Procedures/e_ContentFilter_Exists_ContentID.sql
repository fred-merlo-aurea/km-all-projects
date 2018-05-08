-- =============================================
-- Author:		<Author,,Name>
-- Create date: <Create Date,,>
-- Description:	<Description,,>
-- =============================================
CREATE PROCEDURE [dbo].[e_ContentFilter_Exists_ContentID]
	@ContentID int,
	@CustomerID int
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;

    if exists(Select top 1 cf.FilterID from ContentFilter cf with(nolock) join Content c with(nolock) on cf.contentID = c.ContentID where cf.ContentID = @ContentID and cf.IsDeleted = 0 and c.CustomerID = @CustomerID and c.IsDeleted = 0) 
    begin
		select 1
    end
    else 
    begin select 0
    end
   
END