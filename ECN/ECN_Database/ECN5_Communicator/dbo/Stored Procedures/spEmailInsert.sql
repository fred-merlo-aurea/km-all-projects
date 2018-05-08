CREATE PROCEDURE [dbo].[spEmailInsert]
@EmailAddress varchar(255),
@CustomerID int,
@Title varchar(50),
@FirstName varchar(50),
@LastName varchar(50),
@FullName varchar(50),
@Company varchar(100),
@Occupation varchar(50),
@Address varchar(255),
@Address2 varchar(255),
@City varchar(50),
@State varchar(50),
@Zip varchar(50),
@Country varchar(50),
@Voice varchar(50),
@Mobile varchar(50),
@Fax varchar(50),
@Website varchar(50),
@Age varchar(50),
@Income varchar(50),
@Gender varchar(50),
@User1 varchar(255),
@User2 varchar(255),
@User3 varchar(255),
@User4 varchar(255),
@User5 varchar(255),
@User6 varchar(255),
@Birthdate datetime,
@UserEvent1 varchar(50),
@UserEvent1Date datetime,
@UserEvent2 varchar(50),
@UserEvent2Date datetime,
@Notes varchar(1000),
@DateAdded datetime,
@DateUpdated datetime,
@Password varchar(25),
@BounceScore int
AS
INSERT INTO [ecn5_communicator].[dbo].[Emails]
           ([EmailAddress]
           ,[CustomerID]
           ,[Title]
           ,[FirstName]
           ,[LastName]
           ,[FullName]
           ,[Company]
           ,[Occupation]
           ,[Address]
           ,[Address2]
           ,[City]
           ,[State]
           ,[Zip]
           ,[Country]
           ,[Voice]
           ,[Mobile]
           ,[Fax]
           ,[Website]
           ,[Age]
           ,[Income]
           ,[Gender]
           ,[User1]
           ,[User2]
           ,[User3]
           ,[User4]
           ,[User5]
           ,[User6]
           ,[Birthdate]
           ,[UserEvent1]
           ,[UserEvent1Date]
           ,[UserEvent2]
           ,[UserEvent2Date]
           ,[Notes]
           ,[DateAdded]
           ,[DateUpdated]
           ,[Password]
           ,[BounceScore])
     VALUES
           (@EmailAddress,@CustomerID,@Title,@FirstName,@LastName,@FullName,@Company,@Occupation,@Address,@Address2,
			@City,@State,@Zip,@Country,@Voice,@Mobile,@Fax,@Website,@Age,@Income,@Gender,@User1,@User2,@User3,@User4,@User5,
			@User6,@Birthdate,@UserEvent1,@UserEvent1Date,@UserEvent2,@UserEvent2Date,@Notes,@DateAdded,@DateUpdated,
			@Password,@BounceScore);SELECT @@IDENTITY;
