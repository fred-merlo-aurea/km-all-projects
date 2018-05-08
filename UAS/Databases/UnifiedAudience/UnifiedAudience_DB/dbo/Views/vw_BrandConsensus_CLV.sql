CREATE VIEW [dbo].[vw_BrandConsensus_CLV]
	AS 
SELECT DISTINCT TOP 100 PERCENT bd.BrandID, s.cgrp_no, m.MasterID
FROM         dbo.Subscriptions s WITH (NOLOCK) INNER JOIN
                      dbo.PubSubscriptions AS ps WITH (NOLOCK) ON s.SubscriptionID = ps.SubscriptionID INNER JOIN
                      dbo.PubSubscriptionDetail AS psd ON psd.PubSubscriptionID = ps.PubSubscriptionID INNER JOIN
                      dbo.CodeSheet AS cs WITH (NOLOCK) ON cs.CodeSheetID = psd.CodesheetID INNER JOIN
                      dbo.CodeSheet_Mastercodesheet_Bridge AS cb WITH (NOLOCK) ON cb.CodeSheetID = cs.CodeSheetID INNER JOIN
                      dbo.Mastercodesheet AS m WITH (NOLOCK) ON m.MasterID = cb.MasterID INNER JOIN
                      dbo.ResponseGroups AS rg WITH (NOLOCK) ON rg.ResponseGroupID = cs.ResponseGroupID INNER JOIN
                      dbo.BrandDetails AS bd WITH (NOLOCK) ON rg.PubID = bd.PubID JOIN
                      dbo.Brand b ON b.brandID = bd.brandID
WHERE     b.Isdeleted = 0
UNION
SELECT DISTINCT b.brandID, s.cgrp_no, sd.MasterID
FROM         Subscriptions s WITH (NOLOCK) INNER JOIN
                      SubscriptionDetails sd WITH (NOLOCK) ON s.SubscriptionID = sd.SubscriptionID INNER JOIN
                          (SELECT DISTINCT mc.MasterID
                            FROM          Mastercodesheet mc WITH (NOLOCK) LEFT OUTER JOIN
                                                   CodeSheet_Mastercodesheet_Bridge cmb WITH (NOLOCK) ON mc.MasterID = cmb.MasterID
                            WHERE      cmb.CodeSheetID IS NULL) x ON sd.MasterID = x.MasterID CROSS JOIN
                      Brand b
