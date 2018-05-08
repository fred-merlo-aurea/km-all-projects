CREATE PROCEDURE [dbo].[e_LinkTrackingDomain_Save]   
@Domain varchar(200),
@LTID int,
@CustomerID int,
@UserID int
AS

insert into LinkTrackingDomain(Domain, LTID, CustomerID, CreatedUserID, CreatedDate, IsDeleted) 
values(@Domain, @LTID, @CustomerID, @UserID, GETDATE(), 0)