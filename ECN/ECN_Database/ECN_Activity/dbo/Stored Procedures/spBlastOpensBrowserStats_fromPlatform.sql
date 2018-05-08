-- =============================================
-- Author:		Rohit Pooserla
-- Create date: 05/23/2012
-- Description:	BlastOpensBrowserStats_fromPlatform
-- =============================================
CREATE PROCEDURE [dbo].[spBlastOpensBrowserStats_fromPlatform]  
	-- Add the parameters for the stored procedure here
	@BlastID int, @Platform varchar(50)
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;
	
 DECLARE @PlatformID int;
 
 select @PlatformID=[ecn_Activity].[dbo].[Platforms].[PlatformID] from [ecn_Activity].[dbo].[Platforms]
 where [ecn_Activity].[dbo].[Platforms].[PlatformName]=@Platform;

  
   SELECT [ecn_Activity].[dbo].[EmailClients].EmailClientName as 'EmailClientName',
   CAST(ROUND(CAST((COUNT(EmailID)*100) as float)/ (Select COUNT(EmailID) from [ecn_Activity].[dbo].[BlastActivityOpens] where  BlastID=@BlastID and EmailClientID <> '15'),2) as varchar)+'%' as Usage
  FROM [ecn_Activity].[dbo].[BlastActivityOpens] 
  inner join [ecn_Activity].[dbo].[EmailClients] on
  [ecn_Activity].[dbo].[BlastActivityOpens].EmailClientID=[ecn_Activity].[dbo].[EmailClients].EmailClientID
  where BlastID=@BlastID   and [PlatformID]=@PlatformID  and [ecn_Activity].[dbo].[BlastActivityOpens].EmailClientID <> '15'
  group by [ecn_Activity].[dbo].[EmailClients].EmailClientName
  order by 2 DESC
 
END
