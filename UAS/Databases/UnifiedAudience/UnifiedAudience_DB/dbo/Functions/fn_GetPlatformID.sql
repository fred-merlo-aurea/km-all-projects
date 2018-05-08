 create FUNCTION [dbo].[fn_GetPlatformID](@UserAgent VARCHAR(255))
 
       RETURNS int
 
       AS
 
       BEGIN
 
       DECLARE @PlatformID int
 
       SET @PlatformID = CASE
 
              WHEN PATINDEX('%windows%',@UserAgent) > 0 

                     THEN '1'
                     
              WHEN PATINDEX('%iphone%',@UserAgent) > 0 

                     THEN '2'
 
              WHEN PATINDEX('%android%',@UserAgent) > 0 

                     THEN '2'
 
              WHEN PATINDEX('%ipad%',@UserAgent) > 0 

                     THEN '2'
                     
              WHEN PATINDEX('%linux%',@UserAgent) > 0 

                     THEN '3'
                     
              WHEN PATINDEX('%blackberry%',@UserAgent) > 0 

                     THEN '2'
 
              WHEN PATINDEX('%mac%',@UserAgent) > 0 

                     THEN '4'
                     
              WHEN PATINDEX('%symbian%',@UserAgent) > 0 

                     THEN '2'
                     
              WHEN PATINDEX('%webos%',@UserAgent) > 0 

                     THEN '2'
 
              ELSE 

                     '5'
 
              END
 
       RETURN(@PlatformID)
 
       END