CREATE PROCEDURE [dbo].[e_DomainTrackerActivity_Get_IpLocation]
@ipLong bigint
AS

--DECLARE @ipLong bigint = 7089199237

SELECT TOP 1 * FROM IpLocation 
Where @ipLong >= IpStart
  and @ipLong <= IpEnd