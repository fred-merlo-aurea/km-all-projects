CREATE PROCEDURE [dbo].[e_BlastRSS_Save]
	@BlastID int,
	@FeedID int, 
	@Name varchar(100),
	@FeedHTML varchar(MAX),
	@FeedTEXT varchar(MAX)
AS
	INSERT INTO BlastRSS(BlastID, FeedID, Name, FeedHTML, FeedTEXT)
	VALUES(@BlastID, @FeedID, @Name, @FeedHTML, @FeedTEXT)
	SELECT @@IDENTITY;
