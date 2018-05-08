Create Function dbo.[Remove_Dup_Entry]
(
      @Duplicate_String VARCHAR(MAX),
      @delimiter VARCHAR(2)
)
RETURNS VARCHAR(MAX)
BEGIN
     DECLARE @Xml XML
     DECLARE @Removed_Duplicate_String VARCHAR(Max)
     SET @Duplicate_String=REPLACE(@Duplicate_String,'&','And')
     SET @delimiter=REPLACE(@delimiter,'&','And')
 
     SET @Xml = CAST(('<A>'+REPLACE(@Duplicate_String,@delimiter,'</A><A>')+'</A>') AS XML)
  
     ;WITH CTE AS (SELECT A.value('.', 'varchar(max)') AS [Column]
      FROM @Xml.nodes('A') AS FN(A))
  
      SELECT @Removed_Duplicate_String =Stuff((SELECT '' + @delimiter + '' + [Column]  FROM CTE GROUP BY [column]
      FOR XML PATH('') ),1,1,'')
 
      SET @Removed_Duplicate_String=REPLACE(@Removed_Duplicate_String,'And','&')          
RETURN (@Removed_Duplicate_String)
END
GO
