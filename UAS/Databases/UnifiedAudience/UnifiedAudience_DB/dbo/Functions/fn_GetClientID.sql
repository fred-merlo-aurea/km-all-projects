create FUNCTION [dbo].[fn_GetClientID](@UserAgent VARCHAR(255))
 
       RETURNS int
 
       AS
 
       BEGIN
 
       DECLARE @EmailClient int
 
       SET @EmailClient = CASE
       
			 WHEN PATINDEX('%outlook%',@UserAgent) > 0 

                     THEN '1' 
                     
              WHEN PATINDEX('%IE%',@UserAgent) > 0  or PATINDEX('%InternetExplorer%',@UserAgent) > 0

                     THEN '2'                     
 
              WHEN PATINDEX('%iphone%',@UserAgent) > 0 

                     THEN '3'
 
              WHEN PATINDEX('%ipad%',@UserAgent) > 0 

                     THEN '4'
 
              WHEN PATINDEX('%android%',@UserAgent) > 0 

                     THEN '5'        
 
              WHEN PATINDEX('%firefox%',@UserAgent) > 0 

                     THEN '6' 
 
              WHEN PATINDEX('%Chrome%',@UserAgent) > 0 

                     THEN '7'
                     
               WHEN PATINDEX('%blackberry%',@UserAgent) > 0 

                     THEN '8' 
                     
               WHEN PATINDEX('%webos%',@UserAgent) > 0 

                     THEN '9' 
 
			   WHEN PATINDEX('%symbian%',@UserAgent) > 0 

                     THEN '10' 
 
              WHEN PATINDEX('%Safari%',@UserAgent) > 0 

                     THEN '11' 

              WHEN PATINDEX('%opera%',@UserAgent) > 0 

                     THEN '12' 
                     
              WHEN PATINDEX('%thunderbird%',@UserAgent) > 0 

                     THEN '13'  
                     
               WHEN PATINDEX('%lotus%',@UserAgent) > 0 

                     THEN '14' 
 
              ELSE 
                     '15'
 
              END
 
       RETURN(@EmailClient)
 
       END