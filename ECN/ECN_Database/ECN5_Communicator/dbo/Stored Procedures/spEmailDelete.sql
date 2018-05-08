CREATE PROCEDURE [dbo].[spEmailDelete]
@EmailID int
AS
DELETE FROM [ecn5_communicator].[dbo].[Emails]
      WHERE EmailID = @EmailID
