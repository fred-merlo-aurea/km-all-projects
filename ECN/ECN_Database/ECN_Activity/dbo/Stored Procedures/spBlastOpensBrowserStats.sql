-- =============================================
-- Author:		Rohit Pooserla
-- Create date: 05/22/2012
-- Description:	BlastOpensBrowserStats
-- =============================================
CREATE PROCEDURE [dbo].[spBlastOpensBrowserStats]  
	-- Add the parameters for the stored procedure here
	@BlastID int
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;
	
  
  SELECT [ecn_Activity].[dbo].[EmailClients].EmailClientName as 'Browser',
   COUNT(*) as 'Usage'
  FROM [ecn_Activity].[dbo].[BlastActivityOpens] 
  inner join [ecn_Activity].[dbo].[EmailClients] on
  [ecn_Activity].[dbo].[BlastActivityOpens].EmailClientID=[ecn_Activity].[dbo].[EmailClients].EmailClientID
  where BlastID=@BlastID and [ecn_Activity].[dbo].[BlastActivityOpens].EmailClientID <> '15'
  group by [ecn_Activity].[dbo].[EmailClients].EmailClientName
END
