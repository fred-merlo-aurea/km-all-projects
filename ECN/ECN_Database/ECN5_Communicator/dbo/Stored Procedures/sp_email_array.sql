CREATE PROCEDURE [dbo].[sp_email_array] 
(
 @cid int, 
 @gid int,
 @wc varchar(50),
 @comp varchar(10),
 @whc varchar(50)
)
AS

BEGIN
 if (lower(@comp) = '=')
  BEGIN
   SELECT e.EmailID 
   FROM Emails e JOIN EmailGroups eg ON e.EmailID=eg.EmailID 
   WHERE eg.GroupID=@gid AND e.CustomerID=@cid AND @wc = @whc
  END
 ELSE
  BEGIN
   SELECT e.EmailID 
   FROM Emails e JOIN EmailGroups eg ON e.EmailID=eg.EmailID 
   WHERE eg.GroupID=@gid AND e.CustomerID=@cid AND @wc LIKE @whc
  END
END
