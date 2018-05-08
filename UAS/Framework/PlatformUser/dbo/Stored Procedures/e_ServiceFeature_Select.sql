CREATE PROCEDURE [dbo].[e_ServiceFeature_Select]
AS
	SELECT *
	FROM ServiceFeature With(NoLock)
	ORDER BY DisplayOrder
