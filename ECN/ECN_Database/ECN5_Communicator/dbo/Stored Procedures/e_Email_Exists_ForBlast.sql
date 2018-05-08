CREATE PROCEDURE [dbo].[e_Email_Exists_ForBlast] 
	@EmailID int,
	@EmailAddress varchar(255),
	@CustomerID int,
	@GroupID int,
	@BlastID int
AS     
BEGIN     		
	IF EXISTS 
			(
				SELECT TOP 1 e.EmailID 
				FROM 
					Emails e WITH (NOLOCK)
                    JOIN EmailGroups eg WITH (NOLOCK) ON e.emailID = eg.EmailID
                    JOIN Blast b WITH (NOLOCK) ON b.GroupID = eg.GroupID
                WHERE
					e.EmailAddress = @EmailAddress AND
                    e.EmailID = @EmailID AND
                    e.CustomerID = @CustomerID AND
                    eg.GroupID = @GroupID AND
                    b.BlastID = @BlastID AND 
                    b.StatusCode <> 'Deleted'
			) SELECT 1 ELSE SELECT 0
END