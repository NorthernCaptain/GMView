--- Script for creating SQLite database for Knowhere
--- use 'sqlite3.exe knowhere.sq3' for creating db file
--- and then insert this code into sqlite prompt

create table history_items ( id integer primary key autoincrement, typename varchar(128) not null,
   value varchar(512), flags integer default 0, created datetime default (datetime('now')) );
--- index on typename
create index history_tn_idx on history_items (typename);

---
--- POI types
create table poi_type (id integer primary key autoincrement, name varchar(48) not null, description varchar(128) not null,
	icon varchar(64), icon_cx integer default 0, icon_cy integer default 0, zoom_lvl integer default 12,
	color integer, flags integer default 0, comments varchar(128));
---
create unique index poi_type_name_idx on poi_type (name);
---
insert into poi_type (id, name, description, icon, icon_cx, icon_cy) 
            values(0, 'group', 'type for groups of POIs', 'group.png', 15, 34);
---
insert into poi_type (id, name, description, icon, icon_cx, icon_cy) 
            values(1, 'unknown', 'unknown POI type', 'unknown.png', 15, 34);

            
---   POI and their groups
create table poi (id integer primary key autoincrement, name varchar(128) not null, description varchar(512),
   type integer not null default 0, is_group integer default 0, comments varchar(256), tags varchar(128), 
   lon float not null default 1, lat float not null default 0, alt float not null default 0, 
   flags integer default 0, icon varchar(64), icon_cx integer default 0, icon_cy integer default 0, 
   color integer, zoom_lvl integer default 0,
   foreign key (type) references poi_type (id) on delete set default);
---
create index poi_name_idx on poi (name);
---
create index poi_type_idx on poi (type);

--- virtual table -> RTree index on POI table for fast spatial search
create virtual table poi_spatial using rtree (id, minLon, maxLon, minLat, maxLat);
--- Default poi group
insert into poi (id, name, description, type, is_group) values (0, 'default', 'default group', 0, 1);


--- POI group nested hierarchy
create table poi_group_member (id integer primary key autoincrement, 
	parent_id integer default 0, member_id integer default 0, 
	foreign key (parent_id) references poi (id) on delete set default,
	foreign key (member_id) references poi (id) on delete set default);
---
create index poi_grp_mem_idx on poi_group_member (member_id);
---
create index poi_grp_par_idx on poi_group_member (parent_id);
--- End of file ---