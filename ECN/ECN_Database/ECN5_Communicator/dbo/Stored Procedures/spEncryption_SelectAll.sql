CREATE PROCEDURE [dbo].[spEncryption_SelectAll]
AS
SELECT [ID]
      ,[PassPhrase]
      ,[SaltValue]
      ,[HashAlgorithm]
      ,[PasswordIterations]
      ,[InitVector]
      ,[KeySize]
  FROM ecn_misc..Encryption
