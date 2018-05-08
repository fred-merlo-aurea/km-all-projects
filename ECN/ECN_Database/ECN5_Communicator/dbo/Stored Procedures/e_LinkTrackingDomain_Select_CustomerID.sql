CREATE PROCEDURE [dbo].[e_LinkTrackingDomain_Select_CustomerID]   
@CustomerID int,
@LTID int
AS


select * from LinkTrackingDomain WITH (NOLOCK) where CustomerID=@CustomerID and LTID=@LTID and Isdeleted=0