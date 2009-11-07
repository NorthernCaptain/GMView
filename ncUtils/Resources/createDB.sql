--- Script for creating SQLite database for Knowhere
--- use 'sqlite3.exe knowhere.sq3' for creating db file
--- and then insert this code into sqlite prompt

create table history_items ( id integer primary key autoincrement, typename varchar(128) not null,
   value varchar(512), flags integer default 0, created datetime default (datetime('now')) );
   
   
   
--- End of file ---