CREATE PROCEDURE [dbo].[spEmailUpdate]
@EmailID int,
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
UPDATE [ecn5_communicator].[dbo].[Emails]
   SET [EmailAddress] = @EmailAddress
      ,[CustomerID] = @CustomerID
      ,[Title] = @Title
      ,[FirstName] = @FirstName
      ,[LastName] = @LastName
      ,[FullName] = @FullName
      ,[Company] = @Company
      ,[Occupation] = @Occupation
      ,[Address] = @Address
      ,[Address2] = @Address2
      ,[City] = @City
      ,[State] = @State
      ,[Zip] = @Zip
      ,[Country] = @Country
      ,[Voice] = @Voice
      ,[Mobile] = @Mobile
      ,[Fax] = @Fax
      ,[Website] = @Website
      ,[Age] = @Age
      ,[Income] = @Income
      ,[Gender] = @Gender
      ,[User1] = @User1
      ,[User2] = @User2
      ,[User3] = @User3
      ,[User4] = @User4
      ,[User5] = @User5
      ,[User6] = @User6
      ,[Birthdate] = @Birthdate
      ,[UserEvent1] = @UserEvent1
      ,[UserEvent1Date] = @UserEvent1Date
      ,[UserEvent2] = @UserEvent2
      ,[UserEvent2Date] = @UserEvent2Date
      ,[Notes] = @Notes
      ,[DateAdded] = @DateAdded
      ,[DateUpdated] = @DateUpdated
      ,[Password] = @Password
      ,[BounceScore] = @BounceScore
 WHERE EmailID = @EmailID
