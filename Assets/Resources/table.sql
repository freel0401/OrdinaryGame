create table player(
	pid			integer  not null primary key autoincrement,
	account 	varchar(100) not null,
	level 		integer not null,
	exp 		integer not null,
	gold 		integer not null
);

create table item(
	sid		 	integer not null primary key autoincrement,
	id 			integer not null,
	pid 		integer not null,
	num 		integer not null,
	bind 		integer not null
);
