CREATE PROCEDURE [dbo].[e_SecurityGroup_Select]
AS
SELECT * FROM SecurityGroup With(NoLock)
