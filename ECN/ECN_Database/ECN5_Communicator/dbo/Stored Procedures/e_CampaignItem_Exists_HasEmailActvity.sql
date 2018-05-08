CREATE  PROC dbo.e_CampaignItem_Exists_HasEmailActvity 
(
	@CampaignItemID int,
	@days int
)
AS 
BEGIN
	set @days = @days * -1
	declare @blastID int	
	select @blastID= BlastID from CampaignItemBlast where CampaignItemID=@CampaignItemID and IsDeleted=0
	
	if exists (	select top 1 ClickID  
				FROM ecn_Activity..BlastActivityClicks WITH (NOLOCK) 
				where BlastID=@blastID
				and ClickTime>DATEADD(D, @days, DATEADD(day, DATEDIFF(day, 0, GETDATE()), 0)))
	BEGIN
		select 1
	END
	else if exists (select top 1 OpenID  
				FROM ecn_Activity..BlastActivityOpens WITH (NOLOCK) 
				where BlastID=@blastID
				and OpenTime>DATEADD(D, @days, DATEADD(day, DATEDIFF(day, 0, GETDATE()), 0)))
	BEGIN
		select 1
	END
	else if exists (select top 1 UnsubscribeID  
				FROM ecn_Activity..BlastActivityUnSubscribes WITH (NOLOCK) 
				where BlastID=@blastID
				and UnsubscribeTime>DATEADD(D, @days, DATEADD(day, DATEDIFF(day, 0, GETDATE()), 0)))
	BEGIN
		select 1
	END
	else if exists (select top 1 BounceID  
				FROM ecn_Activity..BlastActivityBounces WITH (NOLOCK) 
				where BlastID=@blastID
				and BounceTime>DATEADD(D, @days, DATEADD(day, DATEDIFF(day, 0, GETDATE()), 0)))
	BEGIN
		select 1
	END
	else
	BEGIN
		select 0
	END
END