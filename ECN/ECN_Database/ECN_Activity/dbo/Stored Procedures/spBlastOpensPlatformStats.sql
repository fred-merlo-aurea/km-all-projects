-- =============================================
-- Author:		Rohit Pooserla
-- Create date: 05/23/2012
-- Description:	BlastOpensPlatformStats
-- =============================================
CREATE PROCEDURE [dbo].[spBlastOpensPlatformStats]  
	-- Add the parameters for the stored procedure here
	@BlastID int
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;
	
  SELECT [ecn_Activity].[dbo].[Platforms].[PlatformName] as 'Platform', COUNT(EmailID)
  FROM [ecn_Activity].[dbo].[BlastActivityOpens] 
  inner join [ecn_Activity].[dbo].[Platforms] on
  [ecn_Activity].[dbo].[BlastActivityOpens].[PlatformID]=[ecn_Activity].[dbo].[Platforms].[PlatformID]
  where BlastID=@BlastID and [ecn_Activity].[dbo].[BlastActivityOpens].[PlatformID] <> '5'
  group by  [ecn_Activity].[dbo].[Platforms].[PlatformName]
  order by 2 DESC
 
END
