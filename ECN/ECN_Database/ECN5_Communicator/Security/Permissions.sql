﻿GRANT CONNECT TO [ecn5]
    AS [dbo];


GO
GRANT CONNECT TO [ecn5writer]
    AS [dbo];


GO
--GRANT CONNECT TO [moa]
--    AS [dbo];


GO
GRANT CONNECT TO [reader]
    AS [dbo];


GO
--GRANT CONNECT TO [testboot]
--    AS [dbo];


GO
GRANT EXECUTE TO [db_executor]
    AS [dbo];


GO
GRANT CONNECT TO [webuser]
    AS [dbo];


GO
GRANT CONNECT TO [bill.hipps]
    AS [dbo];


GO
GRANT CREATE FUNCTION TO [Developer]
    AS [dbo];


GO
GRANT CREATE PROCEDURE TO [Developer]
    AS [dbo];


GO
GRANT CREATE TABLE TO [Developer]
    AS [dbo];


GO
GRANT CREATE VIEW TO [Developer]
    AS [dbo];


GO
GRANT VIEW DEFINITION TO [Developer]
    AS [dbo];


GO
GRANT VIEW DEFINITION TO [QA]
    AS [dbo];


GO
GRANT CONNECT TO [justin.wagner]
    AS [dbo];


GO
GRANT CONNECT TO [justin.welter]
    AS [dbo];


GO
GRANT CONNECT TO [latha.sunil]
    AS [dbo];


GO
GRANT CONNECT TO [jaime.mohs]
    AS [dbo];


GO
GRANT CONNECT TO [Linda.Wilkinson]
    AS [dbo];


GO
--GRANT CONNECT TO [tristan.mccormick]
--    AS [dbo];


GO
GRANT SELECT
    ON SCHEMA::[sys] TO [bill.hipps]
    AS [sys];


GO
GRANT ALTER
    ON SCHEMA::[dbo] TO [Developer]
    AS [dbo];


GO
GRANT EXECUTE
    ON SCHEMA::[dbo] TO [Developer]
    AS [dbo];


GO
GRANT INSERT
    ON SCHEMA::[dbo] TO [Developer]
    AS [dbo];


GO
GRANT REFERENCES
    ON SCHEMA::[dbo] TO [Developer]
    AS [dbo];


GO
GRANT SELECT
    ON SCHEMA::[dbo] TO [Developer]
    AS [dbo];


GO
GRANT UPDATE
    ON SCHEMA::[dbo] TO [Developer]
    AS [dbo];


GO
GRANT SELECT
    ON SCHEMA::[dbo] TO [QA]
    AS [dbo];
