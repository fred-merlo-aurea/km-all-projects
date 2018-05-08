CREATE PROCEDURE [dbo].[v_DashBoard_OpensClicks] 
	@CustomerID INT
AS     
BEGIN 


with TrendsRanked as (
select top 10 b.blastID, cib.EmailSubject from ECN5_COMMUNICATOR..Blast b
join CampaignItemBlast cib
on cib.BlastID=b.BlastID
join CampaignItem ci
on ci.CampaignItemID=cib.CampaignItemID
where CustomerID=@CustomerID and StatusCode='sent' and TestBlast='n'
order by b.BlastID desc
)

select q1.EmailSubject as [Subject], q1.OpensCount, q2.ClicksCount from  (
(select bao.BlastID, COUNT(OpenID) as OpensCount, TrendsRanked.EmailSubject from ECN_ACTIVITY..BlastActivityOpens bao
join TrendsRanked
on bao.BlastID=TrendsRanked.BlastID
group by bao.BlastID, TrendsRanked.EmailSubject) q1
join 
(select bac.BlastID, COUNT(ClickID) as ClicksCount, TrendsRanked.EmailSubject  from ECN_ACTIVITY..BlastActivityClicks bac
join TrendsRanked
on bac.BlastID=TrendsRanked.BlastID
group by bac.BlastID, TrendsRanked.EmailSubject) q2
on q1.BlastID=q2.BlastID)

END