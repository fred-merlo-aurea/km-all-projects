CREATE PROCEDURE [dbo].[e_BlastActivityClicksInternal_Insert]
	@EmailID int,
	@BlastID int,
	@URL varchar(2048),
	@UniqueLinkID int
AS
	INSERT INTO BlastActivityClicksInternal(EmailID, BlastID, URL, UniqueLinkID, ClickTime)
	VALUES(@EmailID, @BlastID, @URL, @UniqueLinkID, GETDATE())
	SELECT @@IDENTITY;
