﻿--CREATE VIEW [dbo].[vw_Demo3136]
--	AS 

--SELECT ps.subscriptionid, 
--		 ps.PubID,
--       x.* 
--FROM   pubsubscriptions ps WITH (nolock) 
--       JOIN (SELECT pubsubscriptionid, 
--                    demo31, 
--                    demo32, 
--                    demo33, 
--                    demo34, 
--                    demo35, 
--                    demo36 
--             FROM   (SELECT pubsubscriptionid, 
--                            marketingid, 
--                            c.codedescription 
--                     FROM   marketingmap mm WITH (nolock) 
--                            JOIN uad_lookup..code c WITH (nolock) 
--                              ON mm.marketingid = c.codeid) p 
--                    PIVOT ( Count (marketingid) 
--                          FOR c.codedescription IN ( demo31, 
--                                                   demo32, 
--                                                   demo33, 
--                                                   demo34, 
--                                                   demo35, 
--                                                   demo36 ) ) AS pvt) x 
--         ON x.pubsubscriptionid = ps.pubsubscriptionid 