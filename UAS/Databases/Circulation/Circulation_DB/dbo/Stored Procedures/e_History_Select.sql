CREATE PROCEDURE e_History_Select
AS
	SELECT * FROM History With(NoLock) 
