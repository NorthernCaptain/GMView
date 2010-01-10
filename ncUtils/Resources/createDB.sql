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
	color integer, flags integer default 0, comments varchar(128), sort_order integer default 0);
--- flags: bit 1 = 0x1 - if set then User defined POI type
---
create unique index poi_type_name_idx on poi_type (name);
---
insert into poi_type (id, name, description, icon, icon_cx, icon_cy) 
            values(0, 'group', 'type for groups of POIs', 'group.png', 15, 34);
---
insert into poi_type (id, name, description, icon, icon_cx, icon_cy) 
            values(1, 'unknown', 'Unknown POI type', 'unknown.png', 15, 34);
---
insert into poi_type (name, description, icon, icon_cx, icon_cy) values('airport', 'Airport', 'airport.png', 15, 34);
---
insert into poi_type (name, description, icon, icon_cx, icon_cy) values('aerodrome', 'Aerodrome', 'airplane-sport.png', 15, 34);
---
insert into poi_type (name, description, icon, icon_cx, icon_cy) values('airdrome', 'Airdrome','airplane-sport.png', 15, 34);
---
---
insert into poi_type (name, description, icon, icon_cx, icon_cy) values('ambulance', 'Ambulance', 'firstaid.png', 15, 34);
---
insert into poi_type (name, description, icon, icon_cx, icon_cy) values('attraction', 'Attraction', 'dancinghall.png', 15, 34);
---
insert into poi_type (name, description, icon, icon_cx, icon_cy) values('bank', 'Bank', 'bank.png', 15, 34);
---
insert into poi_type (name, description, icon, icon_cx, icon_cy) values('bicycle', 'Bicycle', 'cycling.png', 15, 34);
---
insert into poi_type (name, description, icon, icon_cx, icon_cy) values('bicycling', 'Bicycling', 'cycling.png', 15, 34);
---
---
insert into poi_type (name, description, icon, icon_cx, icon_cy) values('border crossing', 'Border Crossing', 'customs.png', 15, 34);
---
insert into poi_type (name, description, icon, icon_cx, icon_cy) values('customs', 'Customs', 'customs.png', 15, 34);
---
---
insert into poi_type (name, description, icon, icon_cx, icon_cy) values('bus station', 'Bus Station', 'bus.png', 15, 34);
---
insert into poi_type (name, description, icon, icon_cx, icon_cy) values('car rental service', 'Car Rental Service', 'carrental.png', 15, 34);
---
insert into poi_type (name, description, icon, icon_cx, icon_cy) values('car repair', 'Car Repair', 'carrepair.png', 15, 34);
---
insert into poi_type (name, description, icon, icon_cx, icon_cy) values('car wash', 'Car Wash', 'carwash.png', 15, 34);
---
insert into poi_type (name, description, icon, icon_cx, icon_cy) values('castle', 'Castle', 'castle.png', 15, 34);
---
insert into poi_type (name, description, icon, icon_cx, icon_cy) values('church', 'Church', 'church.png', 15, 34);
---
insert into poi_type (name, description, icon, icon_cx, icon_cy) values('temple', 'Temple', 'chapel.png', 15, 34);
---
insert into poi_type (name, description, icon, icon_cx, icon_cy) values('beach', 'Beach', 'beach.png', 15, 34);
---
insert into poi_type (name, description, icon, icon_cx, icon_cy) values('landscape', 'Landscape', 'beautiful.png', 15, 34);
---
---
insert into poi_type (name, description, icon, icon_cx, icon_cy) values('cinema', 'Cinema', 'cinema.png', 15, 34);
---
insert into poi_type (name, description, icon, icon_cx, icon_cy) values('movie theatre', 'Movie Theatre', 'cinema.png', 15, 34);
---
insert into poi_type (name, description, icon, icon_cx, icon_cy) values('movie house', 'Movie House', 'cinema.png', 15, 34);
---
---
insert into poi_type (name, description, icon, icon_cx, icon_cy) values('college', 'College', 'university.png', 15, 34);
---
insert into poi_type (name, description, icon, icon_cx, icon_cy) values('university', 'University', 'university.png', 15, 34);
---
---
insert into poi_type (name, description, icon, icon_cx, icon_cy) values('consulate', 'Consulate', 'country.png', 15, 34);
---
insert into poi_type (name, description, icon, icon_cx, icon_cy) values('embassy', 'Embassy', 'country.png', 15, 34);
---
---
insert into poi_type (name, description, icon, icon_cx, icon_cy) values('disneyland', 'Disneyland', 'party.png', 15, 34);
---
insert into poi_type (name, description, icon, icon_cx, icon_cy) values('drug store', 'Drug Store', 'firstaid.png', 15, 34);
---
insert into poi_type (name, description, icon, icon_cx, icon_cy) values('pharmacy', 'Pharmacy', 'firstaid.png', 15, 34);
---
---
insert into poi_type (name, description, icon, icon_cx, icon_cy) values('exhibition center', 'Exhibition Center', 'music-live.png', 15, 34);
---
insert into poi_type (name, description, icon, icon_cx, icon_cy) values('show', 'Show', 'music-live.png', 15, 34);
---
---
insert into poi_type (name, description, icon, icon_cx, icon_cy) values('factory', 'Factory', 'factory.png', 15, 34);
---
insert into poi_type (name, description, icon, icon_cx, icon_cy) values('plant', 'Plant', 'factory.png', 15, 34);
---
insert into poi_type (name, description, icon, icon_cx, icon_cy) values('manufacturer', 'Manufacturer', 'factory.png', 15, 34);
---
---
insert into poi_type (name, description, icon, icon_cx, icon_cy) values('fire station', 'Fire Station', 'firemen.png', 15, 34);
---
insert into poi_type (name, description, icon, icon_cx, icon_cy) values('fire department', 'Fire Department', 'firemen.png', 15, 34);
---
insert into poi_type (name, description, icon, icon_cx, icon_cy) values('fire engine house', 'Fire Engine House', 'firemen.png', 15, 34);
---
---
insert into poi_type (name, description, icon, icon_cx, icon_cy) values('gas station', 'Gas Station', 'gazstation.png', 15, 34);
---
insert into poi_type (name, description, icon, icon_cx, icon_cy) values('filling station', 'Filling Station', 'gazstation.png', 15, 34);
---
insert into poi_type (name, description, icon, icon_cx, icon_cy) values('gasoline station', 'Gasoline Station', 'gazstation.png', 15, 34);
---
insert into poi_type (name, description, icon, icon_cx, icon_cy) values('petrol station', 'Petrol Station', 'gazstation.png', 15, 34);
---
---
insert into poi_type (name, description, icon, icon_cx, icon_cy) values('golf club', 'Golf Club', 'golf.png', 15, 34);
---
insert into poi_type (name, description, icon, icon_cx, icon_cy) values('government', 'Government', 'administration.png', 15, 34);
---
insert into poi_type (name, description, icon, icon_cx, icon_cy) values('hospital', 'Hospital', 'doctor.png', 15, 34);
---
insert into poi_type (name, description, icon, icon_cx, icon_cy) values('hotel', 'Hotel', 'hotel.png', 15, 34);
---
insert into poi_type (name, description, icon, icon_cx, icon_cy) values('hostel', 'Hostel', 'hostel.png', 15, 34);
---
insert into poi_type (name, description, icon, icon_cx, icon_cy) values('inn', 'Inn', 'hotel.png', 15, 34);
---
insert into poi_type (name, description, icon, icon_cx, icon_cy) values('motel', 'Motel', 'campingsite.png', 15, 34);
---
insert into poi_type (name, description, icon, icon_cx, icon_cy) values('camping', 'Camping', 'tent.png', 15, 34);
---
insert into poi_type (name, description, icon, icon_cx, icon_cy) values('resort', 'Resort', 'resort.png', 15, 34);
---
---
insert into poi_type (name, description, icon, icon_cx, icon_cy) values('library', 'Library', 'library.png', 15, 34);
---
insert into poi_type (name, description, icon, icon_cx, icon_cy) values('mcdonalds', 'McDonalds', 'fastfood.png', 15, 34);
---
insert into poi_type (name, description, icon, icon_cx, icon_cy) values('fast food', 'Fast Food', 'fastfood.png', 15, 34);
---
insert into poi_type (name, description, icon, icon_cx, icon_cy) values('cafe', 'Cafe', 'coffee.png', 15, 34);
---
insert into poi_type (name, description, icon, icon_cx, icon_cy) values('bar', 'Bar', 'bar.png', 15, 34);
---
insert into poi_type (name, description, icon, icon_cx, icon_cy) values('restaurant', 'Restaurant', 'restaurant.png', 15, 34);
---
---
insert into poi_type (name, description, icon, icon_cx, icon_cy) values('memorial', 'Memorial', 'modernmonument.png', 15, 34);
---
insert into poi_type (name, description, icon, icon_cx, icon_cy) values('military', 'Military', 'museum-war.png', 15, 34);
---
---insert into poi_type (name, description, icon, icon_cx, icon_cy) values('range', 'Range', 'Range.png', 15, 34);
---
insert into poi_type (name, description, icon, icon_cx, icon_cy) values('ground', 'Ground', 'agriculture.png', 15, 34);
---
insert into poi_type (name, description, icon, icon_cx, icon_cy) values('bridge', 'Bridge', 'bridge.png', 15, 34);
---
insert into poi_type (name, description, icon, icon_cx, icon_cy) values('building', 'Building', 'bigcity.png', 15, 34);
---
---insert into poi_type (name, description, icon, icon_cx, icon_cy) values('ordnance yard', 'Ordnance Yard', 'Ordnance Yard.png', 15, 34);
---
insert into poi_type (name, description, icon, icon_cx, icon_cy) values('mountains', 'Mountains', 'cyclingmountain1.png', 15, 34);
---
---
insert into poi_type (name, description, icon, icon_cx, icon_cy) values('museum', 'Museum', 'museum-historical.png', 15, 34);
---
---insert into poi_type (name, description, icon, icon_cx, icon_cy) values('pagoda', 'Pagoda', 'park.png', 15, 34);
---
insert into poi_type (name, description, icon, icon_cx, icon_cy) values('park', 'Park', 'park.png', 15, 34);
---
insert into poi_type (name, description, icon, icon_cx, icon_cy) values('picnic ground', 'Picnic Ground', 'picnic.png', 15, 34);
---
insert into poi_type (name, description, icon, icon_cx, icon_cy) values('police station', 'Police Station', 'police.png', 15, 34);
---
insert into poi_type (name, description, icon, icon_cx, icon_cy) values('pool', 'Pool', 'pool.png', 15, 34);
---
insert into poi_type (name, description, icon, icon_cx, icon_cy) values('swimming pool', 'Swimming Pool', 'pool.png', 15, 34);
---
---
insert into poi_type (name, description, icon, icon_cx, icon_cy) values('port', 'Port', 'museum-naval.png', 15, 34);
---
insert into poi_type (name, description, icon, icon_cx, icon_cy) values('sea port', 'Sea Port', 'museum-naval.png', 15, 34);
---
insert into poi_type (name, description, icon, icon_cx, icon_cy) values('railway station', 'Railway Station', 'train.png', 15, 34);
---
insert into poi_type (name, description, icon, icon_cx, icon_cy) values('school', 'School', 'school.png', 15, 34);
---
insert into poi_type (name, description, icon, icon_cx, icon_cy) values('shop', 'Shop', 'supermarket.png', 15, 34);
---
insert into poi_type (name, description, icon, icon_cx, icon_cy) values('store', 'Store', 'supermarket.png', 15, 34);
---
---
insert into poi_type (name, description, icon, icon_cx, icon_cy) values('stadium', 'Stadium', 'stadium.png', 15, 34);
---
insert into poi_type (name, description, icon, icon_cx, icon_cy) values('tent', 'Tent', 'tent.png', 15, 34);
---
insert into poi_type (name, description, icon, icon_cx, icon_cy) values('theater', 'Theater', 'theater.png', 15, 34);
---
insert into poi_type (name, description, icon, icon_cx, icon_cy) values('walk', 'Walk', 'hiking.png', 15, 34);
---
insert into poi_type (name, description, icon, icon_cx, icon_cy) values('walking', 'Walking', 'hiking.png', 15, 34);
---
---
insert into poi_type (name, description, icon, icon_cx, icon_cy) values('zoo', 'Zoo', 'zoo.png', 15, 34);
---
insert into poi_type (name, description, icon, icon_cx, icon_cy) values('construction', 'Constuction', 'construction.png', 15, 34);
---
insert into poi_type (name, description, icon, icon_cx, icon_cy) values('wifi', 'WiFi', 'wifi.png', 15, 34);
---
insert into poi_type (name, description, icon, icon_cx, icon_cy) values('toilets', 'Toilets', 'toilets.png', 15, 34);
---
insert into poi_type (name, description, icon, icon_cx, icon_cy) values('wc', 'WC', 'toilets.png', 15, 34);
---
insert into poi_type (name, description, icon, icon_cx, icon_cy) values('skiing', 'Skiing', 'skiing.png', 15, 34);
---
insert into poi_type (name, description, icon, icon_cx, icon_cy) values('pizza', 'Pizza', 'pizza.png', 15, 34);
---
insert into poi_type (name, description, icon, icon_cx, icon_cy) values('subway', 'Subway', 'subway.png', 15, 34);
---
insert into poi_type (name, description, icon, icon_cx, icon_cy) values('metro', 'Metro station', 'subway.png', 15, 34);
---
insert into poi_type (name, description, icon, icon_cx, icon_cy) values('phone', 'Telephone', 'telephone.png', 15, 34);
---
insert into poi_type (name, description, icon, icon_cx, icon_cy) values('home', 'Home', 'home.png', 15, 34);
---

            
---   POI and their groups
create table poi (id integer primary key autoincrement, name varchar(128) not null, description varchar(512),
   type integer not null default 0, is_group integer default 0, comments varchar(256), tags varchar(128), 
   lon float not null default 1, lat float not null default 0, alt float not null default 0, 
   flags integer default 0, icon varchar(64), icon_cx integer default 0, icon_cy integer default 0, 
   color integer, zoom_lvl integer default 0, created datetime default (datetime('now')),
   foreign key (type) references poi_type (id) on delete set default);
---
create index poi_name_idx on poi (name);
---
create index poi_type_idx on poi (type);

--- virtual table -> RTree index on POI table for fast spatial search
create virtual table poi_spatial using rtree (id, minLon, maxLon, minLat, maxLat);
--- Default poi group
insert into poi (id, name, description, type, is_group) values (0, 'root', 'root group', 0, 1);
---
insert into poi (name, description, type, is_group) values ('quick add', 'quick add POI group', 0, 1);
---
insert into poi (name, description, type, is_group) values ('unsorted', 'unsorted POI group', 0, 1);


--- POI group nested hierarchy
create table poi_group_member (id integer primary key autoincrement, 
	parent_id integer default 0, member_id integer default 0, member_is_group integer default 0,
	created datetime default (datetime('now')),
	foreign key (parent_id) references poi (id) on delete set default,
	foreign key (member_id) references poi (id) on delete set default);
---
create index poi_grp_mem_idx on poi_group_member (member_id);
---
create index poi_grp_par_idx on poi_group_member (parent_id);
--- for quick add
insert into poi_group_member (parent_id, member_id, member_is_group) values (0, 1, 1);
--- for unsorted
insert into poi_group_member (parent_id, member_id, member_is_group) values (0, 2, 1);

--- End of file ---