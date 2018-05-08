CREATE VIEW dbo.vw_BrandConsensus
AS
SELECT DISTINCT TOP 100 PERCENT bd.BrandID, ps.SubscriptionID, m.MasterID
FROM         dbo.PubSubscriptions AS ps WITH (NOLOCK) INNER JOIN
                      dbo.PubSubscriptionDetail AS psd ON psd.PubSubscriptionID = ps.PubSubscriptionID INNER JOIN
                      dbo.CodeSheet AS cs WITH (NOLOCK) ON cs.CodeSheetID = psd.CodesheetID INNER JOIN
                      dbo.CodeSheet_Mastercodesheet_Bridge AS cb WITH (NOLOCK) ON cb.CodeSheetID = cs.CodeSheetID INNER JOIN
                      dbo.Mastercodesheet AS m WITH (NOLOCK) ON m.MasterID = cb.MasterID INNER JOIN
                      dbo.ResponseGroups AS rg WITH (NOLOCK) ON rg.ResponseGroupID = cs.ResponseGroupID INNER JOIN
                      dbo.BrandDetails AS bd WITH (NOLOCK) ON rg.PubID = bd.PubID join
                      dbo.Brand b on b.brandID = bd.brandID
Where b.Isdeleted = 0
union
select distinct b.brandID, subscriptionID, sd.MasterID from SubscriptionDetails sd join 
(select distinct mc.MasterID from Mastercodesheet mc left outer join CodeSheet_Mastercodesheet_Bridge cmb on mc.MasterID = cmb.MasterID
where cmb.CodeSheetID is null ) x on sd.MasterID = x.MasterID Cross join Brand b