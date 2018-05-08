-- =============================================
-- Author:		Rohit Pooserla
-- Create date: 06/14/2012
-- Description:	Bounces Report By Domain
-- =============================================
CREATE PROCEDURE [dbo].[sp_GetBounceReportByDomain]
(
@blastIDs varchar(4000),
@CampaignItemID varchar(10)
)
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;
	
		declare @b TABLE (bID int)
		
		if @CampaignItemID <> ''
		Begin;
	SELECT @blastIDs = COALESCE(@blastIDs + ', ', '') + CAST(BlastID as Varchar)
	from ecn5_communicator..CampaignItemBlast  where CampaignItemID=@CampaignItemID and IsDeleted=0 and BlastID is not null

			insert into @b 
			SELECT ITEMS FROM ecn5_communicator..fn_split(@blastIDs, ',') a JOIN ecn5_communicator..Blast b on b.BlastID = a.Items
		End
		else
		Begin
			insert into @b values(@blastIDs)
		End
		
		select sq1.ISP as Domain, ISNULL(sq2.Bounces, 0) as TotalBounces,  
		ISNULL(sq3.HardBounces, 0) as Hard, ISNULL(sq4.SoftBounces, 0) as Soft, ISNULL(sq5.OtherBounces, 0) as Other,
		sq1.Sends as 'MessagesSent', CONVERT(float, (ISNULL(sq2.Bounces, 0)*100))/sq1.Sends as 'PercBounced'
		from 
		(
		select substring(e.EmailAddress, charindex('@', e.EmailAddress) + 1, len(e.EmailAddress)) as ISP, COUNT(e.emailID) as Sends
		from  BlastActivitySends eg with (NOLOCK) JOIN [ecn5_communicator].[dbo].[Emails] e on e.EmailID = eg.EmailID  JOIN 
								@b ids ON ids.bID = eg.blastID 
		group by substring(e.EmailAddress, charindex('@', e.EmailAddress) + 1, len(e.EmailAddress))
		) sq1
		left outer join
		(
		select  substring(EmailAddress, charindex('@', sq3.EmailAddress) + 1, len(sq3.EmailAddress)) as ISP, COUNT(sq3.EmailID) as Bounces
		from 
		(select distinct e.EmailAddress, e.EmailID from [ecn5_communicator].[dbo].[Emails] e
		JOIN 
		BlastActivityBounces eg with (NOLOCK) on e.EmailID = eg.EmailID JOIN 
								@b ids ON ids.bID = eg.blastID ) sq3
		group by substring(sq3.EmailAddress, charindex('@', sq3.EmailAddress) + 1, len(sq3.EmailAddress))
		) sq2
		on sq1.ISP=sq2.ISP
		left outer join
		(
		select  substring(EmailAddress, charindex('@', sq3.EmailAddress) + 1, len(sq3.EmailAddress)) as ISP, COUNT(sq3.EmailID) as HardBounces
		from 
		(select distinct e.EmailAddress, e.EmailID from [ecn5_communicator].[dbo].[Emails] e
		JOIN 
		BlastActivityBounces eg with (NOLOCK) on e.EmailID = eg.EmailID
		 JOIN @b ids ON ids.bID = eg.blastID  and eg.BounceCodeID=6) sq3
		group by substring(sq3.EmailAddress, charindex('@', sq3.EmailAddress) + 1, len(sq3.EmailAddress))
		) sq3
		on sq1.ISP=sq3.ISP
		left outer join
		(
		select  substring(EmailAddress, charindex('@', sq3.EmailAddress) + 1, len(sq3.EmailAddress)) as ISP, COUNT(sq3.EmailID) as SoftBounces
		from 
		(select distinct e.EmailAddress, e.EmailID from [ecn5_communicator].[dbo].[Emails] e
		JOIN 
		BlastActivityBounces eg with (NOLOCK) on e.EmailID = eg.EmailID
		 JOIN @b ids ON ids.bID = eg.blastID  and eg.BounceCodeID=9) sq3
		group by substring(sq3.EmailAddress, charindex('@', sq3.EmailAddress) + 1, len(sq3.EmailAddress))
		) sq4
		on sq1.ISP=sq4.ISP
		left outer join
		(
		select  substring(EmailAddress, charindex('@', sq3.EmailAddress) + 1, len(sq3.EmailAddress)) as ISP, COUNT(sq3.EmailID) as OtherBounces
		from 
		(select distinct e.EmailAddress, e.EmailID from [ecn5_communicator].[dbo].[Emails] e
		JOIN 
		BlastActivityBounces eg with (NOLOCK) on e.EmailID = eg.EmailID
		 JOIN @b ids ON ids.bID = eg.blastID  and eg.BounceCodeID<>9 AND eg.BounceCodeID<>6) sq3
		group by substring(sq3.EmailAddress, charindex('@', sq3.EmailAddress) + 1, len(sq3.EmailAddress))
		) sq5
		on sq1.ISP=sq5.ISP
		where (ISNULL(sq2.Bounces, 0)*100)/sq1.Sends<>0
		order by 7 desc, 6 desc
		
END