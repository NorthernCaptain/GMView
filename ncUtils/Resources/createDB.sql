--- Script for creating SQLite database for Knowhere
--- use 'sqlite3.exe knowhere.sq3' for creating db file
--- and then insert this code into sqlite prompt

create table history_items ( id integer primary key autoincrement, typename varchar(128) not null,
   value varchar(512), flags integer default 0, created datetime default (datetime('now')) );
--- index on typename
create index history_tn_idx on history_items (typename);
---   
create table poi (id integer primary key autoincrement, name varchar(128) not null, description varchar(512),
   type varchar(32) not null default 'poi', comments varchar(256), tags varchar(128), 
   lon float not null, lat float not null, flags integer default 0, icon varchar(32), color integer);
---
create index poi_name_idx on poi (name);
---
create index poi_type_idx on poi (type);

--- virtual table -> RTree index on POI table for fast spatial search
create virtual table poi_spatial using rtree (id, minLon, maxLon, minLat, maxLat);
---
--- POI types
create table poi_type (id integer primary key autoincrement, name varchar(32) not null, 
	icon varchar(32), color integer, flags integer default 0, comments varchar(128));
---
create unique index poi_type_name_idx on poi_type (name);

--- POI groups (their description)
create table poi_group (id integer primary key autoincrement, name varchar(128) not null,
    flags integer default 0, comments varchar(128));

--- POI group nested hierarchy
create table poi_group_member (id integer primary key autoincrement, 
	parent_id integer, member_id integer, 
	foreign key (parent_id) references poi_group (id) on delete cascade);
---
create index poi_grp_mem_idx on poi_group_member (member_id);
---
create index poi_grp_par_idx on poi_group_member (parent_id);
--- End of file ---