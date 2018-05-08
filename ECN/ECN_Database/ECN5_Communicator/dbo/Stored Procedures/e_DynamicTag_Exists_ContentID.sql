-- =============================================
-- Author:		<Author,,Name>
-- Create date: <Create Date,,>
-- Description:	<Description,,>
-- =============================================
CREATE PROCEDURE [dbo].[e_DynamicTag_Exists_ContentID]
	@ContentID int,
	@CustomerID int
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;

    if exists(Select top 1 dt.DynamicTagID from DynamicTag dt with(nolock) where dt.ContentID = @ContentID and dt.IsDeleted = 0 and dt.CustomerID = @CustomerID) 
    begin
		select 1
    end
    else if exists(SELECT top 1 dtr.DynamicTagRuleID from DynamicTagRule dtr with(nolock) where dtr.ContentID = @ContentID and dtr.IsDeleted = 0)
    begin
		select 1
    end 
    else
    begin select 0
    end
   
END