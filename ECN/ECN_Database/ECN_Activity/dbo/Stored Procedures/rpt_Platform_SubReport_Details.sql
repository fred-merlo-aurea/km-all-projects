CREATE PROCEDURE [dbo].[rpt_Platform_SubReport_Details]
	@BlastID int
AS
BEGIN
	 select bao.BlastID,p.PlatformName, ec.EmailClientName , COUNT(bao.OpenID)
 from BlastActivityOpens bao with(nolock)
 join EmailClients ec with(nolock) on bao.EmailClientID = ec.EmailClientID
 join Platforms p with(Nolock) on bao.PlatformID = p.PlatformID
 where bao.BlastID = @BlastID
 Group by bao.BlastID, p.PlatformName, ec.EmailClientName
END