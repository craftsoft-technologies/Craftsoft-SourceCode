@ECHO OFF


TYPE schema\*.sql > 000Schema.sql

TYPE datasript\*.sql > 001Datascript.sql

TYPE UserDefinedTableType\*.sql > 002UserDefinedTable.sql

TYPE view\*.sql > 003View.sql

TYPE fn\*.sql > 004Function.sql

TYPE trigger\*.sql > 005Trigger.sql

TYPE sp\*.sql > 006StorePro.sql

TYPE sqljob\*.sql > 007SQLJob.sql

TYPE setupscript\*.sql > 008setupscript.sql