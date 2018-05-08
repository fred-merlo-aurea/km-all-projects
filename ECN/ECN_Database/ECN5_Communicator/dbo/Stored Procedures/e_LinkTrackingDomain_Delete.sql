CREATE PROCEDURE [dbo].[e_LinkTrackingDomain_Delete]   
@LinkTrackingDomainID int,
@UserID int
AS

update LinkTrackingDomain set IsDeleted=1, UpdatedUserID=@UserID, UpdatedDate=GETDATE() 
where LinkTrackingDomainID=@LinkTrackingDomainID