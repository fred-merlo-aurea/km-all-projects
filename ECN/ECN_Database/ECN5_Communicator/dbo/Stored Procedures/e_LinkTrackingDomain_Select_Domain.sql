CREATE PROCEDURE [dbo].[e_LinkTrackingDomain_Select_Domain]   
@Domain varchar(200),
@CustomerID int,
@LTID int
AS


select * from LinkTrackingDomain WITH (NOLOCK) where Domain=@Domain and CustomerID=@CustomerID and LTID=@LTID and IsDeleted=0