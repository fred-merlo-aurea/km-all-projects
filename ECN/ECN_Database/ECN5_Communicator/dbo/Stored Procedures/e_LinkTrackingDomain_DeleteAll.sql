CREATE PROCEDURE [dbo].[e_LinkTrackingDomain_DeleteAll]   
@CustomerID int,
@LTID int,
@UserID int
AS

update LinkTrackingDomain set IsDeleted=1, UpdatedUserID=@UserID, UpdatedDate=GETDATE() 
where LTID=@LTID and CustomerID=@CustomerID