CREATE PROCEDURE [e_zzzzzTask_Select]
AS
SELECT * FROM Task With(NoLock) ORDER BY DisplayOrder
