CREATE PROCEDURE [dbo].[e_EmailPreview_Insert]
@EmailTestID int,
@BlastID int,
@CustomerID int,
@ZipFile nvarchar(500) = '',
@CreatedByID int,
@LinkTestID int,
@BaseChannelGUID uniqueidentifier
AS
INSERT INTO EmailPreview (EmailTestID,BlastID,CustomerID,ZipFile,CreatedByID,DateCreated,TimeCreated,LinkTestID,BaseChannelGUID)
VALUES(@EmailTestID,@BlastID,@CustomerID,@ZipFile,@CreatedByID,GETDATE(),GETDATE(),@LinkTestID,@BaseChannelGUID)
