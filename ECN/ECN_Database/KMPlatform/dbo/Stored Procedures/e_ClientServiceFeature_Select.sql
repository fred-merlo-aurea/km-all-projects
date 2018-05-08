CREATE PROCEDURE [dbo].[e_ClientServiceFeature_Select]
AS
	SELECT *
	FROM ClientGroupServiceFeatureMap With(NoLock)
	ORDER BY ClientGroupID