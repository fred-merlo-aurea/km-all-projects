CREATE PROCEDURE [dbo].[spQGLoginParam]
	@EmailID int, 
	@AutoLoginParams varchar(255), 
	@Publisher varchar(50),
	@Magazine varchar(50),
	@Issue varchar(50) 
AS
BEGIN 
	if exists(select top 1 EmailID from QGLoginParams where EmailID = @EmailID and Publisher = @Publisher and Magazine = @Magazine and Issue = @Issue)	
		begin
			update QGLoginParams set AutoLoginParams = @AutoLoginParams where EmailID = @EmailID and Publisher = @Publisher and Magazine = @Magazine and Issue = @Issue
		end
	else
		begin
			INSERT INTO QGLoginParams (EmailID, AutoLoginParams, Publisher, Magazine, Issue) VALUES (@EmailID, @AutoLoginParams, @Publisher, @Magazine, @Issue)  
		end	
END
