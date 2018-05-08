CREATE PROCEDURE [dbo].[e_DomainTracking_Select]
AS
BEGIN

	set nocount on

	Select * from DomainTracking With(NoLock)

END
