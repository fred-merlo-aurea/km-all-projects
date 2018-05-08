CREATE PROCEDURE [dbo].[e_Publication_Delete]   
@PublicationID int,
@UserID int

AS
	Update Publication SET IsDeleted = 1, UpdatedUserID = @UserID, UpdatedDate = GETDATE()  where  PublicationID = @PublicationID
	
	update ecn5_communicator.dbo.Groups set GroupName = GroupName + '_deleted(' + CONVERT(varchar(10),getdate(), 101) + ')', UpdatedUserID = @UserID, UpdatedDate = GETDATE() where GroupName = ( select PublicationName + ' Subscribers' from Publication where PublicationID = @PublicationID)