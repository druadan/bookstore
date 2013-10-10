﻿DROP TABLE [dbo].[Order_details];
DROP TABLE [dbo].[Order];
DROP TABLE [dbo].[Review];
DROP TABLE [dbo].[Tag_associations];
DROP TABLE [dbo].[Client];
DROP TABLE [dbo].[Book];
DROP TABLE [dbo].[Salesman];
DROP TABLE [dbo].[Tag];

CREATE TABLE [dbo].[Client]
(
	id int NOT NULL PRIMARY KEY IDENTITY(1,1), 
	name varchar(30), 
	surname varchar(50),
	[address] varchar(100),
	loyal_client int,
	[password] varchar(8000) NOT NULL,
)

CREATE TABLE [dbo].[Book]
(
	id int NOT NULL PRIMARY KEY IDENTITY(1,1), 
	title varchar(150), 
	author varchar(50),
	price float,
)

CREATE TABLE [dbo].[Salesman]
(
	id int NOT NULL PRIMARY KEY IDENTITY(1,1), 
	name varchar(30), 
	surname varchar(30),
)

CREATE TABLE [dbo].[Tag]
(
	tag_id varchar(30) NOT NULL PRIMARY KEY, 
)

CREATE TABLE [dbo].[Order]
(
	id int NOT NULL PRIMARY KEY IDENTITY(1,1), 
	customer_id int NOT NULL, 
	order_date date, 
	salesman_id int,
	foreign key (customer_id ) references Client (id),
	foreign key (salesman_id ) references Salesman (id)
)


CREATE TABLE [dbo].[Order_details]
(
	id int NOT NULL PRIMARY KEY IDENTITY(1,1), 
	order_id int NOT NULL, 
	book_id int NOT NULL,
	quantity int NOT NULL,
	foreign key ( order_id ) references [Order] (id),
	foreign key ( book_id ) references Book (id)
)

CREATE TABLE [dbo].[Review]
(
	id int NOT NULL PRIMARY KEY IDENTITY(1,1), 
	customer_id int NOT NULL, 
	book_id int NOT NULL,
	title varchar(50) NOT NULL,
	content varchar(8000) NOT NULL,
	foreign key ( customer_id ) references Client (id),
	foreign key ( book_id ) references Book (id)
)


CREATE TABLE [dbo].[Tag_associations]
(
	id int NOT NULL PRIMARY KEY IDENTITY(1,1),
	tag_id varchar(30) NOT NULL,
	book_id int NOT NULL,
	foreign key ( book_id ) references Book (id),
	foreign key ( tag_id ) references Tag (tag_id)
)


INSERT into Client(name, surname, [address], loyal_client, [password])
VALUES 
( 'Piotr','Reszke','Luzino', 0, HASHBYTES('SHA1','pr')),
( 'Olga','M','Luzino', 1, HASHBYTES('SHA1','om'))
;

INSERT into Book(title, author, price)
VALUES 
( 'Gotuj z Olgella', 'Olgella', 1000),
( 'Kot jest gupi', 'PR', 34),
( 'Szerlok', 'Doyle', 66),
( 'Robinson', 'xxx', 88.1)
;

INSERT into Salesman(name, surname)
VALUES 
( 'Dawid', 'Bombello'),
( 'Wojtek', 'Grommenstein')
;

INSERT into Tag(tag_id)
VALUES 
( 'much_softness' ),
( 'hungry' ),
( 'wow' ),
( 'adventure' ),
( 'so_experience' )
;

INSERT into [Order](customer_id, order_date, salesman_id)
VALUES 
( 1, '2013-12-01', 1 ),
( 2, '2011-04-21', 1 )
;



INSERT into [Review] (customer_id, book_id, title, content)
VALUES 
( 1, 2, 'no nie wiem', 'taka sobie, mogla by byc fajniejsza' ),
( 1, 1, 'super', 'polecam każdemu' ),
( 2, 2, 'troche nudna', 'ale da się przeczytac' )
;

INSERT into [Tag_associations] (tag_id, book_id)
VALUES 
( 'wow', 2),
( 'wow', 1),
( 'much_softness', 1),
( 'so_experience', 3)
;

INSERT into [Order_details] (order_id, book_id, quantity)
VALUES 
( 1, 2, 1 ),
( 2, 1, 3 ),
( 2, 3, 1 )
;

DROP login adm ;
CREATE login adm with password = 'adm';

select * from master..syslogins
where name = 'adm' or name like  '%Druadan%';