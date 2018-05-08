CREATE PROCEDURE e_Publisher_Select
AS
SELECT * FROM Publisher With(NoLock)
order by ClientID, PublisherName
