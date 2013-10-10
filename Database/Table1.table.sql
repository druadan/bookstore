DROP database Bookstore;
CREATE database Bookstore;
USE Bookstore;

DROP TABLE [Order_details];
DROP TABLE [Order];
DROP TABLE [Review];
DROP TABLE [Tag_associations];
DROP TABLE [Client];
DROP TABLE [Book];
DROP TABLE [Salesman];
DROP TABLE [Tag];

CREATE TABLE [Client]
(
	[login] varchar(30) NOT NULL PRIMARY KEY, 
	name varchar(30), 
	surname varchar(50),
	[address] varchar(100),
	loyal_client int,
	[password] varchar(8000) NOT NULL,
)

CREATE TABLE [Book]
(
	id int NOT NULL PRIMARY KEY IDENTITY(1,1), 
	title varchar(150), 
	author varchar(50),
	price float,
)

CREATE TABLE [Salesman]
(
	id int NOT NULL PRIMARY KEY IDENTITY(1,1), 
	name varchar(30), 
	surname varchar(30),
)

CREATE TABLE [Tag]
(
	tag_id varchar(30) NOT NULL PRIMARY KEY, 
)

CREATE TABLE [Order]
(
	id int NOT NULL PRIMARY KEY IDENTITY(1,1), 
	customer_login varchar(30) NOT NULL, 
	order_date date, 
	salesman_id int,
	foreign key (customer_login ) references Client ([login]),
	foreign key (salesman_id ) references Salesman (id)
)


CREATE TABLE [Order_details]
(
	id int NOT NULL PRIMARY KEY IDENTITY(1,1), 
	order_id int NOT NULL, 
	book_id int NOT NULL,
	quantity int NOT NULL,
	foreign key ( order_id ) references [Order] (id),
	foreign key ( book_id ) references Book (id)
)

CREATE TABLE [Review]
(
	id int NOT NULL PRIMARY KEY IDENTITY(1,1), 
	customer_login varchar(30) NOT NULL, 
	book_id int NOT NULL,
	title varchar(50) NOT NULL,
	content varchar(8000) NOT NULL,
	foreign key ( customer_login ) references Client ([login]),
	foreign key ( book_id ) references Book (id)
)


CREATE TABLE [Tag_associations]
(
	id int NOT NULL PRIMARY KEY IDENTITY(1,1),
	tag_id varchar(30) NOT NULL,
	book_id int NOT NULL,
	foreign key ( book_id ) references Book (id),
	foreign key ( tag_id ) references Tag (tag_id)
)


INSERT into Client
VALUES 
--( 'Piotr','Reszke','Luzino', 0, HASHBYTES('SHA1','pr')),
--( 'Olga','M','Luzino', 1, HASHBYTES('SHA1','om'))
( 'pr','Piotr','Reszke','Luzino', 0, 'pr'),
( 'om','Olga', 'M','Luzino', 1, 'om')
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

INSERT into [Order](customer_login, order_date, salesman_id)
VALUES 
( 'pr', '2013-12-01', 1 ),
( 'om', '2011-04-21', 1 )
;



INSERT into [Review] (customer_login, book_id, title, content)
VALUES 
( 'pr', 2, 'no nie wiem', 'taka sobie, mogla by byc fajniejsza' ),
( 'om', 1, 'super', 'polecam każdemu' ),
( 'om', 2, 'troche nudna', 'ale da się przeczytac' )
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
DROP USER adm;
CREATE USER adm from login adm;
EXEC sp_addrolemember 'db_owner', 'adm'
