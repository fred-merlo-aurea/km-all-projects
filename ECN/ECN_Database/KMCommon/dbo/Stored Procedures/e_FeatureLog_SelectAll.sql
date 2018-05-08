CREATE PROCEDURE dbo.e_FeatureLog_SelectAll
AS
SELECT *
FROM FeatureLog With(NoLock)
ORDER BY IsCompleted,FeaturePriority
